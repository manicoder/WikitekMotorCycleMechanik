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
using WikitekMotorCycleMechanik.Views.MarketPlace;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class PartsFilterViewModel : ViewModelBase
    {
        readonly Page page;
        ApiServices apiServices;
        ObservableCollection<PartsList> market_place_list;
        List<PartsList> part_list;

        #region Ctor
        public PartsFilterViewModel(Page page) : base(page)
        {
            this.page = page;
            apiServices = new ApiServices();

            InitializeCommands();
            Device.BeginInvokeOnMainThread(async () =>
            {
                await GetData();
            });
        } 
        #endregion

        #region Properties
        private ObservableCollection<PartsTypeList> _partTypeList;
        public ObservableCollection<PartsTypeList> partTypeList
        {
            get => _partTypeList;
            set
            {
                _partTypeList = value;
                OnPropertyChanged("partTypeList");
            }
        }

        private ObservableCollection<PartsCategoryList> _partsCategoryList;
        public ObservableCollection<PartsCategoryList> partsCategoryList
        {
            get => _partsCategoryList;
            set
            {
                _partsCategoryList = value;
                OnPropertyChanged("partsCategoryList");
            }
        }

        //private ObservableCollection<PriceFilterList> _priceList;
        //public ObservableCollection<PriceFilterList> priceList
        //{
        //    get => _priceList;
        //    set
        //    {
        //        _priceList = value;
        //        OnPropertyChanged("priceList");
        //    }
        //}

        private ObservableCollection<MetaTagList> _metaTagList;
        public ObservableCollection<MetaTagList> metaTagList
        {
            get => _metaTagList;
            set
            {
                _metaTagList = value;
                OnPropertyChanged("metaTagList");
            }
        }

        private bool _isPartTypeVisible = false;
        public bool isPartTypeVisible
        {
            get => _isPartTypeVisible;
            set
            {
                _isPartTypeVisible = value;
                OnPropertyChanged("isPartTypeVisible");
            }
        }

        private bool _isCategoryVisible = false;
        public bool isCategoryVisible
        {
            get => _isCategoryVisible;
            set
            {
                _isCategoryVisible = value;
                OnPropertyChanged("isCategoryVisible");
            }
        }

        private bool _isPriceListVisible = false;
        public bool isPriceListVisible
        {
            get => _isPriceListVisible;
            set
            {
                _isPriceListVisible = value;
                OnPropertyChanged("isPriceListVisible");
            }
        }

        private bool _isMetaTagVisible = false;
        public bool isMetaTagVisible
        {
            get => _isMetaTagVisible;
            set
            {
                _isMetaTagVisible = value;
                OnPropertyChanged("isMetaTagVisible");
            }
        }

        private string _partTypeUpDownImage = "ic_sort_down.png";
        public string partTypeUpDownImage
        {
            get => _partTypeUpDownImage;
            set
            {
                _partTypeUpDownImage = value;
                OnPropertyChanged("partTypeUpDownImage");
            }
        }

        private string _categoryUpDownImage = "ic_sort_down.png";
        public string categoryUpDownImage
        {
            get => _categoryUpDownImage;
            set
            {
                _categoryUpDownImage = value;
                OnPropertyChanged("categoryUpDownImage");
            }
        }

        private string _priceUpDownImage = "ic_sort_down.png";
        public string priceUpDownImage
        {
            get => _priceUpDownImage;
            set
            {
                _priceUpDownImage = value;
                OnPropertyChanged("priceUpDownImage");
            }
        }

        private string _metaTagUpDownImage = "ic_sort_down.png";
        public string metaTagUpDownImage
        {
            get => _metaTagUpDownImage;
            set
            {
                _metaTagUpDownImage = value;
                OnPropertyChanged("metaTagUpDownImage");
            }
        }

        private double _minPrice = 0;
        public double minPrice
        {
            get => _minPrice;
            set
            {
                _minPrice = value;
                OnPropertyChanged("minPrice");
            }
        }

        private double _maxPrice = 20000;
        public double maxPrice
        {
            get => _maxPrice;
            set
            {
                _maxPrice = value;
                OnPropertyChanged("maxPrice");
            }
        }

        private double _lowerPrice = 0;
        public double lowerPrice
        {
            get => _lowerPrice;
            set
            {
                _lowerPrice = value;
                OnPropertyChanged("lowerPrice");
            }
        }

        private double _upperPrice = 20000;
        public double upperPrice
        {
            get => _upperPrice;
            set
            {
                _upperPrice = value;
                OnPropertyChanged("upperPrice");
            }
        }
        #endregion

        #region Methods
        public void InitializeCommands()
        {
            try
            {
                DropDownSelectionCommand = new Command(async (obj) =>
                    {
                        var selectedItem = (PartsCategoryList)obj;
                        selectedItem.part_category.isExpanded = !selectedItem.part_category.isExpanded;
                        selectedItem.part_category.ic_updown = selectedItem.part_category.isExpanded == true ? "ic_sort_up.png" : "ic_sort_down.png";
                    });
                CheckChangedCommand = new Command(async (obj) =>
                {
                    var selectedItem = (PartsCategoryList)obj;
                    selectedItem.part_category.isSelected = !selectedItem.part_category.isSelected;
                    foreach (var item in selectedItem.part_category.subs)
                    {
                        item.Selected = selectedItem.part_category.isSelected;
                    }
                });
                SubmitCommand = new Command(async (obj) =>
                {
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(100);
                        var val1 = lowerPrice;
                        var val2 = upperPrice;
                        List<PartsList> list1 = new List<PartsList>();
                        List<PartsList> list2 = new List<PartsList>();
                        List<PartsList> list3 = new List<PartsList>();
                        List<PartsList> list4 = new List<PartsList>();
                        List<PartsList> CommonList = new List<PartsList>();
                        List<PartsList> CommonList1 = new List<PartsList>();
                        List<PartsList> CommonList2 = new List<PartsList>();
                        //part_list = new List<PartsList>();
                        foreach (var item in partTypeList)
                        {
                            if (item.isSelected)
                            {
                                var list = market_place_list.Where(x => x.part_type.name == item.name).ToList();
                                list.ForEach(x =>
                                {
                                    list1.Add(x);
                                });
                            }
                        }

                        foreach (var category in partsCategoryList)
                        {
                            foreach (var subcategory in category.part_category.subs)
                            {
                                if (subcategory.Selected)
                                {
                                    var list = market_place_list.Where(x => x.sub_category.name == subcategory.name).ToList();
                                    list.ForEach(x =>
                                    {
                                        list2.Add(x);
                                    });
                                }
                            }
                        }


                        list3 = market_place_list.Where(x => x.mrp >= lowerPrice
                                                                        && x.mrp <= upperPrice).ToList();
                        //foreach (var item in priceList)
                        //{
                        //    if (item.isSelected)
                        //    {
                        //        var list = market_place_list.Where(x => x.mrp >= item.min_price
                        //                                                && x.mrp <= item.max_price).ToList();
                        //        list.ForEach(x =>
                        //        {
                        //            list3.Add(x);
                        //        });
                        //    }
                        //}

                        foreach (var item in metaTagList)
                        {
                            if (item.isSelected)
                            {
                                foreach (var item2 in market_place_list)
                                {
                                    var list = item2.meta_tags.Where(x => x.name == item.name).ToList();
                                    if (list.Count > 0)
                                        list4.Add(item2);
                                }
                            }
                        }

                        //var list2 = part_list;
                        //var noDupes = list2.Distinct().ToList();
                        list2.Distinct();
                        list3.Distinct();
                        list4.Distinct();
                        if (list1.Count > 0 && list2.Count > 0)
                            CommonList1 = list1.Intersect(list2).ToList();
                        else if (list1.Count > 0)
                            CommonList1 = list1;
                        else
                            CommonList1 = list2;

                        if (CommonList1.Count > 0 && list3.Count > 0)
                            CommonList2 = CommonList1.Intersect(list3).ToList();
                        else if (CommonList1.Count > 0)
                            CommonList2 = CommonList1;
                        else
                            CommonList2 = list3;

                        if (CommonList2.Count > 0 && list4.Count > 0)
                            CommonList = CommonList2.Intersect(list4).ToList();
                        else if (CommonList2.Count > 0)
                            CommonList = CommonList2;
                        else
                            CommonList = list4;

                        CommonList = CommonList.Distinct().ToList();

                        if (CommonList.Any())
                        {
                            App.partTypeList = new ObservableCollection<PartsTypeList>();
                            App.partsCategoryList = new ObservableCollection<PartsCategoryList>();
                            App.metaTagList = new ObservableCollection<MetaTagList>();
                            //App.priceList = new ObservableCollection<PriceFilterList>();
                            App.partTypeList = partTypeList;
                            App.partsCategoryList = partsCategoryList;
                            App.metaTagList = metaTagList;
                            //App.priceList = priceList;
                            App.lowerPrice = lowerPrice;
                            App.upperPrice = upperPrice;
                            UserDialogs.Instance.Toast($"{CommonList.Count} match found", new TimeSpan(0, 0, 0, 4));
                            ObservableCollection<PartsList> newMarketList = new ObservableCollection<PartsList>(CommonList);
                            //await page.Navigation.PushAsync(new Views.MarketPlace.MarketPlacePage(newMarketList));
                            await page.Navigation.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new MarketPlacePage(newMarketList)) });
                        }
                        else
                        {
                            App.partTypeList = null;
                            App.partsCategoryList = null;
                            App.metaTagList = null;
                            //App.priceList = null;
                            UserDialogs.Instance.Toast("No match found", new TimeSpan(0, 0, 0, 2));
                            //await page.Navigation.PushAsync(new Views.MarketPlace.MarketPlacePage(null));
                            await page.Navigation.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new MarketPlacePage(null)) });
                        }
                    }
                        
                });

                ClearFilterCommand = new Command(async (obj) =>
                {
                    foreach (var item in partTypeList)
                    {
                        item.isSelected = false;
                    }
                    foreach (var category in partsCategoryList)
                    {
                        category.part_category.isSelected = false;
                        foreach (var subcategory in category.part_category.subs)
                        {
                            subcategory.Selected = false;
                        }
                    }
                    //foreach (var priceRange in priceList)
                    //{
                    //    priceRange.isSelected = false;
                    //}
                    foreach (var metaTag in metaTagList)
                    {
                        metaTag.isSelected = false;
                    }
                });

                CategoryListCommand = new Command(async (obj) =>
                {
                    isCategoryVisible = !isCategoryVisible;
                    categoryUpDownImage = isCategoryVisible == true ? "ic_sort_up.png" : "ic_sort_down.png";
                });

                partTypeListCommand = new Command(async (obj) =>
                {
                    isPartTypeVisible = !isPartTypeVisible;
                    partTypeUpDownImage = isPartTypeVisible == true ? "ic_sort_up.png" : "ic_sort_down.png";
                });

                priceListCommand = new Command(async (obj) =>
                {
                    isPriceListVisible = !isPriceListVisible;
                    priceUpDownImage = isPriceListVisible == true ? "ic_sort_up.png" : "ic_sort_down.png";
                });

                metaTagCommand = new Command(async (obj) =>
                {
                    isMetaTagVisible = !isMetaTagVisible;
                    metaTagUpDownImage = isMetaTagVisible == true ? "ic_sort_up.png" : "ic_sort_down.png";
                });
            }
            catch (Exception ex)
            {

            }
        }

        private async Task GetData()
        {
            try
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(100);
                    var response = await apiServices.GetMarketPlaceList(Xamarin.Essentials.Preferences.Get("token", null));
                    market_place_list = new ObservableCollection<PartsList>(response.results);

                    var list = market_place_list.OrderBy(x => x.mrp).ToList();
                    minPrice = lowerPrice = list.FirstOrDefault().mrp.Value;
                    maxPrice = upperPrice = list.LastOrDefault().mrp.Value;

                    if (App.partTypeList == null && App.partsCategoryList == null)
                    {
                        //priceList = new ObservableCollection<PriceFilterList>();
                        //priceList.Add(new PriceFilterList { min_price = 0, max_price = 99 });
                        //priceList.Add(new PriceFilterList { min_price = 100, max_price = 999 });
                        //priceList.Add(new PriceFilterList { min_price = 1000, max_price = 10000 });

                        var resp = await apiServices.GetPartType();
                        partTypeList = new ObservableCollection<PartsTypeList>(resp);
                        var resp1 = await apiServices.GetPartCategory();
                        partsCategoryList = new ObservableCollection<PartsCategoryList>(resp1);
                        var resp2 = await apiServices.GetMetatags();
                        metaTagList = new ObservableCollection<MetaTagList>(resp2);
                    }
                    else
                    {
                        partTypeList = App.partTypeList;
                        partsCategoryList = App.partsCategoryList;
                        metaTagList = App.metaTagList;
                        //priceList = App.priceList;
                    }
                    
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region ICommands
        public ICommand DropDownSelectionCommand { get; set; }
        public ICommand CheckChangedCommand { get; set; }
        public ICommand SubmitCommand { get; set; }
        public ICommand ClearFilterCommand { get; set; }
        public ICommand CategoryListCommand { get; set; }
        public ICommand partTypeListCommand { get; set; } 
        public ICommand priceListCommand { get; set; } 
        public ICommand metaTagCommand { get; set; } 
        #endregion
    }

    //public class PriceFilterList : BaseViewModel
    //{
    //    public int min_price { get; set; }
    //    public int max_price { get; set; }

    //    private bool _isSelected;
    //    public bool isSelected
    //    {
    //        get => _isSelected;
    //        set
    //        {
    //            _isSelected = value;
    //            OnPropertyChanged("isSelected");
    //        }
    //    }
    //}
}
