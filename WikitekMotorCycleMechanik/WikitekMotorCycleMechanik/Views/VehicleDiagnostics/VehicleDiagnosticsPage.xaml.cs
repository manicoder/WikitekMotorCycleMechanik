using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
using WikitekMotorCycleMechanik.Views.Dashboad;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.VehicleDiagnostics
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehicleDiagnosticsPage : ContentPage
    {
        VehicleDiagnosticsViewModel viewModel;
        public VehicleDiagnosticsPage(LoginResponse user_data)
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new VehicleDiagnosticsViewModel(this, user_data);
            }
            catch (Exception ex)
            {
            }
            
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (string.IsNullOrEmpty(Preferences.Get("list_gui", null)))
            {
                viewModel.bulleted_list_visible = true;
                viewModel.grid_list_visible = false;
                viewModel.change_gui_button = "ic_grid_list.png";
            }
            else if (Preferences.Get("list_gui", null).Contains("tabel_list"))
            {
                viewModel.bulleted_list_visible = false;
                viewModel.grid_list_visible = true;
                viewModel.change_gui_button = "ic_bulleted_list.png";
            }
            else if (Preferences.Get("list_gui", null).Contains("bulleted_list"))
            {
                viewModel.bulleted_list_visible = true;
                viewModel.grid_list_visible = false;
                viewModel.change_gui_button = "ic_grid_list.png";
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                Application.Current.MainPage = new MasterDetailView(App.user) { Detail = new NavigationPage(new DashboadPage(App.user)) };
            });
            return true;
        }
    }
}