using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using WikitekMotorCycleMechanik.Views.VehicleDiagnostics;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestServerPage : ContentPage
    {
        LoginResponse user = new LoginResponse();
        public TestServerPage()
        {
            InitializeComponent();
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            var update = Preferences.Get("IsUpdate", null);

            if (string.IsNullOrEmpty(update))
            {
                App.is_update = true;
            }
            else
            {
                if (update.Contains("true"))
                {
                    App.is_update = true;
                }
                else
                {
                    App.is_update = false;
                }
            }

            DateTime now = DateTime.Now.Date;
            int offset = CalculateOffset(now.DayOfWeek, DayOfWeek.Monday);
            DateTime nextUpdate = now.AddDays(offset);

            if (nextUpdate <= DateTime.Now.Date)
            {
                Application.Current.MainPage = new CustomControls.CustomNavigationPage(new Views.Login.LoginPage());
            }
            else
            {
                var token = Preferences.Get("token", null);
                if (string.IsNullOrEmpty(token))
                {
                    Application.Current.MainPage = new CustomControls.CustomNavigationPage(new Views.Login.LoginPage());
                }
                else
                {
                    LoginResponse loginResponse = new LoginResponse();
                    var user_data = Preferences.Get("LoginResponse", null);


                    App.user =  loginResponse = JsonConvert.DeserializeObject<LoginResponse>(user_data);
                    user = loginResponse;
                    //loginResponse.status_code = httpResponse.StatusCode;
                    //Preferences.Set("user_name", model.email);
                    //Preferences.Set("password", model.password);
                    //Preferences.Set("token", loginResponse.token?.access);
                    //Application.Current.Properties["refresh_token"] = App.refresh_token = loginResponse.token?.refresh;
                    //Application.Current.Properties["access_token"] = App.access_token = loginResponse.token?.access;
                    //Application.Current.Properties["user_mail"] = App.user_mail = loginResponse?.email;
                    App.user_first_name = loginResponse?.first_name;
                    App.user_last_name = loginResponse?.last_name;
                    Application.Current.SavePropertiesAsync();
                    App.user_id = loginResponse?.user_id;
                    App.user_type = loginResponse?.user_type;
                    App.country_id = loginResponse?.agent?.workshop?.country;
                    Application.Current.MainPage = new MasterDetailView(user) { Detail = new NavigationPage(new VehicleDiagnosticsPage(user)) };
                }
            }



            //var access_token = Application.Current.Properties.FirstOrDefault(x => x.Key == "access_token");
            //var refresh_token = Application.Current.Properties.FirstOrDefault(x => x.Key == "refresh_token");
            //if (string.IsNullOrEmpty(access_token.Key) || string.IsNullOrEmpty(refresh_token.Key))
            //{
            //    Application.Current.Properties["access_token"] = "";
            //    Application.Current.Properties["refresh_token"] = "";
            //    Application.Current.Properties["mac_id"] = "";
            //}
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
    }
}