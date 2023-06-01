using WikitekMotorCycleMechanik.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.NewWorkshop
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewWorkshopPage : ContentPage
    {
        NewWorkshopViewModel viewModel;
        public NewWorkshopPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new NewWorkshopViewModel(this);
        }
    }
}