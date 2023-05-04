using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.Jobcard
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class JobcardDetailPage : ContentPage
    {
        JobcardDetailViewModel viewModel;
        public JobcardDetailPage(JobcardResult jobcard_detail)
        {
            InitializeComponent();

            BindingContext = viewModel = new JobcardDetailViewModel(this, jobcard_detail);
        }
    }
}