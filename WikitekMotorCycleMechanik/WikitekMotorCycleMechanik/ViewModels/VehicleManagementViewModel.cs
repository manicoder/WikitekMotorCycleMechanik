using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class VehicleManagementViewModel : ViewModelBase
    {
        public VehicleManagementViewModel(Page page, LoginResponse use) : base(page)
        {
            try
            {
                InitializeCommands();
                isshowList = true;
                isshowMap = false;
                toolbarImage = "map.png";
                my_vehicle_list = new ObservableCollection<VehicleListResult>();
                VehicleListResult vehicleList = new VehicleListResult();
                vehicleList.id = "2";
                vehicleList.registration_id = "HR-36";
                vehicleList.status = true;
                my_vehicle_list.Add(vehicleList);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        private ObservableCollection<VehicleListResult> _my_vehicle_list;
        public ObservableCollection<VehicleListResult> my_vehicle_list
        {
            get => _my_vehicle_list;
            set
            {
                _my_vehicle_list = value;
                OnPropertyChanged("my_vehicle_list");
            }
        }

        private VehicleListResult _selected_vehicle;
        public VehicleListResult selected_vehicle
        {
            get => _selected_vehicle;
            set
            {
                _selected_vehicle = value;
                //App.selected_vehicle = value.id;
                OnPropertyChanged("selected_vehicle");
            }
        }

        private string _selected_vehicle_picture;
        public string selected_vehicle_picture
        {
            get => _selected_vehicle_picture;
            set
            {
                _selected_vehicle_picture = value;
                OnPropertyChanged("selected_vehicle_picture");
            }
        }


        private bool _isshowMap;
        public bool isshowMap
        {
            get => _isshowMap;
            set
            {
                _isshowMap = value;
                OnPropertyChanged("isshowMap");
            }
        }

        private string _toolbarImage;
        public string toolbarImage
        {
            get => _toolbarImage;
            set
            {
                _toolbarImage = value;
                OnPropertyChanged("toolbarImage");
            }
        }

        private bool _isshowList;
        public bool isshowList
        {
            get => _isshowList;
            set
            {
                _isshowList = value;
                OnPropertyChanged("isshowList");
            }
        }
        #region Methods
        [Obsolete]
        public void InitializeCommands()
        {
            ToolbarImageCommand = new Command((obj) =>
            {
                if (toolbarImage == "map.png")
                {
                    isshowList = false;
                    isshowMap = true;
                    toolbarImage = "list.png";
                }
                else if (toolbarImage == "list.png")
                {
                    isshowList = true;
                    isshowMap = false;
                    toolbarImage = "map.png";
                }
            });

            AddNewAssociateVehicleCommand = new Command( async(obj) =>
            {
                try
                {
                    await this.page.Navigation.PushAsync(new Views.AssociateVehicle.AssociateVehicle());
                }
                catch (Exception ex)
                {
                }
            });
        }
        #endregion

        #region ICommands
        public ICommand ToolbarImageCommand { get; set; }
        public ICommand AddNewAssociateVehicleCommand { get; set; }
        #endregion
    }
}
