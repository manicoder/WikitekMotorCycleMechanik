using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.ViewModels;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.Subscription
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SubscriptionPage : ContentPage
    {
        SubscriptionViewModel viewModel;
        string pack;
        public SubscriptionPage(string pack)
        {
            try
            {
                InitializeComponent();
                this.pack = pack;
                BindingContext = viewModel = new SubscriptionViewModel(this, pack);

            }
            catch (Exception ex)
            {
            }
        }

        protected async override void OnAppearing()
        {
            try
            {
                PurchasePackageModel paymentServerRequestModel = new PurchasePackageModel();
                ApiServices apiServices = new ApiServices();
                var data = App.paymentResponseModel;

                if (!string.IsNullOrEmpty(data?.OrderId) && !string.IsNullOrEmpty(data?.PaymentId))
                {
                    paymentServerRequestModel = new PurchasePackageModel
                    {
                        user_type = App.user.user_type,
                        package_visibility = viewModel.selected_package.package_visibility,
                        oem_specific = viewModel.selected_package.oem_specific,
                        amount = viewModel.selected_package.amount,
                        code = viewModel.selected_package.code,
                        country = viewModel.selected_package.country.id,
                        segments = (int)App.user.segment_id,
                        email = App.user.email,
                        mobile = App.user.mobile,
                        grace_period = viewModel.selected_package.grace_period,
                        oem = viewModel.selected_package.oem,
                        recurrence_period = viewModel.selected_package.recurrence_period,
                        start_date = DateTime.Now,
                        //validity_period= viewModel.selected_package.v,
                        usability = viewModel.selected_package.usability,
                        recurrence_unit = viewModel.selected_package.recurrence_unit,
                        user = App.user.user_id,
                        razorpay_order_id = data.OrderId,
                        razorpay_payment_id = data.PaymentId,
                        razorpay_signature = data.Signature,
                        package_name = viewModel.pack,
                    };

                    var json = JsonConvert.SerializeObject(paymentServerRequestModel);

                    Device.InvokeOnMainThreadAsync(async () =>
                    {
                        var result = await apiServices.RegisterSubscriptionToServer(Xamarin.Essentials.Preferences.Get("token", null), paymentServerRequestModel);

                        if (!string.IsNullOrEmpty(result?.razorpay_order_id))
                        {
                            //var cart = await apiServices.DeletePurchaseCart(App.access_token);

                            await DisplayAlert("Success!", "Payment Successfully done.", "OK");
                            App.paymentResponseModel = null;
                            //App.Current.MainPage = new NavigationPage(new MasterDetailView(App.user) { Detail = new NavigationPage(new Views.Settings.SettingPage(App.user)) });
                            App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                        }
                        else
                        {
                            await DisplayAlert("Failed!", "Payment failed.", "OK");
                        }
                    });

                    //if (!string.IsNullOrEmpty(data.OrderId) && !string.IsNullOrEmpty(data.PaymentId))
                    //{
                    //    ApiServices apiServices = new ApiServices();

                    //    paymentServerRequestModel.payment_detail = new Paymentdetail
                    //    {
                    //        currency = "INR",
                    //        extended_price = viewModel.package_amount,
                    //        order_id = data.OrderId,
                    //        payment_id = data.PaymentId,
                    //        signature = data.Signature,
                    //        user_contact = data.UserContact,
                    //        user_email = data.UserEmail,
                    //        user_id = App.user_id,
                    //        package_detail = viewModel.selected_package,
                    //    };

                    //    var json = JsonConvert.SerializeObject(paymentServerRequestModel.payment_detail);

                    //    Device.InvokeOnMainThreadAsync(async () =>
                    //    {
                    //        var result = await apiServices.RegisterSubscriptionToServer(App.access_token, paymentServerRequestModel.payment_detail);

                    //        if (!string.IsNullOrEmpty(result?.razorpay_order_id))
                    //        {
                    //            //var cart = await apiServices.DeletePurchaseCart(App.access_token);

                    //            await DisplayAlert("Success!", "Payment Successfully done.", "OK");
                    //            App.paymentResponseModel = null;
                    //            App.Current.MainPage = new NavigationPage(new MasterDetailView(App.user) { Detail = new NavigationPage(new Views.Settings.SettingPage(App.user)) });

                    //        }
                    //        else
                    //        {
                    //            await DisplayAlert("Failed!", "Payment failed.", "OK");
                    //        }
                    //    });


                    //}


                    base.OnAppearing();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async void zxing_OnScanResult(ZXing.Result result)
        {
            try
            {
                zxing.IsAnalyzing = false;
                if (pack != "dongle")
                {
                    string partNo = result.Text.Substring(0, result.Text.IndexOf("*"));
                    string serialNo = result.Text.Substring(result.Text.IndexOf("*") + 1);
                    
                    await viewModel.GetSubscription(serialNo, partNo); 
                }
                else
                {
                    string macId  = result.Text.Substring(result.Text.IndexOf("*") + 1);
                    string partNo = result.Text.Substring(0, result.Text.IndexOf("*"));
                    await viewModel.RegisterDongle(partNo, macId);
                }
            }
            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Alert", "This is not valid code", "Ok");
                });
            }
        }
    }
}