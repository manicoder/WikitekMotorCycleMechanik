using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Models1;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class TechnicianUserDetailViewModel : ViewModelBase
    {
        string json;

        public TechnicianUserDetailViewModel(Page page, LoginResponse use) : base(page)
        {
            InitializeCommands();
            SelectedTechnician = App.SelectedTechnician;

            json = Preferences.Get("LoginResponse", null);

            LoginResponse login = JsonSerializer.Deserialize<LoginResponse>(json);
            WorkShopId = login.agent.workshop.name;
            Fullname = SelectedTechnician.first_name + " " + SelectedTechnician.last_name;
        }
        //        public static NewTechnicianList SelectedTechnician;

        private NewTechnicianList mSelectedTechnician;
        public NewTechnicianList SelectedTechnician
        {
            get { return mSelectedTechnician; }
            set
            {
                mSelectedTechnician = value;
                OnPropertyChanged(nameof(SelectedTechnician));
            }
        }

        private string mFullname;

        public string Fullname
        {
            get { return mFullname; }
            set
            {
                mFullname = value;
                OnPropertyChanged(nameof(Fullname));
            }
        }
        private string mWorkShopId;

        public string WorkShopId
        {
            get { return mWorkShopId; }
            set
            {
                mWorkShopId = value;
                OnPropertyChanged(nameof(WorkShopId));
            }
        }

        #region Methods
        [Obsolete]
        public void InitializeCommands()
        {
            AssignVechicleCommand = new Command(async (obj) =>
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
                    await this.page.Navigation.PushAsync(new Views.DiassociateTechnician.DiassociateTechnician());
                }
                catch (Exception ex)
                {
                }
            });
        }
        #endregion
        #region ICommands
        public ICommand AssignVechicleCommand { get; set; }
        public ICommand DiassociateCommand { get; set; }
        #endregion
    }
}
