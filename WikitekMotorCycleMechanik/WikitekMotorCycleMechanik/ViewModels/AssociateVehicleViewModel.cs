using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class AssociateVehicleViewModel : ViewModelBase
    {
        public AssociateVehicleViewModel(Page page, LoginResponse use) : base(page)
        {
            InitializeCommands();
        }
        #region Methods
        [Obsolete]
        public void InitializeCommands()
        {
            SendOTPCommand = new Command(async (obj) =>
            {
                try
                {
                    await this.page.Navigation.PushAsync(new Views.AssociateVehicleDetail.AssociateVehicleDetail());
                }
                catch (Exception ex)
                {
                }
            });
        }
        #endregion
        #region ICommands
        public ICommand SendOTPCommand { get; set; }
        #endregion
    }
}
