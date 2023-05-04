using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class VehicleModelYearViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler CloseCountryPopup;
        public VehicleModelYearViewModel(List<VehicleSubModel> sub_models, int SubModelId)
        {
            try
            {
                apiServices = new ApiServices();
                var SubModelYear = new List<VehicleSubModel>();
                SubModelYear.Add(new VehicleSubModel
                {
                    ecus = sub_models.FirstOrDefault(x => x.id == SubModelId).ecus,
                    id = sub_models.FirstOrDefault(x => x.id == SubModelId).id,
                    //engine_sl_no = sub_models.FirstOrDefault(x => x.id == SubModelId).engine_sl_no,
                    //injector_charecterization = sub_models.FirstOrDefault(x => x.id == SubModelId).injector_charecterization,
                    model_year = sub_models.FirstOrDefault(x => x.id == SubModelId).model_year,
                    name = sub_models.FirstOrDefault(x => x.id == SubModelId).name,
                    segment_name = sub_models.FirstOrDefault(x => x.id == SubModelId).segment_name,
                    selected_sub_model = sub_models.FirstOrDefault(x => x.id == SubModelId).selected_sub_model,
                });
                vehicle_model_year_list = new ObservableCollection<VehicleSubModel>(SubModelYear);


                ClosePopupCommand = new Command(async (obj) =>
                {
                    CloseCountryPopup?.Invoke("", new EventArgs());
                });

                OkCommand = new Command(async (obj) =>
                {
                    CloseCountryPopup?.Invoke("", new EventArgs());

                    var item = vehicle_model_year_list.FirstOrDefault(x => x.selected_sub_model == true);

                    if (item == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "Please select a sub model", "Ok");
                        return;
                    }
                    else
                    {
                        MessagingCenter.Send<VehicleModelYearViewModel, VehicleSubModel>(this, "selected_vehicle_model_year", item);
                        MessagingCenter.Unsubscribe<VehicleModelYearViewModel, VehicleSubModel>(this, "selected_vehicle_model_year");
                    }

                });

                CancelCommand = new Command(async (obj) =>
                {
                    CloseCountryPopup?.Invoke("", new EventArgs());
                });
            }
            catch (Exception ex)
            {
            }
        }


        private ObservableCollection<VehicleSubModel> _vehicle_model_year_static_list;
        public ObservableCollection<VehicleSubModel> vehicle_model_year_static_list
        {
            get => _vehicle_model_year_static_list;
            set
            {
                _vehicle_model_year_static_list = value;
                OnPropertyChanged("vehicle_model_year_static_list");
            }
        }

        private ObservableCollection<VehicleSubModel> _vehicle_model_year_list;
        public ObservableCollection<VehicleSubModel> vehicle_model_year_list
        {
            get => _vehicle_model_year_list;
            set
            {
                _vehicle_model_year_list = value;
                OnPropertyChanged("vehicle_model_year_list");
            }
        }

        private VehicleSubModel _selected_model_year;
        public VehicleSubModel selected_model_year
        {
            get => _selected_model_year;
            set
            {
                _selected_model_year = value;

                if (selected_model_year != null)
                {
                    if (!string.IsNullOrEmpty(selected_model_year.name))
                    {

                        MessagingCenter.Send<VehicleModelYearViewModel, VehicleSubModel>(this, "selected_vehicle_model_year", selected_model_year);
                        //switch (StaticMethods.last_page)
                        //{
                        //    case "registration":
                        //        MessagingCenter.Send<VehicleModelYearViewModel, VehicleModelYearModel>(this, "selected_vehicle_model_year", selected_model_year);
                        //        break;

                        //        //case "CreateRSAgent":
                        //        //    MessagingCenter.Send<CountyViewModel, RsUserTypeCountry>(this, "selected_country_CreateRSAgentVM", selected_country);
                        //        //    break;
                        //}

                        //CloseCountryPopup?.Invoke("", new EventArgs());
                    }
                }

                OnPropertyChanged("selected_country");
            }
        }

        #region Methods

        //public async Task GetModelYearList()
        //{
        //    var response = await apiServices.ModelYears("miBud");

        //    var api_status_code = StaticMethods.http_status_code(response.status_code);

        //    if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
        //    {
        //        //country_list = response.results;
        //        vehicle_model_year_list = vehicle_model_year_static_list = new ObservableCollection<VehicleModelYearModel>(response.results.First().rs_user_type_country);
        //    }
        //    else
        //    {

        //    }


        //}

        #endregion

        public ICommand ClosePopupCommand { get; set; }

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }
    }
}

