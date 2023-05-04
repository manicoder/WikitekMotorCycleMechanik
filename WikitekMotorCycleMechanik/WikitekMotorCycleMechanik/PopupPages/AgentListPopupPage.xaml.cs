using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using WikitekMotorCycleMechanik.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AgentListPopupPage : PopupPage
    {
        AgentViewModel viewModel;
        public AgentListPopupPage(int country_id, string pin_code)
        {
            InitializeComponent();

            BindingContext = viewModel = new AgentViewModel(country_id, pin_code);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var vm = this.BindingContext as AgentViewModel;
            if (vm != null)
            {
                vm.CloseAgentListPopup += Vm_CloseAgentListPopup;
            }
        }

        private async void Vm_CloseAgentListPopup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }

        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}