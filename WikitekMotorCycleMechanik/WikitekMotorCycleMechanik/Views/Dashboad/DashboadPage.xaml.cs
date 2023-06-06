using MiBud;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.Dashboad
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DashboadPage : ContentPage
    {
        DashboadViewModel viewModel;
        public DashboadPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new DashboadViewModel(this,null);
        }

        public DashboadPage(LoginResponse user)
        {
            InitializeComponent();
            BindingContext = viewModel = new DashboadViewModel(this,user);
        }

        protected override void OnAppearing()
        {
            viewModel.RemoveSelection();
            base.OnAppearing();

            string userRole = Preferences.Get("user_role", null);
            switch (userRole)
            {
                case "WikitekMechanik":
                    ChangeStatusColor(Color.Blue);
                    break;

                case "MobitekMechanik":
                    ChangeStatusColor((Color)Application.Current.Resources["theme_color"]);
                    break;

                case "RSAngelMechanik":
                    ChangeStatusColor(Color.Green);
                    break;

            }
        }
        private static void ChangeStatusColor(Color color)
        {
            var statusbar = DependencyService.Get<IStatusBarPlatformSpecific>();
            statusbar.SetStatusBarColor(color);
            var aa = Application.Current.MainPage;
            var mdPage = Application.Current.MainPage as MasterDetailPage;
            var navPage = mdPage.Detail as NavigationPage;
            navPage.BarBackgroundColor = color;
        }
    }
}