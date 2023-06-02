
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.AddTechnician
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTechnician : ContentPage
    {
        AddTechnicianViewModel viewModel;
        public AddTechnician()
        {
            InitializeComponent();
            BindingContext = viewModel = new AddTechnicianViewModel(this, null);
        }
    }
}