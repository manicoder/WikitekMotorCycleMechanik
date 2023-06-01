using WikitekMotorCycleMechanik.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using WikitekMotorCycleMechanik.Models;
using System.Collections.ObjectModel;
using System.Linq;
using Plugin.Media;
using Plugin.Media.Abstractions;
using WikitekMotorCycleMechanik.StaticInfo;
using System.Threading.Tasks;
using System.Net;
using Xamarin.Essentials;
using WikitekMotorCycleMechanik.CustomControls;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class SettingViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        MediaFile file = null;

        //MediaFile file = null;
        public SettingViewModel(Page page, LoginResponse user) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                this.navigationService = page.Navigation;
                //this.name = $"{user.first_name} {user.last_name}";
                this.user = user;

                var res = App.user.dongles.FirstOrDefault();
                var res1 = App.user.dongles;
                if (res != null)
                {
                    dongleList = new ObservableCollection<Dongles>(res1);
                }
                //if (App.user.segment == "CV" || App.user.segment == "BUS")
                //{
                //    hd_dongle_visible = true;
                //    var res = App.user.dongles.FirstOrDefault(x => x.device_type.Contains("24v"));
                //    if (res != null)
                //    {
                //        hd_dongle_add_btn_visible = false;
                //        hd_dongle = $"24v Dongle : {res.mac_id}";
                //    }
                //}
                //else
                //{
                //    ld_dongle_visible = true;
                //    //var res = App.user.dongles.FirstOrDefault(x => x.device_type.Contains("wkbtd"));
                //    var res = App.user.dongles.FirstOrDefault();
                //    var res1 = App.user.dongles;
                //    if (res != null)
                //    {
                //        dongleList = new ObservableCollection<Dongles>(res1);
                //        ld_dongle_add_btn_visible = false;
                //        ld_dongle = $"12v Dongle : {res.mac_id}";
                //    }
                //}

                if (App.user.subscriptions != null || App.user.subscriptions?.Count > 0)
                {
                    diy_subs_list = new ObservableCollection<Subscription>();
                    foreach (var item in App.user.subscriptions)
                    {
                        switch (item.package_name)
                        {
                            case "BasicPack":
                                is_basic_diag_pack = true;
                                basic_pack = $"Operation Pack Expiring : {item.end_date.ToString("dd MMM yyyy")}";
                                break;
                            case "DIYDiagnosticsPack":
                                diy_subs = item;
                                is_diy_diag_pack = true;
                                diyd_pack = $"DIYDiagnosticsPack Expiring : {item.end_date.ToString("dd MMM yyyy")}";
                                if (item.oem_specific)
                                {
                                    diy_subs.txt_is_oem_speci = "YES";
                                }
                                diy_subs_list.Add(diy_subs);
                                break;
                            case "AssistedDiagnosticsPack":
                                is_ass_diag_pack = true;
                                ad_pack = $"AssistedDiagnosticsPack Expiring : {item.end_date.ToString("dd MMM yyyy")}";
                                break;
                        }
                    }
                    ValidatePackage();
                }

                //this.user_profile_pic = user.picture;
                //var user_url= (HttpWebRequest)HttpWebRequest.Create(user.picture);


                //this.email= user.email;
                //this.mobile = user.mobile;
                //this.device_id = user.mac_id;
                //this.agent = user.agent.workshop.name;

                //foreach (var item in user.subscriptions)
                //{
                //    subscription_list.Add(
                //        new Subscription
                //        {
                //            show_end_date = item.end_date.ToString("dd/MM/yyyy"),
                //            show_start_date = item.start_date.ToString("dd/MM/yyyy"),
                //            package = item.package,
                //        });
                //}

                //list_height = 55 * subscription_list.Count;

                //subscription_list = new ObservableCollection<Subscription>(user.subscriptions);

                InitializeCommands();
                user_profile_pic = user.picture_local;
            }
            catch (Exception ex)
            { }
        }

        #region Properties


        //private string _first_name;
        //public string first_name
        //{
        //    get => _first_name;
        //    set
        //    {
        //        _first_name = value;
        //        OnPropertyChanged("first_name");
        //    }
        //}

        //private string _last_name;
        //public string last_name
        //{
        //    get => _last_name;
        //    set
        //    {
        //        _last_name = value;
        //        OnPropertyChanged("last_name");
        //    }
        //}

        private byte[] _user_profile_pic;
        public byte[] user_profile_pic
        {
            get => _user_profile_pic;
            set
            {
                _user_profile_pic = value;
                OnPropertyChanged("user_profile_pic");
            }
        }

        private string _name;
        public string name
        {
            get => $"{user.first_name} {user.last_name}";
            set
            {
                _name = value;
                OnPropertyChanged("name");
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

        //private string _mobile;
        //public string mobile
        //{
        //    get => _mobile;
        //    set
        //    {
        //        _mobile = value;
        //        OnPropertyChanged("mobile");
        //    }
        //}

        //private string _email;
        //public string email
        //{
        //    get => _email;
        //    set
        //    {
        //        _email = value;
        //        OnPropertyChanged("email");
        //    }
        //}

        //private string _device_id;
        //public string device_id
        //{
        //    get => _device_id;
        //    set
        //    {
        //        _device_id = value;
        //        OnPropertyChanged("device_id");
        //    }
        //}

        //private string _agent;
        //public string agent
        //{
        //    get => _agent;
        //    set
        //    {
        //        _agent = value;
        //        OnPropertyChanged("agent");
        //    }
        //}

        private string _ld_dongle = "12v Dongle : (to be purchase)";
        public string ld_dongle
        {
            get => _ld_dongle;
            set
            {
                _ld_dongle = value;

                //ld_button = ld_dongle.Contains("Not Found LD Dongle") ? "\U000f0417" : "\U000f08d5"; //IconFont.PlusCircle : IconFont.CircleEditOutline;
                OnPropertyChanged("ld_dongle");
            }
        }

        //private string _ld_button;
        //public string ld_button
        //{
        //    get => _ld_button;
        //    set
        //    {
        //        _ld_button = value;
        //        OnPropertyChanged("ld_button");
        //    }
        //}

        private string _hd_dongle = "24v Dongle : (to be purchase)";
        public string hd_dongle
        {
            get => _hd_dongle;
            set
            {
                _hd_dongle = value;

                // hd_button = hd_dongle.Contains("Not Found HD Dongle") ? "\U000f0417" : "\U000f08d5"; //IconFont.PlusCircle : IconFont.CircleEditOutline;
                OnPropertyChanged("hd_dongle");
            }
        }

        //private string _hd_button;
        //public string hd_button
        //{
        //    get => _hd_button;
        //    set
        //    {
        //        _hd_button = value;
        //        OnPropertyChanged("hd_button");
        //    }
        //}

        //private string _device_type;
        //public string device_type
        //{
        //    get => _device_type;
        //    set
        //    {
        //        _device_type = value;
        //        OnPropertyChanged("device_type");
        //    }
        //}

        //private ObservableCollection<Subscription> _subscription_list;
        //public ObservableCollection<Subscription> subscription_list
        //{
        //    get => _subscription_list;
        //    set
        //    {
        //        _subscription_list = value;
        //        OnPropertyChanged("subscription_list");
        //    }
        //}


        //private double _list_height;
        //public double list_height
        //{
        //    get => _list_height;
        //    set
        //    {
        //        _list_height = value;
        //        OnPropertyChanged("list_height");
        //    }
        //}

        private string _dongle_arrow = "\U000F013C";
        public string dongle_arrow
        {
            get => _dongle_arrow;
            set
            {
                _dongle_arrow = value;
                OnPropertyChanged("dongle_arrow");
            }
        }

        private string _change_password_arrow = "\U000F013C";
        public string change_password_arrow
        {
            get => _change_password_arrow;
            set
            {
                _change_password_arrow = value;
                OnPropertyChanged("change_password_arrow");
            }
        }

        private string _subscription_arrow = "\U000F013C";
        public string subscription_arrow
        {
            get => _subscription_arrow;
            set
            {
                _subscription_arrow = value;
                OnPropertyChanged("subscription_arrow");
            }
        }

        private bool _dongle_list_visible;
        public bool dongle_list_visible
        {
            get => _dongle_list_visible;
            set
            {
                _dongle_list_visible = value;
                OnPropertyChanged("dongle_list_visible");
            }
        }

        private bool _subscription_list_visible;
        public bool subscription_list_visible
        {
            get => _subscription_list_visible;
            set
            {
                _subscription_list_visible = value;
                OnPropertyChanged("subscription_list_visible");
            }
        }

        private bool _change_password_visible;
        public bool change_password_visible
        {
            get => _change_password_visible;
            set
            {
                _change_password_visible = value;
                OnPropertyChanged("change_password_visible");
            }
        }

        private string _current_password;
        public string current_password
        {
            get => _current_password;
            set
            {
                _current_password = value;
                OnPropertyChanged("current_password");
            }
        }

        private string _new_password;
        public string new_password
        {
            get => _new_password;
            set
            {
                _new_password = value;
                OnPropertyChanged("new_password");
            }
        }

        private string _confirm_password;
        public string confirm_password
        {
            get => _confirm_password;
            set
            {
                _confirm_password = value;
                OnPropertyChanged("confirm_password");
            }
        }

        private bool _current_is_password = true;
        public bool current_is_password
        {
            get => _current_is_password;
            set
            {
                _current_is_password = value;
                OnPropertyChanged("current_is_password");
            }
        }

        private bool _new_is_password = true;
        public bool new_is_password
        {
            get => _new_is_password;
            set
            {
                _new_is_password = value;
                OnPropertyChanged("new_is_password");
            }
        }

        private bool _confirm_is_password = true;
        public bool confirm_is_password
        {
            get => _confirm_is_password;
            set
            {
                _confirm_is_password = value;
                OnPropertyChanged("confirm_is_password");
            }
        }

        private string _current_password_image = "\U000F06D0";
        public string current_password_image
        {
            get => _current_password_image;
            set
            {
                _current_password_image = value;
                OnPropertyChanged("current_password_image");
            }
        }

        private string _new_password_image = "\U000F06D0";
        public string new_password_image
        {
            get => _new_password_image;
            set
            {
                _new_password_image = value;
                OnPropertyChanged("new_password_image");
            }
        }

        private string _confirm_password_image = "\U000F06D0";
        public string confirm_password_image
        {
            get => _confirm_password_image;
            set
            {
                _confirm_password_image = value;
                OnPropertyChanged("confirm_password_image");
            }
        }

        private string _basic_pack = "Operation Pack (to be purchase)";
        public string basic_pack
        {
            get => _basic_pack;
            set
            {
                _basic_pack = value;
                OnPropertyChanged("basic_pack");
            }
        }

        private string _diyd_pack = "DIYDiagnosticsPack (to be purchase)";
        public string diyd_pack
        {
            get => _diyd_pack;
            set
            {
                _diyd_pack = value;
                OnPropertyChanged("diyd_pack");
            }
        }

        private string _ad_pack = "AssistedDiagnosticsPack (to be purchase)";
        public string ad_pack
        {
            get => _ad_pack;
            set
            {
                _ad_pack = value;
                OnPropertyChanged("ad_pack");
            }
        }

        private bool _is_basic_diag_pack = false;
        public bool is_basic_diag_pack
        {
            get => _is_basic_diag_pack;
            set
            {
                _is_basic_diag_pack = value;
                OnPropertyChanged("is_basic_diag_pack");
            }
        }

        private bool _is_diy_diag_pack = false;
        public bool is_diy_diag_pack
        {
            get => _is_diy_diag_pack;
            set
            {
                _is_diy_diag_pack = value;
                OnPropertyChanged("is_diy_diag_pack");
            }
        }

        private bool _is_ass_diag_pack = false;
        public bool is_ass_diag_pack
        {
            get => _is_ass_diag_pack;
            set
            {
                _is_ass_diag_pack = value;
                OnPropertyChanged("is_ass_diag_pack");
            }
        }

        private bool _ld_dongle_visible = false;
        public bool ld_dongle_visible
        {
            get => _ld_dongle_visible;
            set
            {
                _ld_dongle_visible = value;
                OnPropertyChanged("ld_dongle_visible");
            }
        }

        private bool _hd_dongle_visible = false;
        public bool hd_dongle_visible
        {
            get => _hd_dongle_visible;
            set
            {
                _hd_dongle_visible = value;
                OnPropertyChanged("hd_dongle_visible");
            }
        }

        private bool _ld_dongle_add_btn_visible = true;
        public bool ld_dongle_add_btn_visible
        {
            get => _ld_dongle_add_btn_visible;
            set
            {
                _ld_dongle_add_btn_visible = value;
                OnPropertyChanged("ld_dongle_add_btn_visible");
            }
        }

        private bool _hd_dongle_add_btn_visible = true;
        public bool hd_dongle_add_btn_visible
        {
            get => _hd_dongle_add_btn_visible;
            set
            {
                _hd_dongle_add_btn_visible = value;
                OnPropertyChanged("hd_dongle_add_btn_visible");
            }
        }

        private Subscription _diy_subs;
        public Subscription diy_subs
        {
            get => _diy_subs;
            set
            {
                _diy_subs = value;
                OnPropertyChanged("diy_subs");
            }
        }

        private ObservableCollection<Subscription> _diy_subs_list;
        public ObservableCollection<Subscription> diy_subs_list
        {
            get => _diy_subs_list;
            set
            {
                _diy_subs_list = value;
                OnPropertyChanged("diy_subs_list");
            }
        }

        private ObservableCollection<Dongles> _dongleList;
        public ObservableCollection<Dongles> dongleList
        {
            get => _dongleList;
            set
            {
                _dongleList = value;
                OnPropertyChanged("dongleList");
            }
        }

        //private string _agent;
        //public string agent
        //{
        //    get => _agent;
        //    set
        //    {
        //        _agent = value;
        //        OnPropertyChanged("agent");
        //    }
        //}

        //private string _agent;
        //public string agent
        //{
        //    get => _agent;
        //    set
        //    {
        //        _agent = value;
        //        OnPropertyChanged("agent");
        //    }
        //}
        #endregion

        #region Methods
        [Obsolete]
        public void InitializeCommands()
        {
            DogleRegisterCommand = new Command(async (obj) =>
            {
                try
                {
                    if (App.user.subscriptions.Count > 0)
                    {
                        await this.navigationService.PushAsync(new Views.Subscription.SubscriptionPage("dongle"));
                        //var ss = obj;
                        //if (!string.IsNullOrEmpty(ss.ToString()))
                        //{
                        //    var type = ss.ToString().Contains("12v") ? "12V" : "24V";
                        //    var part_number = ss.ToString().Contains("12v") ? "WKBTD004" : "WKBTD001-HD";
                        //    var image = ss.ToString().Contains("12v") ? "ic_ld_dongle" : "ic_hd_dongle.jpg";
                        //    App.dongle_type = ss.ToString().Contains("12v") ? "wikitek" : "OBDII BT Dongle";
                        //    await this.navigationService.PushAsync(new Views.DongleRegistration.DongleRegistrationPage(type, part_number, image));
                        //    await this.navigationService.PushAsync(new Views.Subscription.SubscriptionPage("dongle"));
                        //}
                    }
                    else
                    {
                        await page.DisplayAlert("Alert", "Please firstly subscribe a package", "Ok");
                    }
                }
                catch (Exception ex)
                {
                }
            });

            ProfileCommand = new Command(async (obj) =>
            {
                try
                {
                    await CrossMedia.Current.Initialize();

                    if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                    {
                        await page.DisplayAlert("No Camera", ":( No camera available.", "OK");
                        return;
                    }

                    file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "profile.jpg",
                        PhotoSize = Plugin.Media.Abstractions.PhotoSize.Small,
                        MaxWidthHeight = 150,
                        CompressionQuality = 50,
                    });

                    if (file == null)
                        return;

                    //user_profile_pic = ImageSource.FromFile(file.Path);

                    var response = await apiServices.UserUpdate(file, user.user_id, user.email);

                    if (!string.IsNullOrEmpty(response.null_error))
                    {
                        await page.DisplayAlert("Error", response.null_error, "Ok");
                        return;
                    }

                    var api_status_code = StaticMethods.http_status_code(response.status_code);

                    if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                    {
                        await page.DisplayAlert("Success!", "User profile update successfully!", "Ok");
                        user_profile_pic = await apiServices.DownloadImageAsync(response.userResponse.user_profile_pic);
                        await page.Navigation.PopAsync();
                    }
                    else
                    {
                        await page.DisplayAlert(api_status_code, "Update user profile service not working", "Ok");
                    }
                }
                catch (Exception ex)
                {
                }
            });

            SubscriptionCommand = new Command(async (obj) =>
            {
                try
                {
                    var sender = (string)obj;

                    if (string.IsNullOrEmpty(sender))
                        return;

                    string pack_type = string.Empty;
                    //switch (sender)
                    //{
                    //    case "BasicPack":
                    //        pack_type = sender;
                    //        break;
                    //    case "AssistedDiagnosticsPack":
                    //        pack_type = sender;
                    //        break;
                    //    case "DIYDiagnosticsPack":
                    //        pack_type = sender;
                    //        break;
                    //}

                    if (sender.Contains("BasicPack"))
                    {
                        pack_type = "BasicPack";
                    }
                    else if (sender.Contains("DIYDiagnosticsPack"))
                    {
                        pack_type = "DIYDiagnosticsPack";
                    }
                    else if (sender.Contains("AssistedDiagnosticsPack"))
                    {
                        pack_type = "AssistedDiagnosticsPack";
                    }

                    await this.navigationService.PushAsync(new Views.Subscription.SubscriptionPage(pack_type));
                }
                catch (Exception ex)
                {
                }
            });

            ShowHideDongleListCommand = new Command(async (obj) =>
            {
                change_password_arrow = subscription_arrow = "\U000F013C";
                change_password_visible = subscription_list_visible = false;

                //change_password_arrow = "\U000F013C";
                //change_password_visible = false;

                if (dongle_arrow == "\U000F013C")
                {
                    dongle_arrow = "\U000F013F";
                    dongle_list_visible = true;
                }
                else
                {
                    dongle_arrow = "\U000F013C";
                    dongle_list_visible = false;
                }
            });

            ShowHideSubscriptionListCommand = new Command(async (obj) =>
            {
                change_password_arrow = dongle_arrow = "\U000F013C";
                change_password_visible = dongle_list_visible = false;

                //change_password_arrow = "\U000F013C";
                //change_password_visible = false;

                if (subscription_arrow == "\U000F013C")
                {
                    subscription_arrow = "\U000F013F";
                    subscription_list_visible = true;
                }
                else
                {
                    subscription_arrow = "\U000F013C";
                    subscription_list_visible = false;
                }

            });

            ShowHideChangePasswordCommand = new Command(async (obj) =>
            {
                subscription_arrow = dongle_arrow = "\U000F013C";
                subscription_list_visible = dongle_list_visible = false;

                //subscription_arrow = "\U000F013C";
                //subscription_list_visible = false;

                if (change_password_arrow == "\U000F013C")
                {
                    change_password_arrow = "\U000F013F";
                    change_password_visible = true;
                }
                else
                {
                    change_password_arrow = "\U000F013C";
                    change_password_visible = false;
                }

            });

            CurrentPasswordCommand = new Command(async (obj) =>
            {
                if (current_is_password)
                {
                    current_password_image = "\U000F06D1";//;
                    current_is_password = false;
                    //password_image_color = Color.Red;
                }
                else
                {
                    current_password_image = "\U000F06D0";//F06D1
                    current_is_password = true;
                    //password_image_color = Color.FromHex("#01FE2F");
                }
            });

            NewPasswordCommand = new Command(async (obj) =>
            {
                if (new_is_password)
                {
                    new_password_image = "\U000F06D1";//;
                    new_is_password = false;
                    //password_image_color = Color.Red;
                }
                else
                {
                    new_password_image = "\U000F06D0";//F06D1
                    new_is_password = true;
                    //password_image_color = Color.FromHex("#01FE2F");
                }
            });

            ConfirmPasswordCommand = new Command(async (obj) =>
            {
                if (confirm_is_password)
                {
                    confirm_password_image = "\U000F06D1";//;
                    confirm_is_password = false;
                    //password_image_color = Color.Red;
                }
                else
                {
                    confirm_password_image = "\U000F06D0";//F06D1
                    confirm_is_password = true;
                    //password_image_color = Color.FromHex("#01FE2F");
                }
            });
        }

        public async void ValidatePackage()
        {
            try
            {
                if (is_diy_diag_pack == true)
                {
                    var res = await GetPackageList("DIYDiagnosticsPack");
                    if (res != null)
                    {
                        if (res.Count > 0)
                        {
                            //btn_diy_diagnostics_pack_visible = true;
                        }
                        else
                        {
                            diyd_pack = "DIY Diagnostics Pack : Not Available";
                            is_diy_diag_pack = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<List<PackageResult>> GetPackageList(string pack)
        {
            try
            {
                List<PackageResult> packageResults = new List<PackageResult>();
                var response = await apiServices.GetPackageBySegment(Xamarin.Essentials.Preferences.Get("token", null), "wikitekMechanik", pack, App.country_id, (int)App.user.segment_id);

                var api_status_code = StaticMethods.http_status_code(response.status_code);

                if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                {
                    packageResults = response.results;
                }
                else
                {
                    packageResults = null;
                }
                return packageResults;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public void GetDeviceType()
        //{
        //    switch (Device.RuntimePlatform)
        //    {
        //        case Device.iOS:
        //            break;
        //        case Device.Android:
        //            device_type = "android";
        //            break;
        //        case Device.UWP:
        //            device_type = "windows";
        //            break;
        //        default:
        //            break;
        //    }
        //}
        #endregion

        #region ICommands
        public ICommand DogleRegisterCommand { get; set; }
        public ICommand SubscriptionCommand { get; set; }
        public ICommand ProfileCommand { get; set; }
        public ICommand ShowHideDongleListCommand { get; set; }
        public ICommand ShowHideSubscriptionListCommand { get; set; }
        public ICommand ShowHideChangePasswordCommand { get; set; }
        public ICommand CurrentPasswordCommand { get; set; }
        public ICommand NewPasswordCommand { get; set; }
        public ICommand ConfirmPasswordCommand { get; set; }
        public ICommand ChangePasswordCommand => new Command(async (obj) =>
        {
            try
            {
                if (string.IsNullOrEmpty(current_password))
                {
                    await page.DisplayAlert("", "Enter your current password", "Ok");
                    return;
                }

                if (string.IsNullOrEmpty(new_password))
                {
                    await page.DisplayAlert("", "Enter new password", "Ok");
                    return;
                }

                if (string.IsNullOrEmpty(confirm_password))
                {
                    await page.DisplayAlert("", "Enter confirm password", "Ok");
                    return;
                }

                if (new_password.Length < 6)
                {
                    await page.DisplayAlert("", "Minimum 6 digit required.", "Ok");
                    return;
                }

                if (confirm_password.Length < 6)
                {
                    await page.DisplayAlert("", "Minimum 6 digit required.", "Ok");
                    return;
                }

                if (new_password.Equals(confirm_password))
                {
                    var result = await apiServices.UserChangePassword(Preferences.Get("token", null), current_password, new_password);

                    if (result.error == "No internet connection")
                    {
                        await page.DisplayAlert("Network Issue", "No internet connection.", "Ok");
                        return;
                    }

                    if (result.status_code == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await Application.Current.MainPage.DisplayAlert("Alert", "Token Expired. \nPlease login again", "Ok");
                        App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                        return;
                    }


                    if (result?.status_code == System.Net.HttpStatusCode.OK || result?.status_code == System.Net.HttpStatusCode.Created)
                    {

                        if (result.success)
                        {
                            await page.DisplayAlert("", result.message, "Ok");
                            Preferences.Set("user_name", "");
                            Preferences.Set("password", "");
                            Preferences.Set("token", "");
                            Application.Current.MainPage = new CustomNavigationPage(new Views.Login.LoginPage());
                            //await page.Navigation.PushAsync(new ResetPasswordPage(result.reset_url));
                        }
                        else
                        {
                            await page.DisplayAlert("", result.message, "Ok");
                        }
                    }
                    else if (result?.status_code == System.Net.HttpStatusCode.NotFound)
                    {
                        await page.DisplayAlert("", "Not found!", "Ok");
                        return;
                    }
                    else
                    {
                        await page.DisplayAlert(Convert.ToString(result?.status_code), result.error, "Ok");
                        return;
                    }
                }
                else
                {
                    await page.DisplayAlert("", "New password and confirm password is not matching", "Ok");
                }


            }
            catch (Exception ex)
            {
            }
        });
        #endregion
    }
}
