using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class AgentViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler CloseAgentListPopup;
        int country_id;
        string pin_code;
        public AgentViewModel(int country_id, string pin_code)
        {
            apiServices = new ApiServices();
            agent_list = new ObservableCollection<AgentUserType>();
            selected_agent = new AgentUserType();

            this.country_id = country_id;
            this.pin_code = pin_code;

            Device.InvokeOnMainThreadAsync(async () =>
            {
                await GetAgentList();
            });

            ClosePopupCommand = new Command(async (obj) =>
            {
                CloseAgentListPopup?.Invoke("", new EventArgs());
                //StaticMethods.last_page = "CreateRSAgent";
                //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.CountyPopupPage());
            });
        }


        private string _search_key;
        public string search_key
        {
            get => _search_key;
            set
            {
                _search_key = value;

                if (!string.IsNullOrEmpty(search_key))
                {
                    agent_list = new ObservableCollection<AgentUserType>(agent_static_list.Where(x => x.name.ToLower().Contains(search_key.ToLower())).ToList());
                }

                OnPropertyChanged("search_key");
            }
        }

        private ObservableCollection<AgentUserType> _agent_static_list;
        public ObservableCollection<AgentUserType> agent_static_list
        {
            get => _agent_static_list;
            set
            {
                _agent_static_list = value;
                OnPropertyChanged("agent_static_list");
            }
        }

        private ObservableCollection<AgentUserType> _agent_list;
        public ObservableCollection<AgentUserType> agent_list
        {
            get => _agent_list;
            set
            {
                _agent_list = value;
                OnPropertyChanged("agent_list");
            }
        }


        private AgentUserType _selected_agent;
        public AgentUserType selected_agent
        {
            get => _selected_agent;
            set
            {
                _selected_agent = value;

                if (selected_agent != null)
                {
                    if (!string.IsNullOrEmpty(selected_agent.name))
                    {
                        MessagingCenter.Send<AgentViewModel, AgentUserType>(this, "selected_agent_registrationVM", selected_agent);
                        CloseAgentListPopup?.Invoke("", new EventArgs());
                    }
                }

                OnPropertyChanged("selected_country");
            }
        }

        #region Methods

        public async Task GetAgentList()
        {
            var response = await apiServices.GetAgentList("wikitekMechanik", country_id,pin_code,App.is_update);

            if (response == null)
            {
                await Application.Current.MainPage.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
                return;
            }


            var api_status_code = StaticMethods.http_status_code(response.status_code);

            if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
            {
                //country_list = response.results;
                agent_list = agent_static_list = new ObservableCollection<AgentUserType>(response.results.FirstOrDefault().agent_user_type);
            }
            else
            {

            }


        }

        #endregion

        public ICommand ClosePopupCommand { get; set; }
    }
}
