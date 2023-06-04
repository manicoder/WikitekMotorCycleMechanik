using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.PopupPages;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.Views.Login;
using WikitekMotorCycleMechanik.Views.MarketPlace;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using WikitekMotorCycleMechanik.Views.Settings;
using WikitekMotorCycleMechanik.Views.VehicleDiagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class DashboadViewModel : ViewModelBase
    {
        ApiServices apiServices;
        LoginResponse user;
        string vehicle;
        string pack;
        readonly INavigation navigationService;
        public string usability = string.Empty;
        public DashboadViewModel(Page page, LoginResponse user) : base(page)
        {
            this.user = user;
            vehicle = string.Empty;
            pack = string.Empty;
            apiServices = new ApiServices();

            Device.BeginInvokeOnMainThread(async () =>
            {
                await GetHotPartList();
            });

            MessagingCenter.Subscribe<PackPopupViewModel>(this, "RemoveSelection", async (sender) =>
            {
                RemoveSelection();
            });
            MessagingCenter.Subscribe<PackPopupViewModel>(this, "NavigateToPackHelp", async (sender) =>
            {
                RemoveSelection();
                await this.page.Navigation.PushAsync(new Views.PackHelp.PackHelpPage());
            });
            MessagingCenter.Subscribe<DPPackViewModel>(this, "NavigateToSettingPage", async (sender) =>
            {
                RemoveSelection();
                await this.page.Navigation.PushAsync(new Views.Settings.SettingPage(user));
            });
            MessagingCenter.Subscribe<DPPackViewModel>(this, "NavigateToMarketplace", async (sender) =>
            {
                RemoveSelection();
                App.isFilter = true;
                await page.Navigation.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new MarketPlacePage(null)) });
            });
            MessagingCenter.Subscribe<DPPackViewModel>(this, "NavigateToPackHelp", async (sender) =>
            {
                RemoveSelection();
                await this.page.Navigation.PushAsync(new Views.PackHelp.PackHelpPage());
            });
            MessagingCenter.Subscribe<PackPopupPage>(this, "RemoveSelection", async (sender) =>
            {
                RemoveSelection();
            });
            MessagingCenter.Subscribe<DPPackPopupPage>(this, "RemoveSelection", async (sender) =>
            {
                RemoveSelection();
            });
            MessagingCenter.Subscribe<DPPackViewModel>(this, "NavigateToOBD2", async (sender) =>
            {
                RemoveSelection();
                await page.Navigation.PushAsync(new MasterDetailView(user) { Detail = new NavigationPage(new VehicleDiagnosticsPage(user)) });
            });
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

        private List<MarketplaceHotproduct> _HotPartList;
        public List<MarketplaceHotproduct> HotPartList
        {
            get => _HotPartList;
            set
            {
                _HotPartList = value;
                OnPropertyChanged("HotPartList");
            }
        }

        private List<MarketplaceBanner> _bannerList;
        public List<MarketplaceBanner> bannerList
        {
            get => _bannerList;
            set
            {
                _bannerList = value;
                OnPropertyChanged("bannerList");
            }
        }

        private List<Attachment> _imageList;
        public List<Attachment> imageList
        {
            get => _imageList;
            set
            {
                _imageList = value;
                OnPropertyChanged("imageList");
            }
        }

        private Color _DPColor = Color.White;
        public Color DPColor
        {
            get => _DPColor;
            set
            {
                _DPColor = value;
                OnPropertyChanged("DPColor");
            }
        }

        private Color _ADPColor = Color.White;
        public Color ADPColor
        {
            get => _ADPColor;
            set
            {
                _ADPColor = value;
                OnPropertyChanged("ADPColor");
            }
        }

        private Color _OPColor = Color.White;
        public Color OPColor
        {
            get => _OPColor;
            set
            {
                _OPColor = value;
                OnPropertyChanged("OPColor");
            }
        }

        private Color _bikeColor = Color.White;
        public Color bikeColor
        {
            get => _bikeColor;
            set
            {
                _bikeColor = value;
                OnPropertyChanged("bikeColor");
            }
        }

        private Color _rikshawColor = Color.White;
        public Color rikshawColor
        {
            get => _rikshawColor;
            set
            {
                _rikshawColor = value;
                OnPropertyChanged("rikshawColor");
            }
        }

        private Color _carColor = Color.White;
        public Color carColor
        {
            get => _carColor;
            set
            {
                _carColor = value;
                OnPropertyChanged("carColor");
            }
        }

        private Color _truckColor = Color.White;
        public Color truckColor
        {
            get => _truckColor;
            set
            {
                _truckColor = value;
                OnPropertyChanged("truckColor");
            }
        }

        #endregion

        #region Methods
        public async Task GetHotPartList()
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                await Task.Delay(100);
                try
                {
                    //var response = await apiServices.GetHotPartList();
                    //bannerList = response.results.FirstOrDefault().marketplace_banner;
                    //HotPartList = response.results.FirstOrDefault().marketplace_hotproducts;

                    Task.Run(async () =>
                    {
                        try
                        {
                            var response = await apiServices.GetHotPartList();
                            bannerList = response.results.FirstOrDefault().marketplace_banner;
                            var hotList = response.results.FirstOrDefault().marketplace_hotproducts;
                            HotPartList = hotList.OrderBy(x => x.priority).ToList();
                            //imageList = new List<Attachment>();
                            //foreach (var item in HotPartList)
                            //{
                            //    if (item.part_no.documents.Count > 0)
                            //    {
                            //        item.part_no.documents.FirstOrDefault().attachment.parts = item.part_no.id;
                            //        imageList.Add(item.part_no.documents.FirstOrDefault().attachment);
                            //    }
                            //}
                        }
                        catch (Exception ex)
                        {

                        }
                    });
                }
                catch (Exception ex)
                {

                }

            }
        }

        public async Task SubscriptionValidation()
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                await Task.Delay(100);
                try
                {
                    if (App.user.subscriptions == null || !App.user.subscriptions.Any())
                    {
                        var result = await page.DisplayAlert("Alert", "you have no subscription enabled, please visit settings page and subscribe for DIY Diagnostics Pack or Assisted Diagnostics Pack .", null, "OK");
                        if (!result)
                        {
                            await page.Navigation.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new SettingPage(App.user)) });
                            //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.DPPackPopupPage(vehicle));
                            return;
                        }
                    }
                    else if (App.user.dongles == null || !App.user.dongles.Any())
                    {
                        var result = await page.DisplayAlert("Alert", "you don't have any registered dongle, please visit settings page and purchase a dongle.", null, "OK");
                        if (!result)
                        {
                            await page.Navigation.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new SettingPage(App.user)) });
                            //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.DPPackPopupPage(vehicle));
                            return;
                        }
                    }
                    else
                    {
                        bool is12vPresent = false;
                        bool is24vPresent = false;
                        App.user.dongles.ForEach(d =>
                        {
                            if (d.device_type == "wkosb001" || d.device_type.Contains("12v"))
                            {
                                is12vPresent = true;
                            }
                            else if (d.device_type == "wkosb002" || d.device_type.Contains("24v"))
                            {
                                is24vPresent = true;
                            }
                        });

                        if (App.selectedSegment == "CV")
                        {
                            if (!is24vPresent)
                            {
                                var result = await page.DisplayAlert("Alert", "you don't have dongle registration for Commercial Vehicle, please visit settings page and purchase a dongle.", null, "OK");
                                if (!result)
                                {
                                    await page.Navigation.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new SettingPage(App.user)) });
                                    //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.DPPackPopupPage(vehicle));
                                    return;
                                }
                            }
                        }
                        else
                        {
                            if (!is12vPresent)
                            {
                                var result = await page.DisplayAlert("Alert", "you don't have 12V dongle registration, please visit settings page and purchase a dongle.", null, "OK");
                                if (!result)
                                {
                                    await page.Navigation.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new SettingPage(App.user)) });
                                    //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.DPPackPopupPage(vehicle));
                                    return;
                                }
                            }
                        }
                        if (App.user.subscriptions != null && App.user.subscriptions.Count > 0)
                        {
                            //var subcribe = App.user.subscriptions.FirstOrDefault(x => x.segments.segment_name == App.user.segment);
                            var subscribe1 = App.user.subscriptions.FirstOrDefault(x => x.segments.segment_name == App.selectedSegment);
                            var isObd2 = App.user.subscriptions.FirstOrDefault(x => x.segments.segment_name == "OBD2");
                            if (subscribe1 == null)
                            {
                                if (isObd2 != null)
                                {
                                    App.selectedSegment = "OBD2";
                                }
                                await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.DPPackPopupPage(vehicle));
                                return;
                            }
                        }
                        else
                        {
                            await page.DisplayAlert("Can't Login", "Problem found with subscription. Please contact to admin", "OK");
                            return;
                        }

                        //Will Make Change
                        subscribe_pack = App.user.subscriptions.FirstOrDefault();

                        if (subscribe_pack != null)
                        {
                            selected_segment = new VehicleSegment
                            {
                                id = (int)subscribe_pack.segments?.id,
                                segment_name = (string)subscribe_pack.segments.segment_name,
                            };
                            usability = subscribe_pack.usability;
                            App.getModel.segment = selected_segment.id;
                            await page.Navigation.PushAsync(new MasterDetailView(user) { Detail = new NavigationPage(new VehicleDiagnosticsPage(user)) });
                            //return;
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
        }

        public async void submitMethod()
        {
            if (string.IsNullOrEmpty(pack) && string.IsNullOrEmpty(vehicle))
            {
                await page.DisplayAlert("Alert", "Please select Pack and vehicle", "Ok");
            }
            else if (string.IsNullOrEmpty(pack))
            {
                await page.DisplayAlert("Alert", "Please select Pack", "Ok");
            }
            else if (string.IsNullOrEmpty(vehicle))
            {
                await page.DisplayAlert("Alert", "Please select vehicle", "Ok");
            }
            else
            {
                if (pack == "DP")
                {
                    await SubscriptionValidation();
                }
                else
                {
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.PackPopupPage(pack));
                }
            }
        }

        public void RemoveSelection()
        {
            DPColor = Color.White;
            ADPColor = Color.White;
            OPColor = Color.White;
            bikeColor = Color.White;
            carColor = Color.White;
            rikshawColor = Color.White;
            truckColor = Color.White;
            pack = string.Empty;
            vehicle = string.Empty;
        }
        #endregion

        #region Commands
        public ICommand OperationsCommand => new Command(async (obj) =>
        {
            try
            {
                await this.page.Navigation.PushAsync(new Views.Jobcard.CreateJobPage());
            }
            catch (Exception ex)
            {
            }
        });

        public ICommand DiagnosticCommand => new Command(async (obj) =>
        {
            try
            {
                await page.Navigation.PushAsync(new MasterDetailView(user) { Detail = new NavigationPage(new VehicleDiagnosticsPage(user)) });
            }
            catch (Exception ex)
            {
            }
        });

        public ICommand SettingCommand => new Command(async (obj) =>
        {
            try
            {
                await this.page.Navigation.PushAsync(new Views.Settings.SettingPage(user));
            }
            catch (Exception ex)
            {
            }
        });

        public ICommand VehicleManagementCommand => new Command(async (obj) =>
        {
            try
            {
                await this.page.Navigation.PushAsync(new Views.VechicleManagement.VehicleManagement());
            }
            catch (Exception ex)
            {
            }
        });

        public ICommand TechnicianManagementCommand => new Command(async (obj) =>
        {
            try
            {
                await this.page.Navigation.PushAsync(new Views.TechnicianManagement.TechnicianManagement());
            }
            catch (Exception ex)
            {
            }
        });


        public ICommand SwitchUserCommand => new Command(async (obj) =>
        {
            try
            {
                //await this.page.Navigation.PushAsync(new Views.UserType.UserTypePage());

                await this.page.Navigation.PushAsync(new Views.VehicleService.VehicleServicePage());
            }
            catch (Exception ex)
            {
            }
        });

        public ICommand VehicleCommand => new Command(async (obj) =>
        {
            try
            {
                vehicle = obj as string;
                switch (vehicle)
                {
                    case "bike":
                        bikeColor = Color.LightSteelBlue;
                        carColor = Color.White;
                        rikshawColor = Color.White;
                        truckColor = Color.White;
                        App.selectedSegment = "2W";
                        break;
                    case "car":
                        carColor = Color.LightSteelBlue;
                        rikshawColor = Color.White;
                        bikeColor = Color.White;
                        truckColor = Color.White;
                        App.selectedSegment = "PV";
                        break;
                    case "rikshaw":
                        rikshawColor = Color.LightSteelBlue;
                        carColor = Color.White;
                        bikeColor = Color.White;
                        truckColor = Color.White;
                        App.selectedSegment = "3W";
                        break;
                    case "truck":
                        truckColor = Color.LightSteelBlue;
                        carColor = Color.White;
                        bikeColor = Color.White;
                        rikshawColor = Color.White;
                        App.selectedSegment = "CV";
                        break;
                }

                if (!string.IsNullOrEmpty(vehicle) && !string.IsNullOrEmpty(pack))
                {
                    submitMethod();
                }
            }
            catch (Exception ex)
            {

            }
        });

        public ICommand PackCommand => new Command(async (obj) =>
        {
            try
            {
                
                pack = obj as string;
                switch (pack)
                {
                    case "DP":
                        DPColor = Color.LightSteelBlue;
                        ADPColor = Color.White;
                        OPColor = Color.White;
                        App.selectedPack = "DIYDP";
                        break;
                    case "ADP":
                        ADPColor = Color.LightSteelBlue;
                        DPColor = Color.White;
                        OPColor = Color.White;
                        App.selectedPack = "ADP";
                        break;
                    case "OP":
                        OPColor = Color.LightSteelBlue;
                        ADPColor = Color.White;
                        DPColor = Color.White;
                        App.selectedPack = "OP";
                        break;
                }
                await this.page.Navigation.PushAsync(new Views.VehicleService.VehicleServicePage());
                if (!string.IsNullOrEmpty(vehicle) && !string.IsNullOrEmpty(pack))
                {
                   // submitMethod();
                }
            }
            catch (Exception ex)
            {

            }
        });

        public ICommand HotPartSelectCommand => new Command(async (obj) =>
        {
            try
            {
                var selectedItem = obj as Attachment;

                if (selectedItem != null)
                {
                    PartsList selected_hot_part = new PartsList();
                    foreach (var item in HotPartList)
                    {
                        if (item.part_no.id == selectedItem.parts)
                        {
                            selected_hot_part = item.part_no;
                        }
                    }
                    if (selected_hot_part.id != null)
                    {
                        await page.Navigation.PushAsync(new MasterDetailView(user) { Detail = new NavigationPage(new MarketPlaceDetailPage(selected_hot_part)) });
                    }
                }
            }
            catch (Exception ex)
            {

            }
        });

        public ICommand BannerImageSelectCommand => new Command(async (obj) =>
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                await Task.Delay(100);
                var selectedBanner = obj as MarketplaceBanner;

                if (selectedBanner.action == "Part_Number")
                {
                    App.selectedBannerId = selectedBanner.part_no;
                    await page.Navigation.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new MarketPlacePage(null)) });
                }
            }

        });
        #endregion

    }
}
