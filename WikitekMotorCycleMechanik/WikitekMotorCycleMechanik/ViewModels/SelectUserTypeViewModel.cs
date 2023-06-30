using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class SelectUserTypeViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler CloseSelectUserTypePopup;

        public SelectUserTypeViewModel()
        {
            apiServices = new ApiServices();
            usertype_list = new ObservableCollection<UserType>();
            selected_userType = new UserType();
            Device.InvokeOnMainThreadAsync(() =>
            {
                GetUserTypeList();
            });

            ClosePopupCommand = new Command(async (obj) =>
            {
                CloseSelectUserTypePopup?.Invoke("", new EventArgs());
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
                    usertype_list = new ObservableCollection<UserType>(usertype_static_list.Where(x => x.name.ToLower().Contains(search_key.ToLower())).ToList());
                }

                OnPropertyChanged("search_key");
            }
        }

        private ObservableCollection<UserType> _usertype_static_list;
        public ObservableCollection<UserType> usertype_static_list
        {
            get => _usertype_static_list;
            set
            {
                _usertype_static_list = value;
                OnPropertyChanged("usertype_static_list");
            }
        }

        private ObservableCollection<UserType> _usertype_list;
        public ObservableCollection<UserType> usertype_list
        {
            get => _usertype_list;
            set
            {
                _usertype_list = value;
                OnPropertyChanged("usertype_list");
            }
        }

        private string _emptyView = "Loading...";
        public string emptyView
        {
            get => _emptyView;
            set
            {
                _emptyView = value;
                OnPropertyChanged("emptyView");
            }
        }

        private UserType _selected_userType;
        public UserType selected_userType
        {
            get => _selected_userType;
            set
            {
                _selected_userType = value;

                if (selected_userType != null)
                {
                    if (!string.IsNullOrEmpty(selected_userType.name))
                    {


                        switch (StaticMethods.last_page)
                        {
                            case "registration":
                                MessagingCenter.Send<SelectUserTypeViewModel, UserType>(this, "selected_userType_registrationVM", selected_userType);
                                break;

                            //case "CreateWorkshop":
                            //    MessagingCenter.Send<SelectUserTypeViewModel, UserType>(this, "selected_userType_CreateRSAgentVM", selected_userType);
                            //    break;
                            //case "billing":
                            //    MessagingCenter.Send<SelectUserTypeViewModel, UserType>(this, "selected_userType_billing", selected_userType);
                            //    break;
                        }

                        CloseSelectUserTypePopup?.Invoke("", new EventArgs());
                    }
                }

                OnPropertyChanged("selected_userType");
            }
        }

        #region Methods

        public void GetUserTypeList()
        {

            List<UserType> userTypeList = new List<UserType>();
            UserType utype = new UserType();
            utype.id = 1;
            utype.name = "Admin";
            userTypeList.Add(utype);
            UserType utype1 = new UserType();
            utype1.id = 2;
            utype1.name = "Employee";
            userTypeList.Add(utype1);

            usertype_list = usertype_static_list = new ObservableCollection<UserType>(userTypeList);


            //var response = await apiServices.Countries("wikitekMechanik", App.is_update);

            //if (response == null)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
            //    return;
            //}
            //else if (response.count == 0)
            //{
            //    emptyView = "Country list not found";
            //}

            //var api_status_code = StaticMethods.http_status_code(response.status_code);

            //if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
            //{
            //    //country_list = response.results;
            //    usertype_list = usertype_static_list = new ObservableCollection<UserTypeModel>(response.results.First().rs_user_type_country);
            //}
            //else
            //{

            //}


        }

        #endregion

        public ICommand ClosePopupCommand { get; set; }
    }
}
