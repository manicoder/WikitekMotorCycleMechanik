using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.RSManagerSection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RSManagerSection : ContentPage
    {
        RSManagerSectionViewModel viewModel;
        public RSManagerSection()
        {
            InitializeComponent();
            BindingContext = viewModel = new RSManagerSectionViewModel(this, null);
        }
        protected async override void OnAppearing()
        {
            try
            {
                ic_down_button.IsVisible = false;
                actionButtons.IsVisible = false;
                base.OnAppearing();
                var locator = CrossGeolocator.Current;
                var position = await locator.GetPositionAsync(TimeSpan.FromSeconds(5));
                var currentPostion = new Position(position.Latitude, position.Longitude);
                MapSpan mapSpan = MapSpan.FromCenterAndRadius(currentPostion, Distance.FromKilometers(10));
                map.MoveToRegion(mapSpan);
                var selected_jobcard = App.selected_jobcard;
                if (selected_jobcard != null)
                {
                    var json1 = Preferences.Get("LoginResponse", null);
                    LoginResponse login = JsonSerializer.Deserialize<LoginResponse>(json1);

                    this.Title = selected_jobcard.job_card_name + " (" + selected_jobcard.status + ") \r\n" + selected_jobcard.job_card_name;
                    lblCustomerName.Text = login.first_name + " " + login.last_name;// "Customer Name";
                    lblMobileNo.Text = login.mobile;

                    lblStatus.Text = selected_jobcard.status;
                    lblVechiclename.Text= selected_jobcard.job_card_name;
                    lblCustomerName1.Text = login.first_name + " " + login.last_name;// "Customer Name";
                    lblMobileNo1.Text = login.mobile;
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
                listItem.IsVisible = true;
            }
            else
            {
                customerPanel.HeightRequest = 400;
                ic_up_button.IsVisible = false;
                ic_down_button.IsVisible = true;
                actionButtons.IsVisible = true;
                listItem.IsVisible = false;
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
        private async void TalPhoneCall_Tapped(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblMobileNo.Text))
            {
                await Call(lblMobileNo.Text);
            }
        }
        public async Task Call(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }

            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.  
            }
            catch (Exception ex)
            {
                // Other error has occurred.  
            }
        }

        private async void TalPhoneCall1_Tapped(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(lblMobileNo1.Text))
            {
                await Call(lblMobileNo.Text);
            }
        }
    }
}