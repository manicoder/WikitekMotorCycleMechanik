using WikitekMotorCycleMechanik.ViewModels;
using WikitekMotorCycleMechanik.Views.PrivacyPolicy;
using WikitekMotorCycleMechanik.Views.ResetPassword;
using WikitekMotorCycleMechanik.Views.TermsAndConditions;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.Otp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OtpPage : ContentPage
    {
        OtpViewModel viewModel;
        bool forgot_pass_page = false;
        public OtpPage(bool term_condition_visible,string description,string mobile_number, string title)
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new OtpViewModel(this, term_condition_visible, description, mobile_number, title);
            }
            catch (Exception ex)
            {
            }
        }
    }
}