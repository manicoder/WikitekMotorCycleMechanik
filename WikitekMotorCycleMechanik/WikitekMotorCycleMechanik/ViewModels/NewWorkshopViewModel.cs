using Acr.UserDialogs;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class NewWorkshopViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly Page page;
        public NewWorkshopViewModel(Page page) : base(page)
        {
            apiServices = new ApiServices();
            rs_agent_detail = new CreateRSAgentModel();
            selected_country = new RsUserTypeCountry();
            //segement_detail = new VehicleSegment { segment_name = "Select segment" };
            this.page = page;
            InitializeCommands();
            Device.InvokeOnMainThreadAsync(async () =>
            {
            });
        }

        #region Properties

        private string _rs_agent_name;
        public string rs_agent_name
        {
            get => _rs_agent_name;
            set
            {
                _rs_agent_name = value;
                OnPropertyChanged("rs_agent_name");
            }
        }


        private string _email_id;
        public string email_id
        {
            get => _email_id;
            set
            {
                _email_id = value;
                OnPropertyChanged("email_id");
            }
        }


        private string _mobile;
        public string mobile
        {
            get => _mobile;
            set
            {
                _mobile = value;
                OnPropertyChanged("mobile");
            }
        }

        private string _rs_country = "Select country";
        public string rs_country
        {
            get => _rs_country;
            set
            {
                _rs_country = value;
                if (!string.IsNullOrEmpty(rs_country) && !rs_country.Contains("Select country"))
                {
                    country_text_color = (Color)Application.Current.Resources["text_color"];
                    //_rs_pincode = value;
                    if (!string.IsNullOrEmpty(rs_pincode))
                    {
                        if (!rs_country.Contains("Select country"))
                        {
                            Device.InvokeOnMainThreadAsync(async () =>
                            {
                                var response = await apiServices.DistricAndState(rs_pincode, selected_country.id, "wikitekMechanik");
                                if (response == null)
                                {
                                    await page.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
                                    return;
                                }
                                if (response != null)
                                {
                                    var item = response.results.FirstOrDefault().rs_user_type_pincode.FirstOrDefault();
                                    pin_code_id = item.id;
                                    district_id = item.district.id;
                                    rs_district = item.district.name;
                                    state_id = item.district.state.id;
                                    state = item.district.state.name;
                                }
                            });
                        }
                        else
                        {
                            //rs_pincode = string.Empty;
                            //page.DisplayAlert("Alert", "Please select county then enter pin code", "Ok");
                        }
                    }
                }
                OnPropertyChanged("rs_country");
            }
        }


        private string _rs_district = "District";
        public string rs_district
        {
            get => _rs_district;
            set
            {
                _rs_district = value;

                if (!string.IsNullOrEmpty(rs_district) && !rs_district.Contains("District"))
                {
                    distric_text_color = (Color)Application.Current.Resources["text_color"];
                }
                else
                {
                    distric_text_color = (Color)Application.Current.Resources["placeholder_color"];
                }

                OnPropertyChanged("rs_district");
            }
        }

        private int _district_id;
        public int district_id
        {
            get => _district_id;
            set
            {
                _district_id = value;
                OnPropertyChanged("district_id");
            }
        }

        private int _pin_code_id;
        public int pin_code_id
        {
            get => _pin_code_id;
            set
            {
                _pin_code_id = value;
                OnPropertyChanged("pin_code_id");
            }
        }

        private int _state_id;
        public int state_id
        {
            get => _state_id;
            set
            {
                _state_id = value;
                OnPropertyChanged("state_id");
            }
        }


        private string _rs_pincode;
        public string rs_pincode
        {
            get => _rs_pincode;
            set
            {
                _rs_pincode = value;
                if (!string.IsNullOrEmpty(rs_pincode))
                {
                    if (!rs_country.Contains("Select country"))
                    {
                        Device.InvokeOnMainThreadAsync(async () =>
                        {
                            var response = await apiServices.DistricAndState(rs_pincode, selected_country.id, "wikitekMechanik");
                            if (response == null)
                            {
                                await page.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
                                return;
                            }
                            if (response != null)
                            {
                                var item = response.results.FirstOrDefault().rs_user_type_pincode.FirstOrDefault();
                                pin_code_id = item.id;
                                district_id = item.district.id;
                                rs_district = item.district.name;
                                state_id = item.district.state.id;
                                state = item.district.state.name;
                            }
                        });
                    }
                    else
                    {
                        //rs_pincode = string.Empty;
                        //page.DisplayAlert("Alert", "Please select county then enter pin code","Ok");
                    }
                }
                OnPropertyChanged("rs_pincode");
            }
        }


        //private string _is_active;
        //public string is_active
        //{
        //    get => _is_active;
        //    set
        //    {
        //        _is_active = value;
        //        OnPropertyChanged("is_active");
        //    }
        //}


        private string _rs_address;
        public string rs_address
        {
            get => _rs_address;
            set
            {
                _rs_address = value;
                OnPropertyChanged("rs_address");
            }
        }


        private string _state = "State";
        public string state
        {
            get => _state;
            set
            {
                _state = value;
                if (!string.IsNullOrEmpty(state) && !state.Contains("State"))
                {
                    state_text_color = (Color)Application.Current.Resources["text_color"];
                }
                else
                {
                    state_text_color = (Color)Application.Current.Resources["placeholder_color"];
                }
                OnPropertyChanged("state");
            }
        }


        private string _distric = "Distric";
        public string distric
        {
            get => _distric;
            set
            {
                _distric = value;
                if (!string.IsNullOrEmpty(distric) && !distric.Contains("Distric"))
                {
                    distric_text_color = (Color)Application.Current.Resources["text_color"];
                }
                OnPropertyChanged("distric");
            }
        }

        private Color _country_text_color = (Color)Application.Current.Resources["placeholder_color"];
        public Color country_text_color
        {
            get => _country_text_color;
            set
            {
                _country_text_color = value;
                OnPropertyChanged("country_text_color");
            }
        }

        private Color _distric_text_color = (Color)Application.Current.Resources["placeholder_color"];
        public Color distric_text_color
        {
            get => _distric_text_color;
            set
            {
                _distric_text_color = value;
                OnPropertyChanged("distric_text_color");
            }
        }


        private Color _state_text_color = (Color)Application.Current.Resources["placeholder_color"];
        public Color state_text_color
        {
            get => _state_text_color;
            set
            {
                _state_text_color = value;
                OnPropertyChanged("state_text_color");
            }
        }


        private CreateRSAgentModel _rs_agent_detail;
        public CreateRSAgentModel rs_agent_detail
        {
            get => _rs_agent_detail;
            set
            {
                _rs_agent_detail = value;
                OnPropertyChanged("rs_agent_detail");
            }
        }

        private RsUserTypeCountry _selected_country;
        public RsUserTypeCountry selected_country
        {
            get => _selected_country;
            set
            {
                _selected_country = value;
                OnPropertyChanged("selected_country");
            }
        }

        private VehicleSegment _segement_detail;
        //public VehicleSegment segement_detail
        //{
        //    get => _segement_detail;
        //    set
        //    {
        //        _segement_detail = value;
        //        OnPropertyChanged("segement_detail");
        //    }
        //}

        #endregion



        #region Methods
        [Obsolete]
        public void InitializeCommands()
        {
            ////Country country = new Country();
            //MessagingCenter.Subscribe<SegmentViewModel, VehicleSegment>(this, "selected_segment_workshop", async (sender, arg) =>
            //{
            //    //segment = arg.segment_name;
            //    //segment_id = arg.id;
            //    segement_detail = arg;
            //});

            MessagingCenter.Subscribe<CountyViewModel, RsUserTypeCountry>(this, $"selected_country_CreateRSAgentVM", async (sender, arg) =>
            {
                selected_country = arg;
                rs_country = arg.name;
                //txt_country.TextColor = (Color)Application.Current.Resources["text_color"];
            });

            SubmitCommand = new Command(async (obj) =>
            {

                try
                {
                    var IsError = await Validation();
                    if (!IsError)
                    {
                        using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                        {
                            await Task.Delay(200);
                            rs_agent_detail.name = rs_agent_name;
                            rs_agent_detail.status = true;
                            rs_agent_detail.country = selected_country.id;
                            rs_agent_detail.district = district_id;
                            rs_agent_detail.mobile = mobile;
                            rs_agent_detail.gps_location = rs_address;
                            rs_agent_detail.pincode = pin_code_id;
                            rs_agent_detail.state = state_id;
                            rs_agent_detail.segment= 2;
                            rs_agent_detail.user_type.Add(2);

                            var response = await apiServices.CreateRSAgent(rs_agent_detail);
                            if (response == null)
                            {
                                await page.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
                                return;
                            }
                            var api_status_code = StaticMethods.http_status_code(response.status_code);

                            if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                            {
                                await page.DisplayAlert("Success!", "Workshop Created successfully!", "Ok");
                                await page.Navigation.PopAsync();
                            }
                            else
                            {
                                var error = StaticMethods.http_status_code(response.status_code);
                                {
                                    if (string.IsNullOrEmpty(response.detail))
                                    {
                                        await page.DisplayAlert(error, "Create workshop service not working", "Ok");
                                    }
                                    else
                                    {
                                        await page.DisplayAlert(error, response.detail, "Ok");
                                    }


                                }

                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            });

            //SegmentCommand = new Command(async (obj) =>
            //{
            //    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            //    {
            //        await Task.Delay(200);
            //        StaticMethods.last_page = "CreateWorkshop";
            //        await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.SegmentPopupPage(StaticMethods.last_page));
            //    }
            //});

            CountryCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);
                    StaticMethods.last_page = "CreateWorkshop";
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.CountyPopupPage());
                }
            });

        }

        public async Task<bool> Validation()
        {
            bool IsError = false;

            if (string.IsNullOrEmpty(rs_agent_name))
            {
                await page.DisplayAlert("Alert", "Please enter agent name", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(email_id))
            {
                await page.DisplayAlert("Alert", "Please enter email address", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(mobile))
            {
                await page.DisplayAlert("Alert", "Please enter mobile number", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(rs_country))
            {
                await page.DisplayAlert("Alert", "Please select a country", "Ok");
                IsError = true;
            }
            else if (string.IsNullOrEmpty(rs_pincode))
            {
                await page.DisplayAlert("Alert", "Please enter pin code", "Ok");
                IsError = true;
            }
            //else if (segement_detail.segment_name.Contains("Select"))
            //{
            //    await page.DisplayAlert("Alert", "Please select segment", "Ok");
            //    IsError = true;
            //}
            //else if (string.IsNullOrEmpty(device_type))
            //{
            //    await page.DisplayAlert("Alert", "Device type not found", "Ok");
            //    IsError = true;
            //}
            //else if (string.IsNullOrEmpty(mac_id))
            //{
            //    await page.DisplayAlert("Alert", "Mac address not found", "Ok");
            //    IsError = true;
            //}
            //else if (string.IsNullOrEmpty(user_profile_pic))
            //{
            //    await page.DisplayAlert("Alert", "please click your profile", "Ok");
            //    IsError = true;
            //}
            //else if (string.IsNullOrEmpty(pin_code))
            //{
            //    await page.DisplayAlert("Alert", "Please enter pin code", "Ok");
            //    IsError = true;
            //}
            //else if (string.IsNullOrEmpty(rs_agent_id))
            //{
            //    await page.DisplayAlert("Alert", "Please select RSAgent.\n\nIf RSAgent is not created so create firstly RSAgent.", "Ok");
            //    IsError = true;
            //}

            return IsError;
        }

        #endregion


        #region ICommands
        public ICommand CountryCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand SegmentCommand { get; set; }
        #endregion
    }
}
