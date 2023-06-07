using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.AssociateVehicleDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AssociateVehicleDetail : ContentPage
    {
        AssociateVehicleDetailViewModel viewModel;
        public AssociateVehicleDetail()
        {
            InitializeComponent();
            BindingContext = viewModel = new AssociateVehicleDetailViewModel(this, null);
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            progressPanel.IsVisible = true;
            progress.IsRunning = true;
            await Task.Delay(100);
            await viewModel.Init();
            progressPanel.IsVisible = false;
            progress.IsRunning = false;
        }
    }
}