using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.ViewModels;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.MarketPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OrderSummaryPage : ContentPage
    {
        OrderSummaryViewModel viewModel;
        public OrderSummaryPage(ObservableCollection<CartItem> cart_list, ShipmentAddressModel shipment_address, BillingAddressModel billing_address)
        {
            InitializeComponent();
            BindingContext = viewModel = new OrderSummaryViewModel(this, cart_list, shipment_address, billing_address);
        }

        protected override async void OnAppearing()
        {
            try
            {
                PaymentServerRequestModel paymentServerRequestModel = new PaymentServerRequestModel();
                var data = App.paymentResponseModel;

                //*For Registration api testing
                //if (string.IsNullOrEmpty(data.OrderId) && string.IsNullOrEmpty(data.PaymentId))
                if (!string.IsNullOrEmpty(data.OrderId) && !string.IsNullOrEmpty(data.PaymentId))
                {
                    ApiServices apiServices = new ApiServices();

                    //*
                    //paymentServerRequestModel.payment_detail = new Paymentdetail
                    //{
                    //    currency = "INR",
                    //    extended_price = 33040.0,
                    //    order_id = "order_JoRumXXuYPBJZx",
                    //    payment_id = "pay_JoRvAvIe05LY74",
                    //    signature = "e84d6e7879c0dd2af993935ce5230f9e395a2f6b81ee331d89e28962c53760a3",
                    //    user_contact = "+918446448159",
                    //    user_email = "mayur@wikitek.com",
                    //    gst_no = "GHGFDRT345676"
                    //};

                    paymentServerRequestModel.payment_detail = new Paymentdetail
                    {
                        currency = viewModel.generate_order_id.currency,
                        extended_price = viewModel.total_price,
                        order_id = data.OrderId,
                        payment_id = data.PaymentId,
                        signature = data.Signature,
                        user_contact = data.UserContact,
                        user_email = data.UserEmail,
                        gst_no = viewModel.isValidGST ? viewModel.gstin : String.Empty
                    };

                    //*
                    //paymentServerRequestModel.shipment_address = new ShipmentAddress
                    //{
                    //    address = "Office no. 305, Kesnand Rd, Wagholi, Pune, Maharashtra",
                    //    country = "India",
                    //    pin_code = "412207",
                    //    user_id = Preferences.Get("userIdIT", String.Empty),
                    //};

                    paymentServerRequestModel.shipment_address = new ShipmentAddress
                    {
                        address = viewModel.shipment_address.address,
                        country = viewModel.shipment_address.country,
                        pin_code = viewModel.shipment_address.pin_code,
                        user_id = Preferences.Get("userIdIT", String.Empty),
                    };

                    //*
                    //paymentServerRequestModel.billing_address = new BillingAddress
                    //{
                    //    address = "Home no. 1828, Pahad Lane, Yeola, Nashik, Maharashtra",
                    //    country = "India",
                    //    pin_code = "423401",
                    //    user_id = Preferences.Get("userIdIT", String.Empty),
                    //};

                    paymentServerRequestModel.billing_address = new BillingAddress
                    {
                        address = viewModel.billing_address.address,
                        country = viewModel.billing_address.country,
                        pin_code = viewModel.billing_address.pin_code,
                        user_id = Preferences.Get("userIdIT", String.Empty),
                    };

                    //*
                    //paymentServerRequestModel.cart_list.Add(
                    //        new CartList
                    //        {
                    //            part_id = "21a0d681-2ca8-4fc4-a3b2-f6c92e6bb9f0",
                    //            //part_number = item.part_number,
                    //            quantity = 2,
                    //            unit_price = 28000.0,
                    //        });

                    foreach (var item in viewModel.cart_list)
                    {
                        paymentServerRequestModel.cart_list.Add(
                            new CartList
                            {
                                part_id = item.parts_id.id,
                                //part_number = item.part_number,
                                quantity = item.quantity,
                                unit_price = item.unit_price,
                            });
                    }
                    Device.InvokeOnMainThreadAsync(async () =>
                    {
                        var result = await apiServices.RegisterOrderToServer(Xamarin.Essentials.Preferences.Get("token", null), paymentServerRequestModel);

                        if (!string.IsNullOrEmpty(result?.id))
                        {
                            var cart = await apiServices.DeletePurchaseCart(Xamarin.Essentials.Preferences.Get("token", null));

                            await DisplayAlert("Success!", "Payment Successfully done.", "OK");
                            App.paymentResponseModel = null;
                            //App.Current.MainPage = new NavigationPage(new MasterDetailView(App.user) { Detail = new NavigationPage(new Views.Settings.SettingPage(App.user)) });
                            App.Current.MainPage = new NavigationPage(new MyOrdersTabbedPage());
                        }
                        else
                        {
                            await DisplayAlert("Failed!", "Order registration failed.", "OK");
                        }
                    });


                }
            }
            catch (Exception ex)
            {
            }


            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
        }

        private void GST_TextChanged(object sender, TextChangedEventArgs e)
        {
            Regex regex = new Regex("^[0-9]{2}[A-Z]{5}[0-9]{4}[A-Z]{1}[1-9A-Z]{1}Z[0-9A-Z]{1}$", RegexOptions.IgnoreCase);
            
            if (viewModel.gstin.Length == 15)
            {
                if (!regex.IsMatch(viewModel.gstin))
                {
                    viewModel.isValidGST = false;
                    DisplayAlert("Alert", "Please enter valid GST Identification number", "Ok");
                }
                else
                {
                    viewModel.isValidGST = true;
                }
            }
        }
    }
}