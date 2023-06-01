using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
//using WikitekMotorCycleMechanik.Views.JobCardPages;
using WikitekMotorCycleMechanik.Views.Login;
//using WikitekMotorCycleMechanik.Views.User;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WikitekMotorCycleMechanik.View.PopupPages;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.Views.Settings;
using Xamarin.Essentials;
using WikitekMotorCycleMechanik.Views.AppFeature;
using WikitekMotorCycleMechanik.Views.MarketPlace;
using WikitekMotorCycleMechanik.Views.Dashboad;

namespace WikitekMotorCycleMechanik.Views.MasterDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailView : MasterDetailPage
    {
        MasterViewModel viewModel;
        ApiServices apiServices;
        public MasterDetailView(LoginResponse user)
        {
            try
            {
                InitializeComponent();
                apiServices = new ApiServices();
                BindingContext = viewModel = new MasterViewModel(0, user);
                //Detail = new NavigationPage(new MyJobCardPage());
                IsPresented = false;

                MessagingCenter.Subscribe<VehicleModelsViewModel>(this, "OpenMasterDetailSwip", (sender) =>
                {
                    this.IsPresented = true;
                });

                MessagingCenter.Subscribe<VehicleDiagnosticsViewModel>(this, "OpenMasterDetailSwip", (sender) =>
                {
                    this.IsPresented = true;
                });

                MessagingCenter.Subscribe<AppFeaturePage>(this, "StartMasterDetailSwip", (sender) =>
                {
                    this.IsGestureEnabled = true;
                });

                MessagingCenter.Subscribe<AppFeaturePage>(this, "StopMasterDetailSwip", (sender) =>
                {
                    this.IsGestureEnabled = false;
                });

                if (user.role == "mobitekMechanik" || user.role == "rsangleMechanik")
                {

                }
            }
            catch (Exception ex)
            {
            }
        }

        public void GetUserCUrrLocation()
        {
            Device.StartTimer(TimeSpan.FromMinutes(1), () =>
            {
                Device.BeginInvokeOnMainThread(async() =>
                {
                    double latitude = 0;
                    double longitude = 0;
                    var request = new GeolocationRequest(GeolocationAccuracy.Best);
                    var location = await Geolocation.GetLocationAsync(request);

                    if (location != null)
                    {
                        if (location.IsFromMockProvider)
                        {
                            //Put a message if detect a mock location.
                        }
                        else
                        {
                            latitude = location.Latitude;
                            longitude = location.Longitude;
                        }
                    }
                });
                return true;
            });
        }


        private void menu_clicked(object sender, EventArgs e)
        {

        }

        private async void listView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);
                    var item = e.Item as MasterModel;
                    if (item != null)
                    {
                        if (item.Title == "Logout")
                        {
                            //Preferences.Set("LoginResponse", "");
                            //Preferences.Set("user_name", "");
                            //Preferences.Set("password", "");
                            Preferences.Set("token", "");
                            Preferences.Set("IsUpdate", "true");
                            App.Current.MainPage = new NavigationPage(new LoginPage());
                        }
                        else if (item.Title == "Select Language")
                        {
                            Detail = new NavigationPage(new LanguagePopupPage());
                            IsPresented = false;
                        }
                        else if (item.Title == "Dashboard")
                        {
                            Application.Current.MainPage = new MasterDetailView(App.user) { Detail = new NavigationPage(new DashboadPage(App.user)) };
                            IsPresented = false;
                        }
                        else if (item.Title == "Sync Data")
                        {
                            if (App.user.subscriptions.FirstOrDefault() == null)
                            {
                                var result = await DisplayAlert("Alert", "you have no subscription enabled, please visit the settings step and subscribe for a package.", null, "OK");
                                if (!result)
                                {
                                    IsPresented = false;
                                    App.Current.MainPage = new NavigationPage(new MasterDetailView(App.user) { Detail = new NavigationPage(new SettingPage(App.user)) });
                                }
                            }
                            else if (App.user.dongles.FirstOrDefault() == null)
                            {

                                var result = await DisplayAlert("Alert", "you have no registered dongle, please visit the settings step and register a dongle.", null, "OK");
                                if (!result)
                                {
                                    IsPresented = false;
                                    App.Current.MainPage = new NavigationPage(new MasterDetailView(App.user) { Detail = new NavigationPage(new SettingPage(App.user)) });
                                    //await this.Navigation.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new SettingPage(App.user)) });
                                }
                            }
                            else
                            {
                                if (NetworkConnection())
                                {
                                    App.is_update = true;
                                    IsPresented = false;
                                    await UpdateLocalData();
                                    await DisplayAlert("", "Local data update successfully", "Ok");
                                    Preferences.Set("token", "");

                                    //var oems = await apiServices.GetAllOems(Preferences.Get("token", null), App.getModel.segment, App.is_update);
                                    //if (oems.status_code != System.Net.HttpStatusCode.Unauthorized)
                                    //{
                                    //    //await apiServices.GetModelByOem(Preferences.Get("token", null), App.getModel, App.is_update);
                                    //    //await apiServices.GetPid(Preferences.Get("token", null), 0, App.is_update);
                                    //    //await apiServices.GetDtc(Preferences.Get("token", null), 0, App.is_update);
                                    //    //await apiServices.GetDongleStatus(Preferences.Get("token", null), App.user.dongles.FirstOrDefault().mac_id, App.is_update);
                                    //    //await apiServices.Countries("wikitekMechanik", App.is_update);
                                    //    //App.Current.MainPage = new NavigationPage(new LoginPage());
                                    //    await UpdateLocalData();
                                    //}
                                    //else
                                    //{
                                    //    await DisplayAlert("Unauthorized", "Your access token has been expired please login again and update local data", "OK");
                                    //}
                                    //App.Current.MainPage = new NavigationPage(new LoginPage());
                                    //IsPresented = false;
                                    //return;
                                }
                            }
                        }
                        else if (item.Title == "Marketplace")
                        {
                            Detail = new NavigationPage(new MarketPlacePage(null));
                            IsPresented = false;
                        }
                        else if (item.Title == "My Orders")
                        {
                            Detail = new NavigationPage(new MyOrdersTabbedPage());
                            IsPresented = false;
                        }
                        else
                        {
                            Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType, item.args));
                            listView.SelectedItem = null;
                            IsPresented = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        List<int> oem_ids_list;
        public async Task UpdateLocalData()
        {
            try
            {
                oem_ids_list = new List<int>();
                var subscribe_pack = App.user.subscriptions.FirstOrDefault(x => x.segments.segment_name != "OBD2");

                if (subscribe_pack == null)
                {
                    subscribe_pack = App.user.subscriptions.FirstOrDefault(x => x.segments.segment_name == "OBD2");
                }
                    
                //if (subscribe_pack == null)
                //{
                //    subscribe_pack = App.user.subscriptions.FirstOrDefault(x => x.package_name == "DIYDiagnosticsPack" && x.segments.segment_name == "2W");
                //}
                //if (subscribe_pack == null)
                //{
                //    subscribe_pack = App.user.subscriptions.FirstOrDefault(x => x.package_name == "BasicPack" && x.segments.segment_name == "2W");
                //}


                if (subscribe_pack.oem_specific)
                {
                    foreach (var item in subscribe_pack.oem)
                    {

                        oem_ids_list.Add(item.id);
                    }
                    App.getModel.oems = oem_ids_list;
                    await apiServices.GetModelByOem(Preferences.Get("token", null), App.getModel, App.is_update);
                    await apiServices.GetPid(Preferences.Get("token", null), 0, App.is_update);
                    await apiServices.GetDtc(Preferences.Get("token", null), 0, App.is_update);
                    await apiServices.GetDongleStatus(Preferences.Get("token", null), App.user.dongles.FirstOrDefault().mac_id, App.is_update);
                    //await apiServices.Countries("wikitekMechanik", App.is_update);
                    App.Current.MainPage = new NavigationPage(new LoginPage());
                    Preferences.Set("WeeekUpdate", "Updated");
                }
                else
                {
                    var response = await apiServices.GetAllOems(Xamarin.Essentials.Preferences.Get("token", null), App.getModel.segment, App.is_update);

                    if (response.detail.Contains("Please check your internet connection."))
                    {
                        await DisplayAlert("Alert", "Please check your internet connection.", "Ok");
                        return;
                    }

                    if (response.status_code == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await DisplayAlert("Alert", "Token Expired. \nPlease login again", "Ok");
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                        return;
                    }

                    if (response == null)
                    {
                        await DisplayAlert("Alert", "Service not working...", "Ok");
                        return;
                    }

                    if (response.results == null)
                    {
                        await DisplayAlert("Alert", "Service not working...", "Ok");
                        return;
                    }

                    if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                    {
                        if (response.results != null && response.results.Count > 0)
                        {
                            foreach (var item in response.results.ToList())
                            {
                                oem_ids_list.Add(item.oem.id);
                            }
                            App.getModel.oems = oem_ids_list;
                            await apiServices.GetModelByOem(Preferences.Get("token", null), App.getModel, App.is_update);
                            await apiServices.GetPid(Preferences.Get("token", null), 0, App.is_update);
                            await apiServices.GetDtc(Preferences.Get("token", null), 0, App.is_update);
                            await apiServices.GetDongleStatus(Preferences.Get("token", null), App.user.dongles.FirstOrDefault().mac_id, App.is_update);
                            //await apiServices.Countries("wikitekMechanik", App.is_update);
                        }
                        else
                        {
                            await DisplayAlert("Alert", "Oems not found", "Ok");
                            await apiServices.GetPid(Preferences.Get("token", null), 0, App.is_update);
                            await apiServices.GetDtc(Preferences.Get("token", null), 0, App.is_update);
                            await apiServices.GetDongleStatus(Preferences.Get("token", null), App.user.dongles.FirstOrDefault().mac_id, App.is_update);
                            //await apiServices.Countries("wikitekMechanik", App.is_update);
                        }
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                    else
                    {
                        await DisplayAlert(response.status_code.ToString(), "Service not working...", "Ok");
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Ok");
            }
        }

        public bool NetworkConnection()
        {
            bool is_network = false;
            var current = Connectivity.NetworkAccess;
            var profiles = Connectivity.ConnectionProfiles;
            if (current == NetworkAccess.Internet)
            {
                is_network = true;
            }

            return is_network;
        }

        private void profile_Tapped(object sender, EventArgs e)
        {
            // Navigation.PushAsync(new UserPage(false));
        }
    }
}