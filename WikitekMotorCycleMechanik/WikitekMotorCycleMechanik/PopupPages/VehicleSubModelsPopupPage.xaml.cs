using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehicleSubModelsPopupPage : PopupPage
    {
        VehicleSubModelViewModel viewModel;
        public VehicleSubModelsPopupPage(int model_id, List<VehicleSubModel> sub_models)
        {
            InitializeComponent();
            BindingContext = viewModel = new VehicleSubModelViewModel(model_id,sub_models);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var vm = this.BindingContext as VehicleSubModelViewModel;
            if (vm != null)
            {
                vm.CloseCountryPopup += Vm_CloseCountryPopup;
            }
        }

        private async void Vm_CloseCountryPopup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return false;
        }
    }
}