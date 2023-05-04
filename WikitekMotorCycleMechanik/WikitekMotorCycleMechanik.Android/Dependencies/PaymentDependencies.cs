using Com.Razorpay;
using Org.Json;
using System;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Droid.Dependencies;
using WikitekMotorCycleMechanik.Models;
using Xamarin.Forms;
using WikitekMotorCycleMechanik.Interfaces;

[assembly: Dependency(typeof(PaymentDependencies))]
namespace WikitekMotorCycleMechanik.Droid.Dependencies
{
    public class PaymentDependencies : IPayment
    {
        public void OnPaymentError(int p0, string p1, PaymentData p2)
        {
            Console.WriteLine("error in payment");
        }
        public void OnPaymentSuccess(string p0, PaymentData p1)
        {
            PaymentResponseModel model = new PaymentResponseModel
            {
                Data = p1.Data,
                ExternalWallet = p1.ExternalWallet,
                OrderId = p1.OrderId,
                PaymentId = p1.PaymentId,
                Signature = p1.Signature,
                UserContact = p1.UserContact,
                UserEmail = p1.UserEmail,
            };
            Console.WriteLine("success");
        }

        public async Task<bool> StartPaymet(GenerateOrderIdResponseModel model)
        {
            try
            {
                //var time = DateTime.Now.ToString("MMddyyyyHHmmss");
                //GenerateOrderIdModel model = new GenerateOrderIdModel
                //{
                //    amount = 100,
                //    currency = "INR",
                //    receipt = $"reciept_{time}",
                //};

                //ApiServices api = new ApiServices();
                //var response = await api.GenerateOrderId(model);

                var mainActivity = MainActivity.Instance;
                if (mainActivity == null)
                    return false;


                Checkout checkOut = new Checkout();
                checkOut.SetKeyID("rzp_live_r21sPXRDz2eJvu");
                var activity = Android.App.Application.Context;

                JSONObject options = new JSONObject();
                options.Put("name", "WikitekMotorCycleMechanik Systems");
                options.Put("description", "Test Payment");
                options.Put("order_id", model.id);
                options.Put("currency", $"{model.currency}");
                options.Put("amount", $"{model.amount}");
                options.Put("payment_capture", "1");
                checkOut.Open(mainActivity, options);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("error in payment");
                return false;
            }
        }

     }
}