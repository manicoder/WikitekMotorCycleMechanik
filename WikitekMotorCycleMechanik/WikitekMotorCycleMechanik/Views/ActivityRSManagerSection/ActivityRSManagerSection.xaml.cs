using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.ActivityRSManagerSection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityRSManagerSection : ContentPage
    {
        ActivityRSManagerSectionViewModel viewModel;
        public ActivityRSManagerSection()
        {
            InitializeComponent();
            BindingContext = viewModel = new ActivityRSManagerSectionViewModel(this, null);
        }
    }
}