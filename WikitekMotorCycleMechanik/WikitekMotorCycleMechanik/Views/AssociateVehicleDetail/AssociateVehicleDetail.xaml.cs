using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.AssociateVehicleDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssociateVehicleDetail : ContentPage
    {
        AssociateVehicleDetailViewModel viewModel;
        public AssociateVehicleDetail()
        {
            InitializeComponent();
            BindingContext = viewModel = new AssociateVehicleDetailViewModel(this, null);
        }
    }
}