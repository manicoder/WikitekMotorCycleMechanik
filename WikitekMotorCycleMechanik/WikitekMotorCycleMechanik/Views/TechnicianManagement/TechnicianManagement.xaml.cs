using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.TechnicianManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TechnicianManagement : ContentPage
    {
        TechnicianManagementViewModel viewModel;
        public TechnicianManagement()
        {
            InitializeComponent();
            BindingContext = viewModel = new TechnicianManagementViewModel(this, null);
        }
    }
}