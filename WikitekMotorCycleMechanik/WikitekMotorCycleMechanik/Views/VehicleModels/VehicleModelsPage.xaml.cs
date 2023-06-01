using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.VehicleModels
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VehicleModelsPage : ContentPage
    {
        VehicleModelsViewModel viewModel;
        public VehicleModelsPage(LoginResponse user_data, Oem oem, ObservableCollection<VehicleModelResult> model_list)
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new VehicleModelsViewModel(this, user_data, oem, model_list);
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
            if (viewModel.submodel_view_visible | viewModel.modelyear_view_visible | viewModel.submitbutton_view_visible)
            {
                viewModel.CancelSelectedModel();
                return true;
            }
            else
            {
                return base.OnBackButtonPressed();
            }
        }
    }
}