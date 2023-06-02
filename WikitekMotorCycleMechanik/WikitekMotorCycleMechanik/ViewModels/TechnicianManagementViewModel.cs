using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class TechnicianManagementViewModel : ViewModelBase
    {
        public TechnicianManagementViewModel(Page page, LoginResponse use) : base(page)
        {
			try
			{
                InitializeCommands();
                my_user_list = new ObservableCollection<UserResponse>();
                UserResponse userList2 = new UserResponse();
                userList2.first_name = "1";
                userList2.last_name = "HR-35";
                userList2.mobile = "1231231232";
                my_user_list.Add(userList2);
            }
			catch (Exception)
			{

				throw;
			}
        }
        private ObservableCollection<UserResponse> _my_user_list;
        public ObservableCollection<UserResponse> my_user_list
        {
            get => _my_user_list;
            set
            {
                _my_user_list = value;
                OnPropertyChanged("my_user_list");
            }
        }

        private UserResponse _selected_user;
        public UserResponse selected_user
        {
            get => _selected_user;
            set
            {
                _selected_user = value;
                OnPropertyChanged("selected_user");
            }
        }

        private string _selected_user_picture;
        public string selected_user_picture
        {
            get => _selected_user_picture;
            set
            {
                _selected_user_picture = value;
                OnPropertyChanged("selected_user_picture");
            }
        }
      
        #region Methods
        [Obsolete]
        public void InitializeCommands()
        {
            AddNewTechnicianCommand = new Command(async (obj) =>
            {
                try
                {
                   await this.page.Navigation.PushAsync(new Views.AddTechnician.AddTechnician());
                }
                catch (Exception ex)
                {
                }
            });
        }
        #endregion

        #region ICommands
        public ICommand AddNewTechnicianCommand { get; set; }
        #endregion
    }
}
