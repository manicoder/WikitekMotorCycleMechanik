using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models1;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.ActivityDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityDetail : ContentPage
    {
        ActvityDetailViewModel viewModel;
        public ActivityDetail(VechicleTechicianList selected_vehicletechnician)
        {
            InitializeComponent();
            BindingContext = viewModel = new ActvityDetailViewModel(this, selected_vehicletechnician);
        }
    }
}