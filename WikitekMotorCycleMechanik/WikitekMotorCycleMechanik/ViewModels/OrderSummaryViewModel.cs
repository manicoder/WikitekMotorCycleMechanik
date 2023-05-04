using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class OrderSummaryViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly IPayment payment;
        readonly Page page;

        public OrderSummaryViewModel(Page page, ObservableCollection<CartItem> cart_list, ShipmentAddressModel shipment_address, BillingAddressModel billing_address) : base(page)
        {
            try
            {
                this.page = page;
                apiServices = new ApiServices();
                this.cart_list = cart_list;
                this.shipment_address = shipment_address;
                this.billing_address = billing_address;
                this.payment = DependencyService.Get<IPayment>();
                //user_name = new ObservableCollection<MarketPlaceModel>();

                CalculateFinalPrice();

                InitializeCommands();

                

                //Device.InvokeOnMainThreadAsync(async () =>
                //{
                //    GetCartList();
                //});
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties

        private bool _isIgstVisible;
        public bool isIgstVisible
        {
            get => _isIgstVisible;
            set
            {
                _isIgstVisible = value;
                OnPropertyChanged("isIgstVisible");
            }
        }

        private bool _isCsgstVisible;
        public bool isCsgstVisible
        {
            get => _isCsgstVisible;
            set
            {
                _isCsgstVisible = value;
                OnPropertyChanged("isCsgstVisible");
            }
        }

        private bool _isGstVisible = false;
        public bool isGstVisible
        {
            get => _isGstVisible;
            set
            {
                _isGstVisible = value;
                OnPropertyChanged("isGstVisible");
            }
        }

        private string _gstin = String.Empty;
        public string gstin
        {
            get => _gstin;
            set
            {
                _gstin = value;
                OnPropertyChanged("gstin");
            }
        }

        private double _total_price = 0;
        public double total_price
        {
            get => _total_price;
            set
            {
                _total_price = value;
                OnPropertyChanged("total_price");
            }
        }

        private double _amount = 0;
        public double amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged("amount");
            }
        }

        private double _sgst = 0;
        public double sgst
        {
            get => _sgst;
            set
            {
                _sgst = value;
                OnPropertyChanged("sgst");
            }
        }

        private double _cgst = 0;
        public double cgst
        {
            get => _cgst;
            set
            {
                _cgst = value;
                OnPropertyChanged("cgst");
            }
        }

        private double _igst = 0;
        public double igst
        {
            get => _igst;
            set
            {
                _igst = value;
                OnPropertyChanged("igst");
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

        public bool isValidGST { get; set; }
        #endregion


        #region Methods

        public void InitializeCommands()
        {
            try
            {

                PaymentCommand = new Command(async (obj) =>
                {
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        try
                        {
                            await Task.Delay(200);

                            if (await Validation())
                            {
                                int amount = 0;

                                var time = DateTime.Now.ToString("MMddyyyyHHmmss");
                                if (total_price.ToString().Contains("."))
                                {
                                    string str1 = string.Format("{0:0.00}", total_price);
                                    string str = str1.Replace(".", string.Empty);
                                    amount = int.Parse(str);
                                }
                                else
                                {
                                    amount = int.Parse(total_price.ToString() + "00");
                                }
                                generate_order_id = new GenerateOrderIdModel
                                {
                                    amount = amount,
                                    currency = "INR",
                                    receipt = $"reciept_{time}",
                                };

                                ApiServices api = new ApiServices();
                                var response = await api.GenerateOrderId(generate_order_id);
                                var payment_result = await payment.StartPaymet(response);
                            }
                            else
                                await page.DisplayAlert("Alert", "Please enter valid GST Identification number", "Ok");

                            //MessagingCenter.Send<OrderSummaryViewModel, string>(this, "payment", "");

                            //await page.Navigation.PushAsync(new Views.MarketPlace.OrderSummaryPage());
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                });

                GstTappedCommand = new Command(async (obj) =>
                {
                    isGstVisible = !isGstVisible;
                });
            }
            catch (Exception ex)
            {
            }
        }

        public void CalculateFinalPrice()
        {
            foreach (var item in cart_list)
            {
                amount = amount + item.extended_price;
            }
            if (shipment_address.state == billing_address.state)
            {
                sgst = amount * 9 / 100;
                cgst = amount * 9 / 100;
                total_price = amount + cgst + sgst;
                isCsgstVisible = true;
            }
            else
            {
                igst = amount * 18 / 100;
                total_price = amount + igst;
                isIgstVisible = true;
            }
        }

        public async Task<bool> Validation()
        {
            if (gstin.Length == 15)
            {
                if (isValidGST)
                    return true;
                else
                    return false;
            }
            else if (gstin.Length == 0)
                return true;
            else
                return false;
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
        public ICommand PaymentCommand { get; set; }
        public ICommand GstTappedCommand { get; set; }
        #endregion
    }
}