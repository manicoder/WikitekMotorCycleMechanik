using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class RemoteRequestViewModel : ViewModelBase
    {
        ApiServices apiServices;

        public RemoteRequestViewModel(Page page) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                GetRequestList();
            }
            catch (Exception ex)
            {
            }
        }


        #region Properties
        private ObservableCollection<RemoteRequestModel> _request_list;
        public ObservableCollection<RemoteRequestModel> request_list
        {
            get => _request_list;
            set
            {
                _request_list = value;
                OnPropertyChanged("request_list");
            }
        }

        private RemoteRequestModel _selected_request;
        public RemoteRequestModel selected_request
        {
            get => _selected_request;
            set
            {
                _selected_request = value;
                OnPropertyChanged("selected_request");
            }
        }

        public ICommand AcceptRequestCommand => new Command(async (obj) =>
        {
            try
            {
                selected_request = (RemoteRequestModel)obj;
               
                //App.InitExpert(selected_request.remote_session_id, selected_request.job_card_session);
                var result = await App.controlEventManager.Init();

                App.Current.MainPage = new NavigationPage(new Views.AppFeature.AppFeaturePage(string.Empty,null,null,null
                    ,new string[2] { selected_request.remote_session_id, selected_request.job_card_session }));

                if (result == "Connected")
                {
                    Acr.UserDialogs.UserDialogs.Instance.Toast("Remote connected", new TimeSpan(0, 0, 0, 3));
                }
                else
                {
                    Acr.UserDialogs.UserDialogs.Instance.Toast("Remote could not be connect", new TimeSpan(0, 0, 0, 3));
                }
            }
            catch (Exception ex)
            {
            }
        });
        #endregion


        #region Methods
        public async Task GetRequestList()
        {
            //await apiServices.GetCollaborate(Preferences.Get("token", null), dtc_code);

            request_list = new ObservableCollection<RemoteRequestModel>
            {
                new RemoteRequestModel
                {
                    id = "100",
                    case_id = "100",
                    expert_device = new RemoteRequestModel(),
                    expert_email = "expert3w@gmail.com",
                    expert_user = 100,
                    request_status = true,
                    remote_session_id="12ccc84bvdv98dv89fff89",
                    job_card_session = "jobcardsessionid10001",
                    status ="Onlile",
                }
            };


        }
        #endregion

    }
}
