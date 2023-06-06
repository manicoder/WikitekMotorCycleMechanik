using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.DiassociateTechnician
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiassociateTechnician : ContentPage
    {
        DiassociateTechnicianViewModel viewModel;
        public DiassociateTechnician()
        {
            InitializeComponent();
            BindingContext = viewModel = new DiassociateTechnicianViewModel(this, null);
        }
        private void Button_Clicked(object sender, EventArgs e)
        {
            otpPanel.IsVisible = true;
        }

        private void otp1_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length == 1)
            {
                otp2.Focus();
            }
        }

        private void otp2_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length == 1)
            {
                otp3.Focus();
            }
        }

        private void otp3_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (e.NewTextValue.Length == 1)
            {
                otp4.Focus();
            }
        }
    }
}