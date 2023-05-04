using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class MarketPlaceDetailViewModel : ViewModelBase
    {
        ApiServices apiServices;
        //readonly INavigation navigationService;
        readonly Page page;
        //public double ex_price = 0;

        public MarketPlaceDetailViewModel(Page page, PartsList selected_market_place) : base(page)
        {
            try
            {
                this.page = page;
                apiServices = new ApiServices();
                this.selected_market_place = selected_market_place;
                //ex_price = Convert.ToDouble(selected_market_place.price);
                discounted_price = (selected_market_place.mrp.HasValue) ? selected_market_place.mrp.Value : 0 * quantities;//Convert.ToDouble(selected_market_place.price);
                total_price = discounted_price * quantities;
                title = selected_market_place.part_number;
                InitializeCommands();
                //DependencyService.Get<IToolbarItemBadgeService>().SetBadge(this, ToolbarItems.First(), $"10", Color.Red, Color.White);
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    //GetSegmentList();
                    //GetMarketPlaceList();
                });
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties

        private string _title;
        public string title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("title");
            }
        }

        private double _discounted_price;
        public double discounted_price
        {
            get => _discounted_price;
            set
            {
                _discounted_price = value;
                OnPropertyChanged("discounted_price");
            }
        }

        private double _total_price;
        public double total_price
        {
            get => _total_price;
            set
            {
                _total_price = value;
                OnPropertyChanged("total_price");
            }
        }

        private int _quantities = 1;
        public int quantities
        {
            get => _quantities;
            set
            {
                _quantities = value;
                OnPropertyChanged("quantities");
            }
        }

        private PartsList _selected_market_place;
        public PartsList selected_market_place
        {
            get => _selected_market_place;
            set
            {
                _selected_market_place = value;
                OnPropertyChanged("selected_market_place");
            }
        }

        private bool _badge_count_visible = false;
        public bool badge_count_visible
        {
            get => _badge_count_visible;
            set
            {
                _badge_count_visible = value;
                OnPropertyChanged("badge_count_visible");
            }
        }

        private string _badge_count;
        public string badge_count
        {
            get => _badge_count;
            set
            {
                _badge_count = value;
                OnPropertyChanged("badge_count");
            }
        }

        private double? _discount = 0;
        public double? discount
        {
            get => _discount;
            set
            {
                _discount = value;
                OnPropertyChanged("discount");
            }
        }

        private string _discountInfo;
        public string discountInfo
        {
            get => _discountInfo;
            set
            {
                _discountInfo = value;
                OnPropertyChanged("discountInfo");
            }
        }

        private Color _discountColor = Color.Red;
        public Color discountColor
        {
            get => _discountColor;
            set
            {
                _discountColor = value;
                OnPropertyChanged("discountColor");
            }
        }
        #endregion


        #region Methods

        public void InitializeCommands()
        {
            try
            {
                AddProductQuantityCommand = new Command(async (obj) =>
                {
                    try
                    {
                        quantities = quantities + 1;
                        //discounted_price = selected_market_place.price* quantities;
                        if (selected_market_place.prices.Count > 0)
                        {
                            foreach (var item in selected_market_place.prices)
                            {
                                if (quantities >= item.min_quantity && quantities <= item.max_quantity)
                                {
                                    discounted_price = (item.price.HasValue) ? item.price.Value : 0 * quantities;
                                    discount = (selected_market_place.mrp - discounted_price) / discounted_price * 100;
                                    discountColor = discount == 0 ? Color.Red : Color.Green;
                                    discountInfo = (selected_market_place.mrp - discounted_price).ToString()
                                                    + $" ({discount}%)";
                                    total_price = discounted_price * quantities;
                                }
                            }
                        }
                        else
                            discounted_price = (selected_market_place.mrp.HasValue) ? selected_market_place.mrp.Value : 0 * quantities;
                        //string curCulture = Thread.CurrentThread.CurrentCulture.ToString();
                        //CultureInfo hindi = new CultureInfo("hi-IN");
                        //discounted_price = Convert.ToDouble(string.Format(hindi, "{0:C}", discounted_price));
                    }
                    catch (Exception ex)
                    {

                    }
                });

                RemoveProductQuantityCommand = new Command(async (obj) =>
                {
                    try
                    {
                        if (quantities > 0)
                        {
                            quantities = quantities - 1;
                            if (selected_market_place.prices.Count > 0)
                            {
                                foreach (var item in selected_market_place.prices)
                                {
                                    if (quantities >= item.min_quantity && quantities <= item.max_quantity)
                                    {
                                        discounted_price = (item.price.HasValue) ? item.price.Value : 0 * quantities;
                                        discount = (selected_market_place.mrp - discounted_price) / discounted_price * 100;
                                        discountColor = discount == 0 ? Color.Red : Color.Green;
                                        discountInfo = (selected_market_place.mrp - discounted_price).ToString()
                                                    + $" ({discount}%)";
                                        total_price = discounted_price * quantities;
                                    }
                                    else if (quantities == 0)
                                        total_price = 0;
                                }
                            }
                            else
                                discounted_price = (selected_market_place.mrp.HasValue) ? selected_market_place.mrp.Value : 0 * quantities;
                            //discounted_price = selected_market_place.price * quantities;
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                });

                AddToCartCommand = new Command(async (obj) =>
                {
                    try
                    {
                        if (quantities > 0)
                        {
                            //*+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
                            AddCartModel addCartModel = new AddCartModel
                            {
                                fav = false,
                                parts_id = selected_market_place.id,
                                qty = quantities,
                                unit_price = discounted_price
                                //unit_price = (selected_market_place.mrp.HasValue) ? selected_market_place.mrp.Value : 0,
                                //unit_price = selected_market_place.price,
                            };
                            var result = await apiServices.AddToCart(Xamarin.Essentials.Preferences.Get("token", null), addCartModel);

                            bool resp = await page.DisplayAlert("","Do you want to continue shopping or go to cart?","Continue Shopping","Go to cart");
                            if (resp)
                            {
                                await page.Navigation.PopAsync();
                            }
                            else
                                await page.Navigation.PushAsync(new Views.MarketPlace.AddToCartPage());
                        }
                        else
                        {
                            await page.DisplayAlert("Alert", "Please add quantity of this product", "Ok");
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                });

                GoToCartCommand = new Command(async (obj) =>
                {
                    try
                    {
                        await page.Navigation.PushAsync(new Views.MarketPlace.AddToCartPage());
                    }
                    catch (Exception ex)
                    {

                    }
                });
            }
            catch (Exception ex)
            {
            }
        }


        public async void GetMarketPlaceList()
        {
            try
            {
                //var response = await apiServices.GetMarketPlaceList(App.access_token);

                //var api_status_code = StaticMethods.http_status_code(response.status_code);

                //if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                //{
                //    //market_place_list = new ObservableCollection<MarketPlaceModel>(response);
                //}
                //else
                //{
                //    await page.DisplayAlert(api_status_code, response.error, "Ok");
                //}
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region ICommands
        public ICommand AddProductQuantityCommand { get; set; }
        public ICommand RemoveProductQuantityCommand { get; set; }
        public ICommand AddToCartCommand { get; set; }
        public ICommand GoToCartCommand { get; set; }
        #endregion
    }
}
