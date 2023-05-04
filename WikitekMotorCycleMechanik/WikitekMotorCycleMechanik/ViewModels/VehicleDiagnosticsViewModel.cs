using Acr.UserDialogs;
using FFImageLoading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using WikitekMotorCycleMechanik.Views.Login;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using WikitekMotorCycleMechanik.Views.Settings;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class VehicleDiagnosticsViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        readonly Page page;
        readonly IDeviceMacAddress device_mac_id;
        LoginResponse user_data;
        public string usability = string.Empty;
        public string mac_id = string.Empty;
        List<int> oem_ids_list;

        public VehicleDiagnosticsViewModel(Page page, LoginResponse user_data) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                this.user_data = user_data;
                this.page = page;
                this.navigationService = page.Navigation;
                this.device_mac_id = DependencyService.Get<IDeviceMacAddress>();
                empty_view_detail = new ErrorModel();
                oem_list = new ObservableCollection<OemResult>();
                SubscriptionValidation();

                UserValidation();
                InitializeCommands();

                
                    Device.InvokeOnMainThreadAsync(async () =>
                    {
                        oem_list = new ObservableCollection<OemResult>();
                            oem_ids_list = new List<int>();
                            GetAllOemsList();
                    });    
            }
            catch (Exception ex)
            {
            }
        }

       

        #region Properties
        private Subscription _subscribe_pack;
        public Subscription subscribe_pack
        {
            get => _subscribe_pack;
            set
            {
                _subscribe_pack = value;
                OnPropertyChanged("subscribe_pack");
            }
        }

        private ObservableCollection<VehicleSegment> _segment_list;
        public ObservableCollection<VehicleSegment> segment_list
        {
            get => _segment_list;
            set
            {
                _segment_list = value;
                OnPropertyChanged("segment_list");
            }
        }

        private VehicleSegment _selected_segment;
        public VehicleSegment selected_segment
        {
            get => _selected_segment;
            set
            {
                _selected_segment = value;
                OnPropertyChanged("selected_segment");
            }
        }

        private ObservableCollection<OemResult> _oem_list;
        public ObservableCollection<OemResult> oem_list
        {
            get => _oem_list;
            set
            {
                _oem_list = value;
                OnPropertyChanged("oem_list");
            }
        }

        private OemResult _selected_oem;
        public OemResult selected_oem
        {
            get => _selected_oem;
            set
            {
                _selected_oem = value;
                OnPropertyChanged("selected_oem");
            }
        }

        private ErrorModel _empty_view_detail;
        public ErrorModel empty_view_detail
        {
            get => _empty_view_detail;
            set
            {
                _empty_view_detail = value;
                OnPropertyChanged("empty_view_detail");
            }
        }

        private string _change_gui_button = "ic_grid_list.png";
        public string change_gui_button
        {
            get => _change_gui_button;
            set
            {
                _change_gui_button = value;
                OnPropertyChanged("change_gui_button");
            }
        }

        private bool _bulleted_list_visible = true;
        public bool bulleted_list_visible
        {
            get => _bulleted_list_visible;
            set
            {
                _bulleted_list_visible = value;
                OnPropertyChanged("bulleted_list_visible");
            }
        }

        private bool _grid_list_visible = false;
        public bool grid_list_visible
        {
            get => _grid_list_visible;
            set
            {
                _grid_list_visible = value;
                OnPropertyChanged("grid_list_visible");
            }
        }
        #endregion


        #region Methods
        public async void SubscriptionValidation()
        {
            try
            {


                if (App.user.subscriptions == null || !App.user.subscriptions.Any())
                {
                    var result = await page.DisplayAlert("Alert", "you have no subscription enabled, please visit settings page and subscribe for DIY Diagnostics Pack or Assisted Diagnostics Pack .", null, "OK");
                    if (!result)
                    {
                        await navigationService.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new SettingPage(App.user)) });
                        return;
                    }
                }
                else if (App.user.dongles == null || !App.user.dongles.Any())
                {
                    var result = await page.DisplayAlert("Alert", "you don't have any registered dongle, please visit settings page and purchase a dongle.", null, "OK");
                    if (!result)
                    {
                        await navigationService.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new SettingPage(App.user)) });
                        return;
                    }
                }
                else
                {
                    if (App.user.subscriptions != null && App.user.subscriptions.Count > 0)
                    {
                        var subcribe = App.user.subscriptions.FirstOrDefault(x => x.segments.segment_name == App.selectedSegment/*App.user.segment*/);
                        var isObd2 = App.user.subscriptions.FirstOrDefault(x => x.segments.segment_name == "OBD2");

                        if (subcribe == null && isObd2 == null)
                        {
                            await page.DisplayAlert("Can't Login", "Problem found with subscription. Please contact to admin", "OK");
                            return;
                        }
                    }
                    else
                    {
                        await page.DisplayAlert("Can't Login", "Problem found with subscription. Please contact to admin", "OK");
                        return;
                    }

                    //Will Make Change
                    subscribe_pack = App.user.subscriptions.FirstOrDefault(x => x.segments.segment_name == App.selectedSegment);

                    if (subscribe_pack != null)
                    {
                        selected_segment = new VehicleSegment
                        {
                            id = (int)subscribe_pack.segments?.id,
                            segment_name = (string)subscribe_pack.segments.segment_name,
                        };
                        usability = subscribe_pack.usability;
                        App.getModel.segment = selected_segment.id;
                        return;
                    }
                    else
                    {
                        await page.DisplayAlert("Can't Login", "Your subscription is not valid for \"2W Segment\"", "OK");
                        Preferences.Set("user_name", "");
                        Preferences.Set("password", "");
                        Preferences.Set("token", "");
                        Preferences.Set("IsUpdate", "true");
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                        return;
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        public async void UserValidation()
        {
            await GetMacId();
            //mac_id = "10df3f643ce3e4af";
            if (string.IsNullOrEmpty(mac_id))
            {
                await page.DisplayAlert("Error", "Your device id not found.", "Ok");
                return;
            }
            if (usability == "SDMD" || usability == "SDSD")
            {
                if (mac_id != App.user.mac_id)
                {
                    await page.DisplayAlert("", "This device not registered for this dongle.", "Ok");
                    return;
                }
            }
            else if (usability == "SDSD" || usability == "MDSD")
            {

            }
        }

        public async Task GetMacId()
        {
            mac_id = await device_mac_id.GetDeviceUniqueId();

            if (App.user.email == "universaluser@wikitek.in")
            {
                mac_id = "10df3f643ce3e4af";
            }
            else if (App.user.email == "sofi6878@gmail.com")
            {
                mac_id = "e859a27f622bdff2";
            }

            //mac_id = "af531103492a2a59";
            //WifiManager wifi = (WifiManager)Android.App.Application.Context.GetSystemService(Android.Content.Context.WifiService);
            //if (wifi.IsWifiEnabled != true)
            //{
            //    wifi.SetWifiEnabled(true);
            //    await Task.Delay(100);
            //    mac_id = await device_mac_id.GetMacAddress();
            //    wifi.SetWifiEnabled(false);
            //}
            //else
            //{
            //    mac_id = await device_mac_id.GetMacAddress();
            //}
        }

        public void InitializeCommands()
        {
            try
            {
                //MessagingCenter.Subscribe<SegmentViewModel, VehicleSegment>(this, "oems_list_page", async (sender, arg) =>
                //{
                //    //country = arg.name;
                //    selected_segment = arg;
                //    GetAllOemsList();
                //});

                //ItemSelectionCommand = new Command(async (obj) =>
                //{
                //    try
                //    {

                //        selected_oem = (OemResult)obj;
                //        await Device.InvokeOnMainThreadAsync(async () =>
                //        {
                //            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                //            {
                //                //await Task.Delay(200);
                //                await page.Navigation.PushAsync(new Views.VehicleModels.VehicleModelsPage(selected_oem.oem, selected_segment.id, oem_ids_list));
                //                //selected_oem = new OemResult();
                //            }
                //        });
                //    }
                //    catch (Exception ex)
                //    {
                //    }
                //});

                //FilterOemsCommand = new Command(async (obj) =>
                //{
                //    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                //    {
                //        try
                //        {
                //            if (oem_list.Count > 0)
                //            {
                //                //await Task.Delay(200);
                //                await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.SegmentPopupPage("oems_list_page"));
                //            }
                //            else
                //            {
                //                var result = await page.DisplayAlert("Alert", "you have no subscription enabled, please visit the settings step and subscribe for a package.", null, "OK");
                //                if (!result)
                //                {
                //                    await navigationService.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new SettingPage(App.user)) });
                //                }
                //            }
                //        }
                //        catch (Exception ex)
                //        {
                //        }
                //    }
                //});

                //ChangeGuiCommand = new Command(async (obj) =>
                //{
                //    try
                //    {
                //        if(bulleted_list_visible)
                //        {
                //            bulleted_list_visible = false;
                //            grid_list_visible = true;
                //            change_gui_button = "ic_bulleted_list.png";
                //            Preferences.Set("list_gui", "tabel_list");
                //        }
                //        else
                //        {

                //            bulleted_list_visible = true;
                //            grid_list_visible = false;
                //            change_gui_button = "ic_grid_list.png";
                //            Preferences.Set("list_gui", "bulleted_list");
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //    }
                //});
            }
            catch (Exception ex)
            {
            }
        }

        public async void GetAllOemsList()
        {
            try
            {
                oem_list = new ObservableCollection<OemResult>();
                if (subscribe_pack.oem_specific)
                {
                    foreach (var item in subscribe_pack.oem)
                    {

                        oem_list.Add(
                            new OemResult
                            {
                                oem = new Oem { id = item.id, oem_file = $"https://wikitek.io{item.oem_file}", name = item.name, oem_file_local = item.oem_file_local },
                                segment_name = new Segment { id = subscribe_pack.segments.id, segment_name = subscribe_pack.segments.segment_name, }
                                //oem. = item.id
                            });
                        oem_ids_list.Add(item.id);
                    }
                    App.getModel.oems = oem_ids_list;
                    App.getModel.segment = selected_segment.id;
                }
                else
                {
                    var response = await apiServices.GetAllOems(Xamarin.Essentials.Preferences.Get("token", null), selected_segment.id, true);

                    if (response == null)
                    {
                        empty_view_detail.is_runing = empty_view_detail.is_visible = false;
                        empty_view_detail.error_message = "Somthing went wrong...";
                        await page.DisplayAlert("Alert", "Service not working...\n\nPlease update your local data by click \"Sync Data\" button inside menu item...", "Ok");
                        MessagingCenter.Send<VehicleDiagnosticsViewModel>(this, "OpenMasterDetailSwip");
                        return;
                    }


                    if (response.detail.Contains("Please check your internet connection."))
                    {
                        empty_view_detail.is_runing = empty_view_detail.is_visible = false;
                        empty_view_detail.error_message = "Please check your internet connection...";
                        return;
                    }

                    if (response.status_code == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await page.DisplayAlert("Alert", "Token Expired. \nPlease login again", "Ok");
                        Preferences.Set("token", "");
                        Preferences.Set("IsUpdate", "true");
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                        return;
                    }


                    if (response.results == null)
                    {
                        empty_view_detail.is_runing = empty_view_detail.is_visible = false;
                        empty_view_detail.error_message = "Somthing went wrong...";
                        await page.DisplayAlert("Alert", "Service not working...\n\nPlease update your local data by click \"Sync Data\" button inside menu item...", "Ok");
                        MessagingCenter.Send<VehicleDiagnosticsViewModel>(this, "OpenMasterDetailSwip");
                        return;
                    }

                    if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                    {
                        if (response.results != null && response.results.Count > 0)
                        {
                            foreach (var item in response.results.ToList())
                            {
                                oem_list.Add(item);
                                oem_ids_list.Add(item.oem.id);
                            }
                            App.getModel.oems = oem_ids_list;
                            App.getModel.segment = selected_segment.id;

                            
                        }
                        else
                        {
                            empty_view_detail.is_runing = empty_view_detail.is_visible = false;
                            empty_view_detail.error_message = "Oem not found";
                        }
                    }
                    else
                    {
                        empty_view_detail.is_runing = empty_view_detail.is_visible = false;
                        empty_view_detail.error_message = response.detail;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        #region ICommands
        //public ICommand FilterOemsCommand { get; set; }
        public ICommand ItemSelectionCommand => new Command(async (obj) =>
        {
            try
            {

                selected_oem = (OemResult)obj;
                selected_oem = (OemResult)obj;
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);

                    GetModel getModel = new GetModel
                    {
                        segment = selected_segment.id,
                        oems = oem_ids_list
                    };
                    App.getModel = getModel;

                    var response = await apiServices.GetModelByOem(Xamarin.Essentials.Preferences.Get("token", null), getModel, App.is_update);

                    if (response == null)
                    {
                        empty_view_detail.error_message = "Model not found...\n\nPlease update your local data by click \"Sync Data\" button inside menu item...";
                        await page.DisplayAlert("Alert", empty_view_detail.error_message, "OK");
                        MessagingCenter.Send<VehicleDiagnosticsViewModel>(this, "OpenMasterDetailSwip");
                        return;
                    }

                    if (!string.IsNullOrEmpty(response.detail))
                    {
                        empty_view_detail.error_message = "Model not found...\n\nPlease update your local data by click \"Sync Data\" button inside menu item...";
                        await page.DisplayAlert("Alert", empty_view_detail.error_message, "OK");
                        MessagingCenter.Send<VehicleDiagnosticsViewModel>(this, "OpenMasterDetailSwip");
                        return;
                    }

                    if (response.data == null || (!response.data.Any()))
                    {
                        empty_view_detail.error_message = "Model not found...\n\nPlease update your local data by click \"Sync Data\" button inside menu item...";
                        await page.DisplayAlert("Alert", empty_view_detail.error_message, "OK");
                        MessagingCenter.Send<VehicleDiagnosticsViewModel>(this, "OpenMasterDetailSwip");
                        return;
                    }

                    await page.Navigation.PushAsync(new Views.VehicleModels.VehicleModelsPage(user_data, selected_oem.oem, new ObservableCollection<VehicleModelResult>(response.data.ToList())));
                    //selected_oem = new OemResult();
                }
            }
            catch (Exception ex)
            {
            }
        });
        public ICommand ChangeGuiCommand => new Command(async (obj) =>
        {
            try
            {
                if (bulleted_list_visible)
                {
                    bulleted_list_visible = false;
                    grid_list_visible = true;
                    change_gui_button = "ic_bulleted_list.png";
                    Preferences.Set("list_gui", "tabel_list");
                }
                else
                {

                    bulleted_list_visible = true;
                    grid_list_visible = false;
                    change_gui_button = "ic_grid_list.png";
                    Preferences.Set("list_gui", "bulleted_list");
                }
            }
            catch (Exception ex)
            {
            }
        });
        #endregion
    }

    public class GetModel
    {
        public int segment { get; set; }
        public List<int> oems { get; set; }
    }
}
