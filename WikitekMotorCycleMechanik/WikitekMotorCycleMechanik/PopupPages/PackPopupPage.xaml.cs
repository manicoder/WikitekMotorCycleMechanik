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
    public partial class PackPopupPage : PopupPage
    {
        PackPopupViewModel viewModel;
        public PackPopupPage(string pack)
        {
            InitializeComponent();
            BindingContext = viewModel = new PackPopupViewModel(pack);
        }

        protected override bool OnBackButtonPressed()
        {
            MessagingCenter.Send<PackPopupPage>(this, "RemoveSelection");
            return base.OnBackButtonPressed();
        }
    }
}