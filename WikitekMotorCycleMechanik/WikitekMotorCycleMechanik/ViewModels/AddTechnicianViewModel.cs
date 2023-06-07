using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Models1;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.Views.AssociateVehicle;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class AddTechnicianViewModel : ViewModelBase
    {
        ApiServices1 apiServices;
        public AddTechnicianViewModel(Page page, LoginResponse use) : base(page)
        {
            InitializeCommands();
            apiServices = new ApiServices1();
        }

        public async Task Init()
        {
            try
            {
                var msgs = await apiServices.TechnicianList();
                Technicians = new ObservableCollection<NewTechnicianList>(msgs.results.Take(50));
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private NewTechnicianList mCurrentSelectedTechnician;

        public NewTechnicianList CurrentSelectedTechnician
        {
            get { return mCurrentSelectedTechnician; }
            set
            {
                mCurrentSelectedTechnician = value;
                //  this.TechnicianId = value.id;
                OnPropertyChanged(nameof(CurrentSelectedTechnician));
            }
        }

        private ObservableCollection<NewTechnicianList> mTechnicians;
        public ObservableCollection<NewTechnicianList> Technicians
        {
            get { return mTechnicians; }
            set
            {
                mTechnicians = value;
                OnPropertyChanged(nameof(Technicians));
            }
        }
        private string mTechnicianId;// = "fafbbd01-f6ef-4763-be67-a8285c494fce";
        public string TechnicianId
        {
            get { return mTechnicianId; }
            set
            {
                mTechnicianId = value;
                OnPropertyChanged(nameof(TechnicianId));
            }
        }

        private string motp1;
        public string otp1
        {
            get { return motp1; }
            set { motp1 = value; }
        }

        private string motp2;
        public string otp2
        {
            get { return motp2; }
            set { motp2 = value; }
        }

        private string motp3;
        public string otp3
        {
            get { return motp3; }
            set { motp3 = value; }
        }

        private string motp4;
        public string otp4
        {
            get { return motp4; }
            set { motp4 = value; }
        }

        #region Methods
        string json;
        public void InitializeCommands()
        {
            SendOTPCommand = new Command(async (obj) =>
            {
                try
                {
                    // var id = Preferences.Get("associatevehicle", null);
                    SentOtpTechnician sentOtpVehicle = new SentOtpTechnician()
                    {

                        associatetechnician_id = App.associateVechicleId,
                        otp = otp1 + otp2 + otp3 + otp4
                    };

                    var msg = await apiServices.ConfirmTechnicianAssociateVehicle(sentOtpVehicle);
                    await page.DisplayAlert("Success!", msg.message, "OK");

                    await this.page.Navigation.PushAsync(new Views.TechnicianUserDetail.TechnicianUserDetail());
                    //api/v1/workshops/associate-vehicle0099

                }
                catch (Exception ex)
                {
                }
            });

            GetOtpCommand = new Command(async (obj) =>
            {
                try
                {
                    NewTechnicianList idd;
                    idd = this.Technicians.Where(x => x.email == TechnicianId).FirstOrDefault();
                    if (idd == null)
                    {
                        idd = this.Technicians.Where(x => x.mobile == TechnicianId).FirstOrDefault();
                        if (idd == null)
                        {
                            idd = this.Technicians.Where(x => x.email == TechnicianId).FirstOrDefault();

                        }
                    }


                    if (idd != null)
                    {
                        App.SelectedTechnician = idd;

                        json = Preferences.Get("LoginResponse", null);
                        LoginResponse login = JsonSerializer.Deserialize<LoginResponse>(json);

                        TechnicianvehicleModel technicianvehicleModel = new TechnicianvehicleModel();
                        technicianvehicleModel.user_id = login.user_id;// "fafbbd01-f6ef-4763-be67-a8285c494fce";//App.user.user_id;
                        technicianvehicleModel.technician_id = idd.id;
                        technicianvehicleModel.workshop_id = login.agent.workshop.id;
                        var resp = await apiServices.AddTechnician(technicianvehicleModel);
                        App.associateVechicleId = resp.id;

                        await page.DisplayAlert("", resp.message, "OK");
                    }
                    else
                    {
                        await page.DisplayAlert("", "Technician Not found!", "OK");
                    }


                    //api/v1/workshops/associate-vehicle0099

                }
                catch (Exception ex)
                {
                }
            });
        }
        #endregion
        #region ICommands
        public ICommand SendOTPCommand { get; set; }
        public ICommand GetOtpCommand { get; set; }
        #endregion
    }
}
