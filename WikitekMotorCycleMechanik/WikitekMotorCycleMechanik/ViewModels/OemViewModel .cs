using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class OemViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler CloseCountryPopup;
        //int segment_id = -1;
        public OemViewModel(int segment_id)
        {
            apiServices = new ApiServices();
            //this.segment_id = segment_id;
            Device.InvokeOnMainThreadAsync(async () =>
            {
                await GetVehicleBrandList(segment_id);
            });
            //oem_list = new ObservableCollection<VehicleBrandModel>()
            //{
            //    new VehicleBrandModel
            //    {
            //        oem_name = "Vehicle Brand_1"
            //    },
            //    new VehicleBrandModel
            //    {
            //        oem_name = "Vehicle Brand_2"
            //    },
            //    new VehicleBrandModel
            //    {
            //        oem_name = "Vehicle Brand_3"
            //    },
            //    new VehicleBrandModel
            //    {
            //        oem_name = "Vehicle Brand_4"
            //    },
            //    new VehicleBrandModel
            //    {
            //        oem_name = "Vehicle Brand_5"
            //    }
            //};

            Device.InvokeOnMainThreadAsync(async () =>
            {
                //await GetVehicleBrandList();
            });

            ClosePopupCommand = new Command(async (obj) =>
            {
                CloseCountryPopup?.Invoke("", new EventArgs());
                //StaticMethods.last_page = "CreateRSAgent";
                //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.CountyPopupPage());
            });
        }

        private ObservableCollection<OemResult> _oem_static_list;
        public ObservableCollection<OemResult> oem_static_list
        {
            get => _oem_static_list;
            set
            {
                _oem_static_list = value;
                OnPropertyChanged("oem_static_list");
            }
        }

        private ObservableCollection<OemResult> _oem_list;
        public ObservableCollection<OemResult> oem_list
        {
            get => _oem_list;
            set
            {
                _oem_list = value;
                OnPropertyChanged("oem_list");
            }
        }

        private OemResult _selected_oem;
        public OemResult selected_oem
        {
            get => _selected_oem;
            set
            {
                _selected_oem = value;

                if (selected_oem != null)
                {
                    if (!string.IsNullOrEmpty(selected_oem.name))
                    {

                        MessagingCenter.Send<OemViewModel, OemResult>(this, "selected_oem", selected_oem);
                        CloseCountryPopup?.Invoke("", new EventArgs());
                    }
                }

                OnPropertyChanged("selected_oem");
            }
        }

        #region Methods

        public async Task GetVehicleBrandList(int segment_id)
        {
            //var response = await apiServices.GetVehicleBrand(StaticMethods.token, segment_id);

            //var api_status_code = StaticMethods.http_status_code(response.status_code);

            ////if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
            ////{
            ////country_list = response.results;

            //oem_list = oem_static_list = new ObservableCollection<OemResult>(response.results);
            ////}
            ////else
            ////{

            ////}
        }

        #endregion

        public ICommand ClosePopupCommand { get; set; }
    }
}
