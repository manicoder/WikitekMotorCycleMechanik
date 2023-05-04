using Acr.UserDialogs;
using WikitekMotorCycleMechanik.CustomControls;
using WikitekMotorCycleMechanik.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class ResetPasswordViewModel : ViewModelBase
    {
        ApiServices apiServices;
        public string reset_url;
        public ResetPasswordViewModel(Page page, string reset_url) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                this.reset_url = reset_url;
            }
            catch (Exception ex)
            {
            }
        }


        #region Properties

        private string _new_password;
        public string new_password
        {
            get => _new_password;
            set
            {
                _new_password = value;
                OnPropertyChanged("new_password");
            }
        }

        private string _confirm_password;
        public string confirm_password
        {
            get => _confirm_password;
            set
            {
                _confirm_password = value;
                OnPropertyChanged("confirm_password");
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

                    if (string.IsNullOrEmpty(new_password))
                    {
                        await page.DisplayAlert("", "Enter new password.", "Ok");
                        return;
                    }

                    if (string.IsNullOrEmpty(confirm_password))
                    {
                        await page.DisplayAlert("", "Enter confirm password.", "Ok");
                        return;
                    }

                    if (new_password.Length < 6)
                    {
                        await page.DisplayAlert("", "Minimum 6 digit required.", "Ok");
                        return;
                    }

                    if (confirm_password.Length < 6)
                    {
                        await page.DisplayAlert("", "Minimum 6 digit required.", "Ok");
                        return;
                    }

                    if (!new_password.Equals(confirm_password))
                    {
                        await page.DisplayAlert("", "New password and confirm password is not matching.", "Ok");
                        return;
                    }

                    var result = await apiServices.ResetPassword(reset_url.Replace("http","https"),new_password);

                    if (result.error == "No internet connection")
                    {
                        await page.DisplayAlert("Network Issue", "No internet connection.", "Ok");
                        return;
                    }


                    if (result?.status_code == System.Net.HttpStatusCode.OK || result?.status_code == System.Net.HttpStatusCode.Created)
                    {
                        if (result.message.Contains("Password changed successfully"))
                        {
                            await page.DisplayAlert("", result.message, "Ok");
                            Preferences.Set("user_name", "");
                            Preferences.Set("password", "");
                            Preferences.Set("token", "");
                            Application.Current.MainPage = new CustomNavigationPage(new Views.Login.LoginPage());
                            //await page.Navigation.PushAsync(new ResetPasswordPage(result.reset_url));
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
                        await page.DisplayAlert(Convert.ToString(result?.status_code), result.error, "Ok");
                        return;
                    }

                    //await page.Navigation.PushAsync(new ResetPasswordPage());
                }
                catch (System.Exception ex)
                {
                }
            }
        });
        #endregion
    }
}
