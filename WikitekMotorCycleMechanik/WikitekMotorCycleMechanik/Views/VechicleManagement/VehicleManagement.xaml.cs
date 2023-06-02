using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.VechicleManagement
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehicleManagement : ContentPage
    {
        VehicleManagementViewModel viewModel;
        public VehicleManagement()
        {
            InitializeComponent();
            BindingContext = viewModel = new VehicleManagementViewModel(this, null);
        }

        protected async override void OnAppearing()
        {
            try
            {

            
            base.OnAppearing();
            //var locator = CrossGeolocator.Current;
            //var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
            //var currentPostion = new Position(position.Latitude, position.Longitude);
            //MapSpan mapSpan = MapSpan.FromCenterAndRadius(currentPostion, Distance.FromKilometers(10));
            //map.MoveToRegion(mapSpan);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}