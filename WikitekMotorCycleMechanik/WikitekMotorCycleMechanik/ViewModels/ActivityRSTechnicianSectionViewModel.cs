using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Models1;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class ActivityRSTechnicianSectionViewModel : ViewModelBase
    {
        ApiServices1 apiServices;
        public ActivityRSTechnicianSectionViewModel(Page page, LoginResponse use) : base(page)
        {
            //InitializeCommands();
            apiServices = new ApiServices1();
            this.IsToggled = false;
            Initialization = Init();
        }
        Task Initialization { get; }

        private bool _IsToggled;
        public bool IsToggled
        {
            get => _IsToggled;
            set
            {
                _IsToggled = value;
                OnPropertyChanged("IsToggled");
            }
        }

        private string _status;
        public string status
        {
            get => _status;
            set
            {
                _status = value;
                OnPropertyChanged("status");
            }
        }

        private DateTime _fromdate;
        public DateTime fromdate
        {
            get => _fromdate;
            set
            {
                _fromdate = value;
                OnPropertyChanged("fromdate");
            }
        }

        private DateTime _todate;
        public DateTime todate
        {
            get => _todate;
            set
            {
                _todate = value;
                OnPropertyChanged("todate");
            }
        }

        private ObservableCollection<VechicleTechicianList> mVehicleTechnicians;
        public ObservableCollection<VechicleTechicianList> VehicleTechnicians
        {
            get { return mVehicleTechnicians; }
            set
            {
                mVehicleTechnicians = value;
                OnPropertyChanged(nameof(VehicleTechnicians));
            }
        }
        private ObservableCollection<VechicleTechicianList> mVehicleTechns;
        public ObservableCollection<VechicleTechicianList> VehicleTechn
        {
            get { return mVehicleTechns; }
            set
            {
                mVehicleTechns = value;
                OnPropertyChanged(nameof(VehicleTechn));
            }
        }


        private VechicleTechicianList mCurrentSelectedVehicleTechns;

        public VechicleTechicianList CurrentSelectedVehicleTechns
        {
            get { return mCurrentSelectedVehicleTechns; }
            set
            {
                mCurrentSelectedVehicleTechns = value;
                //OnPropertyChanged(nameof(CurrentSelectedVehicleTechnician));

                if (value != null)
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        try
                        {
                            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                            {
                                await Task.Delay(200);
                                await page.Navigation.PushAsync(new Views.ActivityDetail.ActivityDetail(mCurrentSelectedVehicleTechns));
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    });
                }
            }
        }


        public async Task Init()
        {
            try
            {
                string email = App.user.email;
                var msgs = await apiServices.GetSctiveRSTechnicianSectionLoad(email);
                VehicleTechnicians = new ObservableCollection<VechicleTechicianList>(msgs.results);
                if (VehicleTechnicians.Count > 0)
                {
                    this.status = VehicleTechnicians[0].status;
                    this.fromdate = VehicleTechnicians[0].from_date;
                    this.todate = VehicleTechnicians[0].to_date;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public void IsToggledchange(bool Istog)
        {
            this.IsToggled = Istog;
            if (this.IsToggled == true)
            {
                ObservableCollection<VechicleTechicianList> VehicleTechn = new ObservableCollection<VechicleTechicianList>();
                VechicleTechicianList obj = new VechicleTechicianList();
                obj.id = 12;
                obj.from_date = DateTime.Now;
                VehicleTechn.Add(obj);


                //VechicleTechicianList vehicleTechn = new VechicleTechicianList();
                //VechicleTechicianList obj = new VechicleTechicianList();
                //obj.id = 12;
                //obj.from_date = DateTime.Now;
                //VehicleTechn.Add(obj);

                StartTripModel startTripModel = new StartTripModel()
                {
                    associatevehicletechnician_id = 5,
                    start_date = DateTime.Now,
                    latlong = "103,104"
                };
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        var msg = await apiServices.StartTrip(startTripModel);
                        await page.DisplayAlert(msg.message, msg.status, "OK");
                    }
                    catch (Exception ex)
                    {

                    }
                });

            }
            else
            {
                var res = VehicleTechn.FirstOrDefault(e => e.id == 12);
                if (res != null)
                { 
                    res.to_date=DateTime.Now;
                }
                StopTripModel stopTripModel = new StopTripModel()
                {
                    start_trip_id = 2,
                    stop_date = DateTime.Now,
                };
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        var msg = await apiServices.StopTrip(stopTripModel);
                        await page.DisplayAlert(msg.message, msg.status, "OK");
                    }
                    catch (Exception ex)
                    {

                    }
                });

            }
        }
    }
}
