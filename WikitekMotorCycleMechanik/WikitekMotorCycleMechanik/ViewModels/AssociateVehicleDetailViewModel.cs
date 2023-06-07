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
    public class AssociateVehicleDetailViewModel : ViewModelBase
    {
        ApiServices1 apiServices;
        public AssociateVehicleDetailViewModel(Page page, LoginResponse use) : base(page)
        {
            InitializeCommands();
            apiServices = new ApiServices1();
        }
        public async Task Init()
        {
            try
            {
                var msgs = await apiServices.VehicleList();
                Vehicles = new ObservableCollection<VehicleList>(msgs.results);
                SelectedVehicle = Vehicles.Where(x => x.registration_id == App.AssociateVehicleId).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private VehicleList mSelectedVehicle;
        public VehicleList SelectedVehicle
        {
            get { return mSelectedVehicle; }
            set
            {
                mSelectedVehicle = value;
                OnPropertyChanged(nameof(SelectedVehicle));
            }
        }


        private ObservableCollection<VehicleList> mVechicles;
        public ObservableCollection<VehicleList> Vehicles
        {
            get { return mVechicles; }
            set
            {
                mVechicles = value;
                OnPropertyChanged(nameof(Vehicles));
            }
        }

        #region Methods
        [Obsolete]
        public void InitializeCommands()
        {
            AssignTechnicianCommand = new Command(async (obj) =>
            {
                try
                {
                    await this.page.Navigation.PushAsync(new Views.AssignTechnicianVehicle.AssignTechnicianVehicle());
                }
                catch (Exception ex)
                {
                }
            });

            DiassociateCommand = new Command(async (obj) =>
            {
                try
                {
                    await this.page.Navigation.PushAsync(new Views.DiassociateVehicle.DiassociateVehicle());
                }
                catch (Exception ex)
                {
                }
            });
        }
        #endregion
        #region ICommands
        public ICommand AssignTechnicianCommand { get; set; }
        public ICommand DiassociateCommand { get; set; }
        #endregion
    }
}
