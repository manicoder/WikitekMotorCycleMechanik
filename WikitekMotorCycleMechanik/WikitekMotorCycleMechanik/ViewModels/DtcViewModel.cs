using Acr.UserDialogs;
using MultiEventController.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class DtcViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly Page page;
        public bool is_dll_reached = true;
        public DtcViewModel(Page page) : base(page)
        {
            try
            {
                btn_enable = false;
                apiServices = new ApiServices();
                this.page = page;
                ecus_list = new ObservableCollection<DtcEcusModel>();
                dtc_list = new ObservableCollection<DtcCode>();
                dtc_server_list = new List<DtcCode>();
                selected_ecu = StaticMethods.ecu_info.FirstOrDefault().ecu_name;

                //InitializeCommands();

                Device.BeginInvokeOnMainThread(async () =>
                {
                    //using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    //{
                        await GetDTCList();
                    //}
                });

                if (!CurrentUserEvent.Instance.IsExpert)
                {
                    App.controlEventManager.SendRequestData("UserSubsList*#"+ JsonConvert.SerializeObject(App.user.subscriptions));
                }
                var pack1 = App.user.subscriptions.FirstOrDefault(x => x.package_name == "AssistedDiagnosticsPack");
                if (pack1 != null)
                {
                    refresh_dtc_visible = true;
                    clear_dtc_visible = true;
                    return;
                }

                var pack2 = App.user.subscriptions.FirstOrDefault(x => x.package_name == "DIYDiagnosticsPack");
                if (pack2 != null)
                {
                    refresh_dtc_visible = true;
                    clear_dtc_visible = true;
                    return;
                }

                var pack3 = App.user.subscriptions.FirstOrDefault(x => x.package_name == "BasicPack");
                if (pack3 != null)
                {
                    refresh_dtc_visible = true;
                    clear_dtc_visible = true;
                    return;
                }

            }
            catch (Exception ex)
            {
            }
        }

        #region properties
        private string _selected_ecu;
        public string selected_ecu
        {
            get => _selected_ecu;
            set
            {
                _selected_ecu = value;
                OnPropertyChanged("selected_ecu");
            }
        }

        private string _DTCFoundOrNotMessage;
        public string DTCFoundOrNotMessage
        {
            get { return _DTCFoundOrNotMessage; }
            set { _DTCFoundOrNotMessage = value; OnPropertyChanged("DTCFoundOrNotMessage"); }
        }

        private ObservableCollection<DtcEcusModel> _ecus_list;
        public ObservableCollection<DtcEcusModel> ecus_list
        {
            get => _ecus_list;
            set
            {
                _ecus_list = value;
                OnPropertyChanged("ecus_list");
            }
        }

        private ObservableCollection<DtcCode> _dtc_list;
        public ObservableCollection<DtcCode> dtc_list
        {
            get => _dtc_list;
            set
            {
                _dtc_list = value;
                OnPropertyChanged("dtc_list");
            }
        }

        private List<DtcCode> _dtc_server_list;
        public List<DtcCode> dtc_server_list
        {
            get => _dtc_server_list;
            set
            {
                _dtc_server_list = value;
                OnPropertyChanged("dtc_server_list");
            }
        }

        private bool _refresh_dtc_visible = false;
        public bool refresh_dtc_visible
        {
            get => _refresh_dtc_visible;
            set
            {
                _refresh_dtc_visible = value;
                OnPropertyChanged("refresh_dtc_visible");
            }
        }

        private bool _clear_dtc_visible = false;
        public bool clear_dtc_visible
        {
            get => _clear_dtc_visible;
            set
            {
                _clear_dtc_visible = value;
                OnPropertyChanged("clear_dtc_visible");
            }
        }

        private bool _is_running = true;
        public bool is_running
        {
            get => _is_running;
            set
            {
                _is_running = value;
                OnPropertyChanged("is_running");
            }
        }

        private string _empty_view_text = "Loading...";
        public string empty_view_text
        {
            get => _empty_view_text;
            set
            {
                _empty_view_text = value;
                OnPropertyChanged("empty_view_text");
            }
        }

        private string _dtc_count = "";
        public string dtc_count
        {
            get => _dtc_count;
            set
            {
                _dtc_count = value;
                OnPropertyChanged("dtc_count");
            }
        }

        private bool _btn_enable = false;
        public bool btn_enable
        {
            get => _btn_enable;
            set
            {
                _btn_enable = value;
                OnPropertyChanged("btn_enable");
            }
        }

        private string _torch_image = "ic_torch_off.png";
        public string torch_image
        {
            get => _torch_image;
            set
            {
                _torch_image = value;
                OnPropertyChanged("torch_image");
            }
        }
        #endregion


        #region Methods


        //public async Task GetDTCList()
        //{
        //    ReadDtcResponseModel read_dtc = new ReadDtcResponseModel();
        //    try
        //    {
        //        btn_enable = false;
        //        DTCFoundOrNotMessage = "Looking for DTC Record";
        //        is_running = true;
        //        empty_view_text = "Loading...";
        //        int count = 0;
        //        foreach (var ecu in StaticMethods.ecu_info)
        //        {
        //            count++;
        //            int dtc_dataset = ecu.dtc_dataset_id;
        //            var dtc_li = await apiServices.GetDtc(Xamarin.Essentials.Preferences.Get("token", null), dtc_dataset, App.is_update);
        //            if (dtc_li == null)
        //            {
        //                await page.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
        //                DTCFoundOrNotMessage = "Internet connection Issue.";
        //                btn_enable = true;
        //                return;
        //            }

        //            dtc_server_list = dtc_li.codes;

        //            DtcEcusModel dtcEcusModel = new DtcEcusModel();
        //            dtcEcusModel.ecu_name = ecu.ecu_name;
        //            dtcEcusModel.opacity = count == 1 ? 1 : .5;
        //            Debug.WriteLine($"Dtc View Model : ECU NAME = {ecu.ecu_name},  DTC DATASET ID = {dtc_dataset}");
        //            //DependencyService.Get<Interfaces.IBlueToothDevices>().SendHeader(ecu.tx_header, ecu.rx_header);
        //            read_dtc = await DependencyService.Get<Interfaces.IBlueToothDevices>().ReadDtc(ecu.read_dtc_index);

        //            if (read_dtc != null)
        //            {
        //                if (read_dtc.status == "NO_ERROR")
        //                {
        //                    var code = read_dtc.dtcs.GetLength(0);
        //                    var status = read_dtc.dtcs.GetLength(1);
        //                    for (int i = 0; i <= code - 1; i++)
        //                    {
        //                        DtcCode dtcListModel = new DtcCode();
        //                        dtcListModel.code = read_dtc.dtcs[i, 0].ToString();
        //                        for (int j = 0; j <= 0; j++)
        //                        {
        //                            var dtc_status = read_dtc.dtcs[i, 1].ToString();
        //                            string[] split_string = dtc_status.Split(':');

        //                            if (dtc_status.Contains("Current"))
        //                            {
        //                                dtcListModel.status_activation = dtc_status;
        //                                dtcListModel.status_activation_color = Color.Red;
        //                            }
        //                            else if (dtc_status.Contains("Pending"))
        //                            {
        //                                dtcListModel.status_activation = dtc_status;
        //                                dtcListModel.status_activation_color = Color.Green;
        //                            }
        //                            else
        //                            {

        //                                if (split_string[0] == "Inactive")
        //                                {
        //                                    dtcListModel.status_activation = split_string[0];
        //                                    dtcListModel.status_activation_color = Color.Green;
        //                                }
        //                                else if (split_string[0] == "Active")
        //                                {

        //                                    dtcListModel.status_activation = split_string[0];
        //                                    dtcListModel.status_activation_color = Color.Red;
        //                                }

        //                                if (split_string[1] == "LampOff")
        //                                {
        //                                    dtcListModel.lamp_activation = split_string[1];
        //                                    dtcListModel.lamp_activation_color = Color.Green;
        //                                }
        //                                else if (split_string[1] == "LampOn")
        //                                {

        //                                    dtcListModel.lamp_activation = split_string[1];
        //                                    dtcListModel.lamp_activation_color = Color.Red;
        //                                }
        //                            }
        //                            Debug.WriteLine($"Dtc View Model : CODE = {dtcListModel.code}, "); //STATUS= {split_string[0]}:{split_string[1]}");
        //                        }
        //                        if (dtc_server_list != null)
        //                        {
        //                            var desc = dtc_server_list.FirstOrDefault(x => x.code == dtcListModel.code);
        //                            if (desc != null)
        //                            {
        //                                dtcListModel.description = desc.description;
        //                            }
        //                            else
        //                            {
        //                                dtcListModel.description = "Description not found";
        //                            }
        //                            dtc_list.Add(dtcListModel);
        //                        }
        //                        else
        //                        {
        //                            DTCFoundOrNotMessage = "Please check DataSet Record !!! \nOR\n DataSet is Active or Not !!!";
        //                        }
        //                    }

        //                    //btn_enable = true;
        //                }
        //                else
        //                {
        //                    //btn_enable = true;
        //                    if (!string.IsNullOrEmpty(read_dtc.status))
        //                    {
        //                        //await page.DisplayAlert("Alert", read_dtc.status , "Ok");
        //                        is_running = false;
        //                        empty_view_text = read_dtc.status;
        //                        dtcEcusModel.dtc_list = new List<DtcCode>(dtc_list);
        //                        return;
        //                    }
        //                    else
        //                    {
        //                        //await page.DisplayAlert("Alert", "ECU_COMMUNICATION_ERROR", "Ok");
        //                        is_running = false;
        //                        empty_view_text = "ECU_COMMUNICATION_ERROR";
        //                        dtcEcusModel.dtc_list = new List<DtcCode>(dtc_list);
        //                        return;
        //                    }
        //                    //DTC_Error = read_dtc.status;
        //                }
        //                dtcEcusModel.dtc_list = new List<DtcCode>(dtc_list);
        //                btn_enable = true;
        //            }
        //            else
        //            {
        //                btn_enable = true;
        //                dtcEcusModel.dtc_list = new List<DtcCode>();
        //            }
        //            ecus_list.Add(dtcEcusModel);

        //            if (ecus_list.FirstOrDefault().dtc_list.Count() == 0)
        //            {
        //                is_running = false;
        //                empty_view_text = "NO DTC FOUND FOR THIS ECU";
        //                return;
        //                //await page.DisplayAlert("NO DTC Found", "NO DTC Found for this ECU", "Ok");
        //                //DTCFoundOrNotMessage = "NO DTC Found for this ECU !!!";
        //            }
        //            else
        //            {
        //                is_running = false;
        //                empty_view_text = "NO DTC FOUND FOR THIS ECU";
        //                dtc_count = $"{ecus_list.FirstOrDefault().dtc_list.Count()} DTCs";
        //                //await page.DisplayAlert("Number of DTC", ecus_list.FirstOrDefault().dtc_list.Count() + " DTC Found !!!", "Ok");
        //            }
        //        }
        //        //btn_enable = true;
        //        dtc_list = new ObservableCollection<DtcCode>(ecus_list.FirstOrDefault().dtc_list);
        //        selected_ecu = ecus_list[0].ecu_name;
        //        btn_enable = true;
        //        //});

        //    }
        //    catch (Exception ex)
        //    {
        //        btn_enable = true;
        //        is_running = false;
        //        empty_view_text = ex.Message+"\n\n\n" +ex.StackTrace+"\n\n\n" + JsonConvert.SerializeObject(read_dtc); ;
        //    }
        //}

        public ReadDtcResponseModel read_dtc;
        public async Task GetDTCList()
        {
            //ReadDtcResponseModel read_dtc = new ReadDtcResponseModel
            //{
            //    status = "NO_ERROR",
            //    dtcs = new string[1, 2] { { "P0115", "Inactive" } }//new string[1, 2] { { "", "" } };
            //};;
            //ReadDtcResponseModel read_dtc = null;
            try
            {
                btn_enable = false;
                DTCFoundOrNotMessage = "Looking for DTC Record";
                is_running = true;
                empty_view_text = "Loading...";
                int count = 0;
                if (!CurrentUserEvent.Instance.IsExpert)
                {
                    foreach (var ecu in StaticMethods.ecu_info)
                    {
                        count++;
                        int dtc_dataset = ecu.dtc_dataset_id;
                        var dtc_li = await apiServices.GetDtc(Xamarin.Essentials.Preferences.Get("token", null), dtc_dataset, App.is_update);
                        if (dtc_li == null)
                        {
                            App.controlEventManager.SendRequestData("NoDtcFromServer*#");
                            await UserDialogs.Instance.AlertAsync("DTC not found from server", "Failed", "Ok");
                            DTCFoundOrNotMessage = "DTC not found from server.";
                            btn_enable = true;
                            App.controlEventManager.SendRequestData("ButtonEnable*#" + "true");
                            return;
                        }

                        dtc_server_list = dtc_li.codes;

                        DtcEcusModel dtcEcusModel = new DtcEcusModel();
                        dtcEcusModel.ecu_name = ecu.ecu_name;
                        dtcEcusModel.opacity = count == 1 ? 1 : .5;
                        Debug.WriteLine($"Dtc View Model : ECU NAME = {ecu.ecu_name},  DTC DATASET ID = {dtc_dataset}");
                        //DependencyService.Get<Interfaces.IBlueToothDevices>().SendHeader(ecu.tx_header, ecu.rx_header);
                        read_dtc = await DependencyService.Get<Interfaces.IBlueToothDevices>().ReadDtc(ecu.read_dtc_index);

                        App.controlEventManager.SendRequestData("DtcFromServer*#" + JsonConvert.SerializeObject(read_dtc));
                        if (read_dtc != null)
                        {
                            if (read_dtc.status == "NO_ERROR")
                            {
                                var code = read_dtc.dtcs.GetLength(0);
                                var status = read_dtc.dtcs.GetLength(1);
                                for (int i = 0; i <= code - 1; i++)
                                {
                                    DtcCode dtcListModel = new DtcCode();
                                    dtcListModel.code = read_dtc.dtcs[i, 0].ToString();
                                    for (int j = 0; j <= 0; j++)
                                    {
                                        var dtc_status = read_dtc.dtcs[i, 1].ToString();
                                        string[] split_string = dtc_status.Split(':');

                                        if (dtc_status.Contains("Current"))
                                        {
                                            dtcListModel.status_activation = dtc_status;
                                            dtcListModel.status_activation_color = Color.Red;
                                        }
                                        else if (dtc_status.Contains("Pending"))
                                        {
                                            dtcListModel.status_activation = dtc_status;
                                            dtcListModel.status_activation_color = Color.Green;
                                        }
                                        else
                                        {

                                            if (split_string[0] == "Inactive")
                                            {
                                                dtcListModel.status_activation = split_string[0];
                                                dtcListModel.status_activation_color = Color.Green;
                                            }
                                            else if (split_string[0] == "Active")
                                            {

                                                dtcListModel.status_activation = split_string[0];
                                                dtcListModel.status_activation_color = Color.Red;
                                            }
                                            if (split_string.Count() > 1)
                                            {
                                                if (split_string[1] == "LampOff")
                                                {
                                                    dtcListModel.lamp_activation = split_string[1];
                                                    dtcListModel.lamp_activation_color = Color.Green;
                                                }
                                                else if (split_string[1] == "LampOn")
                                                {

                                                    dtcListModel.lamp_activation = split_string[1];
                                                    dtcListModel.lamp_activation_color = Color.Red;
                                                }
                                            }
                                        }
                                        Debug.WriteLine($"Dtc View Model : CODE = {dtcListModel.code}, "); //STATUS= {split_string[0]}:{split_string[1]}");
                                    }
                                    if (dtc_server_list != null)
                                    {
                                        var desc = dtc_server_list.FirstOrDefault(x => x.code == dtcListModel.code);
                                        if (desc != null)
                                        {
                                            dtcListModel.description = desc.description;
                                        }
                                        else
                                        {
                                            dtcListModel.description = "Description not found";
                                        }
                                        dtc_list.Add(dtcListModel);
                                    }
                                    else
                                    {
                                        App.controlEventManager.SendRequestData("DatasetIssue*#");
                                        DTCFoundOrNotMessage = "Please check DataSet Record !!! \nOR\n DataSet is Active or Not !!!";
                                    }
                                }

                                //btn_enable = true;
                            }
                            else
                            {
                                //btn_enable = true;
                                if (!string.IsNullOrEmpty(read_dtc.status))
                                {
                                    //await page.DisplayAlert("Alert", read_dtc.status , "Ok");
                                    is_running = false;
                                    App.controlEventManager.SendRequestData("IsRunning*#" + "false");
                                    empty_view_text = read_dtc.status;
                                    dtcEcusModel.dtc_list = new List<DtcCode>(dtc_list);
                                }
                                else
                                {
                                    //await page.DisplayAlert("Alert", "ECU_COMMUNICATION_ERROR", "Ok");
                                    is_running = false;
                                    App.controlEventManager.SendRequestData("IsRunning*#" + "false");
                                    empty_view_text = "ECU_COMMUNICATION_ERROR";
                                    dtcEcusModel.dtc_list = new List<DtcCode>(dtc_list);
                                }
                                App.controlEventManager.SendRequestData("EcuError*#");

                                return;//DTC_Error = read_dtc.status;
                            }
                            dtcEcusModel.dtc_list = new List<DtcCode>(dtc_list);
                            btn_enable = true;
                            App.controlEventManager.SendRequestData("ButtonEnable*#" + "true");
                        }
                        else
                        {
                            btn_enable = true;
                            App.controlEventManager.SendRequestData("ButtonEnable*#" + "true");
                            dtcEcusModel.dtc_list = new List<DtcCode>();
                        }
                        ecus_list.Add(dtcEcusModel);

                        if (ecus_list.FirstOrDefault().dtc_list.Count() == 0)
                        {
                            App.controlEventManager.SendRequestData("NODTCFOUNDFORTHISECU*#");
                            is_running = false;
                            App.controlEventManager.SendRequestData("IsRunning*#" + "false");
                            empty_view_text = "NO DTC FOUND FOR THIS ECU";
                            return;
                            //await page.DisplayAlert("NO DTC Found", "NO DTC Found for this ECU", "Ok");
                            //DTCFoundOrNotMessage = "NO DTC Found for this ECU !!!";
                        }
                        else
                        {
                            //App.controlEventManager.SendRequestData("NODTCFOUNDFORTHISECU*#");
                            //DTCFoundOrNotMessage = "Looking for DTC Record";
                            //is_running = true;
                            dtc_count = $"{ecus_list.FirstOrDefault().dtc_list.Count()} DTCs";
                            App.controlEventManager.SendRequestData("DtcCount*#"+$"{dtc_count}");
                            //await page.DisplayAlert("Number of DTC", ecus_list.FirstOrDefault().dtc_list.Count() + " DTC Found !!!", "Ok");
                        }
                    }
                    //btn_enable = true;
                    dtc_list = new ObservableCollection<DtcCode>(ecus_list.FirstOrDefault().dtc_list);
                    selected_ecu = ecus_list[0].ecu_name;
                    btn_enable = true;
                    App.controlEventManager.SendRequestData("ButtonEnable*#" + "true");

                    App.controlEventManager.SendRequestData("EcuList*#" + JsonConvert.SerializeObject(ecus_list));
                    App.controlEventManager.SendRequestData("DtcList*#" + JsonConvert.SerializeObject(dtc_list));
                    App.controlEventManager.SendRequestData("SelectedEcu*#" + selected_ecu);
                }
                //});

            }
            catch (Exception ex)
            {
                btn_enable = true;
                is_running = false;
                App.controlEventManager.SendRequestData("ButtonEnable*#" + "true");
                App.controlEventManager.SendRequestData("IsRunning*#" + "false");
                empty_view_text = ex.Message + "\n\n\n" + ex.StackTrace + "\n\n\n" + JsonConvert.SerializeObject(read_dtc);
                App.controlEventManager.SendRequestData("Exception*#" + empty_view_text);
            }
        }
        #endregion

        #region Commands
        public ICommand RefreshCommand => new Command((obj) =>
        {
            try
            {
                if (is_dll_reached)
                {
                    Task.Run(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            //using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                            //{
                                if (CurrentUserEvent.Instance.IsRemote && CurrentUserEvent.Instance.IsExpert)
                                {
                                    await Task.Delay(100);
                                    App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                                    {
                                        ElementName = "BtnRefreshClicked",
                                        ElementValue = "BtnRefreshClicked",
                                        ToUserId = CurrentUserEvent.Instance.ToUserId,
                                        IsExpert = CurrentUserEvent.Instance.IsExpert
                                    });
                                }
                
                                btn_enable = is_dll_reached = false;
                                ecus_list = new ObservableCollection<DtcEcusModel>();
                                dtc_list = new ObservableCollection<DtcCode>();
                                dtc_server_list = new List<DtcCode>();
                                await GetDTCList();
                                btn_enable = is_dll_reached = true;
                            //}
                        });
                    });
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                //btn_enable = is_dll_reached = true;
            }
        });

        public string Clear_dtc = string.Empty;
        public ICommand ClearCommand => new Command((obj) =>
        {
            try
            {
                Clear_dtc = string.Empty;
                if (is_dll_reached)
                {

                    //await Task.Delay(200);

                    btn_enable = is_dll_reached = false;
                    var clear_dtc_index = StaticMethods.ecu_info.FirstOrDefault(X => X.ecu_name == selected_ecu).clear_dtc_index;
                    Task.Run(() =>
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                            {

                                if (CurrentUserEvent.Instance.IsExpert)
                                {
                                    App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                                    {
                                        ElementName = clear_dtc_index,
                                        ElementValue = "BtnClearClicked",
                                        ToUserId = CurrentUserEvent.Instance.ToUserId,
                                        IsExpert = CurrentUserEvent.Instance.IsExpert
                                    });

                                    DependencyService.Get<ILodingPageService>().HideLoadingPage();
                                    DependencyService.Get<ILodingPageService>().ShowLoadingPage();
                                    await Task.Delay(200);
                                }


                                Clear_dtc = await DependencyService.Get<Interfaces.IBlueToothDevices>().ClearDtc(clear_dtc_index);

                                btn_enable = is_dll_reached = true;
                                await Task.Delay(1000);
                                if (Clear_dtc == "NOERROR")
                                {
                                    var RemoteSessionClosed = new Views.Popup.DisplayAlertPage("Dtc Cleared", "", "OK");
                                    await PopupNavigation.Instance.PushAsync(RemoteSessionClosed);
                                }
                                else
                                {
                                    var RemoteSessionClosed = new Views.Popup.DisplayAlertPage(Clear_dtc, "", "OK");
                                    await PopupNavigation.Instance.PushAsync(RemoteSessionClosed);
                                }

                                //var RemoteSessionClosed = new Views.Popup.DisplayAlertPage(elementEventHandler.ElementName, "", "OK");
                                //await PopupNavigation.Instance.PushAsync(RemoteSessionClosed);
                                //await page.DisplayAlert("Success!", "DTC Cleared", "OK");
                                ecus_list = new ObservableCollection<DtcEcusModel>();
                                dtc_list = new ObservableCollection<DtcCode>();
                                dtc_server_list = new List<DtcCode>();
                                GetDTCList();
                            }
                        });
                    });
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
            }
        });
        public ICommand EcuTabCommand => new Command(async (obj) =>
        {
            try
            {
                var selected = (DtcEcusModel)obj;
                selected.opacity = 1;
                dtc_list = new ObservableCollection<DtcCode>(selected.dtc_list);
                selected_ecu = selected.ecu_name;
                foreach (var ecu in ecus_list)
                {
                    if (selected != ecu)
                    {
                        ecu.opacity = .5;
                    }
                }

                var rxHeader = StaticMethods.ecu_info.FirstOrDefault(X => X.ecu_name == selected_ecu).rx_header;
                var txHeader = StaticMethods.ecu_info.FirstOrDefault(X => X.ecu_name == selected_ecu).tx_header;

                DependencyService.Get<Interfaces.IBlueToothDevices>().SendHeader(txHeader, rxHeader);

                //GetPidList();
            }
            catch (Exception ex)
            {
            }
        });
        public ICommand TorchCommand => new Command(async (obj) =>
        {
            try
            {

                if (torch_image.Contains("ic_torch_off"))
                {
                    torch_image = "ic_torch_on.png";
                    await Flashlight.TurnOnAsync();
                }
                else
                {
                    torch_image = "ic_torch_off.png";
                    await Flashlight.TurnOffAsync();
                }
                //await Device.InvokeOnMainThreadAsync(async () =>
                //{
                //    ecus_list = new ObservableCollection<DtcEcusModel>();
                //    dtc_list = new ObservableCollection<DtcCode>();
                //    dtc_server_list = new List<DtcCode>();
                //    await GetDTCList();
                //});
            }
            catch (Exception ex)
            {
            }
        });
        #endregion
    }
}
