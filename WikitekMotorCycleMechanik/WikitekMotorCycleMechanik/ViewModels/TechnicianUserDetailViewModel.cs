using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class TechnicianUserDetailViewModel : ViewModelBase
    {
        public TechnicianUserDetailViewModel(Page page, LoginResponse use) : base(page)
        {
            InitializeCommands();
        }

        #region Methods
        [Obsolete]
        public void InitializeCommands()
        {
            AssignVechicleCommand = new Command(async (obj) =>
            {
                try
                {
                    await this.page.Navigation.PushAsync(new Views.AssignVehicle.AssignVehicle());
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
