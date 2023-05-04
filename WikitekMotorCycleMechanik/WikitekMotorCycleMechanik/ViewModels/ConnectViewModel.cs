using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using Xamarin.Forms;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using WikitekMotorCycleMechanik.Views.Settings;
using WikitekMotorCycleMechanik.Views.VehicleDiagnostics;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class ConnectViewModel : ViewModelBase
    {
        LoginResponse user_data;
        public ConnectViewModel(Page page, LoginResponse user_data, VehicleModelResult selected_model, VehicleSubModel selected_submodel, Oem oem) : base(page)
        {
            this.user_data = user_data;
            this.model_image = selected_model.model_file_lacal;
            this.selected_model = selected_model;
            this.selected_sub_model = selected_submodel;
            this.selected_oem = oem;
            SetConnectorProperty();
        }


        #region Properties
        private byte[] _model_image;
        public byte[] model_image
        {
            get => _model_image;
            set
            {
                _model_image = value;
                OnPropertyChanged("model_image");
            }
        }

        private VehicleModelResult _selected_model;
        public VehicleModelResult selected_model
        {
            get => _selected_model;
            set
            {
                _selected_model = value;
                OnPropertyChanged("selected_model");
            }
        }

        private VehicleSubModel _selected_sub_model;
        public VehicleSubModel selected_sub_model
        {
            get => _selected_sub_model;
            set
            {
                _selected_sub_model = value;
                OnPropertyChanged("selected_sub_model");
            }
        }

        private Oem _selected_oem;
        public Oem selected_oem
        {
            get => _selected_oem;
            set
            {
                _selected_oem = value;
                OnPropertyChanged("selected_oem");
            }
        }

        private bool _connector_view_visible = true;
        public bool connector_view_visible
        {
            get => _connector_view_visible;
            set
            {
                _connector_view_visible = value;
                OnPropertyChanged("connector_view_visible");
            }
        }

        private string _connector_image;
        public string connector_image
        {
            get => _connector_image;
            set
            {
                _connector_image = value;
                OnPropertyChanged("connector_image");
            }
        }

        private string _connector_message;
        public string connector_message
        {
            get => _connector_message;
            set
            {
                _connector_message = value;
                OnPropertyChanged("connector_message");
            }
        }
        #endregion


        #region Methods
        public void SetConnectorProperty()
        {
            try
            {
                switch (selected_sub_model.connector_type)
                {
                    case "CONNECTOR1_WHITE_6PIN":
                        connector_image = "wtk_conn_one.jpg";
                        connector_message = "Use 6 pin connector 1";
                        break;

                    case "CONNECTOR2_WHITE_6PIN":
                        connector_image = "wtk_conn_two.jpg";
                        connector_message = "Use 6 pin connector 2";
                        break;

                    case "CONNECTOR3_WHITE_6PIN":
                        connector_image = "wtk_conn_three.jpg";
                        connector_message = "Use 6 pin connector 3";
                        break;

                    case "CONNECTOR4_RED_6PIN":
                        connector_image = "wtk_conn_four.jpg";
                        connector_message = "Use 6 pin connector 4";
                        break;

                    case "OBD2_16PIN_TYPEA":
                        connector_image = "wtk_conn_obdtype.jpg";
                        connector_message = "Use 16 pin OBD connector";
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion


        #region ICommands
        public ICommand BackCommand => new Command(async (obj) =>
        {
            Application.Current.MainPage = new MasterDetailView(user_data)
            {
                Detail = new NavigationPage(new VehicleDiagnosticsPage(user_data))
            };
            //await page.Navigation.PopAsync();
        });
        public ICommand BluetoothCommand => new Command(async (obj) =>
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                await Task.Delay(200);
                if (App.user.segment == "CV" || App.user.segment == "BUS")
                {
                    var register_dongle = App.user.dongles.FirstOrDefault(x => x.device_type.Contains("24v"));
                    App.dongle_type = "OBDII BT Dongle";
                    if (register_dongle == null)
                    {
                        await page.DisplayAlert("Alert", $"Pls register your dongle before you can connect to it. Pls go to the settings page to register your dongle !!", "Ok");
                        await page.Navigation.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new SettingPage(App.user)) });
                        return;
                    }
                }
                else
                {
                    //var register_dongle = App.user.dongles.FirstOrDefault(x => x.device_type.Contains("wkbtd001"));
                    var register_dongle = App.user.dongles.FirstOrDefault();
                    App.dongle_type = "wikitek";
                    if (register_dongle == null)
                    {
                        await page.DisplayAlert("Alert", $"Pls register your dongle before you can connect to it. Pls go to the settings page to register your dongle !!", "Ok");
                        await page.Navigation.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new SettingPage(App.user)) });
                        return;
                    }
                }
                //DependencyService.Get<Interfaces.IBlueToothDevices>().DisconnectDongle();
                await page.Navigation.PushAsync(new Views.DeviceList.DongleListPage(selected_model,selected_sub_model, selected_oem));
            }
        });

        public ICommand OkCommand => new Command(async (obj) =>
        {
            connector_view_visible = false;
        });
        #endregion
    }
}