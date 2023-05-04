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
    public class VehicleSubModelViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler CloseCountryPopup;

        public VehicleSubModelViewModel(int model_id, List<VehicleSubModel> sub_models)
        {
            apiServices = new ApiServices();

            vehicle_sub_model_list = new ObservableCollection<VehicleSubModel>(sub_models);
            //vehicle_sub_model_list = new ObservableCollection<VehicleSubModelModel>()
            //{
            //    new VehicleSubModelModel
            //    {
            //       vehicle_sub_model= "Vehicle Sub Model_1"
            //    },
            //    new VehicleSubModelModel
            //    {
            //       vehicle_sub_model= "Vehicle Sub Model_2"
            //    },
            //    new VehicleSubModelModel
            //    {
            //       vehicle_sub_model= "Vehicle Sub Model_3"
            //    },
            //    new VehicleSubModelModel
            //    {
            //       vehicle_sub_model= "Vehicle Sub Model_4"
            //    },
            //    new VehicleSubModelModel
            //    {
            //       vehicle_sub_model= "Vehicle Sub Model_5"
            //    },
            //};

            Device.InvokeOnMainThreadAsync(async () =>
            {
                await GetVehicleSubModelList(model_id);
            });

            ClosePopupCommand = new Command(async (obj) =>
            {
                CloseCountryPopup?.Invoke("", new EventArgs());
                //StaticMethods.last_page = "CreateRSAgent";
                //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.CountyPopupPage());
            });

            OkCommand = new Command(async (obj) =>
            {
                CloseCountryPopup?.Invoke("", new EventArgs());

                var item = vehicle_sub_model_list.FirstOrDefault(x=>x.selected_sub_model ==true);

                if (item == null)
                {
                    await Application.Current.MainPage.DisplayAlert("Alert","Please select a sub model","Ok");
                    return;
                }
                else
                {
                    MessagingCenter.Send<VehicleSubModelViewModel, VehicleSubModel>(this, "selected_vehicle_sub_model", item);
                    MessagingCenter.Unsubscribe<VehicleSubModelViewModel, VehicleSubModel>(this, "selected_vehicle_sub_model");
                }

            });

            CancelCommand = new Command(async (obj) =>
            {
                CloseCountryPopup?.Invoke("", new EventArgs());
            });
        }

        private ObservableCollection<VehicleSubModel> _vehicle_sub_model_static_list;
        public ObservableCollection<VehicleSubModel> vehicle_sub_model_static_list
        {
            get => _vehicle_sub_model_static_list;
            set
            {
                _vehicle_sub_model_static_list = value;
                OnPropertyChanged("vehicle_sub_model_static_list");
            }
        }

        private ObservableCollection<VehicleSubModel> _vehicle_sub_model_list;
        public ObservableCollection<VehicleSubModel> vehicle_sub_model_list
        {
            get => _vehicle_sub_model_list;
            set
            {
                _vehicle_sub_model_list = value;
                OnPropertyChanged("vehicle_sub_model_list");
            }
        }

        private VehicleSubModel _selected_sub_model;
        public VehicleSubModel selected_sub_model
        {
            get => _selected_sub_model;
            set
            {
                _selected_sub_model = value;

                if (selected_sub_model != null)
                {

                    //MessagingCenter.Send<VehicleSubModelViewModel, VehicleSubModel>(this, "selected_vehicle_sub_model", selected_sub_model);
                    //switch (StaticMethods.last_page)
                    //{
                    //    case "registration":
                    //        MessagingCenter.Send<VehicleSubModelViewModel, VehicleSubModelModel>(this, "selected_vehicle_sub_model", selected_sub_model);
                    //        break;

                    //        //case "CreateRSAgent":
                    //        //    MessagingCenter.Send<CountyViewModel, RsUserTypeCountry>(this, "selected_country_CreateRSAgentVM", selected_country);
                    //        //    break;
                    //}

                    //CloseCountryPopup?.Invoke("", new EventArgs());
                }

                OnPropertyChanged("selected_sub_model");
            }
        }

        #region Methods

        public async Task GetVehicleSubModelList(int model_id)
        {
            ////var response = await apiServices.GetVehicleSubModel(StaticMethods.token, model_id);

            //////var api_status_code = StaticMethods.http_status_code(response.status_code);

            //////if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
            //////{
            //////country_list = response.results;
            ////vehicle_sub_model_list = vehicle_sub_model_static_list = new ObservableCollection<SubModelResult>(response.results);
            //////}
            //////else
            //////{
            //////}

        }

        #endregion

        public ICommand ClosePopupCommand { get; set; }

        public ICommand OkCommand { get; set; }

        public ICommand CancelCommand { get; set; }
    }
}

