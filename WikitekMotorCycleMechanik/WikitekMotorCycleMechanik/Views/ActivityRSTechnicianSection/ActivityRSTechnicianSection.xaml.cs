using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.ActivityRSTechnicianSection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityRSTechnicianSection : ContentPage
    {
        ActivityRSTechnicianSectionViewModel viewModel;
        public ActivityRSTechnicianSection()
        {
            InitializeComponent();
            BindingContext = viewModel = new ActivityRSTechnicianSectionViewModel(this, null);
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            viewModel.IsToggledchange(e.Value);
        }
    }
}