using System.Collections.ObjectModel;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Views.Dashboad;
using WikitekMotorCycleMechanik.Views.DtcFinder;
using WikitekMotorCycleMechanik.Views.Jobcard;
using WikitekMotorCycleMechanik.Views.Settings;
using WikitekMotorCycleMechanik.Views.VehicleDiagnostics;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class MasterViewModel : BaseViewModel
    {
        public MasterViewModel(int roll_id, LoginResponse user)
        {
            try
            {
                user_name = $"{user.first_name} {user.last_name}";
                this.user = user;
                //user_roll = user.role;
                if (roll_id == 0)
                {
                    menu_list = new ObservableCollection<MasterModel>
                    {
                        new MasterModel
                        {
                            Title= "Dashboard",
                            //TargetType = typeof(DashboadPage),
                            //args = new object[]{ user},
                            IconMargin = 10,
                            IconSource = "ic_home.png"
                        },
                        //new MasterModel
                        //{
                        //    Title= "Vehicle Diagnostics",
                        //    TargetType = typeof(VehicleDiagnosticsPage),
                        //    args = new object[]{ user},
                        //    IconMargin = 10,
                        //    IconSource = "ic_diag.png"
                        //},
                        //new MasterModel
                        //{
                        //    Title= "Jobacard",
                        //    IconMargin = 13,
                        //    IconSource = "ic_change_lang.png",
                        //    TargetType = typeof(JobcardPage),
                        //},
                        //new MasterModel
                        //{
                        //    Title= "DTC Finder",
                        //    IconMargin = 13,
                        //    IconSource = "ic_change_lang.png",
                        //    TargetType = typeof(DtcFinderPage),
                        //},
                        new MasterModel
                        {
                            Title = "Marketplace",
                            IconMargin= 10,
                            IconSource = "ic_cart.png"
                        },
                        new MasterModel
                        {
                            Title = "My Orders",
                            IconMargin= 10,
                            IconSource = "ic_my_order.png"
                        },
                        new MasterModel
                        {
                            Title= "Select Language",
                            IconMargin = 13,
                            IconSource = "ic_change_lang.png"
                            //TargetType = typeof(MyAssetPage),
                        },
                        new MasterModel
                        {
                            Title= "Sync Data",
                            IconMargin = 8,
                            IconSource = "ic_sync_data.png"
                            //TargetType = typeof(MyAssetPage),
                        },
                        new MasterModel
                        {
                            Title = "Logout",
                            IconMargin = 8,
                            IconSource = "ic_logout.png",
                            TargetType = typeof(Views.Login.LoginPage),
                        },
                    };
                }
            }
            catch (System.Exception ex)
            {
            }
        }

        private ObservableCollection<MasterModel> _menu_list;
        public ObservableCollection<MasterModel> menu_list
        {
            get => _menu_list;
            set
            {
                _menu_list = value;
                OnPropertyChanged("menu_list");
            }
        }

        //private string _roll_designation;
        //public string roll_designation
        //{
        //    get => _roll_designation;
        //    set
        //    {
        //        _roll_designation = value;
        //        OnPropertyChanged("roll_designation");
        //    }
        //}

        private string _user_name;
        public string user_name
        {
            get => _user_name;
            set
            {
                _user_name = value;
                OnPropertyChanged("user_name");
            }
        }

        private LoginResponse _user;
        public LoginResponse user
        {
            get => _user;
            set
            {
                _user = value;
                OnPropertyChanged("user");
            }
        }

        private string _version = $"App Version  {DependencyService.Get<IVersionAndBuildNumber>().GetVersionNumber()}";
        public string version
        {
            get => _version;
            set
            {
                _version = value;
                OnPropertyChanged("version");
            }
        }
    }
}