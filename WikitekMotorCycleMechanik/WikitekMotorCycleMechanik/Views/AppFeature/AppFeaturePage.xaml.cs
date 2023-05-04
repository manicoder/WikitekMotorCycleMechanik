using Acr.UserDialogs;
using MultiEventController.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.AppFeature
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppFeaturePage : ContentPage
    {
        AppFeatureViewModel viewModel;
        string[] remote_detail;
        string firmware_version;
        VehicleModelResult selected_model;
        VehicleSubModel selected_submodel;
        Oem selected_oem;
        public AppFeaturePage(string firmware_version, VehicleModelResult selected_model, VehicleSubModel selected_submodel, Oem selected_oem,
            string[] remote_detail)
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new AppFeatureViewModel(this, firmware_version, selected_model, selected_submodel, selected_oem);
                
                this.firmware_version = firmware_version;
                this.selected_model = selected_model;
                this.selected_submodel = selected_submodel;
                this.selected_oem = selected_oem;
                this.remote_detail = remote_detail;

            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            App.controlEventManager.OnRecieved += ControlEventManager_OnRecieved;
            App.controlEventManager.OnRecievedData += ControlEventManager_OnRecievedData;

            if (remote_detail != null)
            {
                App.InitExpert(remote_detail[0], remote_detail[1]);

                //DependencyService.Get<ILodingPageService>().ShowLoadingPage();
                App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                {
                    ElementName = "RemoteStarted",
                    ElementValue = "RemoteStarted",
                    ToUserId = CurrentUserEvent.Instance.ToUserId,
                    IsExpert = CurrentUserEvent.Instance.IsExpert
                });
            }

            NavigationPage.SetHasBackButton(this, false);
            MessagingCenter.Send<AppFeaturePage>(this, "StopMasterDetailSwip");
            //MessagingCenter.Unsubscribe<AppFeaturePage>(this, "StopMasterDetailSwip");;
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
                    ElementName = "GoBackAP",
                    ElementValue = "GoBackAP",
                    ToUserId = CurrentUserEvent.Instance.ToUserId,
                    IsExpert = CurrentUserEvent.Instance.IsExpert
                });
            }
            //MessagingCenter.Send<AppFeaturePage>(this, "StartMasterDetailSwip");
            //MessagingCenter.Unsubscribe<AppFeaturePage>(this, "StartMasterDetailSwip");
        }

        private void ControlEventManager_OnRecievedData(object sender, EventArgs e)
        {
            if (CurrentUserEvent.Instance.IsRemote && CurrentUserEvent.Instance.IsExpert)
            {
                //await Task.Delay(100);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    string data = (string)sender; //sender as string;
                    if (!string.IsNullOrEmpty(data))
                    {
                        string[] PairedData = new string[2];
                        if (data.Contains("SendStaticDataRD#"))
                        {
                            PairedData = data.Split('#');
                            StaticMethods.ecu_info = JsonConvert.DeserializeObject<List<EcuDataSet>>(PairedData[1]);
                            var DTCFound = new Popup.DisplayAlertPage("Remote Session started", "", "OK");
                            await PopupNavigation.Instance.PushAsync(DTCFound);
                            Acr.UserDialogs.UserDialogs.Instance.Toast("Static Data Recieved", new TimeSpan(0, 0, 0, 3));
                        }
                        if (data.Contains("IDsRD#"))
                        {
                            PairedData = data.Split('#');
                            int[] arr = JsonConvert.DeserializeObject<int[]>(PairedData[1]);
                            App.model_id = arr[0];
                            App.submodel_id = arr[1];
                        }
                        else if (data.Contains("FirmwareRD#"))
                        {
                            PairedData = data.Split('#');
                            if (PairedData[1].Contains("ELM327"))
                            {
                                viewModel.firmware_version = PairedData[1].Replace("ELM327 v", "WKFV ");
                            }
                            else
                            {
                                viewModel.firmware_version = PairedData[1];
                            }
                        }
                        else if (data.Contains("SelectedModelRD#"))
                        {
                            PairedData = data.Split('#');
                            viewModel.selected_model = JsonConvert.DeserializeObject<VehicleModelResult>(PairedData[1]);
                        }
                        else if (data.Contains("SelectedSubModelRD#"))
                        {
                            PairedData = data.Split('#');
                            viewModel.selected_sub_model = JsonConvert.DeserializeObject<VehicleSubModel>(PairedData[1]);
                        }
                        else if (data.Contains("SelectedOemRD#"))
                        {
                            PairedData = data.Split('#');
                            viewModel.selected_oem = JsonConvert.DeserializeObject<Oem>(PairedData[1]);
                        }
                        else if (data.Contains("StopMasterDetailSwipRD#"))
                        {
                            NavigationPage.SetHasBackButton(this, false);
                            MessagingCenter.Send<AppFeaturePage>(this, "StopMasterDetailSwip");
                        }
                    }
                });
            }
        }

        private async void ControlEventManager_OnRecieved(object sender, EventArgs e)
        {
            string ReceiveValue = string.Empty;
            var elementEventHandler = (sender as ElementEventHandler);
            ReceiveValue = elementEventHandler.ElementValue;
            if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("RemoteStarted"))
            {
                DependencyService.Get<ILodingPageService>().IsExpertShow_Active();

                var RemoteSessionClosed = new Popup.DisplayAlertPage("Remote Session started", "", "OK");
                await PopupNavigation.Instance.PushAsync(RemoteSessionClosed);
                App.PageFreezIsEnable = true;
                //AppFeature.IsEnabled = App.PageFreezIsEnable = true;

                App.controlEventManager.SendRequestData("SendStaticData" + "RD#" + JsonConvert.SerializeObject(StaticMethods.ecu_info));
                App.controlEventManager.SendRequestData("IDs" + "RD#" + JsonConvert.SerializeObject(new int[2] {App.model_id,App.submodel_id}));
                App.controlEventManager.SendRequestData("Firmware" + "RD#" + this.firmware_version);
                App.controlEventManager.SendRequestData("SelectedModel" + "RD#" + JsonConvert.SerializeObject(this.selected_model));
                App.controlEventManager.SendRequestData("SelectedSubModel" + "RD#" + JsonConvert.SerializeObject(this.selected_submodel));
                App.controlEventManager.SendRequestData("SelectedOem" + "RD#" + JsonConvert.SerializeObject(this.selected_oem));
                App.controlEventManager.SendRequestData("StopMasterDetailSwip" + "RD#");
                DependencyService.Get<ILodingPageService>().HideLoadingPage();
            }
            else if (!CurrentUserEvent.Instance.IsExpert && elementEventHandler.ElementValue == "DTCBtnClicked")
            {
                DependencyService.Get<ILodingPageService>().IsExpertShow_Active();
                await Navigation.PushAsync(new Views.DtcList.DtcListPage());
                App.PageFreezIsEnable = true;
                DependencyService.Get<ILodingPageService>().HideLoadingPage();
            }
            else if (!CurrentUserEvent.Instance.IsExpert && elementEventHandler.ElementValue == "LiveParameterBtnClicked")
            {
                DependencyService.Get<ILodingPageService>().IsExpertShow_Active();
                await Navigation.PushAsync(new Views.LiveParameter.LiveParameterPage());
                App.PageFreezIsEnable = true;
                DependencyService.Get<ILodingPageService>().HideLoadingPage();
            }
            else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("GoBackAP"))
            {
                await this.Navigation.PopAsync();
            }
            //DependencyService.Get<ILodingPageService>().HideLoadingPage();
            App.controlEventManager.RecieveCallControlEvents(this, elementEventHandler, CurrentUserEvent.Instance.OwnerUserId);
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}