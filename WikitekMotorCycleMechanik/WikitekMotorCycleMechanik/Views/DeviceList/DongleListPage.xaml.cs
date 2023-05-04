using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.DeviceList
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DongleListPage : ContentPage
    {
        DongleListViewModel viewModel;
        public DongleListPage(VehicleModelResult selected_model, VehicleSubModel selected_submodel, Oem oem)
        {
            InitializeComponent();
            BindingContext = viewModel = new DongleListViewModel(this, string.Empty, selected_model, selected_submodel, oem);
        }


        protected override void OnAppearing()
        {
            App.is_global_method = true;
            DependencyService.Get<IBlueToothDevices>().DisconnectDongle();
            base.OnAppearing();
        }

        protected override void OnDisappearing()
        {
            //App.is_global_method = false;
            DependencyService.Get<IBlueToothDevices>().UnRegisterBluetoothReceiver();
            base.OnDisappearing();
        }
    }
}