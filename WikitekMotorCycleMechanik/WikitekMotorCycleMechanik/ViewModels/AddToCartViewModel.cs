using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class AddToCartViewModel : ViewModelBase
    {
        ApiServices apiServices;
        //readonly INavigation navigationService;
        readonly Page page;

        public AddToCartViewModel(Page page) : base(page)
        {
            try
            {
                this.page = page;
                apiServices = new ApiServices();
                cart_list = new ObservableCollection<CartItem>();

                InitializeCommands();
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    //GetSegmentList();
                    await GetCartList();
                    await GetPriceDetails();
                });
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties


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

        private double _price;
        public double price
        {
            get => _price;
            set
            {
                _price = value;
                OnPropertyChanged("price");
            }
        }

        private double _discount;
        public double discount
        {
            get => _discount;
            set
            {
                _discount = value;
                OnPropertyChanged("discount");
            }
        }

        private double _totalAmount;
        public double totalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
                OnPropertyChanged("totalAmount");
            }
        }

        private string _emptyView = "Wait for products to be loaded...";
        public string emptyView
        {
            get => _emptyView;
            set
            {
                _emptyView = value;
                OnPropertyChanged("emptyView");
            }
        }


        //private CartItem _selected_cart_list;
        //public CartItem selected_cart_list
        //{
        //    get => _selected_cart_list;
        //    set
        //    {
        //        _selected_cart_list = value;
        //        if (selected_cart_list != null)
        //        {
        //            //page.Navigation.PushAsync(new Views.MarketPlace.(selected_cart_list));
        //        }
        //        OnPropertyChanged("selected_cart_list");
        //    }
        //}
        #endregion


        #region Methods

        public void InitializeCommands()
        {
            try
            {


                AddProductQuantityCommand = new Command(async (obj) =>
                {
                    var item = (CartItem)obj;
                    if (item == null)
                        return;

                    item.quantity = item.quantity + 1;

                    if (item.parts_id.prices.Count > 0)
                    {
                        item.parts_id.prices.ForEach(p =>
                        {
                            if (item.quantity >= p.min_quantity && item.quantity <= p.max_quantity)
                            {
                                item.unit_price = p.price;
                                item.discount = (item.parts_id.mrp - item.unit_price) / item.parts_id.mrp * 100;
                            }
                        });
                    }

                    item.extended_price = item.unit_price * item.quantity;
                    item.total_price = item.parts_id.mrp * item.quantity;

                    if (item.extended_price == item.total_price)
                    {
                        item.isVisible = false;
                        item.discountColor = Color.Red;
                    }
                    else
                    {
                        item.isVisible = true;
                        item.discountColor = Color.Green;
                    }

                    GetPriceDetails();

                    var result = await apiServices.UpdateToCart(Xamarin.Essentials.Preferences.Get("token", null), item.id, item.quantity);
                    if (result == null)
                    {
                        await page.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
                        return;
                    }
                    //await page.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
                });

                RemoveProductQuantityCommand = new Command(async (obj) =>
                {
                    var item = (CartItem)obj;
                    if (item == null)
                        return;

                    if (item.quantity == 1)
                    {
                        cart_list.Remove(item);
                        price = 0;
                        discount = 0;
                        totalAmount = 0;
                        var result = await apiServices.DeleteToCart(Xamarin.Essentials.Preferences.Get("token", null), item.id);
                        emptyView = "Please add product to cart";
                        if (result == null)
                        {
                            await page.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
                            return;
                        }
                    }
                    else if (item.quantity > 0)
                    {
                        item.quantity = item.quantity - 1;

                        if (item.parts_id.prices.Count > 0)
                        {
                            item.parts_id.prices.ForEach(p =>
                            {
                                if (item.quantity >= p.min_quantity && item.quantity <= p.max_quantity)
                                {
                                    item.unit_price = p.price;
                                    item.discount = (item.parts_id.mrp - item.unit_price) / item.parts_id.mrp * 100;
                                }
                            });
                        }
                        item.total_price = item.parts_id.mrp * item.quantity;
                        item.extended_price = item.unit_price * item.quantity;

                        if (item.extended_price == item.total_price)
                        {
                            item.isVisible = false;
                            item.discountColor = Color.Red;
                        }
                        else
                        {
                            item.isVisible = true;
                            item.discountColor = Color.Green;
                        }

                        GetPriceDetails();

                        var result = await apiServices.UpdateToCart(Xamarin.Essentials.Preferences.Get("token", null), item.id, item.quantity);
                        if (result == null)
                        {
                            await page.DisplayAlert("Failed", "Please check device internet connection.", "Ok");
                            return;
                        }
                    }
                });

                CheckoutCommand = new Command(async (obj) =>
                {
                    if (cart_list.Count > 0)
                        await page.Navigation.PushAsync(new Views.MarketPlace.ShipmentAddressPage(cart_list));
                    else
                    {
                        await page.DisplayAlert("Cart Empty","Please add product to cart","Ok");
                    }
                });


            }
            catch (Exception ex)
            {
            }
        }


        public async Task GetCartList()
        {
            try
            {
                var response = await apiServices.GetCartList(Xamarin.Essentials.Preferences.Get("token", null), App.user_id);
                //cart_list = new ObservableCollection<MarketPlaceModel>(response);

                if (response.results.FirstOrDefault().cart_items.Count == 0)
                {
                    emptyView = "Please add product to cart";
                }

                foreach (var item in response.results.FirstOrDefault().cart_items)
                {
                    if (item.parts_id.prices.Count > 0)
                    {
                        item.parts_id.prices.ForEach(p =>
                        {
                            if (item.quantity >= p.min_quantity && item.quantity <= p.max_quantity)
                            {
                                item.unit_price = p.price;
                                item.discount = (item.parts_id.mrp - item.unit_price) / item.parts_id.mrp * 100;
                            }
                            if (item.unit_price == item.parts_id.mrp)
                            {
                                item.isVisible = false;
                                item.discountColor = Color.Red;
                            }
                            else
                            {
                                item.isVisible = true;
                                item.discountColor = Color.Green;
                            }
                        });
                    }
                    cart_list.Add(
                        new CartItem
                        {
                            id = item.id,
                            cart_id = item.cart_id,
                            unit_price = item.unit_price,
                            fav = item.fav,
                            parts_id = item.parts_id,
                            quantity = item.quantity,
                            extended_price = item.unit_price * item.quantity,
                            total_price = item.parts_id.mrp * item.quantity,
                            discount = item.discount,
                            discountColor = item.discountColor,
                            isVisible = item.isVisible,
                            //part_number = item.part_number,
                        });
                }

            }
            catch (Exception ex)
            {
            }
        }

        public async Task GetPriceDetails()
        {
            price = 0;
            discount = 0;
            totalAmount = 0;
            double discountPercent = 0;
            foreach (var item in cart_list)
            {
                price = price + item.total_price;
                discountPercent = discountPercent + item.discount;
                if (item.discount > 0)
                    discount = discount + (item.total_price * discountPercent / 100);
                totalAmount = totalAmount + item.extended_price;
            }

        }
        #endregion

        #region ICommands
        public ICommand AddProductQuantityCommand { get; set; }
        public ICommand RemoveProductQuantityCommand { get; set; }
        public ICommand CheckoutCommand { get; set; }
        public ICommand GoToCartCommand { get; set; }
        #endregion
    }
}
