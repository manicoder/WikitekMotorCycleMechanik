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
        public VehicleServicePage ()
		{
			InitializeComponent ();
			ModelYear modelYear = new ModelYear();
			modelYear.id = 20802;
			modelYear.name = "Petrol";

			VehOem vehOem = new VehOem();
			vehOem.id = 1;
			vehOem.name = "TVS";

			SubModel subModel = new SubModel();
			subModel.id = 20802;
			subModel.name = "Petrol";

			Vehicle_Model vehicleModel = new Vehicle_Model();
			vehicleModel.id = 20802;
			vehicleModel.name = "Raider";

			Vehicle selected_vehicle = new Vehicle();
			selected_vehicle.id = "9e0a2aa6-7acb-49e9-96f0-dc2fcac509fd";
			selected_vehicle.model_year = modelYear;
			selected_vehicle.oem = vehOem;
			selected_vehicle.registration_id = "Regd1";
			selected_vehicle.sub_model = subModel;
			selected_vehicle.user = "7af810a8-9ada-4432-a91c-b22222cca9ec";
			selected_vehicle.vehicle_model = vehicleModel;
			selected_vehicle.vin = "Vin1";
			BindingContext = viewModel = new VehicleServiceViewModel(this,selected_vehicle);
        }
	}
}