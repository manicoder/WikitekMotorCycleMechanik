using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
using WikitekMotorCycleMechanik.Views.Dashboad;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.Subscription
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubscrionsDetailPage : ContentPage
    {
        public SubscrionsDetailPage()
        {
            InitializeComponent();

            try
            {
                InitializeComponent();
                BindingContext = viewModel = new SubscrionsDetailViewModel(this, null);
            }
            catch (Exception ex)
            {
            }
        }
        SubscrionsDetailViewModel viewModel;


        public SubscrionsDetailPage(LoginResponse user)
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new SubscrionsDetailViewModel(this, user);
            }
            catch (Exception ex)
            {
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                Application.Current.MainPage = new MasterDetailView(App.user) { Detail = new NavigationPage(new DashboadPage(App.user)) };
            });
            return true;
        }
    }
}