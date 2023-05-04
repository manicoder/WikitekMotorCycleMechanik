using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.MarketPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShipmentAddressPage : ContentPage
    {
        ShipmentAddressViewModel viewModel;
        public ShipmentAddressPage(ObservableCollection<CartItem> cart_list)
        {
            InitializeComponent();
            BindingContext = viewModel = new ShipmentAddressViewModel(this, cart_list);
        }
    }
}