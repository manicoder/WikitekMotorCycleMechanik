using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.MarketPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MyOrdersTabbedPage : TabbedPage
    {
        public MyOrdersTabbedPage()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new MasterDetailView(App.user)
            {
                Detail = new NavigationPage(new Views.Dashboad.DashboadPage(App.user))
            };
            return true;
        }
    }
}