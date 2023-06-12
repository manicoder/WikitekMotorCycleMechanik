using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Models1;
using WikitekMotorCycleMechanik.Views.Dashboad;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class ActivityRSManagerSectionViewModel : ViewModelBase
    {
        ApiServices1 apiServices;
        public ActivityRSManagerSectionViewModel(Page page, LoginResponse use) : base(page)
        {
            //InitializeCommands();
            apiServices = new ApiServices1();
            Initialization = Init();
        }
        Task Initialization { get; }

        private ObservableCollection<ActivityTechnicianList> mTechnicians;
        public ObservableCollection<ActivityTechnicianList> Technicians
        {
            get { return mTechnicians; }
            set
            {
                mTechnicians = value;
                OnPropertyChanged(nameof(Technicians));
            }
        }

        private ActivityTechnicianList mCurrentSelectedTechnicians;

        public ActivityTechnicianList CurrentSelectedTechnician
        {
            get { return mCurrentSelectedTechnicians; }
            set
            {
                mCurrentSelectedTechnicians = value;
                OnPropertyChanged(nameof(CurrentSelectedTechnician));

                if (value != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await GetVechicleTechnicianAssociate(mCurrentSelectedTechnicians.email);
                    });
                }
            }
        }

        private ObservableCollection<VechicleTechicianList> mVehicleTechnicians;
        public ObservableCollection<VechicleTechicianList> VehicleTechnicians
        {
            get { return mVehicleTechnicians; }
            set
            {
                mVehicleTechnicians = value;
                OnPropertyChanged(nameof(VehicleTechnicians));
            }
        }

        private VechicleTechicianList mCurrentSelectedVehicleTechnicians;

        public VechicleTechicianList CurrentSelectedVehicleTechnician
        {
            get { return mCurrentSelectedVehicleTechnicians; }
            set
            {
                mCurrentSelectedVehicleTechnicians = value;
                //OnPropertyChanged(nameof(CurrentSelectedVehicleTechnician));

                if (value != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                            {
                                await Task.Delay(200);
                                await page.Navigation.PushAsync(new Views.ActivityDetail.ActivityDetail(mCurrentSelectedVehicleTechnicians));
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    });
                }
            }
        }



        public async Task Init()
        {
            try
            {
                int workshopid = App.workshopid;
                var msgs = await apiServices.ActivityTechnicianList(workshopid);
                Technicians = new ObservableCollection<ActivityTechnicianList>(msgs.results);

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task GetVechicleTechnicianAssociate(string TechnicianEmail)
        {
            var msgs = await apiServices.GetVechicleTechnicianAssociate(TechnicianEmail);
           VehicleTechnicians = new ObservableCollection<VechicleTechicianList>(msgs.results);
        }
        public ICommand GoToActivateDetailCommand => new Command(async (obj) =>
        {
            try
            {
                if (CurrentSelectedVehicleTechnician != null)
                { 

                }
                //await page.Navigation.PushAsync(new Views.RSManagerSection.RSManagerSection());
            }
            catch (Exception ex)
            {
            }
        });
    }
}
