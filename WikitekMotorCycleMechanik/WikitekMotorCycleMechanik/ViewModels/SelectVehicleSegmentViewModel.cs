using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class SelectVehicleSegmentViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler CloseSelectVehicleSegmentPopup;

        public SelectVehicleSegmentViewModel()
        {
            apiServices = new ApiServices();
            vehiclesegment_list = new ObservableCollection<VehicleSegmentModel>();
            selected_vehiclesegment = new VehicleSegmentModel();
            Device.InvokeOnMainThreadAsync(() =>
            {
                GetVehicleSegmentList();
            });

            ClosePopupCommand = new Command(async (obj) =>
            {
                CloseSelectVehicleSegmentPopup?.Invoke("", new EventArgs());
                //StaticMethods.last_page = "CreateRSAgent";
                //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.CountyPopupPage());
            });
        }


        private string _search_key;
        public string search_key
        {
            get => _search_key;
            set
            {
                _search_key = value;

                if (!string.IsNullOrEmpty(search_key))
                {
                    vehiclesegment_list = new ObservableCollection<VehicleSegmentModel>(vehiclesegment_static_list.Where(x => x.name.ToLower().Contains(search_key.ToLower())).ToList());
                }

                OnPropertyChanged("search_key");
            }
        }

        private ObservableCollection<VehicleSegmentModel> _vehiclesegment_static_list;
        public ObservableCollection<VehicleSegmentModel> vehiclesegment_static_list
        {
            get => _vehiclesegment_static_list;
            set
            {
                _vehiclesegment_static_list = value;
                OnPropertyChanged("vehiclesegment_static_list");
            }
        }

        private ObservableCollection<VehicleSegmentModel> _vehiclesegment_list;
        public ObservableCollection<VehicleSegmentModel> vehiclesegment_list
        {
            get => _vehiclesegment_list;
            set
            {
                _vehiclesegment_list = value;
                OnPropertyChanged("vehiclesegment_list");
            }
        }

        private string _emptyView = "Loading...";
        public string emptyView
        {
            get => _emptyView;
            set
            {
                _emptyView = value;
                OnPropertyChanged("emptyView");
            }
        }

        private VehicleSegmentModel _selected_vehiclesegment;
        public VehicleSegmentModel selected_vehiclesegment
        {
            get => _selected_vehiclesegment;
            set
            {
                _selected_vehiclesegment = value;

                if (selected_vehiclesegment != null)
                {
                    if (!string.IsNullOrEmpty(selected_vehiclesegment.name))
                    {


                        switch (StaticMethods.last_page)
                        {
                            case "registration":
                                MessagingCenter.Send<SelectVehicleSegmentViewModel, VehicleSegmentModel>(this, "selected_userType_registrationVM", selected_vehiclesegment);
                                break;

                            case "CreateWorkshop":
                                MessagingCenter.Send<SelectVehicleSegmentViewModel, VehicleSegmentModel>(this, "selected_userType_CreateRSAgentVM", selected_vehiclesegment);
                                break;
                            case "billing":
                                MessagingCenter.Send<SelectVehicleSegmentViewModel, VehicleSegmentModel>(this, "selected_userType_billing", selected_vehiclesegment);
                                break;
                        }

                        CloseSelectVehicleSegmentPopup?.Invoke("", new EventArgs());
                    }
                }

                OnPropertyChanged("selected_vehiclesegment");
            }
        }

        #region Methods

        public void GetVehicleSegmentList()
        {

            List<VehicleSegmentModel> vehiclesegmentList = new List<VehicleSegmentModel>();
            VehicleSegmentModel vehicleSegment = new VehicleSegmentModel();
            vehicleSegment.id = 1;
            vehicleSegment.name = "Seg";
            vehiclesegmentList.Add(vehicleSegment);
            VehicleSegmentModel vehicleSegment1 = new VehicleSegmentModel();
            vehicleSegment1.id = 2;
            vehicleSegment1.name = "Segment";
            vehiclesegmentList.Add(vehicleSegment1);

            vehiclesegment_list = vehiclesegment_static_list = new ObservableCollection<VehicleSegmentModel>(vehiclesegmentList);


            //var response = await apiServices.Countries("wikitekMechanik", App.is_update);

            //if (response == null)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
            //    return;
            //}
            //else if (response.count == 0)
            //{
            //    emptyView = "Country list not found";
            //}

            //var api_status_code = StaticMethods.http_status_code(response.status_code);

            //if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
            //{
            //    //country_list = response.results;
            //    usertype_list = usertype_static_list = new ObservableCollection<UserTypeModel>(response.results.First().rs_user_type_country);
            //}
            //else
            //{

            //}


        }

        #endregion

        public ICommand ClosePopupCommand { get; set; }
    }
}
