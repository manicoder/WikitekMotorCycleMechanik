using Acr.UserDialogs;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.Views.PrivacyPolicy;
using WikitekMotorCycleMechanik.Views.ResetPassword;
using WikitekMotorCycleMechanik.Views.TermsAndConditions;
using System;
using System.Windows.Input;
using Xamarin.Forms;
using WikitekMotorCycleMechanik.CustomControls;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class OtpViewModel : ViewModelBase
    {
        ApiServices apiServices;
        public OtpViewModel(Page page, bool term_condition_visible, string description, string mobile_number, string title) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                this.description = description;
                this.term_condition_visible = term_condition_visible;
                this.mobile_number = mobile_number;
                this.title = title;
            }
            catch
            { }
        }

        #region Properties
        private string _title;
        public string title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("title");
            }
        }
        private string _description;
        public string description
        {
            get => _description;
            set
            {
                _description = value;
                OnPropertyChanged("description");
            }
        }

        private bool _term_condition_visible;
        public bool term_condition_visible
        {
            get => _term_condition_visible;
            set
            {
                _term_condition_visible = value;
                OnPropertyChanged("term_condition_visible");
            }
        }

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

        private string _first_digit;
        public string first_digit
        {
            get => _first_digit;
            set
            {
                _first_digit = value;
                OnPropertyChanged("first_digit");
            }
        }

        private string _second_digit;
        public string second_digit
        {
            get => _second_digit;
            set
            {
                _second_digit = value;
                OnPropertyChanged("second_digit");
            }
        }

        private string _third_digit;
        public string third_digit
        {
            get => _third_digit;
            set
            {
                _third_digit = value;
                OnPropertyChanged("third_digit");
            }
        }

        private string _fourth_digit;
        public string fourth_digit
        {
            get => _fourth_digit;
            set
            {
                _fourth_digit = value;
                OnPropertyChanged("fourth_digit");
            }
        }

        private string _fifth_digit;
        public string fifth_digit
        {
            get => _fifth_digit;
            set
            {
                _fifth_digit = value;
                OnPropertyChanged("fifth_digit");
            }
        }

        private string _six_digit;
        public string six_digit
        {
            get => _six_digit;
            set
            {
                _six_digit = value;
                OnPropertyChanged("six_digit");
            }
        }

        private string oTP;
        public string OTP
        {
            get => oTP;
            set
            {
                oTP = value;
                OnPropertyChanged("OTP");
            }
        }
        #endregion

        #region ICommands
        public ICommand TermConditionCommand => new Command(async (obj) =>
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                try
                {
                    await page.Navigation.PushAsync(new TermsAndConditionsPage());
                }
                catch (System.Exception ex)
                {
                }
            }
        });

        public ICommand PrivacyPolicyCommand => new Command(async (obj) =>
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                try
                {
                    await page.Navigation.PushAsync(new PrivacyPolicyPage());
                }
                catch (System.Exception ex)
                {
                }
            }
        });

        public ICommand SubmitCommand => new Command(async (obj) =>
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                try
                {

                    if (string.IsNullOrEmpty(OTP))
                    {
                        await page.DisplayAlert("", "Enter 6 digit OTP.", "Ok");
                        return;
                    }

                    if (OTP.Length<6)
                    {
                        await page.DisplayAlert("", "Enter 6 digit OTP.", "Ok");
                        return;
                    }

                    if (title == "Forgot Password")
                    {
                        var result = await apiServices.VerifyOTP(mobile_number, OTP);

                        if (result.error == "No internet connection")
                        {
                            await page.DisplayAlert("Network Issue", "No internet connection.", "Ok");
                            return;
                        }


                        if (result?.status_code == System.Net.HttpStatusCode.OK || result?.status_code == System.Net.HttpStatusCode.Created)
                        {
                            if (result.success)
                            {
                                await page.Navigation.PushAsync(new ResetPasswordPage(result.reset_url));
                            }

                            else
                            {
                                if (result.message.Contains("Invalid OTP request"))
                                {
                                    await page.DisplayAlert("", "Invalid OTP request!", "Ok");
                                }
                            }
                        }
                        else if (result?.status_code == System.Net.HttpStatusCode.NotFound)
                        {
                            await page.DisplayAlert("", "Please check entered mobile number\n\nNo user exists with provided mobile number!", "Ok");
                            return;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(result.error) && !string.IsNullOrEmpty(result.message))
                            {
                                await page.DisplayAlert("", $"{result.message}\n\n{result.message}", "Ok");
                            }
                            else if (string.IsNullOrEmpty(result.error) && string.IsNullOrEmpty(result.message))
                            {
                                await page.DisplayAlert("", $"Somthing Invalid", "Ok");
                            }
                            else if (!string.IsNullOrEmpty(result.error) && string.IsNullOrEmpty(result.message))
                            {
                                await page.DisplayAlert("", result.error, "Ok");
                            }
                            else if (string.IsNullOrEmpty(result.error) && !string.IsNullOrEmpty(result.message))
                            {
                                await page.DisplayAlert("", result.message, "Ok");
                            }
                            return;
                        }

                        //await page.Navigation.PushAsync(new ResetPasswordPage());
                    }
                    else
                    {
                        var result = await apiServices.VerifyRegisteredMobileNumber(mobile_number, OTP);

                        if (result.error == "No internet connection")
                        {
                            await page.DisplayAlert("Network Issue", "No internet connection.", "Ok");
                            return;
                        }


                        if (result?.status_code == System.Net.HttpStatusCode.OK || result?.status_code == System.Net.HttpStatusCode.Created)
                        {
                            if (result.success)
                            {
                                await page.DisplayAlert("", result.message, "Ok");
                                Application.Current.MainPage = new CustomNavigationPage(new Views.Login.LoginPage());
                                //await page.Navigation.PushAsync(new ResetPasswordPage(result.reset_url));
                            }
                            else
                            {

                                await page.DisplayAlert("", result.message, "Ok");
                                await page.Navigation.PopAsync();
                            }
                        }
                        else if (result?.status_code == System.Net.HttpStatusCode.NotFound)
                        {
                            await page.DisplayAlert("", "Please check entered mobile number\n\nNo user exists with provided mobile number!", "Ok");
                            return;
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(result.error) && !string.IsNullOrEmpty(result.message))
                            {
                                await page.DisplayAlert("", $"{result.message}\n\n{result.message}", "Ok");
                            }
                            else if (string.IsNullOrEmpty(result.error) && string.IsNullOrEmpty(result.message))
                            {
                                await page.DisplayAlert("", $"Somthing Invalid", "Ok");
                            }
                            else if (!string.IsNullOrEmpty(result.error) && string.IsNullOrEmpty(result.message))
                            {
                                await page.DisplayAlert("", result.error, "Ok");
                            }
                            else if (string.IsNullOrEmpty(result.error) && !string.IsNullOrEmpty(result.message))
                            {
                                await page.DisplayAlert("", result.message, "Ok");
                            }
                            return;
                        }

                        //await page.Navigation.PushAsync(new ResetPasswordPage());
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
