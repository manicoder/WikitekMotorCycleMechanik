using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class ExpertListViewModel : ViewModelBase
    {
        ApiServices apiServices;
        public ExpertListViewModel(Page page) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                GetExpertList();
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties
        private ObservableCollection<Expert> _expert_list;
        public ObservableCollection<Expert> expert_list
        {
            get => _expert_list;
            set
            {
                _expert_list = value;
                OnPropertyChanged("expert_list");
            }
        }

        private Expert _selected_expert;
        public Expert selected_expert
        {
            get => _selected_expert;
            set
            {
                _selected_expert = value;
                OnPropertyChanged("selected_expert");
            }
        }
        #endregion

        #region Methods
        public async Task GetExpertList()
        {
            var res = await apiServices.GetExpertList(Preferences.Get("token", null));

            //expert_list = new ObservableCollection<Expert>
            //{
            //    new Expert
            //    {
            //        auto_session_id = 100,
            //        auto_session_internal_id = 101,
            //        btnIsActive = true,
            //        email = "expert3w@gmail.com",
            //        first_name ="expert",
            //        last_name ="3w",
            //        id = 102,
            //        mobile = "0909090909",
            //        remote_session_id="12ccc84bvdv98dv89fff89",
            //        remote_session_internal_id= "sjdfjf12nnjbbhb990m8eddd",
            //        status ="Onlile",
            //        status_color ="#17AB00",
            //        workshop = "Wikitek System",
            //        workshop_city = "Pune",
            //    }
            //};
        }
        #endregion

        #region ICommands
        public ICommand SendRemoteRequestCommand => new Command(async (obj) =>
        {
            try
            {
                selected_expert = (Expert)obj;
                App.InitTechnician(selected_expert.remote_session_id, "jobcardsessionid10001");
                var result = await App.controlEventManager.Init();

                if(result== "Connected")
                {
                    await page.Navigation.PopAsync();
                    Acr.UserDialogs.UserDialogs.Instance.Toast("Remote request Sent, Please wait to accept expert side.", new TimeSpan(0, 0, 0, 3));
                }
                else
                {
                    Acr.UserDialogs.UserDialogs.Instance.Toast("Remote request could not be Send", new TimeSpan(0, 0, 0, 3));
                }
            }
            catch (Exception ex)
            {
            }
        });
        #endregion
    }
}
