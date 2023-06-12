using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Models1;
using WikitekMotorCycleMechanik.Services;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class ActvityDetailViewModel : ViewModelBase
    {
        ApiServices1 apiServices;
        VechicleTechicianList selected_vehicletechnician;
        public ActvityDetailViewModel(Page page, VechicleTechicianList selected_vehicleTechnician) : base(page)
        {
            try
            {
                apiServices = new ApiServices1();
                this.selected_vehicletechnician = selected_vehicleTechnician;
                this.technician = this.selected_vehicletechnician.vehicle_association.registration_id;
                this.fromdate = this.selected_vehicletechnician.from_date;
                this.todate = this.selected_vehicletechnician.to_date;
                this.vin = this.selected_vehicletechnician.vehicle_association.vin;
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        await GetVechicleTechnicianAssociate(this.selected_vehicletechnician.id);
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
        private VechicleTechicianList mCurrentSelectedVehicleTechnicians;

        public VechicleTechicianList CurrentSelectedVehicleTechnician
        {
            get { return mCurrentSelectedVehicleTechnicians; }
            set
            {
                mCurrentSelectedVehicleTechnicians = value;
                OnPropertyChanged(nameof(CurrentSelectedVehicleTechnician));

                //if (value != null)
                //{
                //    Device.BeginInvokeOnMainThread(async () =>
                //    {
                //        try
                //        {
                //            using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                //            {
                //                await Task.Delay(200);
                //                await page.Navigation.PushAsync(new Views.ActivityDetail.ActivityDetail(mCurrentSelectedVehicleTechnicians));
                //            }
                //        }
                //        catch (Exception ex)
                //        {

                //        }
                //    });
                //}
            }
        }

        public async Task GetVechicleTechnicianAssociate(int vechicletechnicianassociateid)
        {
            var msgs = await apiServices.GetVechicleTechnicianAssociateDetail(vechicletechnicianassociateid);
            VehicleTechnicians = new ObservableCollection<VechicleTechicianList>(msgs.results);
        }

        private string _technician;
        public string technician
        {
            get => _technician;
            set
            {
                _technician = value;
                OnPropertyChanged("technician");
            }
        }

        private string _vin;
        public string vin
        {
            get => _vin;
            set
            {
                _vin = value;
                OnPropertyChanged("vin");
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
    }
}
