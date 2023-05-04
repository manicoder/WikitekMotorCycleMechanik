using Acr.UserDialogs;
using MultiEventController.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using WikitekMotorCycleMechanik.View.GdSection;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.DtcList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DtcListPage : ContentPage
    {

        DtcViewModel viewModel;
        bool IsPageRemove = false;
        ApiServices services;
        public DtcListPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            App.controlEventManager.OnRecieved += ControlEventManager_OnRecieved;
            App.controlEventManager.OnRecievedData += ControlEventManager_OnRecievedData;
            BindingContext = viewModel = new DtcViewModel(this);
            services = new ApiServices();
        }

        protected override void OnDisappearing()
        {

            base.OnDisappearing();

            App.controlEventManager.OnRecieved -= ControlEventManager_OnRecieved;
            App.controlEventManager.OnRecievedData -= ControlEventManager_OnRecievedData;

            if (CurrentUserEvent.Instance.IsExpert)
            {
                App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                {
                    ElementName = "GoBackDL",
                    ElementValue = "GoBackDL",
                    ToUserId = CurrentUserEvent.Instance.ToUserId,
                    IsExpert = CurrentUserEvent.Instance.IsExpert
                });
            }

            Device.InvokeOnMainThreadAsync(async () =>
            {
                viewModel.torch_image = "ic_torch_off.png";
                await Flashlight.TurnOffAsync();
            });
        }

        private void ControlEventManager_OnRecievedData(object sender, EventArgs e)
        {
            #region Check Internet Connection
            if (CurrentUserEvent.Instance.IsRemote && CurrentUserEvent.Instance.IsExpert)
            {
                //await Task.Delay(100);
                string[] PairedData = new string[2];
                //ReadDtcResponseModel read_dtc = null;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    string data = (string)sender; //sender as string;
                    if (!string.IsNullOrEmpty(data))
                    {
                        if (data.Contains("NoDtcFromServer*#"))
                        {
                            await UserDialogs.Instance.AlertAsync("DTC not found from server", "Failed", "Ok");
                            viewModel.DTCFoundOrNotMessage = "DTC not found from server.";
                            viewModel.btn_enable = true;
                            DependencyService.Get<ILodingPageService>().HideLoadingPage();
                        }
                        else if (data.Contains("ServerDtcList*#"))
                        {
                            PairedData = data.Split('#');
                            //dtc_server_list = dtc_li.codes;

                            viewModel.dtc_server_list = JsonConvert.DeserializeObject<List<DtcCode>>(PairedData[1]); ;
                            DependencyService.Get<ILodingPageService>().HideLoadingPage();
                        }
                        else if (data.Contains("DtcFromServer*#"))
                        {
                            PairedData = data.Split('#');
                            viewModel.read_dtc = JsonConvert.DeserializeObject<ReadDtcResponseModel>(PairedData[1]); ;
                            DependencyService.Get<ILodingPageService>().HideLoadingPage();
                        }
                        else if (data.Contains("DatasetIssue*#"))
                        {
                            viewModel.DTCFoundOrNotMessage = "Please check DataSet Record !!! \nOR\n DataSet is Active or Not !!!";
                        }
                        else if (data.Contains("EcuError*#"))
                        {
                            if (!string.IsNullOrEmpty(viewModel.read_dtc.status))
                            {

                                viewModel.is_running = false;
                                viewModel.empty_view_text = viewModel.read_dtc.status;
                                //dtcEcusModel.dtc_list = new List<DtcCode>(viewModel.dtc_list);
                                return;
                            }
                            else
                            {
                                //await page.DisplayAlert("Alert", "ECU_COMMUNICATION_ERROR", "Ok");
                                viewModel.is_running = false;
                                viewModel.empty_view_text = "ECU_COMMUNICATION_ERROR";
                                //dtcEcusModel.dtc_list = new List<DtcCode>(viewModel.dtc_list);
                                return;
                            }
                        }
                        else if (data.Contains("DatasetIssue*#"))
                        {
                            viewModel.DTCFoundOrNotMessage = "Please check DataSet Record !!! \nOR\n DataSet is Active or Not !!!";
                        }
                        else if (data.Contains("NODTCFOUNDFORTHISECU*#"))
                        {
                            viewModel.is_running = false;
                            viewModel.empty_view_text = "NO DTC FOUND FOR THIS ECU";
                        }
                        else if (data.Contains("ERROR*#"))
                        {
                            viewModel.is_running = false;
                            viewModel.empty_view_text = "NO DTC FOUND FOR THIS ECU";
                        }
                        else if (data.Contains("DtcCount*#"))
                        {
                            PairedData = data.Split('#');
                            viewModel.dtc_count= PairedData[1];
                            DependencyService.Get<ILodingPageService>().HideLoadingPage();
                        }
                        else if (data.Contains("ButtonEnable*#"))
                        {
                            PairedData = data.Split('#');
                            //viewModel.btn_enable = Convert.ToBoolean(PairedData[1]);

                            viewModel.btn_enable = bool.Parse(PairedData[1]);

                            DependencyService.Get<ILodingPageService>().HideLoadingPage();
                        }
                        else if (data.Contains("IsRunning*#"))
                        {
                            PairedData = data.Split('#');
                            //viewModel.btn_enable = Convert.ToBoolean(PairedData[1]);

                            viewModel.is_running = bool.Parse(PairedData[1]);

                            DependencyService.Get<ILodingPageService>().HideLoadingPage();
                        }
                        else if (data.Contains("EcuList*#"))
                        {
                            PairedData = data.Split('#');
                            viewModel.ecus_list = JsonConvert.DeserializeObject<ObservableCollection<DtcEcusModel>>(PairedData[1]); ;
                            DependencyService.Get<ILodingPageService>().HideLoadingPage();
                        }
                        else if (data.Contains("DtcList*#"))
                        {
                            PairedData = data.Split('#');
                            viewModel.dtc_list = JsonConvert.DeserializeObject<ObservableCollection<DtcCode>>(PairedData[1]); ;
                            DependencyService.Get<ILodingPageService>().HideLoadingPage();
                        }
                        else if (data.Contains("SelectedEcu*#"))
                        {
                            PairedData = data.Split('#');
                            viewModel.selected_ecu = PairedData[1];
                            DependencyService.Get<ILodingPageService>().HideLoadingPage();
                        }
                        else if (data.Contains("Exception*#"))
                        {
                            PairedData = data.Split('#');
                            viewModel.is_running = false;
                            viewModel.empty_view_text = PairedData[1];
                            DependencyService.Get<ILodingPageService>().HideLoadingPage();
                        }
                        else if (data.Contains("DtcClearOk*#"))
                        {
                            var errorpage = new Popup.DisplayAlertPage("Alert", "DTC Cleared", "OK");
                            await PopupNavigation.Instance.PushAsync(errorpage);
                        }
                        else if (data.Contains("DtcClearNotOk*#"))
                        {
                            PairedData = data.Split('#');
                            var NegativeAcknowledgement = new Popup.DisplayAlertPage("Alert", $"Negative Acknowledgement \n\n {PairedData[1]}", "OK");
                            await PopupNavigation.Instance.PushAsync(NegativeAcknowledgement);
                        }
                        else if (data.Contains("UserSubsList*#"))
                        {
                            PairedData = data.Split('#');
                            var subscriptions = JsonConvert.DeserializeObject<List<Models.Subscription>>(PairedData[1]);
                           
                            var pack1 = subscriptions.FirstOrDefault(x => x.package_name == "AssistedDiagnosticsPack");
                            if (pack1 != null)
                            {
                                viewModel.refresh_dtc_visible = true;
                                viewModel.clear_dtc_visible = true;
                                return;
                            }

                            var pack2 = subscriptions.FirstOrDefault(x => x.package_name == "DIYDiagnosticsPack");
                            if (pack2 != null)
                            {
                                viewModel.refresh_dtc_visible = true;
                                viewModel.clear_dtc_visible = true;
                                return;
                            }

                            var pack3 = subscriptions.FirstOrDefault(x => x.package_name == "BasicPack");
                            if (pack3 != null)
                            {
                                viewModel.refresh_dtc_visible = true;
                                viewModel.clear_dtc_visible = true;
                                return;
                            }
                        }
                        else if (data.Contains("ClearDtcMsg*#"))
                        {
                            PairedData = data.Split('#');
                            
                            var RemoteSessionClosed = new Views.Popup.DisplayAlertPage(PairedData[1], "", "OK");
                            await PopupNavigation.Instance.PushAsync(RemoteSessionClosed);
                        }


                        //else if (data.Contains("Troubleshoot_ResultGetGD_"))
                        //{
                        //    string[] Remove = { "Troubleshoot_ResultGetGD_" };
                        //    string[] data1 = data.Split(Remove, StringSplitOptions.RemoveEmptyEntries);
                        //    var Result = JsonConvert.DeserializeObject<GdModelGD>(data1[0]);
                        //    if (Result != null)
                        //    {
                        //        if (Result.count != 0)
                        //        {
                        //            App.DCTPage = false;
                        //            await this.Navigation.PushAsync(new InfoPage(Result.results));
                        //        }
                        //        else
                        //        {
                        //            var errorpage = new Popup.DisplayAlertPage("", "Gd not found for this DTC", "OK");
                        //            await PopupNavigation.Instance.PushAsync(errorpage);
                        //        }
                        //    }
                        //    else
                        //    {
                        //        var errorpage = new Popup.DisplayAlertPage("", "Gd not found for this DTC", "OK");
                        //        await PopupNavigation.Instance.PushAsync(errorpage);
                        //    }
                        //}
                        //else if (data.Contains("JDNotFound_"))
                        //{
                        //    var errorpage = new Popup.DisplayAlertPage("", "Gd not found for this DTC", "OK");
                        //    await PopupNavigation.Instance.PushAsync(errorpage);
                        //}
                    }
                });
            }
            #endregion
        }

        private async void ControlEventManager_OnRecieved(object sender, EventArgs e)
        {
            var elementEventHandler = (sender as ElementEventHandler);
            string ReceiveValue = string.Empty;
            ReceiveValue = elementEventHandler.ElementValue;

            if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("BtnRefreshClicked"))
            {
                viewModel.btn_enable = viewModel.is_dll_reached = false;
                viewModel.ecus_list = new ObservableCollection<DtcEcusModel>();
                viewModel.dtc_list = new ObservableCollection<DtcCode>();
                viewModel.dtc_server_list = new List<DtcCode>();
                await viewModel.GetDTCList();
                viewModel.btn_enable = viewModel.is_dll_reached = true;
                //await viewModel.GetDTCList();
            }
            else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("BtnClearClicked"))
            {
                // await viewModel.DtcClear();
                viewModel.Clear_dtc = await DependencyService.Get<Interfaces.IBlueToothDevices>().ClearDtc(elementEventHandler.ElementName);
                viewModel.btn_enable = viewModel.is_dll_reached = true;
                await Task.Delay(1000);

                var RemoteSessionClosed = new Views.Popup.DisplayAlertPage(viewModel.Clear_dtc, "", "OK");
                await PopupNavigation.Instance.PushAsync(RemoteSessionClosed);

                App.controlEventManager.SendRequestData("ClearDtcMsg*#" + viewModel.Clear_dtc);
                
                viewModel.ecus_list = new ObservableCollection<DtcEcusModel>();
                viewModel.dtc_list = new ObservableCollection<DtcCode>();
                viewModel.dtc_server_list = new List<DtcCode>();
                await viewModel.GetDTCList();
            }
            else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("DtcListScrolled"))
            {
                if(!string.IsNullOrWhiteSpace(elementEventHandler.ElementName))
                {
                    if(Convert.ToInt32(elementEventHandler.ElementName)>0)
                    {
                        collectionView.ScrollTo(Convert.ToInt32(elementEventHandler.ElementName), 0, ScrollToPosition.Start);
                    }
                }
                //collectionView.ScrollTo(Convert.ToInt32(elementEventHandler.ElementName), 0, ScrollToPosition.Start);
            }
            else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("GdClicked"))
            {
                var list = JsonConvert.DeserializeObject<string[]>(elementEventHandler.ElementName);
                await Navigation.PushAsync(new ADP.AdpPage(list[0], Convert.ToInt32(list[1]), Convert.ToInt32(list[2])));
            }
            else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("GoBackDL"))
            {
                await this.Navigation.PopAsync();
            }
            else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("GD_"))
            {
                //using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                //{
                //    await Task.Delay(100);
                //    string[] Remove = { "GD_" };
                //    string[] Data = ReceiveValue.Split(Remove, StringSplitOptions.RemoveEmptyEntries);
                //    var ClickedRowValue = JsonConvert.DeserializeObject<DtcListModel>(Data[0]);
                //    if (ClickedRowValue != null)
                //    {
                //        var res = await services.Get_gd(App.JwtToken, ClickedRowValue.code, App.sub_model_id);
                //        if (res != null)
                //        {
                //            if (!CurrentUserEvent.Instance.IsExpert)
                //            {
                //                var jsonData = JsonConvert.SerializeObject(res);
                //                await Task.Delay(200);
                //                App.controlEventManager.SendRequestData("Troubleshoot_ResultGetGD_" + jsonData);
                //            }

                //            if (res.count != 0)
                //            {
                //                await this.Navigation.PushAsync(new InfoPage(res.results));
                //            }
                //            else
                //            {
                //                //await this.DisplayAlert("", "JD Not Found for this DTC", "OK");

                //                if (!CurrentUserEvent.Instance.IsExpert)
                //                {
                //                    await Task.Delay(200);
                //                    App.controlEventManager.SendRequestData("JDNotFound_");
                //                }

                //                var errorpage = new Popup.DisplayAlertPage("", "Gd not found for this DTC", "OK");
                //                await PopupNavigation.Instance.PushAsync(errorpage);
                //            }
                //        }
                //        else
                //        {
                //            if (!CurrentUserEvent.Instance.IsExpert)
                //            {
                //                await Task.Delay(200);
                //                App.controlEventManager.SendRequestData("JDNotFound_");
                //            }

                //            var errorpage = new Popup.DisplayAlertPage("", "Gd not found for this DTC", "OK");
                //            await PopupNavigation.Instance.PushAsync(errorpage);
                //        }


                //        //else
                //        //{
                //        //    await this.DisplayAlert("", "JD Not Found for this DTC", "OK");
                //        //}
                //    }
                //    else
                //    {
                //        if (!CurrentUserEvent.Instance.IsExpert)
                //        {
                //            await Task.Delay(200);
                //            App.controlEventManager.SendRequestData("JDNotFound_");
                //        }

                //        var errorpage = new Popup.DisplayAlertPage("", "Gd not found for this DTC", "OK");
                //        await PopupNavigation.Instance.PushAsync(errorpage);

                //        //await this.DisplayAlert("", "JD Not Found for this DTC", "OK");
                //    }
                //    //else
                //    //{
                //    //    await this.DisplayAlert("", "JD Not Found for this DTC", "OK");
                //    //}
                //}
            }
            DependencyService.Get<ILodingPageService>().HideLoadingPage();
            App.controlEventManager.RecieveCallControlEvents(this, elementEventHandler, CurrentUserEvent.Instance.OwnerUserId);
        }

        private void DtcListScrolled(object sender, ItemsViewScrolledEventArgs e)
        {
            if (CurrentUserEvent.Instance.IsExpert)
            {
                App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                {
                    ElementName = Convert.ToString(e.FirstVisibleItemIndex),
                    ElementValue = "DtcListScrolled",
                    ToUserId = CurrentUserEvent.Instance.ToUserId,
                    IsExpert = CurrentUserEvent.Instance.IsExpert
                });
            }
        }

        private async void GD_Clicked(object sender, EventArgs e)
        {
            try
            {
                var item = (DtcCode)((Button)sender).BindingContext;
                if (CurrentUserEvent.Instance.IsExpert)
                {
                    App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                    {
                        ElementName = JsonConvert.SerializeObject(new string[3] { item.code, Convert.ToString(App.model_id), Convert.ToString(App.submodel_id) }),
                        ElementValue = $"GdClicked",
                        ToUserId = CurrentUserEvent.Instance.ToUserId,
                        IsExpert = CurrentUserEvent.Instance.IsExpert
                    });
                }
                await Navigation.PushAsync(new ADP.AdpPage(item.code, App.model_id, App.submodel_id));
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("", ex.ToString(), "OK");
            }
        }

        private async void GD_Clicked1(object sender, EventArgs e)
        {
            try
            {
                IsPageRemove = false;
                if (CurrentUserEvent.Instance.IsExpert)
                {
                    App.TreeSurveyPage = true;
                    string model = string.Empty;
                    var ee = e;
                    var SenderData = (Button)sender;
                    var ClickedRowValue = (DtcCode)((Button)sender).BindingContext;//SenderData.Parent.BindingContext as DtcListModel;
                    var JsonData = JsonConvert.SerializeObject(ClickedRowValue);
                    App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                    {
                        ElementName = "GD_Clicked",
                        ElementValue = "GD_" + JsonData,
                        ToUserId = CurrentUserEvent.Instance.ToUserId,
                        IsExpert = CurrentUserEvent.Instance.IsExpert
                    });
                }
                else
                {
                    #region Working Code Without Remote
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(100);
                        string model = string.Empty;
                        var ee = e;
                        var SenderData = (Button)sender;
                        //var ClickedRowValue = SenderData.Parent.BindingContext as DtcListModel;
                        var ClickedRowValue = (DtcCode)((Button)sender).BindingContext;
                        //var DtcID = viewModel.AllDtc.FirstOrDefault().dtc_code.FirstOrDefault(x => x.code == ClickedRowValue.code);

                        //if (ClickedRowValue != null)
                        //{

                        var res = await services.Get_gd(Xamarin.Essentials.Preferences.Get("token", null), ClickedRowValue.code, App.submodel_id);
                        if (res != null)
                        {
                            if (res.results.Count() != 0)
                            {
                                //await this.Navigation.PushAsync(new InfoPage(res.results));
                            }
                            else
                            {
                                //await this.DisplayAlert("", "JD Not Found for this DTC", "OK");

                                if (!CurrentUserEvent.Instance.IsExpert)
                                {
                                    App.controlEventManager.SendRequestData("JDNotFound_");
                                    await Task.Delay(200);
                                }

                                var errorpage = new Popup.DisplayAlertPage("", "Gd not found for this DTC", "OK");
                                await PopupNavigation.Instance.PushAsync(errorpage);
                            }
                        }
                        else
                        {
                            if (!CurrentUserEvent.Instance.IsExpert)
                            {
                                App.controlEventManager.SendRequestData("JDNotFound_");
                                await Task.Delay(200);
                            }

                            var errorpage = new Popup.DisplayAlertPage("", "Gd not found for this DTC", "OK");
                            await PopupNavigation.Instance.PushAsync(errorpage);
                        }


                        #region Old Code
                        //var selectedItem = (DtcModel)((Button)sender).BindingContext;



                        //var Gd = GetGdauthor(App.JwtToken, selectedItem.code, model, res.Result);
                        //await this.Navigation.PushAsync(new InfoPage(Gd, selectedItem.description, selectedItem.code));


                        //////var ecuName = GetECU(selectedItem.NodeId.Id);
                        ////if (selectedItem.code == "P0190-13")
                        ////{
                        ////    //model = $"{App.SelectedModel}_NA_NA_EMS";
                        ////    model = "Pro6000 MDE5 And 8 BSVI_NA_NA_EMS";
                        ////}
                        ////else
                        ////{
                        ////    //model = $"{App.SelectedModel}_NA_NA_EMS";
                        ////    model = "Pro2000 E366 DELPHI BSVI_NA_NA_EMS";
                        ////}
                        ////var json = DependencyService.Get<IGdLocalFile>().GetGdData().Result;
                        //////Debug.WriteLine("CatchGDData   "+ json);
                        ////var result = GetGdauthor(App.JwtToken, selectedItem.code, model, json);
                        //////var result = apiServices.GetGdauthor(App.JwtToken, selectedItem.Identifier.FailureName, model, "GdJson.txt");
                        ////if (result != null)
                        ////{
                        ////    await this.Navigation.PushAsync(new InfoPage(result.Result, selectedItem.description, selectedItem.code));
                        ////}
                        ///
                        #endregion
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                await this.DisplayAlert("", ex.ToString(), "OK");
            }
        }
    }
}