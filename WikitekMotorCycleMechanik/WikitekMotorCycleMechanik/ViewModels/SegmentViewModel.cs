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
    public class SegmentViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler CloseCountryPopup;

        public SegmentViewModel(string last_page)
        {
            apiServices = new ApiServices();
            segment_list = new ObservableCollection<VehicleSegment>();
            selected_segment = new VehicleSegment();
            this.last_page = last_page;
            Device.InvokeOnMainThreadAsync(async () =>
            {
                if (last_page == "subscription_page"|| last_page == "registration_page" || last_page == "CreateWorkshop")
                {
                    GetSegmentList();
                }
                else
                {
                    SubscribeSegmentList();
                }
            });

            ClosePopupCommand = new Command(async (obj) =>
            {
                CloseCountryPopup?.Invoke("", new EventArgs());
                //StaticMethods.last_page = "CreateRSAgent";
                //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.CountyPopupPage());
            });
        }


        private string _search_key;
        public string search_key
        {
            get => _search_key;
            set
            {
                _search_key = value;

                if (!string.IsNullOrEmpty(search_key))
                {
                    segment_list = new ObservableCollection<VehicleSegment>(segment_static_list.Where(x => x.segment_name.ToLower().Contains(search_key.ToLower())).ToList());
                }

                OnPropertyChanged("search_key");
            }
        }


        private string _last_page;
        public string last_page
        {
            get => _last_page;
            set
            {
                _last_page = value;
                OnPropertyChanged("last_page");
            }
        }


        private ObservableCollection<VehicleSegment> _segment_static_list;
        public ObservableCollection<VehicleSegment> segment_static_list
        {
            get => _segment_static_list;
            set
            {
                _segment_static_list = value;
                OnPropertyChanged("segment_static_list");
            }
        }

        private ObservableCollection<VehicleSegment> _segment_list;
        public ObservableCollection<VehicleSegment> segment_list
        {
            get => _segment_list;
            set
            {
                _segment_list = value;
                OnPropertyChanged("segment_list");
            }
        }

        private VehicleSegment _selected_segment;
        public VehicleSegment selected_segment
        {
            get => _selected_segment;
            set
            {
                _selected_segment = value;

                if (selected_segment != null)
                {
                    if (!string.IsNullOrEmpty(selected_segment.segment_name))
                    {
                        switch (last_page)
                        {
                            case "CreateWorkshop":
                                MessagingCenter.Send<SegmentViewModel, VehicleSegment>(this, "selected_segment_workshop", selected_segment);
                                break;
                            case "oems_list_page":
                                MessagingCenter.Send<SegmentViewModel, VehicleSegment>(this, "oems_list_page", selected_segment);
                                break;

                            case "model_list_page":
                                MessagingCenter.Send<SegmentViewModel, VehicleSegment>(this, "model_list_page", selected_segment);
                                break;

                            case "subscription_page":
                                MessagingCenter.Send<SegmentViewModel, VehicleSegment>(this, "subscription_page", selected_segment);
                                break;

                            case "registration_page":
                                MessagingCenter.Send<SegmentViewModel, VehicleSegment>(this, "registration_page", selected_segment);
                                break;
                        }
                        //MessagingCenter.Send<SegmentViewModel, VehicleSegment>(this, "selected_segment", selected_segment);
                    }

                    CloseCountryPopup?.Invoke("", new EventArgs());
                }
                OnPropertyChanged("selected_segment");
            }
        }


        #region Methods
        public Task SubscribeSegmentList()
        {
            try
            {
                //foreach (var item in App.user.subscriptions)
                //{
                //    segment_list.Add(
                //        new VehicleSegment
                //        {
                //            segment_icon = GetSegmentIcon(item.package.segments.segment_name),
                //            id = item.package.segments.id,
                //            segment_name = item.package.segments.segment_name
                //        });
                //}
                
            }
            catch (Exception ex)
            {
            }
            return null;
        }


        public async void GetSegmentList()
        {
            try
            {
                var response = await apiServices.GetAllSegment(Xamarin.Essentials.Preferences.Get("token", null));

                var api_status_code = StaticMethods.http_status_code(response.status_code);

                if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                {

                    foreach (var item in response.results)
                    {
                        switch (item.segment_name)
                        {
                            case "2W":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_010" });
                                break;

                            case "3W":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_04" });
                                break;

                            case "PV":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_011" });
                                break;

                            case "CV":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_02." });
                                break;

                            case "BUS":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_03.png" });
                                break;

                            case "CE":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_05.png" });
                                break;
                            case "GENSET":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_08.png" });
                                break;
                            case "EV":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_09.png" });
                                break;

                            case "MARINE":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_07.png" });
                                break;

                            case "INDMOB":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_01.png" });
                                break;

                            case "ROB":
                                segment_list.Add(new VehicleSegment { id = item.id, segment_name = item.segment_name, segment_icon = "ic_vehicle_06.png" });
                                break;
                        }
                    }

                    segment_static_list = segment_list;
                    //segment_list = new ObservableCollection<VehicleSegment>(response.results);
                    //await page.DisplayAlert("Success!", "!", "Ok");
                    //await page.Navigation.PopAsync();
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
        public string GetSegmentIcon(string segment_name)
        {
            try
            {
                string segment_icon = string.Empty;
                switch (segment_name)
                {
                    case "2W":
                        segment_icon = "ic_vehicle_010";
                        break;

                    case "3W":
                        segment_icon = "ic_vehicle_04";
                        break;

                    case "PV":
                        segment_icon = "ic_vehicle_011";
                        break;

                    case "CV":
                        segment_icon = "ic_vehicle_02.";
                        break;

                    case "BUS":
                        segment_icon = "ic_vehicle_03.png";
                        break;

                    case "CE":
                        segment_icon = "ic_vehicle_05.png";
                        break;
                    case "GENSET":
                        segment_icon = "ic_vehicle_08.png";
                        break;
                    case "EV":
                        segment_icon = "ic_vehicle_09.png";
                        break;

                    case "MARINE":
                        segment_icon = "ic_vehicle_07.png";
                        break;

                    case "INDMOB":
                        segment_icon = "ic_vehicle_01.png";
                        break;

                    case "ROB":
                        segment_icon = "ic_vehicle_06.png";
                        break;
                }
                return segment_icon;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        #endregion

        public ICommand ClosePopupCommand { get; set; }
    }
}
