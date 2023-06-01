using Acr.UserDialogs;
using MultiEventController.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
//using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using WikitekMotorCycleMechanik.Views.LiveParameter;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class LiveParameterViewModel : ViewModelBase
    {
        ApiServices apiServices;
        //readonly INavigation navigationService;
        readonly IBlueToothDevices blueTooth;
        //readonly Page page;
        //TapGestureRecognizer tab_gesture;
        //Grid tab_view;
        //Grid list_view;
        public string read_data_fn_index = string.Empty;
        public LiveParameterViewModel(Page page) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                this.blueTooth = DependencyService.Get<IBlueToothDevices>();
                ecus_list = new ObservableCollection<EcusModel>();

                InitializeCommands();
                msg = "Loading...";
                loader_visible = true;
                read_data_fn_index = StaticMethods.ecu_info.FirstOrDefault().read_data_fn_index;
                Device.InvokeOnMainThreadAsync(async () =>
                {
                    GetPidList();
                });
            }
            catch (Exception ex)
            {
            }
        }


        #region properties
        private string _msg;
        public string msg
        {
            get => _msg;
            set
            {
                _msg = value;
                OnPropertyChanged("msg");
            }
        }

        private bool _loader_visible = false;
        public bool loader_visible
        {
            get => _loader_visible;
            set
            {
                _loader_visible = value;
                OnPropertyChanged("loader_visible");
            }
        }

        private string _selected_ecu;
        public string selected_ecu
        {
            get => _selected_ecu;
            set
            {
                _selected_ecu = value;
                OnPropertyChanged("selected_ecu");
            }
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
                    pid_list = new ObservableCollection<PidCode>(static_pid_list.Where(x => x.short_name.ToLower().Contains(search_key.ToLower())).ToList());
                }

                OnPropertyChanged("search_key");
            }
        }

        private string _txt_select_button = "Select All";
        public string txt_select_button
        {
            get => _txt_select_button;
            set
            {
                _txt_select_button = value;
                OnPropertyChanged("txt_select_button");
            }
        }

        private ObservableCollection<PidCode> _static_pid_list;
        public ObservableCollection<PidCode> static_pid_list
        {
            get => _static_pid_list;
            set
            {
                _static_pid_list = value;
                OnPropertyChanged("static_pid_list");
            }
        }

        private ObservableCollection<PidCode> _pid_list;
        public ObservableCollection<PidCode> pid_list
        {
            get => _pid_list;
            set
            {
                _pid_list = value;
                OnPropertyChanged("pid_list");
            }
        }

        private ObservableCollection<PidCode> _selected_pid_list;
        public ObservableCollection<PidCode> selected_pid_list
        {
            get => _selected_pid_list;
            set
            {
                _selected_pid_list = value;
                OnPropertyChanged("selected_pid_list");
            }
        }

        private ObservableCollection<EcusModel> _ecus_list;
        public ObservableCollection<EcusModel> ecus_list
        {
            get => _ecus_list;
            set
            {
                _ecus_list = value;
                OnPropertyChanged("ecus_list");
            }
        }
        #endregion

        #region Methods


        public void InitializeCommands()
        {
            SelectPidCommand = new Command(async (obj) =>
            {
                try
                {

                    if (CurrentUserEvent.Instance.IsRemote && CurrentUserEvent.Instance.IsExpert)
                    {
                        await Task.Delay(100);
                        App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                        {
                            ElementName = txt_select_button,
                            ElementValue = "SelectAllPid",
                            ToUserId = CurrentUserEvent.Instance.ToUserId,
                            IsExpert = CurrentUserEvent.Instance.IsExpert
                        });
                    }
                    if (txt_select_button == "Select All")
                    {
                        txt_select_button = "Unselect All";
                        foreach (var item in pid_list.Take(20))
                        {
                            item.Selected = true;
                        }
                    }
                    else
                    {
                        txt_select_button = "Select All";
                        foreach (var item in pid_list)
                        {
                            item.Selected = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            });

            ContinueCommand = new Command(async (obj) =>
            {
                try
                {
                    if (CurrentUserEvent.Instance.IsExpert)
                    {
                        App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                        {
                            ElementName = $"ContinueFrame",
                            ElementValue = "ContinueClicked",
                            ToUserId = CurrentUserEvent.Instance.ToUserId,
                            IsExpert = CurrentUserEvent.Instance.IsExpert
                        });
                    }
                    if (pid_list != null)
                    {
                        if (pid_list.Count > 0)
                        {
                            var selected_pid = pid_list.Where(x => x.Selected).ToList();

                            if (selected_pid.Count > 0)
                            {
                                await page.Navigation.PushAsync(new LiveParameterSelectedPage(new ObservableCollection<PidCode>(selected_pid)));
                            }
                            else
                            {
                                await page.DisplayAlert("Alert", "Please select any parameter", "OK");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                }
            });

            EcuTabCommand = new Command(async (obj) =>
            {
                try
                {
                    var selected_ecu = (EcusModel)obj;
                    selected_ecu.opacity = 1;
                    pid_list = new ObservableCollection<PidCode>(selected_ecu?.pid_list);
                    read_data_fn_index = StaticMethods.ecu_info.FirstOrDefault(x => x.ecu_name == selected_ecu.ecu_name).read_data_fn_index;
                    foreach (var ecu in ecus_list)
                    {
                        if (selected_ecu != ecu)
                        {
                            ecu.opacity = .5;
                        }
                    }

                    var rxHeader = StaticMethods.ecu_info.FirstOrDefault(X => X.ecu_name == selected_ecu.ecu_name).rx_header;
                    var txHeader = StaticMethods.ecu_info.FirstOrDefault(X => X.ecu_name == selected_ecu.ecu_name).tx_header;

                    DependencyService.Get<Interfaces.IBlueToothDevices>().SendHeader(txHeader, rxHeader);
                }
                catch (Exception ex)
                {
                }
            });

            CheckPressedCommand = new Command(async (obj) =>
            {
                try
                {
                    var selected_pid = (PidCode)obj;
                    if (CurrentUserEvent.Instance.IsRemote && CurrentUserEvent.Instance.IsExpert)
                    {
                        App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                        {
                            ElementName = JsonConvert.SerializeObject(selected_pid),
                            ElementValue = "CheckPressed",
                            ToUserId = CurrentUserEvent.Instance.ToUserId,
                            IsExpert = CurrentUserEvent.Instance.IsExpert
                        });
                    }

                    selected_pid.Selected = (!selected_pid.Selected);

                    

                    if (pid_list.Where(x=>x.Selected).Count() >20)
                    {
                        selected_pid.Selected = (!selected_pid.Selected);
                        var errorpage = new Views.Popup.DisplayAlertPage("Alert", "You can select maximum 20 parameters", "OK");
                        await PopupNavigation.Instance.PushAsync(errorpage);
                    }
                }
                catch (Exception ex)
                {
                }
            });

            //TabCommand = new Command(async (obj) =>
            //{
            //    foreach (var item in child.Children)
            //    {
            //        //var grd = ((Grid)item);

            //        ////var grd = ((Grid)grd_tab.Children.ElementAt(i));
            //        ////var collection = ((CollectionView)collection_view.Children.ElementAt(i));
            //        //if (item.Opacity == class_id && collection.ClassId == class_id)
            //        //{
            //        //    grd.Opacity = 1;
            //        //    collection.IsVisible = true;
            //        //    selected_ecu = lbl.Text;
            //        //}
            //        //else
            //        //{
            //        //    grd.Opacity = 0.5;
            //        //    collection.IsVisible = false;
            //        //}
            //    }
            //});
        }

        public async void GetPidList()
        {
            //using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
            //{
                await Task.Delay(200);
                try
                {
                    //int pid_count = 0;
                    int count = 0;
                    if (read_data_fn_index.Contains("GENERIC"))
                    {
                        //Debug.WriteLine("Start Generic : "+DateTime.Now().)
                        var suported_pid = await blueTooth.GetGenericObdPid();
                        if (suported_pid == null)
                        {
                            msg = "Ecu communication error...";
                            loader_visible = false;
                            App.controlEventManager.SendRequestData("PidNotFound*#" + msg);
                            return;
                        }

                        if (suported_pid[0] == "ERROR")
                        {
                            msg = suported_pid[1];
                            loader_visible = false;
                            App.controlEventManager.SendRequestData("PidNotFound*#" + msg);
                            return;
                        }

                        foreach (var ecu in StaticMethods.ecu_info)
                        {
                            count++;
                            int pid_dataset = ecu.pid_dataset_id;
                            var pid_li = await apiServices.GetPid(Xamarin.Essentials.Preferences.Get("token", null), pid_dataset, App.is_update);
                            List<PidCode> supported_pid_list = new List<PidCode>();

                            if (pid_li == null)
                            {
                                msg = "Service not working...";
                                loader_visible = false;
                                return;
                            }

                            foreach (var item2 in suported_pid)
                            {

                                var item = pid_li.codes.FirstOrDefault(x => x.code == item2);
                                if (item != null)
                                {
                                    supported_pid_list.Add(item);
                                    //pid_count++;
                                    //Debug.WriteLine($"Pid Count : {pid_count}");
                                    //Debug.WriteLine($"Pid Id : {item.id}");
                                    //Debug.WriteLine($"Pid Name : {item.short_name}");
                                    //Debug.WriteLine($"Pid Cde : {item.code}");
                                }
                                //foreach (var item in pid_li.codes)
                                //{

                                //    if (item.code == item2)
                                //    {
                                //        supported_pid_list.Add(item);
                                //    }
                                //}
                            }
                            ecus_list.Add(
                                new EcusModel
                                {
                                    ecu_name = ecu.ecu_name,
                                    opacity = count == 1 ? 1 : .5,
                                    pid_list = supported_pid_list,
                                });
                        }

                        static_pid_list = pid_list = new ObservableCollection<PidCode>(ecus_list.FirstOrDefault().pid_list);
                        App.controlEventManager.SendRequestData("GetEcuGenericPidList*#" + JsonConvert.SerializeObject(suported_pid));
                        //var json = JsonConvert.SerializeObject(pid_list);
                    }
                    else
                    {
                        foreach (var ecu in StaticMethods.ecu_info)
                        {
                            count++;
                            int pid_dataset = ecu.pid_dataset_id;
                            var pid_li = await apiServices.GetPid(Xamarin.Essentials.Preferences.Get("token", null), pid_dataset, false);

                            ecus_list.Add(
                                new EcusModel
                                {
                                    ecu_name = ecu.ecu_name,
                                    opacity = count == 1 ? 1 : .5,
                                    pid_list = pid_li.codes,
                                });
                        }
                        static_pid_list = pid_list = new ObservableCollection<PidCode>(ecus_list.FirstOrDefault().pid_list);
                        App.controlEventManager.SendRequestData("GetEcuPidList*#");
                    }
                }
                catch (Exception ex)
                {
                    msg = "Something went wrong...";
                    loader_visible = false;
                }
            //}
        }

        //public void CreateDynamicView()
        //{
        //    try
        //    {
        //        bool collectiionVisible = false;
        //        double opacity = -1;
        //        int class_id = 0;
        //        CollectionView collectionView;
        //        ecus_pid_list = new ObservableCollection<EcusPid> { new EcusPid { ecu_name = "Tab 1", pid_list = new List<PidCode> { new PidCode { short_name = "list 1" } } }, new EcusPid { ecu_name = "Tab 2", pid_list = new List<PidCode> { new PidCode { short_name = "list 2" } } } };

        //        foreach (var item in ecus_pid_list)
        //        {
        //            /////////////   Tab
        //            if (opacity == -1)
        //            {
        //                opacity = 1;
        //                collectiionVisible = true;
        //            }
        //            else
        //            {
        //                opacity = 0.5;
        //                collectiionVisible = false;
        //            }


        //            ColumnDefinition column = new ColumnDefinition
        //            {
        //                Width = GridLength.Star,
        //            };
        //            tab_view.ColumnDefinitions.Add(column);

        //            Grid grd = new Grid
        //            {
        //                BackgroundColor = (Color)Application.Current.Resources["theme_color"],
        //                Opacity = opacity,
        //                ClassId = Convert.ToString(class_id),
        //            };

        //            Label ecu_lbl = new Label
        //            {
        //                Text = item.ecu_name,
        //                Style = (Style)Application.Current.Resources["txt_tab"]
        //            };

        //            grd.Children.Add(ecu_lbl);
        //            tab_view.Children.Add(grd, tab_view.ColumnDefinitions.Count - 1, 0);
        //            grd.GestureRecognizers.Add(tab_gesture);



        //            ///////////////// List


        //            collectionView = new CollectionView { ClassId = Convert.ToString(class_id), IsVisible = collectiionVisible };
        //            collectionView.SetBinding(ItemsView.ItemsSourceProperty, "item.pid_list");

        //            collectionView.ItemTemplate = new DataTemplate(() =>
        //            {
        //                Grid grid = new Grid { Padding = 10 };
        //                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });
        //                grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 33 });

        //                Label short_name = new Label { Style = (Style)Application.Current.Resources["lbl"] };
        //                short_name.SetBinding(Label.TextProperty, "short_name");

        //                CheckBox selection = new CheckBox { Style = (Style)Application.Current.Resources["check_box"] };
        //                selection.SetBinding(CheckBox.IsCheckedProperty, "Selected");

        //                grid.Children.Add(short_name, 0, 0);
        //                grid.Children.Add(selection, 1, 0);

        //                return grid;
        //            });

        //            list_view.Children.Add(collectionView, 0, 0);

        //            class_id++;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private void Tab_gesture_Tapped(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var sen = (Grid)sender;
        //        if (sen == null) { return; }
        //        int index = 0;
        //        foreach (var item in tab_view.Children)
        //        {
        //            var grd = (Grid)item;
        //            var collection = ((CollectionView)list_view.Children[index]);
        //            if (grd.ClassId == sen.ClassId && collection.ClassId == sen.ClassId)
        //            {
        //                grd.Opacity = 1;
        //                selected_ecu = ((Label)(grd.Children[0])).Text;
        //                collection.IsVisible = true;
        //            }
        //            else
        //            {
        //                grd.Opacity = 0.5;
        //                collection.IsVisible = false;
        //            }
        //            index++;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}


        #endregion

        #region Commmads
        public ICommand ContinueCommand { get; set; }
        public ICommand EcuTabCommand { get; set; }
        public ICommand SelectPidCommand { get; set; }
        public ICommand CheckPressedCommand { get; set; }
        #endregion
    }
}
