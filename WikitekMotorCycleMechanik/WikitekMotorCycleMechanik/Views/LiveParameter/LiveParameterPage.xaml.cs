using MultiEventController.Models;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.LiveParameter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LiveParameterPage : ContentPage
    {
        LiveParameterViewModel viewModel;
        ApiServices apiServices;
        public LiveParameterPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            App.controlEventManager.OnRecieved += ControlEventManager_OnRecieved;
            App.controlEventManager.OnRecievedData += ControlEventManager_OnRecievedData;
            apiServices = new ApiServices();
            BindingContext = viewModel = new LiveParameterViewModel(this);
        }

        protected override void OnDisappearing()
        {

            base.OnDisappearing();

            App.controlEventManager.OnRecieved -= ControlEventManager_OnRecieved;
            App.controlEventManager.OnRecievedData -= ControlEventManager_OnRecievedData;

            if (CurrentUserEvent.Instance.IsExpert)
            {
                App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                {
                    ElementName = "GoBackLP",
                    ElementValue = "GoBackLP",
                    ToUserId = CurrentUserEvent.Instance.ToUserId,
                    IsExpert = CurrentUserEvent.Instance.IsExpert
                });
            }
        }

        private async void ControlEventManager_OnRecievedData(object sender, EventArgs e)
        {
            #region Check Internet Connection
            if (CurrentUserEvent.Instance.IsRemote && CurrentUserEvent.Instance.IsExpert)
            {
                //await Task.Delay(100);
                string[] PairedData = new string[2];
                bool InsternetActive = true;

                Device.BeginInvokeOnMainThread(async () =>
                {
                    string data = (string)sender; //sender as string;
                    if (!string.IsNullOrEmpty(data))
                    {
                        if (data.Contains("GetEcuPidList*#"))
                        {
                            try
                            {
                                //int pid_count = 0;
                                int count1 = 0;
                                foreach (var ecu in StaticMethods.ecu_info)
                                {
                                    count1++;
                                    int pid_dataset = ecu.pid_dataset_id;
                                    var pid_li = await apiServices.GetPid(Xamarin.Essentials.Preferences.Get("token", null), pid_dataset, false);
                                    viewModel.ecus_list.Add(
                                    new EcusModel
                                    {
                                        ecu_name = ecu.ecu_name,
                                        opacity = count1 == 1 ? 1 : .5,
                                        pid_list = pid_li.codes.ToList(),
                                    });
                                }
                                viewModel.static_pid_list = viewModel.pid_list = new ObservableCollection<PidCode>(viewModel.ecus_list.FirstOrDefault().pid_list);
                                //App.controlEventManager.SendRequestData("GetEcuPidList*#");
                                //var ecu1 = JsonConvert.SerializeObject(viewModel.ecus_list);
                                //var pid = JsonConvert.SerializeObject(viewModel.pid_list);
                                //App.controlEventManager.SendRequestData("EcuList*#" + ecu1);
                                //App.controlEventManager.SendRequestData("PidList*#" + pid);
                            }
                            catch (Exception ex)
                            {
                                //msg = "Something went wrong...";
                                //loader_visible = false;
                            }
                            DependencyService.Get<ILodingPageService>().HideLoadingPage();
                        }
                        else if (data.Contains("PidNotFound*#"))
                        {
                            PairedData = data.Split('#');
                            viewModel.msg = PairedData[1];
                            viewModel.loader_visible = false;
                        }
                        else if (data.Contains("GetEcuGenericPidList*#"))
                        {
                            int count = 0;
                            PairedData = data.Split('#');
                            viewModel.msg = "Loading...";
                            viewModel.loader_visible = false;
                            string[] suported_pid = JsonConvert.DeserializeObject<string[]>(PairedData[1]);
                            foreach (var ecu in StaticMethods.ecu_info)
                            {
                                count++;
                                int pid_dataset = ecu.pid_dataset_id;
                                var pid_li = await apiServices.GetPid(Xamarin.Essentials.Preferences.Get("token", null), pid_dataset, App.is_update);
                                List<PidCode> supported_pid_list = new List<PidCode>();

                                if (pid_li == null)
                                {
                                    viewModel.msg = "Service not working...";
                                    viewModel.loader_visible = false;
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
                                viewModel.ecus_list.Add(
                                    new EcusModel
                                    {
                                        ecu_name = ecu.ecu_name,
                                        opacity = count == 1 ? 1 : .5,
                                        pid_list = supported_pid_list,
                                    });
                            }

                            viewModel.static_pid_list = viewModel.pid_list = new ObservableCollection<PidCode>(viewModel.ecus_list.FirstOrDefault().pid_list);
                        }
                        //else if (data.Contains("PidList*#"))
                        //{
                        //    PairedData = data.Split('#');
                        //    viewModel.static_pid_list = viewModel.pid_list = JsonConvert.DeserializeObject<ObservableCollection<PidCode>>(PairedData[1]); ;
                        //    DependencyService.Get<ILodingPageService>().HideLoadingPage();
                        //}
                        //else if (data.Contains("SelectAnyParameter"))
                        //{
                        //    var errorpage = new Popup.DisplayAlertPage("Alert", "Please select any parameter", "OK");
                        //    await PopupNavigation.Instance.PushAsync(errorpage);
                        //}
                    }
                    InsternetActive = false;
                });
            }
            #endregion
        }

        private async void ControlEventManager_OnRecieved(object sender, EventArgs e)
        {
            var elementEventHandler = (sender as ElementEventHandler);
            string ReceiveValue = string.Empty;
            ReceiveValue = elementEventHandler.ElementValue;

            if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("SearchParameter"))
            {
                viewModel.search_key = elementEventHandler.ElementName;
                SearchParameter();
            }
            if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("SelectAllPid"))
            {
                viewModel.txt_select_button = elementEventHandler.ElementName;
                if (viewModel.txt_select_button == "Select All")
                {
                    viewModel.txt_select_button = "Unselect All";
                    foreach (var item in viewModel.pid_list.Take(5))
                    {
                        item.Selected = true;
                    }
                }
                else
                {
                    viewModel.txt_select_button = "Select All";
                    foreach (var item in viewModel.pid_list)
                    {
                        item.Selected = false;
                    }
                }
            }
            else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("CheckPressed"))
            {
                var selected_pid = JsonConvert.DeserializeObject<PidCode>(elementEventHandler.ElementName);

                selected_pid.Selected = (!selected_pid.Selected);

                var static_pid = viewModel.pid_list.FirstOrDefault(x => x.id == selected_pid.id);

                if (static_pid != null)
                {
                    static_pid.Selected = selected_pid.Selected;
                }


                if (viewModel.pid_list.Where(x => x.Selected).Count() > 5)
                {
                    selected_pid.Selected = (!selected_pid.Selected);

                    var static_pid1 = viewModel.pid_list.FirstOrDefault(x => x.id == selected_pid.id);

                    if (static_pid1 != null)
                    {
                        static_pid1.Selected = selected_pid.Selected;
                    }

                    var errorpage = new Popup.DisplayAlertPage("Alert", "You can select maximum 20 parameters", "OK");
                    await PopupNavigation.Instance.PushAsync(errorpage);
                }
                
            }
            else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("CheckPressed"))
            {
                var selected_pid = JsonConvert.DeserializeObject<PidCode>(elementEventHandler.ElementName);

                selected_pid.Selected = (!selected_pid.Selected);

                var static_pid = viewModel.pid_list.FirstOrDefault(x => x.id == selected_pid.id);

                if (static_pid != null)
                {
                    static_pid.Selected = selected_pid.Selected;
                }


                if (viewModel.pid_list.Where(x => x.Selected).Count() > 5)
                {
                    selected_pid.Selected = (!selected_pid.Selected);

                    var static_pid1 = viewModel.pid_list.FirstOrDefault(x => x.id == selected_pid.id);

                    if (static_pid1 != null)
                    {
                        static_pid1.Selected = selected_pid.Selected;
                    }

                    var errorpage = new Popup.DisplayAlertPage("Alert", "You can select maximum 20 parameters", "OK");
                    await PopupNavigation.Instance.PushAsync(errorpage);
                }

            }
            else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("ContinueClicked"))
            {
                if (viewModel.pid_list != null)
                {
                    if (viewModel.pid_list.Count > 0)
                    {
                        var selected_pid = viewModel.pid_list.Where(x => x.Selected).ToList();

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
            else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("ContinueClicked"))
            {
                if (viewModel.pid_list != null)
                {
                    if (viewModel.pid_list.Count > 0)
                    {
                        var selected_pid = viewModel.pid_list.Where(x => x.Selected).ToList();

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
            else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("GoBackLP"))
            {
                await this.Navigation.PopAsync();
            }
            //if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("SelectEcuClicked"))
            //{
            //    SelectPidClicked();
            //}
            //else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("PidListScrolled"))
            //{
            //    collectionView.ScrollTo(Convert.ToInt32(elementEventHandler.ElementName), 0, ScrollToPosition.Start);
            //}
            //else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("CheckBox_CheckedChanged"))
            //{
            //    try
            //    {
            //        PairedData = ReceiveValue.Split('*');
            //        viewModel.check_changed_pid = viewModel.pid_list.FirstOrDefault(x => x.id == Convert.ToInt32(elementEventHandler.ElementName));

            //        if (PairedData[1].Contains("rue"))
            //        {
            //            viewModel.check_changed_pid.Selected = true;
            //        }
            //        else
            //        {
            //            viewModel.check_changed_pid.Selected = false;
            //        }

            //        var static_pid = viewModel.static_pid_list.FirstOrDefault(x => x.id == viewModel.check_changed_pid.id);
            //        static_pid.Selected = viewModel.check_changed_pid.Selected;
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //}
            //else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("MaximumPidSelectionAlert"))
            //{
            //    var errorpage = new Popup.DisplayAlertPage("Alert", "You can select maximum 20 parameters", "OK");
            //    await PopupNavigation.Instance.PushAsync(errorpage);
            //}
            //else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("SearchParameter"))
            //{
            //    viewModel.search_key = elementEventHandler.ElementName;
            //    SearchParameter();
            //}
            //else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("ContinueClicked"))
            //{
            //    ContinueClicked();
            //}

            App.controlEventManager.RecieveCallControlEvents(this, elementEventHandler, CurrentUserEvent.Instance.OwnerUserId);
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (CurrentUserEvent.Instance.IsExpert)
            {
                App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                {
                    ElementName = $"{viewModel.search_key}",
                    ElementValue = "SearchParameter",
                    ToUserId = CurrentUserEvent.Instance.ToUserId,
                    IsExpert = CurrentUserEvent.Instance.IsExpert
                });
            }
            SearchParameter();
        }

        private void SearchParameter()
        {
            if (viewModel.static_pid_list != null)
            {
                var selected = viewModel.static_pid_list.Where(x => x.Selected);
                if (!string.IsNullOrEmpty(viewModel.search_key))
                {
                    viewModel.pid_list = new ObservableCollection<PidCode>(viewModel.static_pid_list.Where(x => x.short_name.ToLower().Contains(viewModel.search_key.ToLower())).ToList());
                }
                else
                {
                    viewModel.pid_list = new ObservableCollection<PidCode>(viewModel.static_pid_list.ToList());
                }
                var selected1 = viewModel.static_pid_list.Where(x => x.Selected);
            }
        }

        //public void create_ems_tab(double opacity)
        //{
        //    try
        //    {
        //        grd_tab.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //        Label lbl = new Label
        //        {
        //            Text = "EMS",
        //            Style = (Style)this.Resources["txt_tab"]
        //        };
        //        ems_grid = new Grid
        //        {
        //            BackgroundColor = (Color)Application.Current.Resources["theme_color"],
        //            Opacity = opacity,
        //            ClassId = "EMS",
        //        };
        //        ems_grid.Children.Add(lbl);
        //        var count = grd_tab.ColumnDefinitions.Count - 1;
        //        grd_tab.Children.Add(ems_grid, count, 0);
        //        ems_grid.GestureRecognizers.Add(tab_gesture);
        //        ems_grid_permitted = true;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //public void create_iecu_tab(double opacity)
        //{
        //    try
        //    {
        //        grd_tab.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //        Label lbl = new Label
        //        {
        //            Text = "IECU",
        //            Style = (Style)this.Resources["txt_tab"]
        //        };
        //        iecu_grid = new Grid
        //        {
        //            BackgroundColor = (Color)Application.Current.Resources["theme_color"],
        //            Opacity = opacity,
        //            ClassId = "IECU",
        //        };
        //        iecu_grid.Children.Add(lbl);
        //        var count = grd_tab.ColumnDefinitions.Count - 1;
        //        grd_tab.Children.Add(iecu_grid, count, 0);
        //        iecu_grid.GestureRecognizers.Add(tab_gesture);
        //        iecu_grid_permitted = true;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //public void create_acm_tab(double opacity)
        //{
        //    try
        //    {
        //        grd_tab.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //        Label lbl = new Label
        //        {
        //            Text = "ACM",
        //            Style = (Style)this.Resources["txt_tab"]
        //        };
        //        acm_grid = new Grid
        //        {
        //            BackgroundColor = (Color)Application.Current.Resources["theme_color"],
        //            Opacity = opacity,
        //            ClassId = "ACM",
        //        };
        //        acm_grid.Children.Add(lbl);
        //        var count = grd_tab.ColumnDefinitions.Count - 1;
        //        grd_tab.Children.Add(acm_grid, count, 0);
        //        acm_grid.GestureRecognizers.Add(tab_gesture);
        //        acm_grid_permitted = true;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //public void create_tgw2_tab(double opacity)
        //{
        //    try
        //    {
        //        grd_tab.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
        //        Label lbl = new Label
        //        {
        //            Text = "TGW2",
        //            Style = (Style)this.Resources["txt_tab"]
        //        };
        //        tgw2_grid = new Grid
        //        {
        //            BackgroundColor = (Color)Application.Current.Resources["theme_color"],
        //            Opacity = opacity,
        //            ClassId = "TGW2",
        //        };
        //        tgw2_grid.Children.Add(lbl);
        //        var count = grd_tab.ColumnDefinitions.Count - 1;
        //        grd_tab.Children.Add(tgw2_grid, count, 0);
        //        tgw2_grid.GestureRecognizers.Add(tab_gesture);
        //        tgw2_grid_permitted = true;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
        //private void Tab_gesture_Tapped(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var sen = sender as Grid;
        //        if (sen == null) { return; }
        //        if (ems_grid_permitted)
        //        {
        //            ems_grid.Opacity = 0.5;
        //        }

        //        if (iecu_grid_permitted)
        //        {
        //            iecu_grid.Opacity = 0.5;
        //        }

        //        if (acm_grid_permitted)
        //        {
        //            acm_grid.Opacity = 0.5;
        //        }

        //        if (tgw2_grid_permitted)
        //        {
        //            tgw2_grid.Opacity = 0.5;
        //        }

        //        EmsView.IsVisible = false;
        //        IecuView.IsVisible = false;
        //        AcmView.IsVisible = false;
        //        TgwView.IsVisible = false;

        //        switch (sen.ClassId)
        //        {
        //            case "EMS":
        //                ems_grid.Opacity = 1;
        //                EmsView.IsVisible = true;
        //                btnContinue.IsVisible = true;
        //                //SelectedECU = "(1.1.0)";
        //                break;

        //            case "IECU":
        //                iecu_grid.Opacity = 1;
        //                IecuView.IsVisible = true;
        //                btnContinue.IsVisible = false;
        //                //SelectedECU = "(6.2.0)";
        //                break;

        //            case "ACM":
        //                acm_grid.Opacity = 1;
        //                AcmView.IsVisible = true;
        //                //SelectedECU = "(2.1.0)";
        //                break;

        //            case "TGW2":
        //                tgw2_grid.Opacity = 1;
        //                TgwView.IsVisible = true;
        //                //SelectedECU = "(12.1.0)";
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(txtSearch.Text))
        //        {
        //            imgClose.IsVisible = false;
        //            ems_List.ItemsSource = viewModel.EMSParameterList;
        //            //iceu_List.ItemsSource = IecuList;
        //            //acm_List.ItemsSource = ACMList;
        //            //tgw_List.ItemsSource = TGW2List;
        //        }
        //        else
        //        {
        //            imgClose.IsVisible = true;
        //            if (EmsView.IsVisible)
        //            {
        //                ems_List.ItemsSource = viewModel.EMSParameterList.Where(x => x.Description.ToLower().Contains(e.NewTextValue.ToLower()) || x.Description.ToLower().Contains(e.NewTextValue.ToLower()));// ;

        //            }
        //            //else if (IecuView.IsVisible)
        //            //{
        //            //    iceu_List.ItemsSource = IecuList.Where(x => x.Description.ToLower().Contains(e.NewTextValue.ToLower()) || x.ParametersID.ToLower().Contains(e.NewTextValue.ToLower()));

        //            //}
        //            //else if (AcmView.IsVisible)
        //            //{
        //            //    acm_List.ItemsSource = ACMList.Where(x => x.Description.ToLower().Contains(e.NewTextValue.ToLower()) || x.ParametersID.ToLower().Contains(e.NewTextValue.ToLower()));

        //            //}
        //            //else if (TgwView.IsVisible)
        //            //{
        //            //    tgw_List.ItemsSource = TGW2List.Where(x => x.Description.ToLower().Contains(e.NewTextValue.ToLower()) || x.ParametersID.ToLower().Contains(e.NewTextValue.ToLower()));

        //            //}
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        //private void CloseClick(object sender, EventArgs e)
        //{
        //    txtSearch.Text = string.Empty;
        //    imgClose.IsVisible = false;
        //}

        //private void Selection_CheckedChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (EmsView.IsVisible)
        //        {
        //            foreach (var EmsItem in viewModel.EMSParameterList)
        //            {
        //                EmsItem.Selected = true;
        //            }
        //            ems_List.ItemsSource = null;
        //            ems_List.ItemsSource = viewModel.EMSParameterList;
        //        }
        //        //else if (IecuView.IsVisible)
        //        //{
        //        //    foreach (var IecuItem in IecuList)
        //        //    {
        //        //        IecuItem.IsSelected = e.Value;
        //        //    }
        //        //    iceu_List.ItemsSource = null;
        //        //    iceu_List.ItemsSource = IecuList;
        //        //}
        //        //else if (AcmView.IsVisible)
        //        //{
        //        //    foreach (var AcmItem in ACMList)
        //        //    {
        //        //        AcmItem.IsSelected = e.Value;
        //        //    }
        //        //    acm_List.ItemsSource = null;
        //        //    acm_List.ItemsSource = ACMList;
        //        //}
        //        //else if (TgwView.IsVisible)
        //        //{
        //        //    foreach (var Tgw2Item in TGW2List)
        //        //    {
        //        //        Tgw2Item.IsSelected = e.Value;
        //        //    }
        //        //    tgw_List.ItemsSource = null;
        //        //    tgw_List.ItemsSource = TGW2List;
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //}

        //private void btnContinue_Clicked(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        var EmsSelectedParameters = viewModel.EMSParameterList.Where(x => x.Selected);
        //        //var IecuSelectedParameters = IecuList.Where(x => x.IsSelected);
        //        //var AcmSelectedParameters = ACMList.Where(x => x.IsSelected);
        //        //var TGW2SelectedParameters = TGW2List.Where(x => x.IsSelected);

        //        if (EmsSelectedParameters != null)
        //        {
        //            foreach (var EmsParameter in EmsSelectedParameters)
        //            {
        //                SelectedParameterList.Add(
        //                    new LiveParameterModel
        //                    {
        //                        Description = EmsParameter.Description,
        //                        //NodeType = EmsParameter.ECUType,
        //                        //ECUType = EmsParameter.ECUType,
        //                        PCode = EmsParameter.PCode,
        //                        Selected = EmsParameter.Selected,
        //                        Unit = EmsParameter.Unit,
        //                        Value = EmsParameter.Value,
        //                        //KeyName = EmsParameter.KeyName,

        //                    });
        //            }
        //        }
        //        //if (IecuSelectedParameters != null)
        //        //{
        //        //    foreach (var IecuParameter in IecuSelectedParameters)
        //        //    {
        //        //        SelectedParameterList.Add(
        //        //            new ParaModel
        //        //            {
        //        //                Description = IecuParameter.Description,
        //        //                //NodeType = IecuParameter .ECUType,
        //        //                ECUType = IecuParameter.ECUType,
        //        //                ParametersID = IecuParameter.ParametersID,
        //        //                Selected = IecuParameter.IsSelected,
        //        //                Unit = IecuParameter.Unit,
        //        //                KeyName = IecuParameter.KeyName,
        //        //            });
        //        //    }
        //        //}
        //        //if (AcmSelectedParameters != null)
        //        //{
        //        //    foreach (var AcmParameter in AcmSelectedParameters)
        //        //    {
        //        //        SelectedParameterList.Add(
        //        //            new ParaModel
        //        //            {
        //        //                Description = AcmParameter.Description,
        //        //                //NodeType = AcmParameter .ECUType,
        //        //                ECUType = AcmParameter.ECUType,
        //        //                ParametersID = AcmParameter.ParametersID,
        //        //                Selected = AcmParameter.IsSelected,
        //        //                Unit = AcmParameter.Unit,
        //        //                KeyName = AcmParameter.KeyName,
        //        //            });
        //        //    }
        //        //}
        //        //if (TGW2SelectedParameters != null)
        //        //{
        //        //    foreach (var Tgw2Parameter in TGW2SelectedParameters)
        //        //    {
        //        //        SelectedParameterList.Add(
        //        //            new ParaModel
        //        //            {
        //        //                Description = Tgw2Parameter.Description,
        //        //                //NodeType = EmsParameter.ECUType,
        //        //                ECUType = Tgw2Parameter.ECUType,
        //        //                ParametersID = Tgw2Parameter.ParametersID,
        //        //                Selected = Tgw2Parameter.IsSelected,
        //        //                Unit = Tgw2Parameter.Unit,
        //        //                KeyName = Tgw2Parameter.KeyName,
        //        //            });
        //        //    }
        //        //}

        //        if (SelectedParameterList.Count < 1)
        //        {
        //            DisplayAlert("Alert", "Please Selected a parameter", "Ok");
        //            return;
        //        }

        //        Navigation.PushAsync(new LiveParameterSelectedPage(SelectedParameterList));
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}
    }
}