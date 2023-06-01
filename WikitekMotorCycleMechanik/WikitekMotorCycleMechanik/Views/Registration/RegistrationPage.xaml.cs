using Plugin.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
//using WikitekMotorCycleMechanik.Views.CreateNewWorkshop;
//using WikitekMotorCycleMechanik.Views.Otp;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.Registration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RegistrationPage : ContentPage
    {
        RegistrationViewModel viewModel;
        public RegistrationPage()
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new RegistrationViewModel(this);
                txtFirstName.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeSentence);
                txtLastName.Keyboard = Keyboard.Create(KeyboardFlags.CapitalizeSentence);
            }
            catch (Exception ex)
            {
            }
        }
    }
}