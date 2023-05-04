using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.Colleborates
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ColleboratePage : ContentPage
    {
        ColleborateViewModel viewModel;
        public ColleboratePage(string dtc_code)
        {
            InitializeComponent();
            BindingContext = viewModel = new ColleborateViewModel(this, dtc_code);
        }
    }
}