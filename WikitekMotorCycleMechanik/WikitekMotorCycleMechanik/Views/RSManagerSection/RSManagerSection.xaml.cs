using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.RSManagerSection
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RSManagerSection : ContentPage
	{
		public RSManagerSection ()
		{
			InitializeComponent ();
		}
        protected async override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
                var currentPostion = new Position(position.Latitude, position.Longitude);
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(currentPostion, Distance.FromKilometers(10));
                map.MoveToRegion(mapSpan);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}