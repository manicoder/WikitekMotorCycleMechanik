using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using WikitekMotorCycleMechanik.Views.Login;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class MarketPlaceViewModel : ViewModelBase
    {
        ApiServices apiServices;
        //readonly INavigation navigationService;
        readonly Page page;

        public MarketPlaceViewModel(ObservableCollection<PartsList> marketList, Page page) : base(page)
        {
            try
            {
                this.page = page;
                apiServices = new ApiServices();
                market_place_list = new ObservableCollection<PartsList>();
                model_list = new ObservableCollection<VehicleModelResult>();
                vehicle_model = new VehicleModelResult { name = "Select model" };
                filter_list = new ObservableCollection<FilterModel>
                {
                    new FilterModel
                    {
                        filter_by = "Model",
                        is_ckecked = false,
                    },
                    new FilterModel
                    {
                        filter_by = "Segment",
                        is_ckecked = false,
                    },
                    new FilterModel
                    {
                        filter_by = "Oem",
                        is_ckecked = false,
                    },
                };
                InitializeCommands();
                if (marketList != null)
                {
                    isFilterListVisible = true;
                    Device.InvokeOnMainThreadAsync(async () =>
                    {
                        GetFilteredMarketPlaceList(marketList);
                    });
                    
                }
                else
                {
                    isFilterListVisible = false;
                    Device.InvokeOnMainThreadAsync(async () =>
                    {
                        GetMarketPlaceList();
                    });
                }
                
            }
            catch (Exception ex)
            {
            }
        }

        #region Properties
        private string _search_key;
        public string search_key
        {
            get => _search_key;
            set
            {
                _search_key = value;

                if (!string.IsNullOrEmpty(search_key))
                {
                    market_place_list = new ObservableCollection<PartsList>(market_place_static_list.Where(x => x.part_number.ToLower().Contains(search_key.ToLower()) || x.short_description.ToLower().Contains(search_key.ToLower())).ToList());
                }
                else
                {
                    market_place_list = market_place_static_list;
                }

                OnPropertyChanged("search_key");
            }
        }

        private ObservableCollection<PartsList> _market_place_static_list;
        public ObservableCollection<PartsList> market_place_static_list
        {
            get => _market_place_static_list;
            set
            {
                _market_place_static_list = value;
                OnPropertyChanged("market_place_static_list");
            }
        }

        private ObservableCollection<PartsList> _market_place_list;
        public ObservableCollection<PartsList> market_place_list
        {
            get => _market_place_list;
            set
            {
                _market_place_list = value;
                OnPropertyChanged("market_place_list");
            }
        }

        private ObservableCollection<FilterModel> _filter_list;
        public ObservableCollection<FilterModel> filter_list
        {
            get => _filter_list;
            set
            {
                _filter_list = value;
                OnPropertyChanged("_filter_list");
            }
        }

        private PartsList _selected_market_place;
        public PartsList selected_market_place
        {
            get => _selected_market_place;
            set
            {
                _selected_market_place = value;
                //if (selected_market_place != null)
                //{
                //    page.Navigation.PushAsync(new Views.MarketPlace.MarketPlaceDetailPage(selected_market_place));
                //}
                OnPropertyChanged("selected_market_place");
            }
        }

        //private string _vehicle_model = "Select model";
        //public string vehicle_model
        //{
        //    get => _vehicle_model;
        //    set
        //    {
        //        _vehicle_model = value;
        //        OnPropertyChanged("vehicle_model");
        //    }
        //}



        private VehicleModelResult _vehicle_model;
        public VehicleModelResult vehicle_model
        {
            get => _vehicle_model;
            set
            {
                _vehicle_model = value;
                OnPropertyChanged("vehicle_model");
            }
        }

        private string _vehicle_segment = "Select segment";
        public string vehicle_segment
        {
            get => _vehicle_segment;
            set
            {
                _vehicle_segment = value;
                OnPropertyChanged("vehicle_segment");
            }
        }

        private string _vehicle_oem = "Select oem";
        public string vehicle_oem
        {
            get => _vehicle_oem;
            set
            {
                _vehicle_oem = value;
                OnPropertyChanged("vehicle_oem");
            }
        }

        private ObservableCollection<VehicleModelResult> _model_list;
        public ObservableCollection<VehicleModelResult> model_list
        {
            get => _model_list;
            set
            {
                _model_list = value;
                OnPropertyChanged("model_list");
            }
        }

        private bool _model_filter_visible = false;
        public bool model_filter_visible
        {
            get => _model_filter_visible;
            set
            {
                _model_filter_visible = value;
                OnPropertyChanged("model_filter_visible");
            }
        }

        private bool _segment_filter_visible = false;
        public bool segment_filter_visible
        {
            get => _segment_filter_visible;
            set
            {
                _segment_filter_visible = value;
                OnPropertyChanged("segment_filter_visible");
            }
        }

        private bool _oem_filter_visible = false;
        public bool oem_filter_visible
        {
            get => _oem_filter_visible;
            set
            {
                _oem_filter_visible = value;
                OnPropertyChanged("oem_filter_visible");
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

        private ObservableCollection<FilterList> _filterList;
        public ObservableCollection<FilterList> filterList
        {
            get => _filterList;
            set
            {
                _filterList = value;
                OnPropertyChanged("filterList");
            }
        }

        private bool _isFilterListVisible;
        public bool isFilterListVisible
        {
            get => _isFilterListVisible;
            set
            {
                _isFilterListVisible = value;
                OnPropertyChanged("isFilterListVisible");
            }
        }
        #endregion

        #region Methods
        [Obsolete]
        public void InitializeCommands()
        {
            try
            {
                MessagingCenter.Subscribe<VehicleModelFilterViewModel, VehicleModelResult>(this, "selected_vehicle_model", async (sender, arg) =>
                {
                    vehicle_model = arg;
                    //txt_model.TextColor = (Color)Application.Current.Resources["text_color"];
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

                ItemSelectionCommand = new Command(async (obj) =>
                {
                    try
                    {
                        selected_market_place = (PartsList)obj;
                        await Device.InvokeOnMainThreadAsync(async () =>
                        {
                            await page.Navigation.PushAsync(new Views.MarketPlace.MarketPlaceDetailPage(selected_market_place));
                        });
                    }
                    catch (Exception ex)
                    {
                    }
                });

                FilterCommand = new Command(async (obj) =>
                {
                    try
                    {
                        var view = (Grid)page.FindByName("filter_view");
                        await view.TranslateTo(0, 5, 500);
                    }
                    catch (Exception ex)
                    {
                    }
                });

                CloseFilterCommand = new Command(async (obj) =>
                {
                    try
                    {
                        var view = (Grid)page.FindByName("filter_view");
                        await view.TranslateTo(0, 205, 500);
                    }
                    catch (Exception ex)
                    {
                    }
                });

                SelectModelCommand = new Command(async (obj) =>
                {
                    try
                    {
                        await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new Views.MarketPlace.VehicleModelFilterPopupPage(model_list));
                    }
                    catch (Exception ex)
                    {
                    }
                });

                SelectSubModelCommand = new Command(async (obj) =>
                {
                    try
                    {
                        //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.SubModelPopupPage(0, selected_model.sub_models));
                    }
                    catch (Exception ex)
                    {
                    }
                });

                ByFilterCommand = new Command(async (obj) =>
                {
                    try
                    {
                        FilterModel filter_model = (FilterModel)obj;
                        foreach (var item in filter_list)
                        {
                            item.is_ckecked = false;
                        }

                        filter_model.is_ckecked = true;

                        model_filter_visible = false;
                        segment_filter_visible = false;
                        oem_filter_visible = false;

                        switch (filter_model.filter_by)
                        {
                            case "Model":
                                model_filter_visible = true;
                                break;
                            case "Segment":
                                segment_filter_visible = true;
                                break;
                            case "Oem":
                                oem_filter_visible = true;
                                break;
                        }

                    }
                    catch (Exception ex)
                    {
                    }
                });

                PartFilterCommand = new Command(async (obj) =>
                {
                    await page.Navigation.PushAsync(new Views.MarketPlace.PartsFilterPage());
                });
                SortListCommand = new Command(async (obj) =>
                {
                    if (market_place_list.First().mrp > market_place_list.Last().mrp)
                        market_place_list = new ObservableCollection<PartsList>(market_place_list.OrderBy(x => x.mrp));
                    else
                    {
                        market_place_list = new ObservableCollection<PartsList>(market_place_list.OrderBy(x => x.mrp).Reverse());
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
                List<int> sub_model_id_list = new List<int>();
                var response = await apiServices.GetMarketPlaceList(Xamarin.Essentials.Preferences.Get("token", null));

                var api_status_code = StaticMethods.http_status_code(response.status_code);

                if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                {
                    market_place_list = market_place_static_list = new ObservableCollection<PartsList>(response.results);

                    if (App.isFilter)
                    {
                        
                        market_place_list = new ObservableCollection<PartsList>(
                            market_place_static_list.Where(x =>
                                            x.part_number.Contains(App.selectedSegment)
                                            && x.part_number.Contains(App.selectedPack)
                                            && x.part_number.Contains("WM")));
                    }
                    else if (!string.IsNullOrEmpty(App.selectedBannerId))
                    {
                        market_place_list = new ObservableCollection<PartsList>(
                            market_place_static_list.Where(x =>
                                            x.id == App.selectedBannerId));
                    }
                    //if (market_place_list.Count > 0)
                    //{
                    //    var model_response = await apiServices.GetAllModels(App.access_token);
                    //    foreach (var item in market_place_list)
                    //    {
                    //        foreach (var sub_model_id in item.spare_parts)
                    //        {

                    //            sub_model_id_list.Add(sub_model_id.name);
                    //            //foreach (var model in model_response.results)
                    //            //{
                    //            //    foreach (var sub_model in model.sub_models)
                    //            //    {
                    //            //        if (sub_model.id == sub_model_id.name)
                    //            //        {
                    //            //            //model.sub_models = new List<VehicleSubModel>();
                    //            //            //model.sub_models.Add(sub_model);
                    //            //            VehicleModelResult vehicleModelResult = new VehicleModelResult();
                    //            //            vehicleModelResult = model;
                    //            //            //vehicleModelResult.sub_models = new List<VehicleSubModel>();
                    //            //            //vehicleModelResult.sub_models.Add(sub_model);
                    //            //            //vehicleModelResult.sub_models = vehicleModelResult.sub_models.Where(x => x.id == sub_model_id.name).ToList();
                    //            //            foreach (var sub in model.sub_models)
                    //            //            {
                    //            //                if (sub.id != sub_model_id.name)
                    //            //                {
                    //            //                    vehicleModelResult.sub_models = new List<VehicleSubModel>();
                    //            //                    vehicleModelResult.sub_models.Add(sub);
                    //            //                    //vehicleModelResult.sub_models = vehicleModelResult.sub_models.Where(x => x.id == sub_model_id.name).ToList();
                    //            //                    //vehicleModelResult.sub_models.Remove(sub);
                    //            //                }
                    //            //            }

                    //            //            model_list.Add(vehicleModelResult);
                    //            //        }

                    //            //    }
                    //            //}

                    //        }

                    //    }

                    //    foreach (var sub_model_id in sub_model_id_list)
                    //    {
                    //        foreach (var model in model_response.results)
                    //        {
                    //            if(model.name == "OBD2")
                    //            {

                    //            }
                    //            //VehicleModelResult vehicleModelResult = new VehicleModelResult();
                    //            var item = model.sub_models.FirstOrDefault(x => x.id == sub_model_id);
                    //            if (item != null)
                    //            {
                    //                List<VehicleSubModel> submodel = new List<VehicleSubModel>();
                    //                submodel.Add(item);
                                    
                    //                //vehicleModelResult = model;
                    //                //vehicleModelResult.sub_models = submodel;
                    //                model_list.Add(new VehicleModelResult
                    //                { 
                    //                    arrow = model.arrow,
                    //                    button_height = model.button_height,
                    //                    name = model.name,
                    //                    id= model.id,
                    //                    model_file= model.model_file,
                    //                    selected_model_year= model.selected_model_year,
                    //                    selected_sub_model= model.selected_sub_model,
                    //                    selection_item_height= model.selection_item_height,
                    //                    space_height= model.space_height,
                    //                    sub_models= submodel,
                    //                });
                    //            }
                    //            //foreach (var sub_model in model.sub_models)
                    //            //{

                    //            //    if()
                    //            //    VehicleModelResult vehicleModelResult = new VehicleModelResult();
                    //            //    vehicleModelResult = model;
                    //            //    //if (sub_model_id==sub_model.id)
                    //            //    //{

                    //            //    //    model_list.Add(model);
                    //            //    //}
                    //            //}
                    //        }
                    //    }
                    //}
                }
                else
                {
                    await page.DisplayAlert(api_status_code, "", "Ok");
                    if (response.status_code == System.Net.HttpStatusCode.Unauthorized)
                    {
                        App.Current.MainPage = new NavigationPage(new LoginPage());
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public async void GetFilteredMarketPlaceList(ObservableCollection<PartsList> marketList)
        {
            try
            {
                market_place_list = market_place_static_list = marketList;
                filterList = new ObservableCollection<FilterList>();
                //foreach (var item in App.priceList)
                //{
                //    if (item.isSelected)
                //    {
                //        filterList.Add(new FilterList
                //        {
                //            name = item.min_price.ToString() + "-" + item.max_price.ToString(),
                //        });
                //    }
                //}

                filterList.Add(new FilterList
                {
                    name = "₹" + App.lowerPrice.ToString() + "-" + "₹" + App.upperPrice.ToString(),
                });

                foreach (var item in App.metaTagList)
                {
                    if (item.isSelected)
                    {
                        filterList.Add(new FilterList
                        {
                            name = item.name.ToString(),
                        });
                    }
                }
                foreach (var item in App.partTypeList)
                {
                    if (item.isSelected)
                    {
                        filterList.Add(new FilterList
                        {
                            name = item.name.ToString(),
                        });
                    }
                }
                foreach (var item in App.partsCategoryList)
                {
                    foreach (var item2 in item.part_category.subs)
                    {
                        if (item2.Selected)
                        {
                            filterList.Add(new FilterList
                            {
                                name = item2.name.ToString(),
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        //public async void GetMarketPlaceList()
        //{
        //    try
        //    {
        //        var response = await apiServices.GetMarketPlaceList(App.access_token);

        //        var api_status_code = StaticMethods.http_status_code(response.status_code);

        //        if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
        //        {
        //            market_place_list = market_place_static_list = new ObservableCollection<MarketPlaceResult>(response.results);
        //            if (market_place_list.Count > 0)
        //            {
        //                var model_response = await apiServices.GetAllModels(App.access_token);
        //                foreach (var item in market_place_list)
        //                {
        //                    foreach (var sub_model_id in item.spare_parts)
        //                    {
        //                        //var modellist = model_response.results.Where(x=>x.sub_models.Where(x => x.id == sub_model.name));
        //                        foreach (var model in model_response.results)
        //                        {
        //                            foreach (var sub_model in model.sub_models)
        //                            {
        //                                if (sub_model.id == sub_model_id.name)
        //                                {
        //                                    //model.sub_models = new List<VehicleSubModel>();
        //                                    //model.sub_models.Add(sub_model);
        //                                    VehicleModelResult vehicleModelResult = new VehicleModelResult();
        //                                    vehicleModelResult = model;
        //                                    //vehicleModelResult.sub_models = new List<VehicleSubModel>();
        //                                    //vehicleModelResult.sub_models.Add(sub_model);
        //                                    //vehicleModelResult.sub_models = vehicleModelResult.sub_models.Where(x => x.id == sub_model_id.name).ToList();
        //                                    foreach (var sub in model.sub_models)
        //                                    {
        //                                        if (sub.id != sub_model_id.name)
        //                                        {
        //                                            vehicleModelResult.sub_models = new List<VehicleSubModel>();
        //                                            vehicleModelResult.sub_models.Add(sub);
        //                                            //vehicleModelResult.sub_models = vehicleModelResult.sub_models.Where(x => x.id == sub_model_id.name).ToList();
        //                                            //vehicleModelResult.sub_models.Remove(sub);
        //                                        }
        //                                    }

        //                                    model_list.Add(vehicleModelResult);
        //                                }

        //                            }
        //                        }
        //                        //model_list.Add(model);
        //                    }
        //                    //model_list.Add();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            await page.DisplayAlert(api_status_code, response.error, "Ok");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        #endregion

        #region ICommands
        public ICommand GoToCartCommand { get; set; }
        public ICommand ItemSelectionCommand { get; set; }
        public ICommand FilterCommand { get; set; }
        public ICommand CloseFilterCommand { get; set; }
        public ICommand SelectModelCommand { get; set; }
        public ICommand SelectSubModelCommand { get; set; }
        public ICommand ByFilterCommand { get; set; }
        public ICommand PartFilterCommand { get; set; }
        public ICommand SortListCommand { get; set; }
        #endregion
    }

    public class FilterList
    {
        public string name { get; set; }
    }
}
