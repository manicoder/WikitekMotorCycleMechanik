using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Acr.UserDialogs;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using WikitekMotorCycleMechanik.Views.VehicleDiagnostics;
using Xamarin.Essentials;
using WikitekMotorCycleMechanik.CustomControls;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class DongleListViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly Page page;
        public event EventHandler ClosebtPopup;
        public string dongle_type = string.Empty;
        public string usability = string.Empty;
        public Oem selected_oem = new Oem();
        public VehicleModelResult selected_model = new VehicleModelResult();
        public VehicleSubModel selected_sub_model = new VehicleSubModel();
        public DongleListViewModel(Page page, string dongle, VehicleModelResult selected_model, VehicleSubModel selected_submodel, Oem oem) : base(page)
        {
            apiServices = new ApiServices();
            bt_list = new ObservableCollection<BluetoothDevicesModel>();
            this.model_image = model_image;
            this.page = page;
            this.selected_oem = oem;
            this.selected_model = selected_model;
            this.selected_sub_model = selected_submodel;
            dongle_type = dongle;
            //selected_bt = new BluetoothDevicesModel();
            App.bluetooth_devices = new ObservableCollection<BluetoothDevicesModel>();
            InitializeCommands();
            GetUsability();
            msg = "Searching...";
            loader_visible = true;
            Device.InvokeOnMainThreadAsync(async () =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    //await Task.Delay(200);
                    await GetBtList();
                }
            });
        }

        #region Properties
        private byte[] _model_image;
        public byte[] model_image
        {
            get => _model_image;
            set
            {
                _model_image = value;
                OnPropertyChanged("model_image");
            }
        }

        private string _msg;
        public string msg
        {
            get => _msg;
            set
            {
                _msg = value;
                OnPropertyChanged("msg");
            }
        }

        private bool _loader_visible = false;
        public bool loader_visible
        {
            get => _loader_visible;
            set
            {
                _loader_visible = value;
                OnPropertyChanged("loader_visible");
            }
        }

        private bool _loader_View_visible = false;
        public bool loader_View_visible
        {
            get => _loader_View_visible;
            set
            {
                _loader_View_visible = value;
                OnPropertyChanged("loader_View_visible");
            }
        }

        private bool _refresh_button_enable = true;
        public bool refresh_button_enable
        {
            get => _refresh_button_enable;
            set
            {
                _refresh_button_enable = value;
                OnPropertyChanged("refresh_button_enable");
            }
        }

        private string _available_protocol;
        public string available_protocol
        {
            get => _available_protocol;
            set
            {
                _available_protocol = value;
                OnPropertyChanged("available_protocol");
            }
        }

        private bool _protocol_view_visible = false;
        public bool protocol_view_visible
        {
            get => _protocol_view_visible;
            set
            {
                _protocol_view_visible = value;
                OnPropertyChanged("protocol_view_visible");
            }
        }


        private ObservableCollection<BluetoothDevicesModel> _bt_list;
        public ObservableCollection<BluetoothDevicesModel> bt_list
        {
            get => _bt_list;
            set
            {
                _bt_list = value;
                OnPropertyChanged("bt_list");
            }
        }

        private BluetoothDevicesModel _selected_bt_device;
        public BluetoothDevicesModel selected_bt_device
        {
            get => _selected_bt_device;
            set
            {
                _selected_bt_device = value;
                if (selected_bt_device != null)
                {
                    if (!string.IsNullOrEmpty(selected_bt_device.mac_address))
                    {
                        if (page == null)
                        {
                            Device.InvokeOnMainThreadAsync(async () =>
                            {
                                await RegisterDongle();
                            });
                        }
                    }
                }
                OnPropertyChanged("selected_bt_device");
            }
        }
        #endregion

        #region Methods
        public void InitializeCommands()
        {
            ConnectCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    try
                    {
                        string message = string.Empty;
                        string dongle_filert = string.Empty;
                        DependencyService.Get<IBlueToothDevices>().CancleScanning();
                        bool is_valid_dongle = false;
                        await Task.Delay(100);
                        selected_bt_device = (BluetoothDevicesModel)obj;

                        is_valid_dongle = false;
                        if (selected_bt_device.mac_address.Replace(":", "") == App.user.dongles.FirstOrDefault().mac_id.Replace(":", ""))
                        {
                            var response = await apiServices.GetDongleStatus(Xamarin.Essentials.Preferences.Get("token", null), selected_bt_device.mac_address, App.is_update);
                            if (response == null)
                            {
                                await page.DisplayAlert("Alert", $"Update your local database", "Ok");
                                return;
                            }

                            if (response.results==null|| !response.results.Any())
                            {
                                await page.DisplayAlert("Alert", $"Update your local database", "Ok"); 
                                return;
                            }

                            if (!response.results[0].is_active)
                            {
                                await page.DisplayAlert("Alert", "you may use this device after approval from the admin, please contact to admin.", "Ok");
                                return;
                            }

                            is_valid_dongle = true;
                        }

                        if (is_valid_dongle)
                        {
                            var res = await DependencyService.Get<IBlueToothDevices>().ConnectionBT(selected_bt_device.mac_address, 250, true);
                            if (res == "connected")
                            {
                                if (StaticMethods.ecu_info.Count > 0)
                                {
                                    var rx_header = StaticMethods.ecu_info.FirstOrDefault().rx_header;
                                    var tx_header = StaticMethods.ecu_info.FirstOrDefault().tx_header;
                                    var protocol = StaticMethods.ecu_info.FirstOrDefault().protocol;
                                    bool is_padding = StaticMethods.ecu_info.FirstOrDefault().is_padding ?? false; ;
                                    var firmwareVersion = await DependencyService.Get<IBlueToothDevices>().GetFirmwareVersion(dongle_filert, tx_header, rx_header, protocol, is_padding);

                                    Application.Current.MainPage = new MasterDetailView(App.user)
                                    {
                                        Detail = new CustomNavigationPage(
                                        new Views.AppFeature.AppFeaturePage(firmwareVersion, selected_model, selected_sub_model, selected_oem,null))
                                    };

                                    //await page.Navigation.PushAsync(new Views.AppFeature.AppFeaturePage(firmwareVersion, model_image, selected_model, selected_oem));
                                }
                                else
                                {
                                    await page.DisplayAlert("Alert", "Ecu not found.", "Ok");
                                }
                            }
                        }
                        else
                        {
                            await page.DisplayAlert("Alert", "Could not connect", "Ok");
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                }
            });

            ProtocolViewCommand = new Command(async (obj) =>
            {
                protocol_view_visible = false;
            });

            ClosePopupCommand = new Command(async (obj) =>
            {
                ClosebtPopup?.Invoke("", new EventArgs());
                //StaticMethods.last_page = "CreateRSAgent";
                //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.CountyPopupPage());
            });

            RefreshCommand = new Command(async (obj) =>
            {
                try
                {
                    bt_list = new ObservableCollection<BluetoothDevicesModel>();
                    App.bluetooth_devices = new ObservableCollection<BluetoothDevicesModel>();
                    msg = "Refreshing...";
                    loader_visible = true;
                    await GetBtList();
                }
                catch (Exception ex)
                {
                }
            });
        }

        public string GetDeviceType()
        {
            string device_type = string.Empty;
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    break;
                case Device.Android:
                    device_type = "android";
                    break;
                case Device.UWP:
                    device_type = "windows";
                    break;
                default:
                    break;
            }
            return device_type;
        }

        public async Task RegisterDongle()
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                await Task.Delay(200);
                if (App.user.dongles.Count > 0)
                {
                    if (App.dongle_type == "OBDII BT Dongle")
                    {
                        var res = App.user.dongles.FirstOrDefault(x => x.device_type.Contains("24v"));
                        if (res != null)
                        {
                            await Application.Current.MainPage.DisplayAlert("Alert", "24v dongle already registered for this user.", "Ok");
                        }
                        else
                        {
                            RegisterDongles();
                        }
                    }
                    else if (App.dongle_type == "wikitek")
                    {
                        var res = App.user.dongles.FirstOrDefault(x => x.device_type.Contains("12v"));
                        if (res != null)
                        {
                            await Application.Current.MainPage.DisplayAlert("Alert", "12v dongle already registered for this user.", "Ok");
                        }
                        else
                        {
                            RegisterDongles();
                        }
                    }

                    ClosebtPopup?.Invoke("", new EventArgs());
                    //await Application.Current.MainPage.DisplayAlert("Alert","A dongle already registered for this user.","Ok");

                }
                else
                {
                    RegisterDongles();
                }
            }
        }

        public async void RegisterDongles()
        {
            try
            {
                RegisterDongleModel RDM = new RegisterDongleModel
                {
                    mac_id = selected_bt_device.mac_address,
                    device_type = this.dongle_type,
                };
                var result = await apiServices.RegisterDongle(RDM, Xamarin.Essentials.Preferences.Get("token", null));

                if (result == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
                    return;
                }

                var api_status_code = StaticMethods.http_status_code(result.status_code);

                if (result.status_code == System.Net.HttpStatusCode.Unauthorized)
                {
                    await Application.Current.MainPage.DisplayAlert("Failed!", $"User Unauthorized", "OK");
                    Preferences.Set("token", "");
                    Preferences.Set("IsUpdate", "true");
                    App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                }

                if (result.status_code == System.Net.HttpStatusCode.OK || result.status_code == System.Net.HttpStatusCode.Created)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", $"{result.error}", "OK");
                    ClosebtPopup?.Invoke("", new EventArgs());
                    App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                }
                else if (result.status_code == System.Net.HttpStatusCode.Forbidden)
                {
                    await Application.Current.MainPage.DisplayAlert("Success!", $"{result.error}\n{result.message}", "OK");
                    ClosebtPopup?.Invoke("", new EventArgs());
                    //App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                }
                else
                {
                    await page.DisplayAlert("Alert", $"{result?.message}", "Ok");
                    return;
                }
            }
            catch (Exception ex)
            {
            }
        }

        int count = 1;
        public async Task GetBtList()
        {
            try
            {
                App.bt_available = true;
                //System.Console.WriteLine("BLE CONNECTION # : Loop Start");
                count = 1;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Device.StartTimer(TimeSpan.FromMilliseconds(1000), () =>
                    {
                        if (App.selectedSegment == "CV")
                        {
                            bt_list = new ObservableCollection<BluetoothDevicesModel>(App.bluetooth_devices.Where(x => x.name == "wikitekhd").ToList());
                        }
                        else
                        {
                            bt_list = new ObservableCollection<BluetoothDevicesModel>(App.bluetooth_devices.Where(x => x.name == "wikitek").ToList());
                        }
                        
                        loader_View_visible = loader_visible = App.bt_available;
                        refresh_button_enable = !App.bt_available;

                        //System.Console.WriteLine($"BLE CONNECTION # : StartTimer ({count++})");
                        //bt_list = new ObservableCollection<BluetoothDevicesModel>();
                        if (!App.bt_available && (bt_list == null || (!bt_list.Any())))
                        {
                            //System.Console.WriteLine($"BLE CONNECTION # : BLE Not Found");
                            msg = "Not Found";
                            loader_visible = App.bt_available;
                            loader_View_visible = refresh_button_enable = !App.bt_available;
                        }

                        return App.bt_available;
                    });
                });
                await DependencyService.Get<IBlueToothDevices>().SearchBlueTooth();
            }
            catch (Exception ex)
            {
            }
        }

        public void GetUsability()
        {
            foreach (var item in App.user.subscriptions)
            {
                switch (item.package_name)
                {
                    case "BasicPack":
                        usability = item.usability;
                        break;
                    case "DIYDiagnosticsPack":
                        usability = item.usability;
                        break;
                    case "AssistedDiagnosticsPack":
                        usability = item.usability;
                        break;
                }
            }
        }
        #endregion

        #region ICommands
        public ICommand ConnectCommand { get; set; }
        public ICommand ClosePopupCommand { get; set; }
        public ICommand ProtocolViewCommand { get; set; }
        public ICommand RefreshCommand { get; set; }
        #endregion
    }
}
