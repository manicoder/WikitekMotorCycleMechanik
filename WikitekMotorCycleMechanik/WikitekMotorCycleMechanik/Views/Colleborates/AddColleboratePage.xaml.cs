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
    public partial class AddColleboratePage : ContentPage
    {
        AddColleborateViewModel viewModel;
        public AddColleboratePage(string dtc_code)
        {
            InitializeComponent();
            BindingContext = viewModel = new AddColleborateViewModel(this, dtc_code);
        }
    }
}