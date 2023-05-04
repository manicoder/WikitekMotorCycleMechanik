using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.Views.Colleborates;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class ColleborateViewModel : ViewModelBase
    {
        ApiServices apiServices;
        string dtc_code;
        public ColleborateViewModel(Page page,string dtc_code) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                this.dtc_code = dtc_code;
                GetCollaborate();
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties
        private double _description_height = 250;
        public double description_height
        {
            get => _description_height;
            set
            {
                _description_height = value;
                OnPropertyChanged("description_height");
            }
        }
        #endregion

        #region Methods
        public async Task GetCollaborate()
        {
            await apiServices.GetCollaborate(Preferences.Get("token", null), dtc_code);
        }
        #endregion

        #region ICommands
        public ICommand AddCollCommand => new Command(async (obj) =>
        {
            try
            {
                await page.Navigation.PushAsync(new AddColleboratePage(dtc_code));
            }
            catch (Exception ex)
            {
            }
        });
        #endregion
    }
}
