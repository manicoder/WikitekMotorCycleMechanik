using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.DtcFinder
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DtcFinderPage : ContentPage
    {
        DtcFinderViewModel viewModel;
        public DtcFinderPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new DtcFinderViewModel(this);
        }
    }
}