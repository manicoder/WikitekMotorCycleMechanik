using WikitekMotorCycleMechanik.ViewModels;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WikitekMotorCycleMechanik.Interfaces;

namespace WikitekMotorCycleMechanik.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DongleListPopupPage : PopupPage
    {
        DongleListViewModel viewModel;
        public DongleListPopupPage(string dongle)
        {
            InitializeComponent(); 
            BindingContext = viewModel = new DongleListViewModel(null, dongle,null,null,null);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var vm = this.BindingContext as DongleListViewModel;
            if (vm != null)
            {
                vm.ClosebtPopup += Vm_ClosePopup;
            }
        }

        protected override void OnDisappearing()
        {
            //DependencyService.Get<IBlueToothDevices>().CancleScanning();
            base.OnDisappearing();
        }

        private async void Vm_ClosePopup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}