using Acr.UserDialogs;
using MultiEventController;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.CustomControls;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.ViewModels;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using WikitekMotorCycleMechanik.Views.VehicleDiagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik
{
    public partial class App : Application
    {
        public static string base_url = "https://wikitek.io/api/v1/";//Original Server
        //public static string base_url = "http://128.199.17.43/api/v1/";//Test Server
        public static double ScreenHeight = 0;
        public static bool is_global_method = true;
        public static bool bt_available = true;
        public static bool is_update = true;
        //public static string refresh_token = string.Empty;
        //public static string access_token = string.Empty;
        //public static string user_mail = string.Empty;
        public static string user_first_name = string.Empty;
        public static string user_last_name = string.Empty;
        public static string user_id = string.Empty;
        public static string user_type = string.Empty;
        public static string dongle_type = string.Empty;
        public static string dongle = string.Empty;
        public static string Status = "Active";
        public static int? country_id;
        public static int submodel_id = 0;
        public static int model_id = 0;
        public static ObservableCollection<BluetoothDevicesModel> bluetooth_devices = new ObservableCollection<BluetoothDevicesModel>();

        public static PaymentResponseModel paymentResponseModel = new PaymentResponseModel();
        public static LoginResponse user = new LoginResponse();
        public static LoginModel login_model = new LoginModel();
        public static GetModel getModel = new GetModel();

        public static ControlEventManager controlEventManager = null;
        public static bool ContentPageIsEnabled { get; set; }
        public static bool DCTPage = true;
        public static bool INFOPage = true;
        public static bool TreeSurveyPage = true;
        public static bool TreeL = true;
        public static JobCardListModel JCM;

        public static bool IsExpert = false;
        public static bool PageFreezIsEnable = false;

        public static ObservableCollection<PartsTypeList> partTypeList;
        public static ObservableCollection<PartsCategoryList> partsCategoryList;
        public static ObservableCollection<MetaTagList> metaTagList;
        //public static ObservableCollection<PriceFilterList> priceList;
        public static double lowerPrice;
        public static double upperPrice;

        public static string selectedSegment = string.Empty;
        public static string selectedPack = string.Empty;
        public static bool isFilter = false;
        public static string selectedBannerId = string.Empty;
        public static JobcardResult selected_jobcard;
        public App()
        {
            Device.SetFlags(new string[] { "MediaElement_Experimentals" });
            InitializeComponent();

            ContentPageIsEnabled = true;
            var SelectedLanguage = DependencyService.Get<ISaveLocalData>().GetData("LastSelectedLanguage");
            if (SelectedLanguage == null)
            {
                Task.Run(async () =>
                {
                    await DependencyService.Get<ISaveLocalData>().SaveData("LastSelectedLanguage", "en");
                });
            }

            if (base_url == "http://128.199.17.43/api/v1/")
            {
                MainPage = new CustomControls.CustomNavigationPage(new Views.TestServerPage());
            }
            else
            {

                var update = Preferences.Get("IsUpdate", null);

                if (string.IsNullOrEmpty(update))
                {
                    is_update = true;
                }
                else
                {
                    if (update.Contains("true"))
                    {
                        is_update = true;
                    }
                    else
                    {
                        is_update = false;
                    }
                }

                DateTime now = DateTime.Now.Date;
                int offset = CalculateOffset(now.DayOfWeek, DayOfWeek.Monday);
                DateTime nextUpdate = now.AddDays(offset);

                if (nextUpdate <= DateTime.Now.Date)
                {
                    MainPage = new CustomControls.CustomNavigationPage(new Views.Login.LoginPage());
                }
                else
                {
                    var token = Preferences.Get("token", null);
                    if (string.IsNullOrEmpty(token))
                    {
                        MainPage = new CustomControls.CustomNavigationPage(new Views.Login.LoginPage());
                    }
                    else
                    {
                        var user_data = Preferences.Get("LoginResponse", null);
                        user = JsonConvert.DeserializeObject<LoginResponse>(user_data);
                        user_first_name = user?.first_name;
                        user_last_name = user?.last_name;
                        user_id = user?.user_id;
                        user_type = user?.user_type;
                        country_id = user?.agent?.workshop?.country;
                        MainPage = new MasterDetailView(user) { Detail = new NavigationPage(new Views.Dashboad.DashboadPage(user)) };
                    }
                }
            }

            controlEventManager = new ControlEventManager();
            //MainPage = new CustomControls.CustomNavigationPage(new Views.LiveParameter.LiveParameterPage());
        }

        public int CalculateOffset(DayOfWeek current, DayOfWeek desired)
        {
            // f( c, d ) = [7 - (c - d)] mod 7
            // f( c, d ) = [7 - c + d] mod 7
            // c is current day of week and 0 <= c < 7
            // d is desired day of the week and 0 <= d < 7
            int c = (int)current;
            int d = (int)desired;
            int offset = (7 - c + d) % 7;
            return offset == 0 ? 7 : offset;
        }

        protected override void OnStart()
        {
        }

        public static void InitExpert(string OwnerUserId, string ToUserId)
        {
            CurrentUserEvent.Instance.OwnerUserId = OwnerUserId;
            CurrentUserEvent.Instance.ToUserId = ToUserId;
            if (OwnerUserId == "RemoteSessionClosedByExpert" && ToUserId == "RemoteSessionAreClosed")
            {
                CurrentUserEvent.Instance.IsExpert = false;
                CurrentUserEvent.Instance.IsRemote = false;
            }
            else
            {
                CurrentUserEvent.Instance.IsExpert = true;
                CurrentUserEvent.Instance.IsRemote = true;
            }
        }
        public static void InitTechnician(string OwnerUserId, string ToUserId)
        {
            ContentPageIsEnabled = false;
            CurrentUserEvent.Instance.OwnerUserId = ToUserId;
            CurrentUserEvent.Instance.ToUserId = OwnerUserId;
            CurrentUserEvent.Instance.IsExpert = false;
            if (OwnerUserId == "RemoteSessionClosedByExpert" && ToUserId == "RemoteSessionAreClosed")
            {
                CurrentUserEvent.Instance.IsRemote = false;
            }
            else
            {
                CurrentUserEvent.Instance.IsRemote = true;
            }
        }

        public static void Expert(string ExpertID, string UserToken)
        {
            CurrentUserEvent.Instance.OwnerUserId = UserToken;
            CurrentUserEvent.Instance.ToUserId = ExpertID;
            CurrentUserEvent.Instance.IsExpert = true;
            CurrentUserEvent.Instance.IsRemote = true;
        }
        public static void Technician(string UserToken, string ExpertID)
        {
            ContentPageIsEnabled = false;
            CurrentUserEvent.Instance.OwnerUserId = ExpertID;
            CurrentUserEvent.Instance.ToUserId = UserToken;
            CurrentUserEvent.Instance.IsExpert = false;
        }



        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }

        public static async void GlobalFuntion()
        {
            try
            {

                is_global_method = false;
                Current.MainPage = new MasterDetailView(user)
                { Detail = new CustomNavigationPage(new VehicleDiagnosticsPage(user)) };

                await Current.MainPage.DisplayAlert("Alert", "Dongle is disconnected", "Ok");


            }
            catch (Exception ex)
            {
            }
        }
    }
}
