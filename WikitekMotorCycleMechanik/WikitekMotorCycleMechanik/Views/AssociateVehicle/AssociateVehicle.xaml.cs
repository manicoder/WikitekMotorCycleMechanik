using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.AssociateVehicle
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssociateVehicle : ContentPage
    {
        AssociateVehicleViewModel viewModel;
        public AssociateVehicle()
        {
            InitializeComponent();
            BindingContext = viewModel = new AssociateVehicleViewModel(this, null);
        }

    }
}