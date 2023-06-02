using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Linq;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectUserTypePopupPage : PopupPage
    {
        SelectUserTypeViewModel viewModel;
        public SelectUserTypePopupPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new SelectUserTypeViewModel();
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            var vm = this.BindingContext as SelectUserTypeViewModel;
            if (vm != null)
            {
                vm.CloseSelectUserTypePopup += Vm_CloseSelectUserTypePopup;
            }
        }

        private async void Vm_CloseSelectUserTypePopup(object sender, EventArgs e)
        {
            await PopupNavigation.Instance.PopAsync();
        }
       
        protected override bool OnBackButtonPressed()
        {
            return true;
        }
    }
}