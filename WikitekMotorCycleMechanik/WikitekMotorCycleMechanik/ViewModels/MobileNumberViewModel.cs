using Acr.UserDialogs;
using WikitekMotorCycleMechanik.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class MobileNumberViewModel: ViewModelBase
    {
        ApiServices apiServices;
        public MobileNumberViewModel(Page page) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
            }
            catch
            { }
        }

        #region Properties

        private string _mobile_number;
        public string mobile_number
        {
            get => _mobile_number;
            set
            {
                _mobile_number = value;
                OnPropertyChanged("mobile_number");
            }
        }
        #endregion


        #region ICommands
        public ICommand SubmitCommand => new Command(async (obj) =>
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                try
                {
                    if(string.IsNullOrEmpty(mobile_number))
                    {
                        await page.DisplayAlert("", "Please enter your registered mobile number.", "Ok");
                        return;
                    }

                    if (mobile_number.Length <10)
                    {
                        await page.DisplayAlert("", "Your entered mobile number not correct.", "Ok");
                        return;
                    }

                    string description = "An OTP has been sent to your mobile and will be valid for 10 mins.Pls enter the OTP here";

                    var result = await apiServices.ForgotPasswordMobile(mobile_number);


                    

                    if (result.error == "No internet connection")
                    {
                        await page.DisplayAlert("Network Issue", "No internet connection.", "Ok");
                        return;
                    }

                    


                    if (result?.status_code == System.Net.HttpStatusCode.OK || result?.status_code == System.Net.HttpStatusCode.Created)
                    {
                        if(result.message.Contains("A OTP has been sent to"))
                        {
                            await page.Navigation.PushAsync(new Views.Otp.OtpPage(false, description,mobile_number, "Forgot Password"));
                        }
                    }
                    else if (result?.status_code == System.Net.HttpStatusCode.NotFound)
                    {
                        await page.DisplayAlert("", "Please check entered mobile number\n\nNo user exists with provided mobile number!", "Ok");
                        return;
                    }
                    else
                    {
                        await page.DisplayAlert(Convert.ToString(result?.status_code), result.error, "Ok");
                        return;
                    }

                   
                }
                catch (System.Exception ex)
                {
                }
            }
        });
        #endregion
    }
}
