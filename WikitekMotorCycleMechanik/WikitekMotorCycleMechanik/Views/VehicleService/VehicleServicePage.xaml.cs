using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.VehicleService
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class VehicleServicePage : ContentPage
	{
        VehicleServiceViewModel viewModel;
        public VehicleServicePage (Vehicle selected_vehicle)
		{
			InitializeComponent ();
            BindingContext = viewModel = new VehicleServiceViewModel(this,selected_vehicle);
        }
	}
}