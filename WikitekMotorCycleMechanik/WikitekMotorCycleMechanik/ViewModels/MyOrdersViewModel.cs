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
using WikitekMotorCycleMechanik.Views.Login;
using WikitekMotorCycleMechanik.Views.MarketPlace;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class MyOrdersViewModel : BaseViewModel
    {
        ApiServices apiServices;
        readonly Page page;
        public MyOrdersViewModel(Page page)
        {
            this.page = page;
            apiServices = new ApiServices();
            GetMyOrdersList();

            OrderSelectCommand = new Command(async (obj) =>
            {
                var selectedOrder = obj as MyOrderListModel;
                if (selectedOrder != null)
                {
                    await page.Navigation.PushAsync(new MyOrderDetailPage(selectedOrder));
                }
            });
        }

        private ObservableCollection<MyOrdersResults> _my_orders_list;
        public ObservableCollection<MyOrdersResults> my_orders_list
        {
            get => _my_orders_list;
            set
            {
                _my_orders_list = value;
                OnPropertyChanged("my_orders_list");
            }
        }

        private ObservableCollection<PartsInvoice> _ordered_parts;
        public ObservableCollection<PartsInvoice> ordered_parts
        {
            get => _ordered_parts;
            set
            {
                _ordered_parts = value;
                OnPropertyChanged("ordered_parts");
            }
        }

        private ObservableCollection<MyOrderListModel> _myOrderList;
        public ObservableCollection<MyOrderListModel> myOrderList
        {
            get => _myOrderList;
            set
            {
                _myOrderList = value;
                OnPropertyChanged("myOrderList");
            }
        }

        private ObservableCollection<MyOrderListModel> _myShippedOrderList;
        public ObservableCollection<MyOrderListModel> myShippedOrderList
        {
            get => _myShippedOrderList;
            set
            {
                _myShippedOrderList = value;
                OnPropertyChanged("myShippedOrderList");
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

        public async void GetMyOrdersList()
        {
            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            {
                await Task.Delay(100);
                try
                {
                    var response = await apiServices.GetMyOrdersData();

                    if (response.status_code == System.Net.HttpStatusCode.Unauthorized)
                    {
                        await page.DisplayAlert("Alert","Please Login Again","Ok");
                        Preferences.Set("token", "");
                        Preferences.Set("IsUpdate", "true");
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                        return;
                    }
                    else if (response.count == 0)
                    {
                        emptyView = "No Orders Found";
                        return;
                    }
                    
                    my_orders_list = new ObservableCollection<MyOrdersResults>(response.results);
                    ordered_parts = new ObservableCollection<PartsInvoice>();
                    myOrderList = new ObservableCollection<MyOrderListModel>();
                    foreach (var order in my_orders_list)
                    {
                        var resp = await apiServices.GetInvoiceData(order.razorpay_order_id);

                        try
                        {
                            if (resp != null && resp.results.Any())
                            {
                                MyOrderListModel myOrderModel = new MyOrderListModel
                                {
                                    orderId = order.razorpay_order_id,
                                    total_amount = order.total_amount,
                                    shipping_address = order.shipping_address,
                                    billing_address = order.billing_address,
                                    mobile = order.mobile,
                                    email = order.email,
                                    gst_no = order.gst_no,
                                    invoice_number = resp.results.FirstOrDefault().invoice_number,
                                    payment_date = resp.results.FirstOrDefault().payment_date
                                };

                                myOrderModel.invoice_number = resp.results[0].invoice_number;
                                myOrderModel.payment_date = resp.results[0].payment_date;

                                foreach (var part in resp.results.FirstOrDefault().parts_invoice)
                                {
                                    myOrderModel.pruductType = part.parts_no.part_type.name;
                                    myOrderModel.serialized_parts = part.parts_no.serialized_parts;
                                    myOrderModel.short_description = part.parts_no.short_description;
                                    myOrderModel.long_description = part.parts_no.long_description;
                                    myOrderModel.part_number = part.customer_part_no;
                                    myOrderModel.price = part.price;
                                    myOrderModel.warranty_period = part.warranty;
                                    myOrderModel.quantity = part.quantity;

                                    foreach (var item in myOrderModel.serialized_parts)
                                    {
                                        item.imageUrl = "https://api.qrserver.com/v1/create-qr-code/?size=150x150&data=" + myOrderModel.part_number + "*" + item.serial_number;
                                    }

                                    myOrderList.Add(myOrderModel);
                                }
                            }
                            else
                            {
                                emptyView = "No Orders Found";
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    if (myOrderList.Count > 0)
                    {
                        myShippedOrderList = new ObservableCollection<MyOrderListModel>(myOrderList.Where(x => x.shipped_status == true).ToList());
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }

        public ICommand OrderSelectCommand { get; set; }
    }
}
