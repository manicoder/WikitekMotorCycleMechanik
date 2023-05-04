using WikitekMotorCycleMechanik.ViewModels;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using WikitekMotorCycleMechanik.Views.VehicleDiagnostics;
using WikitekMotorCycleMechanik.Views.Dashboad;

namespace WikitekMotorCycleMechanik.Views.Settings
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingPage : ContentPage
    {
        SettingViewModel viewModel;
        public SettingPage()
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new SettingViewModel(this, null);
            }
            catch (Exception ex)
            {
            }
        }

        public SettingPage(LoginResponse user)
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new SettingViewModel(this, user);
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