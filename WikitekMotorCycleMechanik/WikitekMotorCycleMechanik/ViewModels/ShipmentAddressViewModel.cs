using Acr.UserDialogs;
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
    public class ShipmentAddressViewModel : ViewModelBase
    {
        ApiServices apiServices;
        //readonly INavigation navigationService;
        readonly Page page;

        public ShipmentAddressViewModel(Page page, ObservableCollection<CartItem> cart_list) : base(page)
        {
            try
            {
                this.page = page;
                apiServices = new ApiServices();
                //user_name = new ObservableCollection<MarketPlaceModel>();
                this.cart_list = cart_list;
                InitializeCommands();
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    //GetSegmentList();
                    GetCartList();
                });
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties
        private bool _isSelected = true;
        public bool isSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged("isSelected");
            }
        }

        private string _user_name = $"{App.user_first_name} {App.user_last_name}";
        public string user_name
        {
            get => _user_name;
            set
            {
                _user_name = value;
                OnPropertyChanged("user_name");
            }
        }

        private string _user_nameBilling = $"{App.user_first_name} {App.user_last_name}";
        public string user_nameBilling
        {
            get => _user_nameBilling;
            set
            {
                _user_nameBilling = value;
                OnPropertyChanged("user_nameBilling");
            }
        }

        private string _country = "Select country";
        public string country
        {
            get => _country;
            set
            {
                _country = value;

                //if (!string.IsNullOrEmpty(country) && !country.Contains("Select country"))
                //{
                //    if (!string.IsNullOrEmpty(pin_code))
                //    {
                //        Device.InvokeOnMainThreadAsync(async () =>
                //        {
                //            var response = await apiServices.DistricAndState(pin_code, country_detail.id, "WikitekMotorCycleMechanik");

                //            var api_status_code = StaticMethods.http_status_code(response.status_code);

                //            if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                //            {
                //                if (response.results.FirstOrDefault().rs_user_type_pincode.Count > 0)
                //                {
                //                    pincode_detail = response.results.FirstOrDefault().rs_user_type_pincode.FirstOrDefault();
                //                    is_visible = true;
                //                }
                //                else
                //                {
                //                    pincode_detail = new RsUserTypePincode();
                //                    is_visible = false;
                //                }
                //            }
                //            else
                //            {
                //                await page.DisplayAlert(api_status_code, "Create user Api  not working", "Ok");
                //            }
                //        });
                //    }
                //}

                OnPropertyChanged("country");
            }
        }

        private string _countryBilling = "Select country";
        public string countryBilling
        {
            get => _countryBilling;
            set
            {
                _countryBilling = value;

                //if (!string.IsNullOrEmpty(country) && !country.Contains("Select country"))
                //{
                //    if (!string.IsNullOrEmpty(pin_code))
                //    {
                //        Device.InvokeOnMainThreadAsync(async () =>
                //        {
                //            var response = await apiServices.DistricAndState(pin_code, country_detail.id, "WikitekMotorCycleMechanik");

                //            var api_status_code = StaticMethods.http_status_code(response.status_code);

                //            if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                //            {
                //                if (response.results.FirstOrDefault().rs_user_type_pincode.Count > 0)
                //                {
                //                    pincode_detail = response.results.FirstOrDefault().rs_user_type_pincode.FirstOrDefault();
                //                    is_visible = true;
                //                }
                //                else
                //                {
                //                    pincode_detail = new RsUserTypePincode();
                //                    is_visible = false;
                //                }
                //            }
                //            else
                //            {
                //                await page.DisplayAlert(api_status_code, "Create user Api  not working", "Ok");
                //            }
                //        });
                //    }
                //}

                OnPropertyChanged("countryBilling");
            }
        }

        private string _pin_code;// = "515842";
        public string pin_code
        {
            get => _pin_code;
            set
            {
                _pin_code = value;

                if (!string.IsNullOrEmpty(pin_code))
                {
                    if (!country.Contains("Select country"))
                    {
                        Device.InvokeOnMainThreadAsync(async () =>
                        {
                            //var response = await apiServices.DistricAndState(pin_code, country_detail.id, "WikitekMotorCycleMechanik");
                            var response = await apiServices.DistricAndStateInfo(pin_code);
                            var api_status_code = StaticMethods.http_status_code(response.status_code);

                            if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                            {
                                if (response.results.Count > 0)
                                {
                                    pincode_detail = response.results.FirstOrDefault();
                                    is_visible = true;
                                }
                                else
                                {
                                    pincode_detail = new PincodeInfo();
                                    is_visible = false;
                                }
                            }
                            else
                            {
                                await page.DisplayAlert(api_status_code, "Create user Api  not working", "Ok");
                            }
                        });
                    }
                }
                OnPropertyChanged("pin_code");
            }
        }

        private string _pin_codeBilling;// = "515842";
        public string pin_codeBilling
        {
            get => _pin_codeBilling;
            set
            {
                _pin_codeBilling = value;

                if (!string.IsNullOrEmpty(pin_codeBilling))
                {
                    if (!countryBilling.Contains("Select country"))
                    {
                        Device.InvokeOnMainThreadAsync(async () =>
                        {
                            //var response = await apiServices.DistricAndState(pin_code, country_detail.id, "WikitekMotorCycleMechanik");
                            var response = await apiServices.DistricAndStateInfo(pin_codeBilling);
                            var api_status_code = StaticMethods.http_status_code(response.status_code);

                            if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                            {
                                if (response.results.Count > 0)
                                {
                                    pincode_detailBilling = response.results.FirstOrDefault();
                                    is_visible2 = true;
                                }
                                else
                                {
                                    pincode_detailBilling = new PincodeInfo();
                                    is_visible2 = false;
                                }
                            }
                            else
                            {
                                await page.DisplayAlert(api_status_code, "Create user Api  not working", "Ok");
                            }
                        });
                    }
                }
                OnPropertyChanged("pin_codeBilling");
            }
        }

        private string _address;
        public string address
        {
            get => _address;
            set
            {
                _address = value;
                OnPropertyChanged("address");
            }
        }

        private string _addressBilling;
        public string addressBilling
        {
            get => _addressBilling;
            set
            {
                _addressBilling = value;
                OnPropertyChanged("addressBilling");
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

        private ObservableCollection<CartItem> _cart_list;
        public ObservableCollection<CartItem> cart_list
        {
            get => _cart_list;
            set
            {
                _cart_list = value;
                OnPropertyChanged("cart_list");
            }
        }

        private ShipmentAddressModel _shipment_address;
        public ShipmentAddressModel shipment_address
        {
            get => _shipment_address;
            set
            {
                _shipment_address = value;
                OnPropertyChanged("shipment_address");
            }
        }

        private BillingAddressModel _billing_address;
        public BillingAddressModel billing_address
        {
            get => _billing_address;
            set
            {
                _billing_address = value;
                OnPropertyChanged("billing_address");
            }
        }

        //private RsUserTypePincode _pincode_detail;
        //public RsUserTypePincode pincode_detail
        //{
        //    get => _pincode_detail;
        //    set
        //    {
        //        _pincode_detail = value;
        //        OnPropertyChanged("pincode_detail");
        //    }
        //}

        private PincodeInfo _pincode_detail;
        public PincodeInfo pincode_detail
        {
            get => _pincode_detail;
            set
            {
                _pincode_detail = value;
                OnPropertyChanged("pincode_detail");
            }
        }

        private PincodeInfo _pincode_detailBilling;
        public PincodeInfo pincode_detailBilling
        {
            get => _pincode_detailBilling;
            set
            {
                _pincode_detailBilling = value;
                OnPropertyChanged("pincode_detailBilling");
            }
        }

        private bool _is_visible;
        public bool is_visible
        {
            get => _is_visible;
            set
            {
                _is_visible = value;
                OnPropertyChanged("is_visible");
            }
        }

        private bool _is_visible2;
        public bool is_visible2
        {
            get => _is_visible2;
            set
            {
                _is_visible2 = value;
                OnPropertyChanged("is_visible2");
            }
        }

        private bool _isBillingVisible = false;
        public bool isBillingVisible
        {
            get => _isBillingVisible;
            set
            {
                _isBillingVisible = value;
                OnPropertyChanged("isBillingVisible");
            }
        }
        #endregion


        #region Methods

        public void InitializeCommands()
        {
            try
            {

                MessagingCenter.Subscribe<CountyViewModel, RsUserTypeCountry>(this, "selected_country_registrationVM", async (sender, arg) =>
                {
                    country = arg.name;
                    country_detail = arg;
                });

                MessagingCenter.Subscribe<CountyViewModel, RsUserTypeCountry>(this, "selected_country_billing", async (sender, arg) =>
                {
                    countryBilling = arg.name;
                    country_detail = arg;
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

                CountryBillingCommand = new Command(async (obj) =>
                {
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(200);
                        StaticMethods.last_page = "billing";
                        await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.CountyPopupPage());
                    }
                });

                SummarizeOrderCommand = new Command(async (obj) =>
                {
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(200);
                        var IsValidate = await ValidationBilling();
                        if (IsValidate)
                        {
                            billing_address = new BillingAddressModel
                            {
                                address = addressBilling + ", " + pincode_detailBilling.district.name + ", "
                                                + pincode_detailBilling.district.state.name,
                                country = countryBilling,
                                pin_code = pin_codeBilling,
                                user_name = user_nameBilling,
                                state = pincode_detailBilling.district.state.name
                            };



                            if (isSelected)
                            {
                                shipment_address = new ShipmentAddressModel
                                {
                                    address = billing_address.address,
                                    country = billing_address.country,
                                    pin_code = billing_address.pin_code,
                                    user_name = billing_address.user_name,
                                    state = billing_address.state
                                };
                                await page.Navigation.PushAsync(new Views.MarketPlace.OrderSummaryPage(cart_list, shipment_address, billing_address));
                            }
                            else
                            {
                                if (await Validation())
                                {
                                    shipment_address = new ShipmentAddressModel
                                    {
                                        address = address + ", " + pincode_detail.district.name + ", "
                                                + pincode_detail.district.state.name,
                                        country = country,
                                        pin_code = pin_code,
                                        user_name = user_name,
                                        state = pincode_detail.district.state.name
                                    };
                                    await page.Navigation.PushAsync(new Views.MarketPlace.OrderSummaryPage(cart_list, shipment_address, billing_address));
                                }
                            }
                        }
                    }
                });

                CheckChangedCommand = new Command(async (obj) =>
                {
                    isSelected = !isSelected;
                    isBillingVisible = !isBillingVisible;
                });
            }
            catch (Exception ex)
            {
            }
        }

        public async Task<bool> Validation()
        {
            try
            {
                bool IsValidate = true;
                if (string.IsNullOrEmpty(user_name))
                {
                    await page.DisplayAlert("Alert", "Please enter user name.", "Ok");
                    IsValidate = false;
                }
                else if (string.IsNullOrEmpty(country))
                {
                    await page.DisplayAlert("Alert", "Please select a country.", "Ok");
                    IsValidate = false;
                }
                else if (string.IsNullOrEmpty(pin_code))
                {
                    await page.DisplayAlert("Alert", "Please  enter pin number.", "Ok");
                    IsValidate = false;
                }
                else if (string.IsNullOrEmpty(address))
                {
                    await page.DisplayAlert("Alert", "Please enter shipment address.", "Ok");
                    IsValidate = false;
                }
                else if (!is_visible || !is_visible2)
                {
                    await page.DisplayAlert("Alert", "Please enter valid pincode.", "Ok");
                    IsValidate = false;
                }

                return IsValidate;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<bool> ValidationBilling()
        {
            try
            {
                bool IsValidate = true;
                if (string.IsNullOrEmpty(user_nameBilling))
                {
                    await page.DisplayAlert("Alert", "Please enter user name.", "Ok");
                    IsValidate = false;
                }
                else if (string.IsNullOrEmpty(countryBilling))
                {
                    await page.DisplayAlert("Alert", "Please select a country.", "Ok");
                    IsValidate = false;
                }
                else if (string.IsNullOrEmpty(pin_codeBilling))
                {
                    await page.DisplayAlert("Alert", "Please enter billing pin number.", "Ok");
                    IsValidate = false;
                }
                else if (string.IsNullOrEmpty(addressBilling))
                {
                    await page.DisplayAlert("Alert", "Please enter billing address.", "Ok");
                    IsValidate = false;
                }
                

                return IsValidate;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async void GetCartList()
        {
            try
            {
                //var response = await apiServices.GetMarketPlaceList(App.access_token);
                ////user_name = new ObservableCollection<MarketPlaceModel>(response);

                //foreach (var item in response)
                //{

                //    user_name.Add(
                //        new MarketPlaceModel
                //        {
                //            sparepart_id = new SparepartId
                //            {
                //                basic_price = item.sparepart_id.basic_price,
                //                equi_oe_part = item.sparepart_id.equi_oe_part,
                //                sparepart_id = item.sparepart_id.sparepart_id,
                //                brand = item.sparepart_id.brand,
                //                extended_price = item.sparepart_id.basic_price,
                //                gst = item.sparepart_id.gst,
                //                HSN = item.sparepart_id.HSN,
                //                id = item.sparepart_id.id,
                //                is_active = item.sparepart_id.is_active,
                //                part_description = item.sparepart_id.part_description,
                //                part_number = item.sparepart_id.part_number,
                //                quantity = item.sparepart_id.quantity,
                //                types = item.sparepart_id.types,
                //            }
                //        });
                //}

                ////user_name = new ObservableCollection<MarketPlaceModel>();

                ////var api_status_code = StaticMethods.http_status_code(response.status_code);

                ////if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                ////{
                ////    //user_name = new ObservableCollection<MarketPlaceModel>(response);
                ////}
                ////else
                ////{
                ////    await page.DisplayAlert(api_status_code, response.error, "Ok");
                ////}
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region ICommands
        public ICommand CountryCommand { get; set; }
        public ICommand CountryBillingCommand { get; set; }
        public ICommand SummarizeOrderCommand { get; set; }
        public ICommand CheckChangedCommand { get; set; }
        #endregion
    }
}