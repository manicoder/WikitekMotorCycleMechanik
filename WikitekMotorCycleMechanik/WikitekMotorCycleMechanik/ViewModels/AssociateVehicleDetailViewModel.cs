using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class AssociateVehicleDetailViewModel : ViewModelBase
    {
        public AssociateVehicleDetailViewModel(Page page, LoginResponse use) : base(page)
        {
            InitializeCommands();
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
