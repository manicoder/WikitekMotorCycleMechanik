using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
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
        }
    }
}