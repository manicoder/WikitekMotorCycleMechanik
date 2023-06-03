using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class VehicleServiceViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        Vehicle selected_vehicle;
        //MediaFile file = null;

        public VehicleServiceViewModel(Page page, Vehicle selected_vehicle) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                GetJobcardList();
                //this.navigationService = page.Navigation;
                this.selected_vehicle = selected_vehicle;
                //InitializeCommands();
                //vehicle_service_list = new ObservableCollection<VehicleServiceModel>
                //{
                //    new VehicleServiceModel
                //    {
                //        image = "ic_user.png",
                //        veh_no = "vehicle_number_1",
                //        model="vehicle_model_submodel_year_1",
                //        Inr="Inr_1",
                //        status="status_1"
                //    },
                //    new VehicleServiceModel
                //    {
                //        image = "ic_user.png",
                //        veh_no = "vehicle_number_2",
                //        model="vehicle_model_submodel_year_2",
                //        Inr="Inr_1",
                //        status="status_2"
                //    },
                //    new VehicleServiceModel
                //    {
                //        image = "ic_user.png",
                //        veh_no = "vehicle_number_3",
                //        model="vehicle_model_submodel_year_3",
                //        Inr="Inr_1",
                //        status="status_3"
                //    },
                //    new VehicleServiceModel
                //    {
                //        image = "ic_user.png",
                //        veh_no = "vehicle_number_4",
                //        model="vehicle_model_submodel_year_4",
                //        Inr="Inr_1",
                //        status="status_4"
                //    },
                //};
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties

        private Color _selectedBgColor;
        public Color selectedBgColor
        {
            get => _selectedBgColor;
            set
            {
                _selectedBgColor = value;
                OnPropertyChanged("selectedBgColor");
            }
        }


        private ObservableCollection<JobcardResult> _jobcard_list;
        public ObservableCollection<JobcardResult> jobcard_list
        {
            get { return _jobcard_list; }
            set
            {
                _jobcard_list = value;
                OnPropertyChanged("jobcard_list");
            }
        }

        private JobcardResult _selected_jobcard;
        public JobcardResult selected_jobcard
        {
            get { return _selected_jobcard; }
            set
            {
                _selected_jobcard = value;
                OnPropertyChanged("selected_jobcard");
            }
        }
        #endregion

        #region Methods
        public async void GetJobcardList()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(100);

                    var result = await apiServices.GetJobcardList(Xamarin.Essentials.Preferences.Get("token", null));

                    if (!result.status)
                    {
                        //DependencyService.Get<Interfaces.IToasts>().Show($"{ mode.status}");
                        UserDialogs.Instance.Toast(result.message, new TimeSpan(0, 0, 0, 3));
                        return;
                    }

                    if (result.results == null || (!result.results.Any()))
                    {
                        UserDialogs.Instance.Toast("Jobcard not found", new TimeSpan(0, 0, 0, 3));
                        return;
                    }

                    jobcard_list = new ObservableCollection<JobcardResult>(result.results.ToList());
                }
            });
        }

        #endregion

        #region ICommands
        public ICommand GoToJobcardDetailCommand => new Command(async (obj) =>
        {

            try
            {
                selected_jobcard = (JobcardResult)obj;
                //await page.Navigation.PushAsync(new Views.VehicleService.ServiceDetailPage(selected_jobcard));
            }
            catch (Exception ex)
            {
            }
        });

        public ICommand AddNewVehicleServiceCommand => new Command(async (obj) =>
        {
           // await page.Navigation.PushAsync(new Views.CreateServiceTicket.CreateServiceTicketPage(selected_vehicle));
        });
        #endregion
    }
}
