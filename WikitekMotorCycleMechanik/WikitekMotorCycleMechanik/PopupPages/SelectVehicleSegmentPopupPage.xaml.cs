using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectVehicleSegmentPopupPage : PopupPage
    {
        SelectVehicleSegmentViewModel viewModel;
        public SelectVehicleSegmentPopupPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SelectVehicleSegmentViewModel();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var vm = this.BindingContext as SelectVehicleSegmentViewModel;
            if (vm != null)
            {
                vm.CloseSelectVehicleSegmentPopup += Vm_CloseSelectVehicleSegmentPopup;
            }
        }

        private async void Vm_CloseSelectVehicleSegmentPopup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}