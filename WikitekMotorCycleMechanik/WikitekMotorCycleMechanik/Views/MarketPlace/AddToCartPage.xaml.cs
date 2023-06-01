using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.MarketPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddToCartPage : ContentPage
    {
        AddToCartViewModel viewModel;
        public AddToCartPage()
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new AddToCartViewModel(this);
            }
            catch (Exception ex)
            {
            }
        }
    }
}