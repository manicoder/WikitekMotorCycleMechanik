using Rg.Plugins.Popup.Pages;
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
    public partial class DPPackPopupPage : PopupPage
    {
        DPPackViewModel viewModel;
        public DPPackPopupPage(string vehicle)
        {
            InitializeComponent();
            BindingContext = viewModel = new DPPackViewModel(vehicle);
        }

        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send<DPPackPopupPage>(this, "RemoveSelection");
            return base.OnBackButtonPressed();
        }
    }
}