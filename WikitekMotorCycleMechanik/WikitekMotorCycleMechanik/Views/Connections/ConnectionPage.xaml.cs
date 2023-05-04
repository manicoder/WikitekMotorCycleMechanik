using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using WikitekMotorCycleMechanik.Views.VehicleDiagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.Connections
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConnectionPage : ContentPage
    {
        ConnectViewModel viewModel;
        LoginResponse user_data;
        public ConnectionPage(LoginResponse user_data,VehicleModelResult selected_model, VehicleSubModel selected_submodel, Oem oem)
        {
            try
            {
                InitializeComponent();
                this.user_data = user_data;
                BindingContext = viewModel = new ConnectViewModel(this, user_data, selected_model, selected_submodel, oem);
            }
            catch (Exception ex)
            {
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Application.Current.MainPage = new MasterDetailView(user_data)
            {
                Detail = new NavigationPage(new VehicleDiagnosticsPage(user_data))
            };
            return true;
        }
    }
}