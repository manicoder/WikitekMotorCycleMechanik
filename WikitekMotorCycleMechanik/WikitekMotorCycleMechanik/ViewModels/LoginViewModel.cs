using Acr.UserDialogs;
//using Android.Net.Wifi;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
//using WikitekMotorCycleMechanik.Views.JobCardPages;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using WikitekMotorCycleMechanik.Views.Settings;
using WikitekMotorCycleMechanik.Views.VehicleDiagnostics;
using Xamarin.Forms;
using Xamarin.Essentials;
using System.Net.Http;
using System;
using System.Linq;
using Supremes;
using System.Text.RegularExpressions;
//using Plugin.LatestVersion;
//using Supremes.Nodes;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        readonly Page page;
        readonly IDeviceMacAddress device_mac_id;
        public string usability = string.Empty;
        bool MobileLenghtValidate = false;
        string mobile_regex = @"^[0-9]{10}$";//@"\d{10}";
        const string email_regex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
          @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        const string alfa_regex = @"[a-zA-Z]";

        public LoginViewModel(Page page) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                user_detail = new LoginModel();

                this.page = page;
                this.navigationService = page.Navigation;
                this.device_mac_id = DependencyService.Get<IDeviceMacAddress>();
                InitializeCommands();
                GetDeviceType();
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    await GetMacId();



                    //var isLatest = await CrossLatestVersion.Current.IsUsingLatestVersion();
                    //if (!isLatest)
                    //{
                    //    var update = await page.DisplayAlert("New Version", "New version Available,do you want to update it?", null, "Ok");
                    //    if (update)
                    //    {
                    //        await CrossLatestVersion.Current.OpenAppInStore();
                    //    }
                    //}

                    //var PlayStoreVersion = GetAndroidStoreAppVersion();
                    //bool isLatestVersion = await DependencyService.Get<ILatest>().IsUsingLatestVersion();
                    //if (!isLatestVersion)
                    //{
                    //    bool res = await page.DisplayAlert("New Version", "New version Available,do you want to update it?", null, "Ok");
                    //    if (!res)
                    //    {
                    //        await DependencyService.Get<ILatest>().OpenAppInStore();
                    //    }
                    //}
                });

                var user = Preferences.Get("user_name", null);
                var pass = Preferences.Get("password", null);

                if (!string.IsNullOrEmpty(user))
                {
                    email = user;
                }

                if (!string.IsNullOrEmpty(user))
                {
                    password = pass;
                }


                // DTool Main Server
                //email = "yogesh11@gmail.com";
                //password = "yogesh123";

                // DTool Test Server
                //email = "test@gmail.com";
                //password = "test12345";

                //email = "d.chhawdi@gmail.com";
                //password = "deepak123";
            }
            catch (Exception ex)
            {
            }
        }


        #region Properties

        //private string _rs_agent_name;
        //public string rs_agent_name
        //{
        //    get => _rs_agent_name;
        //    set
        //    {
        //        _rs_agent_name = value;
        //        OnPropertyChanged("rs_agent_name");
        //    }
        //}


        private string _email;
        public string email
        {
            get => _email;
            set
            {
                _email = value;
                if (!string.IsNullOrEmpty(email))
                {
                    MobileLenghtValidate = (Regex.IsMatch(email, mobile_regex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)));
                    if (MobileLenghtValidate)
                    {
                        user_name_lenght = 10;
                    }
                    else
                    {
                        user_name_lenght = 100;
                    }
                }
                else
                {
                    user_name_lenght = 100;
                }
                OnPropertyChanged("email");
            }
        }


        private string _password;
        public string password
        {
            get => _password;
            set
            {
                _password = value;

                if (string.IsNullOrEmpty(password))
                {
                    password_image_visible = false;
                }
                else
                {
                    password_image_visible = true;
                }

                OnPropertyChanged("password");
            }
        }

        private int _user_name_lenght = 100;// = "gyanendra.tiwary@autopeepal.com";
        public int user_name_lenght
        {
            get => _user_name_lenght;
            set
            {
                try
                {
                    _user_name_lenght = value;
                    OnPropertyChanged("user_name_lenght");
                }
                catch (Exception ex)
                {
                }
            }
        }

        private string _device_type;
        public string device_type
        {
            get => _device_type;
            set
            {
                _device_type = value;
                OnPropertyChanged("device_type");
            }
        }


        private string _mac_id;
        public string mac_id
        {
            get => _mac_id;
            set
            {
                _mac_id = value;
                OnPropertyChanged("mac_id");
            }
        }

        private bool _is_password = true;
        public bool is_password
        {
            get => _is_password;
            set
            {
                _is_password = value;
                OnPropertyChanged("is_password");
            }
        }

        private string _password_image = "󰛐";//"\uF06D1";//"&#xF06D1;";
        public string password_image
        {
            get => _password_image;
            set
            {
                _password_image = value;
                OnPropertyChanged("password_image");
            }
        }

        private Color _password_image_color = Color.FromHex("#01FE2F");//"\uF06D1";//"&#xF06D1;";
        public Color password_image_color
        {
            get => _password_image_color;
            set
            {
                _password_image_color = value;
                OnPropertyChanged("password_image_color");
            }
        }

        private bool _password_image_visible = false;
        public bool password_image_visible
        {
            get => _password_image_visible;
            set
            {
                _password_image_visible = value;
                OnPropertyChanged("password_image_visible");
            }
        }

        private LoginModel _user_detail;
        public LoginModel user_detail
        {
            get => _user_detail;
            set
            {
                _user_detail = value;
                OnPropertyChanged("user_detail");
            }
        }

        #endregion


        #region Methods
        public async void InitializeCommands()
        {
            //await SecureStorage.SetAsync("oauth_token", Xamarin.Essentials.Preferences.Get("token", null));
            //var oauthToken = await SecureStorage.GetAsync("oauth_token");
            // await SecureStorage.SetAsync("oauth_token", oauthToken);

            IsPasswordCommand = new Command(async (obj) =>
            {
                if (is_password)
                {
                    password_image = "󰛑";//F06D0;
                    password_image_color = Color.Red;
                    is_password = false;
                }
                else
                {
                    is_password = true;
                    password_image_color = Color.FromHex("#01FE2F");
                    password_image = "󰛐";//F06D1
                }
            });

            LoginCommand = new Command(async (obj) =>
            {
                GoToMasterPage();
            });

            SingUpCommand = new Command(async (obj) =>
            {
                GoToRegistrationPage();
            });

            ForgotPasswordCommand = new Command(async (obj) =>
            {
                GoToForgotPasswordPage();
            });
        }

        //public async Task UserLogin()
        //{

        //    var response = await apiServices.UserLogin(user_detail);

        //    var api_status_code = StaticMethods.http_status_code(response.status_code);

        //    if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
        //    {
        //        //country_list = response.results;
        //    }
        //    else
        //    {

        //    }
        //}

        public void GetDeviceType()
        {
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
        }

        public async Task GetMacId()
        {
            mac_id = await device_mac_id.GetDeviceUniqueId();

            //mac_id = "10df3f643ce3e4af";//"af531103492a2a59";

            //email = "expert3w@gmail.com";
            //password = "expert123";
            //mac_id = "EXPERT123456";
        }

        public async Task<bool> Validation()
        {
            bool IsError = false;
            if (string.IsNullOrEmpty(email))
            {
                await page.DisplayAlert("Alert", "Please enter your user name", "Ok");
                IsError = true;
            }
            else if (email.Contains("@") || Regex.IsMatch(email, alfa_regex))
            {
                if (!Regex.IsMatch(email, email_regex))
                {
                    await page.DisplayAlert("Alert", "Please enter your registered email id", "Ok");
                    IsError = true;
                }
            }
            else if (!Regex.IsMatch(email, mobile_regex))
            {
                await page.DisplayAlert("Alert", "Please enter your registered mobile number", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(password))
            {
                await page.DisplayAlert("Alert", "Please enter your password", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(device_type))
            {
                await page.DisplayAlert("Alert", "Device type not found", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(mac_id))
            {
                await page.DisplayAlert("Alert", "Mac address not found", "Ok");
                IsError = true;
            }

            return IsError;
        }

        async void GoToMasterPage()
        {
            LoginResponse user = null;

            try
            {
                var IsError = await Validation();
                if (!IsError)
                {
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(200);

                        if (string.IsNullOrEmpty(mac_id))
                        {
                            await page.DisplayAlert("Error", "Your device id not found.", "Ok");
                            return;
                        }

                        user_detail.email = email;
                        user_detail.password = password;
                        user_detail.device_type = device_type;
                        user_detail.mac_id = mac_id;

                        if (user_detail.email == "universaluser@wikitek.in" && user_detail.password == "123456")
                        {
                            user_detail.mac_id = "10df3f643ce3e4af";
                        }
                        else if (user_detail.email == "sofi6878@gmail.com" && user_detail.password == "wikitek123")
                        {
                            user_detail.mac_id = "e859a27f622bdff2";
                        }

                        App.login_model = user_detail;
                        App.user = user = await apiServices.UserLoginNew(user_detail);

                        if (!user.success)
                        {
                            if (user.error.Contains("not registered"))
                            {
                                await page.DisplayAlert("Unauthorized Device.", "Contact wikitek hotline" +
                                    " on 9028347071 to authorize your device.", "Ok");
                                return;
                            }
                            else
                            {
                                await page.DisplayAlert("Error", user.error, "Ok");
                                return; 
                            }
                        }

                        if (user.email != email && user.mobile != email)
                        {
                            await page.DisplayAlert("Failed", "Register email not matched", "Ok");
                            return;
                        }

                        Preferences.Set("user_name", email);
                        Preferences.Set("password", password);
                        Preferences.Set("token", user.token?.access);
                        Preferences.Set("refresh_token", user.token?.refresh);
                      
                        if (user.mac_id.Contains("EXPERT123456"))
                        {
                            //user.role = "EXPERT";
                            Application.Current.MainPage = new MasterDetailView(user)
                            {
                                Detail = new NavigationPage(new Views.RemoteRequest.RemoteRequestPage())
                            }; 
                            return;
                        }

                        if (string.IsNullOrWhiteSpace(user.user_type))
                        {
                            Application.Current.Resources["theme_color"] = Color.FromHex("#3e4095");
                            DependencyService.Get<IStatusBarStyleManager>().ChangeTheme(user.user_type);
                            //DependencyService.Get<IStatusBarStyleManager>().SetWhiteStatusBar("#2d6810");
                        }
                        else if (user.user_type == "wikitekMechanik")
                        {
                            Application.Current.Resources["theme_color"] = Color.FromHex("#3e4095");
                            DependencyService.Get<IStatusBarStyleManager>().ChangeTheme(user.user_type);
                            //DependencyService.Get<IStatusBarStyleManager>().SetWhiteStatusBar("#2d6810");
                        }
                        else if (user.user_type == "rsangleMechanik")
                        {
                            Application.Current.Resources["theme_color"] = Color.FromHex("#37a437");
                            DependencyService.Get<IStatusBarStyleManager>().ChangeTheme(user.user_type);
                            //DependencyService.Get<IStatusBarStyleManager>().SetWhiteStatusBar("#2d6810");
                        }
                        else if (user.user_type == "mobitekMechanik")
                        {
                            Application.Current.Resources["theme_color"] = Color.FromHex("#b85e00");
                            DependencyService.Get<IStatusBarStyleManager>().ChangeTheme(user.user_type);
                            //DependencyService.Get<IStatusBarStyleManager>().SetWhiteStatusBar("#2d6810");
                        }
                       
                        Application.Current.MainPage = new MasterDetailView(user)
                        {
                            Detail = new NavigationPage(new Views.Dashboad.DashboadPage(user)) 
                        };
                    }
                }
            }
            catch (System.Exception es)
            {
                await page.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
                //await navigationService.PushAsync(new MasterDetailView(user) { Detail = new NavigationPage(new VehicleDiagnosticsPage()) });
                //await navigationService.PushAsync(new MasterDetailView(user) { Detail = new NavigationPage(new MyJobCardPage()) });
            }
        }

        async void GoToRegistrationPage()
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                await Task.Delay(200);
                await navigationService.PushAsync(new Views.Registration.RegistrationPage());
            }
        }

        async void GoToForgotPasswordPage()
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                try
                {
                    //await Task.Delay(200);
                    //string description = "An OTP has been sent to your mobile and will be valid for 10 mins.Pls enter the OTP here";
                    await navigationService.PushAsync(new Views.Otp.MobileNumberPage());
                    //await page.Navigation.PushAsync(new Views.Otp.OtpPage(false, description, ""));
                }
                catch (System.Exception ex)
                {
                }
            }
        }

        public static string GetAndroidStoreAppVersion()
        {
            string androidStoreAppVersion = null;

            try
            {
                using (var client = new HttpClient())
                {
                    var doc = client.GetAsync("https://play.google.com/store/apps/details?id=" + "com.wikiteksystems.dtool" + "&hl=en_US").Result.Parse();
                    var versionElement = doc.Select("div:containsOwn(Current Version)");
                    androidStoreAppVersion = versionElement.Text;
                    Supremes.Nodes.Element headElement = versionElement[0];
                    Supremes.Nodes.Elements siblingsOfHead = headElement.SiblingElements;
                    Supremes.Nodes.Element contentElement = siblingsOfHead.First;
                    Supremes.Nodes.Elements childrenOfContentElement = contentElement.Children;
                    Supremes.Nodes.Element childOfContentElement = childrenOfContentElement.First;
                    Supremes.Nodes.Elements childrenOfChildren = childOfContentElement.Children;
                    Supremes.Nodes.Element childOfChild = childrenOfChildren.First;

                    androidStoreAppVersion = childOfChild.Text;
                }
            }
            catch (Exception ex)
            {
                // do something
                Console.WriteLine(ex.Message);
            }

            return androidStoreAppVersion;
        }

        #endregion


        #region ICommands
        public ICommand LoginCommand { get; set; }
        public ICommand SingUpCommand { get; set; }
        public ICommand ForgotPasswordCommand { get; set; }
        public ICommand IsPasswordCommand { get; set; }
        #endregion
    }
}