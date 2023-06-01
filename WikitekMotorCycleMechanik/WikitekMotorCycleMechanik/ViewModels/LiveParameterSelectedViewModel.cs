using Acr.UserDialogs;
using MultiEventController.Models;
using Newtonsoft.Json;
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
    public class LiveParameterSelectedViewModel : ViewModelBase
    {
        readonly Page page;
        public bool is_again_start = false;
        public event EventHandler HideNavButton;
        public event EventHandler ShowNavButton;
        public bool StartTime = false;
        public double deci;

        public LiveParameterSelectedViewModel(Page page, ObservableCollection<PidCode> selectedPid) : base(page)
        {
            try
            {
                this.page = page;
                selected_pid_list = new ObservableCollection<PidCode>();
                selected_pid_list = selectedPid;
                Task.Run(() =>
                {
                    Device.InvokeOnMainThreadAsync(async () =>
                    {
                        using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                        {
                            await GetPidsValue(selectedPid);
                            is_again_start = true;
                        }
                    });
                });
            }
            catch (Exception ex)
            {
            }
        }

        #region properties
        private bool _play_button_enable = true;
        public bool play_button_enable
        {
            get => _play_button_enable;
            set
            {
                _play_button_enable = value;
                OnPropertyChanged("play_button_enable");
            }
        }

        private string _btnText = "Start";
        public string btnText
        {
            get => _btnText;
            set
            {
                _btnText = value;
                OnPropertyChanged("btnText");
            }
        }

        private bool _stop_button_enable = false;
        public bool stop_button_enable
        {
            get => _stop_button_enable;
            set
            {
                _stop_button_enable = value;
                OnPropertyChanged("stop_button_enable");
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
        #endregion


        #region Methods
        public ObservableCollection<ReadPidPresponseModel> read_pid;
        public async Task GetPidsValue(ObservableCollection<PidCode> selectedPid)
        {

            try
            {

                read_pid = await DependencyService.Get<Interfaces.IBlueToothDevices>().ReadPid(selectedPid);
                if (!CurrentUserEvent.Instance.IsExpert)
                {
                    App.controlEventManager.SendRequestData("PidValue*#" + JsonConvert.SerializeObject(read_pid));
                }
                if (read_pid != null)
                {
                    foreach (var pid in read_pid)
                    {
                        var pidlist = selected_pid_list.FirstOrDefault(x => x.id == pid.pidNumber);
                        if (pidlist != null)
                        {
                            if (pid.Status == "NOERROR")
                            {
                                if (pid.responseValue.Contains("."))
                                {
                                    double.TryParse(pid.responseValue, out deci);
                                    pidlist.show_resolution = deci.ToString("0.00");
                                }
                                else
                                {
                                    pidlist.show_resolution = pid.responseValue;
                                }
                                //pidlist.show_resolution = pid.responseValue;
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

                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    //ems_List.ItemsSource = null;
                //    //viewModel.EMSParameterList = ParameterList;
                //    ///ems_List.ItemsSource = viewModel.EMSParameterList;
                //});

                #region Commented Code
                //ObservableCollection<ReadParameterPID> pidList = new ObservableCollection<ReadParameterPID>();
                //parameter_list = new ObservableCollection<ReadParameterPID>();
                //foreach (var item in selected_pid_list)
                //{
                //    var MessageModels = new List<Models.Message>();
                //    pidList = new ObservableCollection<ReadParameterPID>();
                //    foreach (var MessageItem in item.messages)
                //    {
                //        MessageModels.Add(new Models.Message { code = MessageItem.code, message = MessageItem.message });
                //    }

                //    pidList.Add(
                //        new ReadParameterPID
                //        {
                //            pid = item.code,
                //            totalLen = item.code.Length / 2,
                //            //totalbyte -
                //            startByte = item.byte_position,
                //            noOfBytes = item.length,
                //            IsBitcoded = item.bitcoded,
                //            //noofBits = (int?)item.start_bit_position - (int?)item.end_bit_position + 1,
                //            startBit = Convert.ToInt32(item.start_bit_position),
                //            noofBits = item.end_bit_position.GetValueOrDefault() - item.start_bit_position.GetValueOrDefault() + 1,
                //            resolution = item.resolution,
                //            offset = item.offset,
                //            datatype = item.message_type,
                //            //totalBytes = item.length,
                //            pidNumber = item.id,
                //            pidName = item.short_name,
                //            unit = item.unit,
                //            messages = MessageModels,
                //        });

                //    //final_parameter_list = pidList;
                //    //final_parameter_list.Add(pidList.FirstOrDefault());
                //    //parameter_list.Add(pidList.FirstOrDefault());
                //    var read_pid = await DependencyService.Get<Interfaces.IBlueToothDevices>().ReadPid(selected_pid_list);

                //    if (read_pid != null)
                //    {
                //        foreach (var pid in read_pid)
                //        {
                //            var pidlist = selected_pid_list.FirstOrDefault(x => x.id == pid.pidNumber);
                //            if (pidlist != null)
                //            {
                //                if (pid.Status == "NOERROR")
                //                {
                //                    pidlist.show_resolution = pid.responseValue;
                //                }
                //                else
                //                {
                //                    pidlist.show_resolution = "ERR";
                //                    pidlist.unit = "";
                //                }
                //                //item.resolution= Convert.ToDouble(pid.responseValue);
                //            }
                //        }

                //        //Device.BeginInvokeOnMainThread(() =>
                //        //{
                //        //    //ems_List.ItemsSource = null;
                //        //    //viewModel.EMSParameterList = ParameterList;
                //        //    ///ems_List.ItemsSource = viewModel.EMSParameterList;
                //        //});
                //    }
                //}
                #endregion
            }
            catch (Exception ex)
            {
            }
        }

        public void SetPidValue()
        {
            if (read_pid != null)
            {
                foreach (var pid in read_pid)
                {
                    var pidlist = selected_pid_list.FirstOrDefault(x => x.id == pid.pidNumber);
                    if (pidlist != null)
                    {
                        if (pid.Status == "NOERROR")
                        {
                            if (pid.responseValue.Contains("."))
                            {
                                double.TryParse(pid.responseValue, out deci);
                                pidlist.show_resolution = deci.ToString("0.00");
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
                    }
                }
            }

            //if (read_pid != null)
            //{
            //    foreach (var pid in read_pid)
            //    {
            //        var pidlist = selected_pid_list.FirstOrDefault(x => x.id == pid.pidNumber);
            //        if (pidlist != null)
            //        {
            //            if (pid.Status == "NOERROR")
            //            {
            //                if (pid.responseValue.Contains("."))
            //                {
            //                    double.TryParse(pid.responseValue, out deci);
            //                    pidlist.show_resolution = deci.ToString("0.00");
            //                }
            //                else
            //                {
            //                    pidlist.show_resolution = pid.responseValue;
            //                }
            //                //pidlist.show_resolution = pid.responseValue;
            //            }
            //            else
            //            {
            //                pidlist.show_resolution = "ERR";
            //                pidlist.unit = "";
            //            }
            //            //item.resolution= Convert.ToDouble(pid.responseValue);
            //        }
            //    }

            //    //Device.BeginInvokeOnMainThread(() =>
            //    //{
            //    //    //ems_List.ItemsSource = null;
            //    //    //viewModel.EMSParameterList = ParameterList;
            //    //    ///ems_List.ItemsSource = viewModel.EMSParameterList;
            //    //});
            //}
        }

        
        public async void PlayPid()
        {
            if (btnText == "Start")
            {
                StartTime = true;
                btnText = "Stop";
            }
            else
            {
                StartTime = false;
                btnText = "Start";
            }


            //if (!CurrentUserEvent.Instance.IsExpert)
            //{
            //    App.controlEventManager.SendRequestData("ButtonStatus*#");
            //}
            if (is_again_start)
            {

                HideNavButton?.Invoke("", new EventArgs());

                while (StartTime)
                {
                    await Task.Run(async () =>
                    {
                        is_again_start = false;
                        var read_pid = await DependencyService.Get<Interfaces.IBlueToothDevices>().ReadPid(selected_pid_list);
                        if (!CurrentUserEvent.Instance.IsExpert)
                        {
                            App.controlEventManager.SendRequestData("PlayPidValue*#" + JsonConvert.SerializeObject(read_pid));
                        }
                        if (read_pid != null)
                        {
                            foreach (var pid in read_pid)
                            {
                                var pidlist = selected_pid_list.FirstOrDefault(x => x.id == pid.pidNumber);
                                if (pidlist != null)
                                {
                                    if (pid.Status == "NOERROR")
                                    {
                                        if (pid.responseValue.Contains("."))
                                        {
                                            double.TryParse(pid.responseValue, out deci);
                                            pidlist.show_resolution = deci.ToString("0.00");
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
                            StartTime = false;
                        }
                        is_again_start = true;
                    });
                }

                ShowNavButton?.Invoke("", new EventArgs());
            }
        }

        
        #endregion


        #region Commands
        public ICommand PlayCommand => new Command(async (obj) =>
        {
            if (CurrentUserEvent.Instance.IsExpert)
            {
                App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                {
                    ElementName = $"PidPlayPauseClicked",
                    ElementValue = "PidPlayPauseClicked",
                    ToUserId = CurrentUserEvent.Instance.ToUserId,
                    IsExpert = CurrentUserEvent.Instance.IsExpert
                });
            }
            PlayPid();

            #region Working Code
            //if (btnText == "Start")
            //{
            //    StartTime = true;
            //    btnText = "Stop";
            //}
            //else
            //{
            //    StartTime = false;
            //    btnText = "Start";
            //}

            //if (is_again_start)
            //{

            //    HideNavButton?.Invoke("", new EventArgs());

            //    while (StartTime)
            //    {
            //        await Task.Run(async () =>
            //        {
            //            is_again_start = false;
            //            var read_pid = await DependencyService.Get<Interfaces.IBlueToothDevices>().ReadPid(selected_pid_list);

            //            if (read_pid != null)
            //            {
            //                foreach (var pid in read_pid)
            //                {
            //                    var pidlist = selected_pid_list.FirstOrDefault(x => x.id == pid.pidNumber);
            //                    if (pidlist != null)
            //                    {
            //                        if (pid.Status == "NOERROR")
            //                        {
            //                            if (pid.responseValue.Contains("."))
            //                            {
            //                                double.TryParse(pid.responseValue, out deci);
            //                                pidlist.show_resolution = deci.ToString("0.00");
            //                            }
            //                            else
            //                            {
            //                                pidlist.show_resolution = pid.responseValue;
            //                            }
            //                        }
            //                        else
            //                        {
            //                            pidlist.show_resolution = "ERR";
            //                            pidlist.unit = "";
            //                        }
            //                        //item.resolution= Convert.ToDouble(pid.responseValue);
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                StartTime = false;
            //            }
            //            is_again_start = true;
            //        });
            //    }

            //    ShowNavButton?.Invoke("", new EventArgs());
            //}
            #endregion
        });
        public ICommand StopCommand => new Command(async (obj) =>
        {
            StartTime = false;
            play_button_enable = true;
            stop_button_enable = false;
        });
        #endregion
    }
}
