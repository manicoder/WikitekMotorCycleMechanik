using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.Jobcard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobcardPage : ContentPage
    {
        JobcardViewModel viewModel;
        public JobcardPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new JobcardViewModel(this);
        }
    }
}