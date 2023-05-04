using WikitekMotorCycleMechanik.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.ResetPassword
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ResetPasswordPage : ContentPage
    {
        ResetPasswordViewModel viewModel;
        public ResetPasswordPage(string reset_url)
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new ResetPasswordViewModel(this, reset_url);
            }
            catch (Exception ex)
            {
            }
        }


    }
}