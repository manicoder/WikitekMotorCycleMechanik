using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using Xamarin.Forms;
using WikitekMotorCycleMechanik.StaticInfo;
using System.Collections.ObjectModel;
using System.Linq;
using Newtonsoft.Json;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using Xamarin.Essentials;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class SubscriptionViewModel : ViewModelBase
    {
        ApiServices apiServices;
        //readonly INavigation navigationService;
        readonly IPayment payment;
        readonly Page page;
        int segment_id = 0;
        public SubscriptionViewModel(Page page, string pack) : base(page)
        {
            try
            {
                apiServices = new ApiServices();

                this.page = page;
                this.pack = pack;

                if (this.pack == "dongle")
                {
                    pageTitle = "REGISTER DONGLE";
                }
                else
                {
                    pageTitle = "NEW SUBSCRIPTION";
                }

                segment_id = (int)App.user.segment_id;
                this.payment = DependencyService.Get<IPayment>();
                
                //selected_segment = new VehicleSegment();
                //selected_segment.id = App.user.segment_id;

                //this.navigationService = page.Navigation;
                //InitializeCommands();
                //GetPackageList();
                //Device.InvokeOnMainThreadAsync(async () =>
                //{
                //    await GetMacId();
                //});
            }
            catch (Exception ex)
            {
            }
        }
        #region Properties

        //private string _user_mail = $"User ID : {App.user_mail}";
        private string _user_mail = $"User ID : {App.user.first_name} {App.user.last_name}";
        public string user_mail
        {
            get => _user_mail;
            set
            {
                _user_mail = value;

                OnPropertyChanged("user_mail");
            }
        }


        private double _package_amount; //= "Select Vehicle Segment";
        public double package_amount
        {
            get => _package_amount;
            set
            {
                _package_amount = value;
                OnPropertyChanged("package_amount");
            }
        }

        private string _package_description; //= "Select Vehicle Segment";
        public string package_description
        {
            get => _package_description;
            set
            {
                _package_description = value;
                OnPropertyChanged("package_description");
            }
        }

        private bool _package_detail_visible = false;
        public bool package_detail_visible
        {
            get => _package_detail_visible;
            set
            {
                _package_detail_visible = value;
                OnPropertyChanged("package_detail_visible");
            }
        }

        //private string _segment = "Select Vehicle Segment";
        private string _segment = $"{App.user.segment}";
        public string segment
        {
            get => _segment;
            set
            {
                _segment = value;

                //if (!string.IsNullOrEmpty(segment) && !segment.Contains("Select Vehicle Segment"))
                //{
                //    package = "Select package";
                //    package_detail_visible = false;
                //    segment_text_color = (Color)Application.Current.Resources["text_color"];
                //}

                OnPropertyChanged("segment");
            }
        }

        private string _pack;
        public string pack
        {
            get => _pack;
            set
            {
                _pack = value;
                OnPropertyChanged("pack");
            }
        }

        private string _EnteredPackage;// = "INWM2WDIYDPFY";
        public string EnteredPackage
        {
            get => _EnteredPackage;
            set
            {
                _EnteredPackage = value;
                #region commented
                //if (!string.IsNullOrEmpty(EnteredPackage))
                //{
                //    var item = package_list.FirstOrDefault(x => x.code == EnteredPackage);
                //    if (item != null)
                //    {
                //        package = item.code;
                //        selected_package = item;
                //        package_detail_visible = true;
                //        package_description = item.description;
                //        package_amount = item.amount;
                //    }
                //    else
                //    {
                //        package_detail_visible = false;
                //    }
                //}
                #endregion
                OnPropertyChanged("EnteredPackage");
            }
        }

        //private Color _segment_text_color = (Color)Application.Current.Resources["placeholder_color"];
        //public Color segment_text_color
        //{
        //    get => _segment_text_color;
        //    set
        //    {
        //        _segment_text_color = value;
        //        OnPropertyChanged("segment_text_color");
        //    }
        //}

        //private VehicleSegment _selected_segment;
        //public VehicleSegment selected_segment
        //{
        //    get => _selected_segment;
        //    set
        //    {
        //        _selected_segment = value;
        //        OnPropertyChanged("selected_segment");
        //    }
        //}


        private string _package = "Select package";
        public string package
        {
            get => _package;
            set
            {
                _package = value;
                if (!package.Contains("Select package"))
                {
                    if (!string.IsNullOrEmpty(segment) && !segment.Contains("Select Vehicle Segment"))
                    {
                        package_text_color = (Color)Application.Current.Resources["text_color"];
                        package_detail_visible = true;
                    }
                }
                else
                {
                    package_text_color = (Color)Application.Current.Resources["placeholder_color"];
                }
                OnPropertyChanged("package");
            }
        }

        private Color _package_text_color = (Color)Application.Current.Resources["placeholder_color"];
        public Color package_text_color
        {
            get => _package_text_color;
            set
            {
                _package_text_color = value;
                OnPropertyChanged("package_text_color");
            }
        }

        private ObservableCollection<PackageResult> _package_list;
        public ObservableCollection<PackageResult> package_list
        {
            get => _package_list;
            set
            {
                _package_list = value;
                OnPropertyChanged("package_list");
            }
        }

        private PackageResult _selected__package;
        public PackageResult selected_package
        {
            get => _selected__package;
            set
            {
                _selected__package = value;
                OnPropertyChanged("selected_package");
            }
        }

        private GenerateOrderIdModel _generate_order_id;
        public GenerateOrderIdModel generate_order_id
        {
            get => _generate_order_id;
            set
            {
                _generate_order_id = value;
                OnPropertyChanged("generate_order_id");
            }
        }

        private string _pageTitle;
        public string pageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;

                OnPropertyChanged("pageTitle");
            }
        }

        #endregion

        #region Methods

        [Obsolete]
        public void InitializeCommands()
        {
            //MessagingCenter.Subscribe<SegmentViewModel, VehicleSegment>(this, "subscription_page", async (sender, arg) =>
            //{
            //    segment = arg.segment_name;
            //    selected_segment = arg;
            //});

            //MessagingCenter.Subscribe<PackageViewModel, PackageResult>(this, "selected_package", async (sender, arg) =>
            //{
            //    package = arg.code;
            //    selected_package = arg;
            //    package_detail_visible = true;
            //    package_description = arg.description;
            //    //double value = 234.66;
            //    package_amount = arg.amount;
            //});



            //SelectSegmentCommand = new Command(async (obj) =>
            //{
            //    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            //    {
            //        await Task.Delay(200);

            //        await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.SegmentPopupPage("subscription_page"));
            //    }
            //});

            //SelectSubscriptionCommand = new Command(async (obj) =>
            //{
            //    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            //    {
            //        try
            //        {
            //            await Task.Delay(200);

            //            if (!string.IsNullOrEmpty(segment))
            //            {
            //                if (!segment.Contains("Select Vehicle Segment"))
            //                {

            //                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.PackagePopupPage(selected_segment.id, pack));
            //                }
            //                else
            //                {
            //                    await page.DisplayAlert("Alert", "Please select vehicle segment", "Ok");
            //                }
            //            }
            //            else
            //            {
            //                await page.DisplayAlert("Alert", "Please select vehicle segment", "Ok");
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //        }
            //    }
            //});

            PurchaseCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    try
                    {
                        int amount = 0;
                        await Task.Delay(200);
                        var time = DateTime.Now.ToString("MMddyyyyHHmmss");

                        if (package_amount.ToString().Contains("."))
                        {
                            string str = package_amount.ToString().Remove(package_amount.ToString().Length - 2);
                            amount = int.Parse(str);
                        }
                        else
                        {
                            amount = int.Parse(package_amount.ToString() + "00");
                            //int amount = int.Parse(str.ToString()+"00");
                        }
                        if (amount < 0)
                        {
                            generate_order_id = new GenerateOrderIdModel
                            {
                                amount = amount,
                                currency = "INR",
                                receipt = $"reciept_{time}",
                            };
                            ApiServices api = new ApiServices();
                            var response = await api.GenerateOrderId(generate_order_id);
                            if (response != null)
                            {
                                var payment_result = await payment.StartPaymet(response);
                            }
                            else
                            {
                                await page.DisplayAlert("Alert", "Invalid opration", "OK");
                            }
                        }
                        else
                        {
                            PurchasePackageModel paymentServerRequestModel = new PurchasePackageModel();
                            ApiServices apiServices = new ApiServices();
                            var data = App.paymentResponseModel;

                            //if (!string.IsNullOrEmpty(data.OrderId) && !string.IsNullOrEmpty(data.PaymentId))
                            //{
                            paymentServerRequestModel = new PurchasePackageModel
                            {
                                user_type = App.user.user_type,
                                package_visibility = selected_package.package_visibility,
                                oem_specific = selected_package.oem_specific,
                                amount = selected_package.amount,
                                code = selected_package.code,
                                country = selected_package.country.id,
                                segments = (int)App.user.segment_id,
                                email = App.user.email,
                                mobile = App.user.mobile,
                                grace_period = selected_package.grace_period,
                                oem = selected_package.oem,
                                runtime_params = selected_package.packages.runtime_params,
                                recurrence_period = selected_package.recurrence_period,
                                start_date = DateTime.Now,
                                //validity_period= viewModel.selected_package.v,
                                usability = selected_package.usability,
                                recurrence_unit = selected_package.recurrence_unit,
                                user = App.user.user_id,
                                razorpay_order_id = data?.OrderId,
                                razorpay_payment_id = data?.PaymentId,
                                razorpay_signature = data?.Signature,
                                package_name = pack,
                            };
                            var json = JsonConvert.SerializeObject(paymentServerRequestModel);
                            await Device.InvokeOnMainThreadAsync(async () =>
                            {
                                var result = await apiServices.RegisterSubscriptionToServer(Xamarin.Essentials.Preferences.Get("token", null), paymentServerRequestModel);

                                if (result != null)
                                {
                                    //var cart = await apiServices.DeletePurchaseCart(App.access_token);

                                    await page.DisplayAlert("Success!", "Subscription Successfully done.", "OK");
                                    //App.user = await apiServices.UserLogin(App.login_model);
                                    //App.paymentResponseModel = null;
                                    //App.Current.MainPage = new NavigationPage(new MasterDetailView(App.user) { Detail = new NavigationPage(new Views.Settings.SettingPage(App.user)) });
                                    App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                                }
                                else
                                {
                                    await page.DisplayAlert("Failed!", "Payment failed.", "OK");
                                }
                            });
                            //}
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
            });
        }

        public async void GetPackageList()
        {
            try
            {
                var response = await apiServices.GetPackageBySegment(Xamarin.Essentials.Preferences.Get("token", null), "wikitekMechanik", pack, App.country_id, segment_id);

                var api_status_code = StaticMethods.http_status_code(response.status_code);

                if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                {
                    package_list = new ObservableCollection<PackageResult>(response.results);

                    if (package_list != null && package_list.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(EnteredPackage))
                        {
                            var item = package_list.FirstOrDefault(x => x.code == EnteredPackage);
                            if (item != null)
                            {
                                package = item.code;
                                selected_package = item;
                                package_detail_visible = true;
                                package_description = item.description;
                                package_amount = item.amount;
                            }
                            else
                            {
                                package_detail_visible = false;
                            }
                        }
                    }
                }
                else
                {
                    //await page.DisplayAlert(api_status_code, "Create user Api  not working", "Ok");
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async Task GetSubscription(string serialNo, string partNo)
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await page.DisplayAlert("", $"Part No.: {partNo} & \nSerial No.: {serialNo}", "Ok");
                    var result = await apiServices.GetSubscription(string.Empty, partNo, serialNo);

                    if (!result.status)
                    {
                        //DependencyService.Get<Interfaces.IToasts>().Show($"{ mode.status}");
                        Acr.UserDialogs.UserDialogs.Instance.Toast(result.message, new TimeSpan(0, 0, 0, 3));
                        return;
                    }

                    if (result.results == null || (!result.results.Any()))
                    {
                        Acr.UserDialogs.UserDialogs.Instance.Toast("subscription not found", new TimeSpan(0, 0, 0, 3));
                        return;
                    }

                    var result1 = await apiServices.Subscription(Preferences.Get("token", null), App.user_id, result.results[0].batch_serialno[0].serial_number);

                    if (!result1.success)
                    {
                        //DependencyService.Get<Interfaces.IToasts>().Show($"{ mode.status}");
                        Acr.UserDialogs.UserDialogs.Instance.Toast(result1.status, new TimeSpan(0, 0, 0, 3));
                        return;
                    }

                    RegisterDongleModel RDM = new RegisterDongleModel
                    {
                        mac_id = serialNo,
                        device_type = partNo,
                    };
                    var result2 = await apiServices.RegisterDongle(RDM, Xamarin.Essentials.Preferences.Get("token", null));

                    await page.DisplayAlert("", result2.message, "Ok");
                });
                
            }
            catch (Exception ex)
            {

            }
        }

        public async Task RegisterDongle(string partNo, string macId)
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await page.DisplayAlert("", "Mac Id: " + macId + "\nPart No.: " + partNo,"Ok");

                    RegisterDongleModel RDM = new RegisterDongleModel
                    {
                        mac_id = macId,
                        device_type = partNo,
                    };
                    var result = await apiServices.RegisterDongle(RDM, Xamarin.Essentials.Preferences.Get("token", null));

                    if (result == null)
                    {
                        await Application.Current.MainPage.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
                        return;
                    }

                    var api_status_code = StaticMethods.http_status_code(result.status_code);

                    if (result.status_code == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await Application.Current.MainPage.DisplayAlert("Failed!", $"User Unauthorized. \nPlease Login again.", "OK");
                        Preferences.Set("token", "");
                        Preferences.Set("IsUpdate", "true");
                        App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                    }

                    if (result.status_code == System.Net.HttpStatusCode.OK || result.status_code == System.Net.HttpStatusCode.Created)
                    {
                        await Application.Current.MainPage.DisplayAlert("Success!", $"{result.error}", "OK");
                        App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                    }
                    else if (result.status_code == System.Net.HttpStatusCode.Forbidden)
                    {
                        await Application.Current.MainPage.DisplayAlert("Failed!", $"{result.error}\n{result.message}", "OK");
                        //App.Current.MainPage = new NavigationPage(new Views.Login.LoginPage());
                    }
                    else
                    {
                        await page.DisplayAlert("Alert", $"{result?.message}", "Ok");
                        return;
                    }
                });
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region ICommands
        public ICommand SelectSegmentCommand { get; set; }
        public ICommand SelectSubscriptionCommand { get; set; }
        public ICommand PurchaseCommand { get; set; }


        public ICommand ManualSubscriptionCommand => new Command(async (obj) =>
        {
            try
            {
                if (pack != "dongle")
                {
                    string partNo = EnteredPackage.Substring(0, EnteredPackage.IndexOf("*"));
                    string serialNo = EnteredPackage.Substring(EnteredPackage.IndexOf("*") + 1);
                    await GetSubscription(serialNo, partNo);
                }
                else
                {
                    string macId = EnteredPackage.Substring(EnteredPackage.IndexOf("*") + 1);
                    string partNo = EnteredPackage.Substring(0, EnteredPackage.IndexOf("*"));
                    await RegisterDongle(partNo, macId);
                }

            }
            catch (Exception ex)
            {

            }
        });


        #endregion
    }
}
