using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using WikitekMotorCycleMechanik.Views.VehicleDiagnostics;
using Xamarin.Forms;
using WikitekMotorCycleMechanik.Models;
using MultiEventController.Models;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class AppFeatureViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        readonly Page page;
        public AppFeatureViewModel(Page page,string firmware_version, VehicleModelResult selected_model, VehicleSubModel selected_submodel, Oem selected_oem) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                //segment_list = new ObservableCollection<VehicleSegment>();

                this.page = page;
                this.navigationService = page.Navigation;
                this.model_image = model_image;
                this.selected_model = selected_model;
                this.selected_sub_model = selected_submodel;
                this.selected_oem = selected_oem;
                if (firmware_version.Contains("ELM327"))
                {
                    this.firmware_version = firmware_version.Replace("ELM327 v", "WKFV ");
                }
                else
                {
                    this.firmware_version = firmware_version;
                }

                InitializeCommands();
                Device.InvokeOnMainThreadAsync(async () =>
                {
                });

                var pack1 = App.user.subscriptions.FirstOrDefault(x => x.package_name == "AssistedDiagnosticsPack");
                if (pack1 != null)
                {
                    pid_visible = true;
                    dtc_visible = true;
                    return;
                }

                var pack2 = App.user.subscriptions.FirstOrDefault(x => x.package_name == "DIYDiagnosticsPack");
                if (pack2 != null)
                {
                    pid_visible = true;
                    dtc_visible = true;
                    return;
                }

                var pack3 = App.user.subscriptions.FirstOrDefault(x => x.package_name == "BasicPack");
                if (pack3 != null)
                {
                    pid_visible = true;
                    dtc_visible = true;
                    return;
                }
            }
            catch (Exception ex)
            {
            }
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

        private bool _dtc_visible = true;
        public bool dtc_visible
        {
            get => _dtc_visible;
            set
            {
                _dtc_visible = value;
                OnPropertyChanged("dtc_visible");
            }
        }

        private bool _pid_visible = true;
        public bool pid_visible
        {
            get => _pid_visible;
            set
            {
                _pid_visible = value;
                OnPropertyChanged("pid_visible");
            }
        }

        private string _firmware_version;
        public string firmware_version
        {
            get => _firmware_version;
            set
            {
                _firmware_version = value;
                OnPropertyChanged("firmware_version");
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

        #endregion


        #region Methods

        public void InitializeCommands()
        {
            DtcCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(100);
                    if (CurrentUserEvent.Instance.IsRemote && CurrentUserEvent.Instance.IsExpert)
                    {
                        App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                        {
                            ElementName = "DTCFrame",
                            ElementValue = "DTCBtnClicked",
                            ToUserId = CurrentUserEvent.Instance.ToUserId,
                            IsExpert = CurrentUserEvent.Instance.IsExpert
                        });
                    }
                    await page.Navigation.PushAsync(new Views.DtcList.DtcListPage());
                }
            });

            LiveParameterCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(100);
                    if (CurrentUserEvent.Instance.IsRemote && CurrentUserEvent.Instance.IsExpert)
                    {
                        App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                        {
                            ElementName = "LiveParameterFrame",
                            ElementValue = "LiveParameterBtnClicked",
                            ToUserId = CurrentUserEvent.Instance.ToUserId,
                            IsExpert = CurrentUserEvent.Instance.IsExpert
                        });
                    }
                    await page.Navigation.PushAsync(new Views.LiveParameter.LiveParameterPage());
                }
            });

            RemoteDiagnosticCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);
                    //await page.Navigation.PushAsync(new Views.ExpertList.ExpertListPage());
                    await page.DisplayAlert("Coming Soon","This feature will be available soon","Ok");
                }
            });

            BluetoothDisconnectCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);

                    var result = await page.DisplayAlert("Alert", "Do you want to disconnect dongle", "Ok", "Cancel");
                    if (!result)
                        return;

                    App.is_global_method = false;

                    DependencyService.Get<Interfaces.IBlueToothDevices>().DisconnectDongle();
                    MessagingCenter.Send<AppFeatureViewModel>(this, "StartMasterDetailSwip");

                    App.GlobalFuntion();

                    //await navigationService.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new VehicleDiagnosticsPage()) });
                    //await page.Navigation.PushAsync(new Views.DtcList.DtcListPage());
                }
            });
        }

        #endregion

        #region ICommands
        public ICommand DtcCommand { get; set; }
        public ICommand LiveParameterCommand { get; set; }
        public ICommand RemoteDiagnosticCommand { get; set; }
        public ICommand BluetoothDisconnectCommand { get; set; }
        #endregion
    }
}
