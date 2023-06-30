using Acr.UserDialogs;
using Plugin.Media;
using Plugin.Media.Abstractions;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
//using WikitekMotorCycleMechanik.PopupPages;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using System.Linq;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class RegistrationViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly IDeviceMacAddress device_mac_id;
        readonly Page page;
        MediaFile file = null;
        public RegistrationViewModel(Page page) : base(page)
        {
            try
            {
                apiServices = new ApiServices();

                this.page = page;
                //this.navigationService = page.Navigation;
                this.device_mac_id = DependencyService.Get<IDeviceMacAddress>();
                user_profile_pic = ImageSource.FromFile("ic_user.png");
                InitializeCommands();
                GetDeviceType();
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    await GetMacId();
                });
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties

        private string _first_name; //"Sattyajit";
        public string first_name
        {
            get => _first_name;
            set
            {
                _first_name = value;
                OnPropertyChanged("first_name");
            }
        }


        private string _last_name; //= "Kane";
        public string last_name
        {
            get => _last_name;
            set
            {
                _last_name = value;
                OnPropertyChanged("last_name");
            }
        }


        private string _email; //= //"sattyajit.kane@autopeepal.com";
        public string email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("email");
            }
        }


        private string _mobile;// = "9898989898";
        public string mobile
        {
            get => _mobile;
            set
            {
                _mobile = value;
                OnPropertyChanged("mobile");
            }
        }



        private string _password;// = "123456";
        public string password
        {
            get => _password;
            set
            {
                _password = value;
                //if (string.IsNullOrEmpty(password))
                //{
                //    password_image_visible = false;
                //}
                //else
                //{
                //    password_image_visible = true;
                //}
                OnPropertyChanged("password");
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

        //private bool _password_image_visible = false;
        //public bool password_image_visible
        //{
        //    get => _password_image_visible;
        //    set
        //    {
        //        _password_image_visible = value;
        //        OnPropertyChanged("password_image_visible");
        //    }
        //}

        private string _password_image = "\U000F06D1";
        public string password_image
        {
            get => _password_image;
            set
            {
                _password_image = value;
                OnPropertyChanged("password_image");
            }
        }

        private bool _is_password = true;
        public bool is_password
        {
            get => _is_password;
            set
            {
                _is_password = value;
                OnPropertyChanged("is_password");
            }
        }

        private string _confirm_password_image = "\U000F06D1";
        public string confirm_password_image
        {
            get => _confirm_password_image;
            set
            {
                _confirm_password_image = value;
                OnPropertyChanged("confirm_password_image");
            }
        }

        private bool _is_confirm_password = true;
        public bool is_confirm_password
        {
            get => _is_confirm_password;
            set
            {
                _is_confirm_password = value;
                OnPropertyChanged("is_confirm_password");
            }
        }

        private string _segment = "Select segment";
        public string segment
        {
            get => _segment;
            set
            {
                _segment = value;
                OnPropertyChanged("segment");
            }
        }

        private int _segment_id;
        public int segment_id
        {
            get => _segment_id;
            set
            {
                _segment_id = value;
                OnPropertyChanged("segment_id");
            }
        }

        private string _country = "Select country";
        public string country
        {
            get => _country;
            set
            {
                _country = value;

                if (!string.IsNullOrEmpty(country) && !country.Contains("Select country"))
                {
                    //country_text_color = (Color)Application.Current.Resources["text_color"];
                }

                OnPropertyChanged("country");
            }
        }

        private string _userType = "Select User Type";
        public string userType
        {
            get => _userType;
            set
            {
                _userType = value;

                if (!string.IsNullOrEmpty(userType) && !userType.Contains("Select User Type"))
                {
                    //country_text_color = (Color)Application.Current.Resources["text_color"];
                }

                OnPropertyChanged("userType");
            }
        }

        private string _vehiclesegment = "Select Vehicle Segment";
        public string vehiclesegment
        {
            get => _vehiclesegment;
            set
            {
                _vehiclesegment = value;

                if (!string.IsNullOrEmpty(vehiclesegment) && !vehiclesegment.Contains("Select Vehicle Segment"))
                {
                    //country_text_color = (Color)Application.Current.Resources["text_color"];
                }

                OnPropertyChanged("vehiclesegment");
            }
        }


        //private Color _country_text_color = (Color)Application.Current.Resources["placeholder_color"];
        //public Color country_text_color
        //{
        //    get => _country_text_color;
        //    set
        //    {
        //        _country_text_color = value;
        //        OnPropertyChanged("country_text_color");
        //    }
        //}


        private string _device_type;
        public string device_type
        {
            get => _device_type;
            set
            {
                _device_type = value;
                OnPropertyChanged("device_type");
            }
        }


        private string _mac_id;
        public string mac_id
        {
            get => _mac_id;
            set
            {
                _mac_id = value;
                OnPropertyChanged(" mac_id");
            }
        }


        private ImageSource _user_profile_pic;
        public ImageSource user_profile_pic
        {
            get => _user_profile_pic;
            set
            {
                _user_profile_pic = value;
                OnPropertyChanged("user_profile_pic");
            }
        }


        private string _pin_code; //= "515842";
        public string pin_code
        {
            get => _pin_code;
            set
            {
                _pin_code = value;
                OnPropertyChanged("pin_code");
            }
        }


        private string _workshop_id = "Select using workshop";
        public string workshop_id
        {
            get => _workshop_id;
            set
            {
                _workshop_id = value;
                if (!string.IsNullOrEmpty(workshop_id) && !country.Contains("Select using workshop"))
                {
                    workshop_text_color = (Color)Application.Current.Resources["text_color"];
                }
                OnPropertyChanged("workshop_id");
            }
        }


        private Color _workshop_text_color = (Color)Application.Current.Resources["placeholder_color"];
        public Color workshop_text_color
        {
            get => _workshop_text_color;
            set
            {
                _workshop_text_color = value;
                OnPropertyChanged("workshop_text_color");
            }
        }


        private UserModel _user_detail;
        public UserModel user_detail
        {
            get => _user_detail;
            set
            {
                _user_detail = value;
                OnPropertyChanged("user_detail");
            }
        }

        private AgentUserType _workshop_detail;
        public AgentUserType workshop_detail
        {
            get => _workshop_detail;
            set
            {
                _workshop_detail = value;
                OnPropertyChanged("workshop_detail");
            }
        }

        private UserType _selected_userType;
        public UserType selected_userType
        {
            get => _selected_userType;
            set
            {
                _selected_userType = value;
                OnPropertyChanged("selected_userType");
            }
        }

        private VehicleSegmentModel _selectvehiclesegment_list;
        public VehicleSegmentModel selectvehiclesegment_list
        {
            get => _selectvehiclesegment_list;
            set
            {
                _selectvehiclesegment_list = value;
                OnPropertyChanged("selectvehiclesegment_list");
            }
        }

        private RsUserTypeCountry _country_detail;
        public RsUserTypeCountry country_detail
        {
            get => _country_detail;
            set
            {
                _country_detail = value;
                OnPropertyChanged("country_detail");
            }
        }

        private VehicleSegment _segement_detail;
        public VehicleSegment segement_detail
        {
            get => _segement_detail;
            set
            {
                _segement_detail = value;
                OnPropertyChanged("segement_detail");
            }
        }
        #endregion

        #region Methods
        [Obsolete]
        public void InitializeCommands()
        {

            MessagingCenter.Subscribe<SegmentViewModel, VehicleSegment>(this, "registration_page", async (sender, arg) =>
            {
                segment = arg.segment_name;
                segment_id = arg.id;
                segement_detail = arg;
            });

            MessagingCenter.Subscribe<CountyViewModel, RsUserTypeCountry>(this, "selected_country_registrationVM", async (sender, arg) =>
            {
                country = arg.name;
                country_detail = arg;
            });

            MessagingCenter.Subscribe<SelectUserTypeViewModel, UserType>(this, "selected_userType_registrationVM", async (sender, arg) =>
            {
                userType = arg.name;
                selected_userType = arg;
            });

            MessagingCenter.Subscribe<SelectVehicleSegmentViewModel, VehicleSegmentModel>(this, "selected_vehiclesegment_registrationVM", async (sender, arg) =>
            {
                vehiclesegment = arg.name;
                selectvehiclesegment_list = arg;
            });

            MessagingCenter.Subscribe<AgentViewModel, AgentUserType>(this, "selected_agent_registrationVM", async (sender, arg) =>
            {
                workshop_detail = arg;
                workshop_id = arg.name;
            });

            ProfileCommand = new Command(async (obj) =>
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
                    MaxWidthHeight = 70,
                    CompressionQuality = 50,
                });

                if (file == null)
                    return;

                //await page.DisplayAlert("File Location", file.Path, "OK");

                user_profile_pic = ImageSource.FromFile(file.Path);

                //user_profile_pic = ImageSource.FromStream(() =>
                //{
                //    var stream = file.GetStream();
                //    return stream;
                //});
            });

            SubmitCommand = new Command(async (obj) =>
            {
                await UserRegistration();
                //string description = "User Registration authentication An OTP has been sent to your mobile and will be valid for 10 mins.Pls enter the OTP here";
                //await this.navigationService.PushAsync(new Views.Otp.OtpPage(true, description));
            });

            RSAgentCommand = new Command(async (obj) =>
            {
                if (country_detail == null)
                {
                    await page.DisplayAlert("Alert", "Please select country", "Ok");
                }
                else if (string.IsNullOrEmpty(pin_code))
                {
                    await page.DisplayAlert("Alert", "Please select country", "Ok");
                }
                else
                {
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        try
                        {
                            await Task.Delay(200);
                            await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.AgentListPopupPage(country_detail.id, pin_code));
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                }
            });

            NewRSAgentCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);
                    await page.Navigation.PushAsync(new Views.NewWorkshop.NewWorkshopPage());
                }
            });

            SegmentCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);
                    StaticMethods.last_page = "registration";
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.SegmentPopupPage(StaticMethods.last_page));
                }
            });

            CountryCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);
                    StaticMethods.last_page = "registration";
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.CountyPopupPage());
                }
            });

            SelectUserTypeCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);
                    StaticMethods.last_page = "registration";
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.SelectUserTypePopupPage());
                }
            });

            VehicleSegmentCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);
                    StaticMethods.last_page = "registration";
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.SelectVehicleSegmentPopupPage());
                }
            });

            ShowHidePasswordCommand = new Command(async (obj) =>
            {
                if (is_password)
                {
                    is_password = false;
                    password_image = "\U000F06D0";
                }
                else
                {
                    is_password = true;
                    password_image = "\U000F06D1";//
                }
            });

            ShowHideConfirmPasswordCommand = new Command(async (obj) =>
            {
                if (is_confirm_password)
                {
                    is_confirm_password = false;
                    confirm_password_image = "\U000F06D0";
                }
                else
                {
                    is_confirm_password = true;
                    confirm_password_image = "\U000F06D1";//
                }
            });
        }

        public async Task UserRegistration()
        {

            try
            {
                var IsError = await Validation();
                string error_message = string.Empty;
                if (!IsError)
                {
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        if (string.IsNullOrEmpty(mac_id))
                        {
                            await page.DisplayAlert("Error", "Your device id not found.", "Ok");
                            return;
                        }

                        await Task.Delay(200);
                        user_detail = new UserModel();
                        user_detail.first_name = first_name;
                        user_detail.last_name = last_name;
                        user_detail.email = email;
                        user_detail.mobile = mobile;
                        user_detail.password = password;
                        user_detail.device_type = device_type;
                        user_detail.mac_id = /*"EXPERT123456";*/mac_id;
                        user_detail.user_profile_pic = user_profile_pic;
                        user_detail.pin_code = pin_code;
                        //user_detail.rs_agent_id = workshop_detail.code;//"WK2397950556"; 
                        //user_detail.user_type = "wikitekMechanik";
                        //user_detail.segment = "2W";
                        //user_detail.segment_id = 1;
                        user_detail.role = "wikitektechnician";
                        user_detail.country_name = country_detail.name;
                        user_detail.userType_name = selected_userType.name;
                        user_detail.vehicleSegment_name = selectvehiclesegment_list.name;

                        
                        var response = await apiServices.UserRegistrationNew(file, user_detail);

                        if (!string.IsNullOrEmpty(response.null_error))
                        {
                            await page.DisplayAlert("Error", response.null_error, "Ok");
                            return;
                        }


                        var api_status_code = StaticMethods.http_status_code(response.status_code);

                        if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                        {
                            await page.DisplayAlert("Success!", "User Created successfully!", "Ok");
                            string description = "An OTP has been sent to your mobile and will be valid for 10 mins.Pls enter the OTP here";

                            await page.Navigation.PushAsync(new Views.Otp.OtpPage(false, description, user_detail.mobile, "User Ragistration"));
                        }
                        else
                        {
                            if (response.registrationError.mac_id != null)
                            {
                                if (response.registrationError.mac_id.Count > 0)
                                {
                                    error_message = $"{error_message} {response.registrationError.mac_id.FirstOrDefault()}";
                                    await page.DisplayAlert(api_status_code, $"{error_message}, \n", "Ok");
                                    return;
                                }
                            }
                            if (response.registrationError.mobile != null)
                            {
                                if (response.registrationError.mobile.Count > 0)
                                {
                                    error_message = $"{error_message} {response.registrationError.mobile.FirstOrDefault()}";
                                    await page.DisplayAlert(api_status_code, $"{error_message}. \n", "Ok");
                                }
                            }
                            if (response.registrationError.email != null)
                            {
                                if (response.registrationError.email.Count > 0)
                                {
                                    error_message = $"{error_message} {response.registrationError.email.FirstOrDefault()}";
                                    await page.DisplayAlert(api_status_code, $"{error_message}, \n", "Ok");
                                }
                            }
                            if (response.registrationError.country != null)
                            {
                                if (response.registrationError.country.Count > 0)
                                {
                                    error_message = $"{error_message} {response.registrationError.country.FirstOrDefault()}";
                                    await page.DisplayAlert(api_status_code, $"{error_message}, \n", "Ok");
                                }
                            }
                            if (response.registrationError.pincode != null)
                            {
                                if (response.registrationError.pincode.Count > 0)
                                {
                                    error_message = $"{error_message} {response.registrationError.pincode.FirstOrDefault()}";
                                    await page.DisplayAlert(api_status_code, $"{error_message}, \n", "Ok");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void GetDeviceType()
        {
            switch (Device.RuntimePlatform)
            {
                case Device.iOS:
                    break;
                case Device.Android:
                    device_type = "android";
                    break;
                case Device.UWP:
                    device_type = "windows";
                    break;
                default:
                    break;
            }
        }

        public async Task GetMacId()
        {
            mac_id = await device_mac_id.GetDeviceUniqueId();
        }

        public async Task<bool> Validation()
        {
            bool IsError = false;

            //if (file == null)
            //{
            //    await page.DisplayAlert("Alert", "Click your profile image", "Ok");
            //    IsError = true;
            //}
             if (string.IsNullOrEmpty(first_name))
            {
                await page.DisplayAlert("Alert", "Please enter your first name", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(last_name))
            {
                await page.DisplayAlert("Alert", "Please enter your last name", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(email))
            {
                await page.DisplayAlert("Alert", "Please enter your user name", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(mobile))
            {
                await page.DisplayAlert("Alert", "Please enter your mobile number", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(password))
            {
                await page.DisplayAlert("Alert", "Please enter your password", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(confirm_password))
            {
                await page.DisplayAlert("Alert", "Please enter your confirm password", "Ok");
                IsError = true;
            }
            if (password.Length < 6)
            {
                await page.DisplayAlert("Alert", "Minimum 6 digit required.", "Ok");
                IsError = true;
                // return;
            }
            if (confirm_password.Length < 6)
            {
                await page.DisplayAlert("Alert", "Minimum 6 digit required.", "Ok");
                IsError = true;
                //return;
            }
            else if (string.IsNullOrEmpty(device_type))
            {
                await page.DisplayAlert("Alert", "Device type not found", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(mac_id))
            {
                await page.DisplayAlert("Alert", "Mac address not found", "Ok");
                IsError = true;
            }
            else if (user_profile_pic == null)
            {
                await page.DisplayAlert("Alert", "please click your profile", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(pin_code))
            {
                await page.DisplayAlert("Alert", "Please enter pin code", "Ok");
                IsError = true;
            }
            else if (password != confirm_password)
            {
                await page.DisplayAlert("Alert", "Password did not matched", "Ok");
                IsError = true;
            }
            else if (!string.IsNullOrEmpty(mobile))
            {
                if (mobile.Length != 10)
                {
                    await page.DisplayAlert("Alert", "Please enter correct mobile number", "Ok");
                    IsError = true;
                }
            }

            return IsError;
        }
        #endregion

        #region ICommands
        public ICommand ProfileCommand { get; set; }
        public ICommand CountryCommand { get; set; }
        public ICommand SelectUserTypeCommand { get; set; }
        public ICommand RSAgentCommand { get; set; }
        public ICommand NewRSAgentCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand SegmentCommand { get; set; }
        public ICommand ShowHidePasswordCommand { get; set; }
        public ICommand ShowHideConfirmPasswordCommand { get; set; }
        public ICommand VehicleSegmentCommand { get; set; }
        #endregion
    }
}
