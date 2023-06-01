using MultiEventController.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.LiveParameter
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LiveParameterSelectedPage : ContentPage
    {
        LiveParameterSelectedViewModel viewModel;
        public LiveParameterSelectedPage(ObservableCollection<PidCode> SelectedParameterList)
        {
            try
            {
                InitializeComponent();
                BindingContext = viewModel = new LiveParameterSelectedViewModel(this, SelectedParameterList);
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.controlEventManager.OnRecievedData += ControlEventManager_OnRecievedData;
            App.controlEventManager.OnRecieved += ControlEventManager_OnRecieved;
            viewModel.HideNavButton += Vm_HideNavButton;
            viewModel.ShowNavButton += Vm_ShowNavButton;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (CurrentUserEvent.Instance.IsExpert)
            {
                App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                {
                    ElementName = "GoBackSLP",
                    ElementValue = "GoBackSLP",
                    ToUserId = CurrentUserEvent.Instance.ToUserId,
                    IsExpert = CurrentUserEvent.Instance.IsExpert
                });
            }
            App.controlEventManager.OnRecievedData -= ControlEventManager_OnRecievedData;
            App.controlEventManager.OnRecieved -= ControlEventManager_OnRecieved;


            viewModel.StartTime = false;
            viewModel.HideNavButton -= Vm_HideNavButton;
            viewModel.ShowNavButton -= Vm_ShowNavButton;
        }


        private async void ControlEventManager_OnRecievedData(object sender, EventArgs e)
        {
            #region Check Internet Connection
            if (CurrentUserEvent.Instance.IsRemote && CurrentUserEvent.Instance.IsExpert)
            {
                //await Task.Delay(100);
                bool InsternetActive = true;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    string[] PairedData = new string[2];
                    string data = (string)sender; //sender as string;
                    if (!string.IsNullOrEmpty(data))
                    {
                        if (data.Contains("ButtonStatus*#"))
                        {
                            if (viewModel.btnText == "Start")
                            {
                                viewModel.StartTime = true;
                                viewModel.btnText = "Stop";
                            }
                            else
                            {
                                viewModel.StartTime = false;
                                viewModel.btnText = "Start";
                            }
                        }

                        else if (data.Contains("PlayPidValue*#"))
                        {
                            viewModel.is_again_start = false;
                            PairedData = data.Split('#');
                            viewModel.read_pid = JsonConvert.DeserializeObject<ObservableCollection<ReadPidPresponseModel>>(PairedData[1]); ;

                            if (viewModel.read_pid != null)
                            {
                                foreach (var pid in viewModel.read_pid)
                                {
                                    var pidlist = viewModel.selected_pid_list.FirstOrDefault(x => x.id == pid.pidNumber);
                                    if (pidlist != null)
                                    {
                                        if (pid.Status == "NOERROR")
                                        {
                                            if (pid.responseValue.Contains("."))
                                            {
                                                double.TryParse(pid.responseValue, out viewModel.deci);
                                                pidlist.show_resolution = viewModel.deci.ToString("0.00");
                                            }
                                            else
                                            {
                                                pidlist.show_resolution = pid.responseValue;
                                            }
                                        }
                                        else
                                        {
                                            pidlist.show_resolution = "ERR";
                                            pidlist.unit = "";
                                        }
                                        //item.resolution= Convert.ToDouble(pid.responseValue);
                                    }
                                }
                            }
                            else
                            {
                                viewModel.StartTime = false;
                            }
                            viewModel.is_again_start = true;
                        }
                        else if (data.Contains("InitValue*#"))
                        {
                            PairedData = data.Split('#');
                            viewModel.read_pid = JsonConvert.DeserializeObject<ObservableCollection<ReadPidPresponseModel>>(PairedData[1]); ;
                            viewModel.SetPidValue();
                            DependencyService.Get<ILodingPageService>().HideLoadingPage();
                        }
                        else if (data.Contains("PidValue*#"))
                        {
                            PairedData = data.Split('#');
                            viewModel.read_pid = JsonConvert.DeserializeObject<ObservableCollection<ReadPidPresponseModel>>(PairedData[1]); ;
                            viewModel.SetPidValue();
                            DependencyService.Get<ILodingPageService>().HideLoadingPage();
                        }
                        //else if (data.Contains("SnapshotStart*#"))
                        //{
                        //    PlayPauseItem.BackgroundColor = Color.Gray;
                        //    SnapshotItem.BackgroundColor = Color.FromHex("#3e4095");
                        //    RecordingItem.BackgroundColor = Color.Gray;

                        //    PlayPauseItem.IsEnabled = false;
                        //    SnapshotItem.IsEnabled = true;
                        //    RecordingItem.IsEnabled = false;

                        //    Snapshot = true;

                        //    value = string.Empty;
                        //    code = string.Empty;
                        //}
                        //else if (data.Contains("SnapShotData*#"))
                        //{
                        //    PairedData = data.Split('#');
                        //    Values = JsonConvert.DeserializeObject<List<snapshot_record>>(PairedData[1]);
                        //}
                        //else if (data.Contains("SnapShotSaved"))
                        //{
                        //    var RecordingSaved = new Popup.DisplayAlertPage("SUCCESS", "Snapshot Saved Successfully", "OK");
                        //    await PopupNavigation.Instance.PushAsync(RecordingSaved);
                        //    PlayPauseItem.BackgroundColor = Color.FromHex("#3e4095");
                        //    SnapshotItem.BackgroundColor = Color.FromHex("#3e4095");
                        //    RecordingItem.BackgroundColor = Color.FromHex("#3e4095");

                        //    PlayPauseItem.IsEnabled = true;
                        //    SnapshotItem.IsEnabled = true;
                        //    RecordingItem.IsEnabled = true;

                        //    Snapshot = false;
                        //}
                        //else if (data.Contains("SnapShotFailled"))
                        //{
                        //    var RecordingSaved = new Popup.DisplayAlertPage("ERROR", "Snapshot Not Saved Successfully", "OK");
                        //    await PopupNavigation.Instance.PushAsync(RecordingSaved);
                        //    PlayPauseItem.BackgroundColor = Color.FromHex("#3e4095");
                        //    SnapshotItem.BackgroundColor = Color.FromHex("#3e4095");
                        //    RecordingItem.BackgroundColor = Color.FromHex("#3e4095");

                        //    PlayPauseItem.IsEnabled = true;
                        //    SnapshotItem.IsEnabled = true;
                        //    RecordingItem.IsEnabled = true;

                        //    Snapshot = false;
                        //}
                        //else if (data.Contains("InternetIssue"))
                        //{
                        //    Snapshot = false;
                        //}
                        //else if (data.Contains("RecordingStartFalse*#"))
                        //{
                        //    PlayPauseItem.BackgroundColor = Color.Gray;
                        //    SnapshotItem.BackgroundColor = Color.Gray;
                        //    RecordingItem.BackgroundColor = Color.FromHex("#EE204D");

                        //    PlayPauseItem.IsEnabled = false;
                        //    SnapshotItem.IsEnabled = false;
                        //    RecordingItem.IsEnabled = true;

                        //    RecordingStart = true;
                        //    RecordingItem.Source = "ic_recording.png";
                        //}
                        //else if (data.Contains("RecordingStartTrue*#"))
                        //{
                        //    PlayPauseItem.BackgroundColor = Color.FromHex("#3e4095");
                        //    SnapshotItem.BackgroundColor = Color.FromHex("#3e4095");
                        //    RecordingItem.BackgroundColor = Color.FromHex("#3e4095");

                        //    PlayPauseItem.IsEnabled = true;
                        //    SnapshotItem.IsEnabled = true;
                        //    RecordingItem.IsEnabled = true;

                        //    RecordingStart = false;
                        //}
                        //else if (data.Contains("InitializeList*#"))
                        //{
                        //    LiveRecord = new List<PIDLiveRecord>(); // 1
                        //    Live = new List<PidLive>(); // 2
                        //    YAxisPoint = new List<YAxisPointName>(); // 3

                        //    //j = 0;
                        //    NavigationPage.SetHasBackButton(this, false);
                        //}
                        else if (data.Contains("SetHasBackButtonTrue*#"))
                        {
                            NavigationPage.SetHasBackButton(this, true);
                        }
                        else if (data.Contains("SetHasBackButtonFalse*#"))
                        {
                            NavigationPage.SetHasBackButton(this, false);
                        }
                        else if (data.Contains("ShowLoader*#"))
                        {
                            NavigationPage.SetHasBackButton(this, true);
                        }
                        else if (data.Contains("HideLoader*#"))
                        {
                            NavigationPage.SetHasBackButton(this, true);
                        }
                        //else if (data.Contains("SendTimer*#"))
                        //{
                        //    PairedData = data.Split('#');
                        //    LabRecordingTimer.Text = PairedData[1];
                        //}
                        //else if (data.Contains("ResetRecordingTimer*#"))
                        //{
                        //    PairedData = data.Split('#');
                        //    LabRecordingTimer.Text = PairedData[1];
                        //}
                        //else if (data.Contains("RecodingSaved"))
                        //{
                        //    var RecordingSaved = new Popup.DisplayAlertPage("SUCCESS", "Recording Saved Successfully", "OK");
                        //    await PopupNavigation.Instance.PushAsync(RecordingSaved);
                        //}
                        //else if (data.Contains("RecordingFailled"))
                        //{
                        //    var RecordingSaved = new Popup.DisplayAlertPage("ERROR", "Recording Not Saved Successfully", "OK");
                        //    await PopupNavigation.Instance.PushAsync(RecordingSaved);
                        //}
                    }
                    InsternetActive = false;
                });
            }
            #endregion
        }



        private async void ControlEventManager_OnRecieved(object sender, EventArgs e)
        {
            string ReceiveValue = string.Empty;
            var elementEventHandler = (sender as ElementEventHandler);
            ReceiveValue = elementEventHandler.ElementValue;
           if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("PidPlayPauseClicked"))
            {
                viewModel.PlayPid();
            }
            else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("PidListScrolled"))
            {
                //collectionView.ScrollTo(Convert.ToInt32(elementEventHandler.ElementName), 0, ScrollToPosition.Start);
            }
            else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("GoBackSLP"))
            {
                await this.Navigation.PopAsync();
            }
            //else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("PidRecordingClicked"))
            //{
            //    PidRecordingClicked();
            //}
            //else if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("PidSnapshotClicked"))
            //{
            //    PidSnapshotClicked();
            //}
            App.controlEventManager.RecieveCallControlEvents(this, elementEventHandler, CurrentUserEvent.Instance.OwnerUserId);
        }


        private void Vm_ShowNavButton(object sender, EventArgs e)
        {
            NavigationPage.SetHasBackButton(this, true);
            if (!CurrentUserEvent.Instance.IsExpert)
            {
                App.controlEventManager.SendRequestData("SetHasBackButtonTrue*#" );
            }
        }

        private void Vm_HideNavButton(object sender, EventArgs e)
        {
            NavigationPage.SetHasBackButton(this, false);
            if (!CurrentUserEvent.Instance.IsExpert)
            {
                App.controlEventManager.SendRequestData("SetHasBackButtonFalse*#");
            }
        }

        protected override bool OnBackButtonPressed()
        {
            //return base.OnBackButtonPressed();
            if (viewModel.is_again_start)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
