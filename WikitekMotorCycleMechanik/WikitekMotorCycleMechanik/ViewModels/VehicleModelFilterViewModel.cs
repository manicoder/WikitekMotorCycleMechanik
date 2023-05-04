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
    public class VehicleModelFilterViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler CloseCountryPopup;
        public VehicleModelFilterViewModel(ObservableCollection<VehicleModelResult> model_list)
        {
            apiServices = new ApiServices();
            try
            {
                vehicle_model_list = new ObservableCollection<VehModel>();
                this.model_list = model_list;

                foreach (var item in model_list)
                {
                    vehicle_model_list.Add(
                        new VehModel
                        {
                            model_id = item.id,
                            name = $"{item.name} - {item.sub_models.FirstOrDefault().name} - {item.sub_models.FirstOrDefault().model_year}",
                            model_image = item.model_file,
                            sub_model_id = item.sub_models.FirstOrDefault().id,
                        });
                }

                ClosePopupCommand = new Command(async (obj) =>
                {
                    CloseCountryPopup?.Invoke("", new EventArgs());
                });
            }
            catch (Exception)
            {
            }
        }

        private ObservableCollection<VehicleModelResult> _model_list;
        public ObservableCollection<VehicleModelResult> model_list
        {
            get => _model_list;
            set
            {
                _model_list = value;
                OnPropertyChanged("model_list");
            }
        }

        private ObservableCollection<VehModel> _vehicle_model_static_list;
        public ObservableCollection<VehModel> vehicle_model_static_list
        {
            get => _vehicle_model_static_list;
            set
            {
                _vehicle_model_static_list = value;
                OnPropertyChanged("vehicle_model_static_list");
            }
        }

        private ObservableCollection<VehModel> _vehicle_model_list;
        public ObservableCollection<VehModel> vehicle_model_list
        {
            get => _vehicle_model_list;
            set
            {
                _vehicle_model_list = value;
                OnPropertyChanged("vehicle_model_list");
            }
        }



        private VehModel _selected_model;
        public VehModel selected_model
        {
            get => _selected_model;
            set
            {
                _selected_model = value;
                try
                {
                    if (_selected_model != null)
                    {
                        var item = model_list.FirstOrDefault(x => x.id == selected_model.model_id && x.sub_models.FirstOrDefault().id == selected_model.sub_model_id);

                        MessagingCenter.Send<VehicleModelFilterViewModel, VehicleModelResult>(this, "selected_vehicle_model", item);
                        CloseCountryPopup?.Invoke("", new EventArgs());
                    }

                }
                catch (Exception ex)
                {
                }
                OnPropertyChanged("selected_model");
            }
        }


        #region Methods

        #endregion

        public ICommand ClosePopupCommand { get; set; }
    }

    public class VehModel
    {
        public string model_image { get; set; }
        public string name { get; set; }
        public int model_id { get; set; }
        public int sub_model_id { get; set; }
    }
}
