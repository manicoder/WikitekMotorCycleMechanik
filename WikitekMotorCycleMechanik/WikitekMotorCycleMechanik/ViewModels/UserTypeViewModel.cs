using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Views.Dashboad;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class UserTypeViewModel
    {
        readonly Page page;
        public UserTypeViewModel(Page page)
        {
            this.page = page;
            InitializeCommands();
        }

        public void InitializeCommands()
        {
            wikitekCommand = new Command(async (obj) =>
            {
                try
                {
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(200);
                        UserDialogs.Instance.Toast("User Type: Wikitek Mechanik");
                        await page.Navigation.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new DashboadPage(App.user)) });
                    }
                }
                catch (Exception ex)
                {

                }
            });
            rsangelCommand = new Command(async (obj) =>
            {
                await page.DisplayAlert("Coming Soon","RSAngel Mechanik will be available soon","Ok");
            });
            mobitekCommand = new Command(async (obj) =>
            {
                await page.DisplayAlert("Coming Soon", "Mobitech Mechanik will be available soon", "Ok");
            });
        }

        public ICommand wikitekCommand { get; set; }
        public ICommand rsangelCommand { get; set; }
        public ICommand mobitekCommand { get; set; }
    }
}
