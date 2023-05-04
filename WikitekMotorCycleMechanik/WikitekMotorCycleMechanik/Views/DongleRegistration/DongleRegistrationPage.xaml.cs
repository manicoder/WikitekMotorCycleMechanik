using WikitekMotorCycleMechanik.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.DongleRegistration
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DongleRegistrationPage : ContentPage
    {
        DongleRegistrationViewModel viewModel;
        public DongleRegistrationPage(string dongle_type,string part_number, string image)
        {
            InitializeComponent();
            BindingContext = viewModel = new DongleRegistrationViewModel(this,dongle_type, part_number, image);
        }
    }
}