using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.RSManagerSection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RSManagerSection : ContentPage
    {
        public RSManagerSection()
        {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            try
            {
                ic_down_button.IsVisible = false;
                base.OnAppearing();
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
                var currentPostion = new Position(position.Latitude, position.Longitude);
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(currentPostion, Distance.FromKilometers(10));
                map.MoveToRegion(mapSpan);
                var selected_jobcard = App.selected_jobcard;
                if (selected_jobcard != null)
                {
                    this.Title = selected_jobcard.job_card_name + " (" + selected_jobcard.status + ") \r\n" + selected_jobcard.job_card_name;
                    lblCustomerName.Text = "Customer Name";
                    lblMobileNo.Text = "+91 9888098880";
                    if (selected_jobcard.status == "ApprovedTransport")
                    {

                    }
                    else if (selected_jobcard.status == "Pickup")
                    {

                    }
                    else if (selected_jobcard.status == "EntryCheck")
                    {

                    }
                }

            }
            catch (Exception ex)
            {

                throw;
            }
        }



        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            if (customerPanel.Height == 400)
            {
                customerPanel.HeightRequest = 90;
                ic_up_button.IsVisible = true;
                ic_down_button.IsVisible = false;
                actionButtons.IsVisible = false;
            }
            else
            {
                customerPanel.HeightRequest = 400;
                ic_up_button.IsVisible = false;
                ic_down_button.IsVisible = true;
                actionButtons.IsVisible = true;
            }
        }

        private void ImageButton_Clicked_1(object sender, EventArgs e)
        {
            try
            {
                PhoneDialer.Open("9888098880");
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
        }
    }
}