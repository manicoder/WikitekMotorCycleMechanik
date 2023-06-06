using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
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
        private string mTechnicianId = "fafbbd01-f6ef-4763-be67-a8285c494fce";
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
                    SentOtpVehicle sentOtpVehicle = new SentOtpVehicle()
                    {
                        associatetechnician_id = 1,
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
                    json = Preferences.Get("LoginResponse", null);
                    LoginResponse login = JsonSerializer.Deserialize<LoginResponse>(json);

                    TechnicianvehicleModel technicianvehicleModel = new TechnicianvehicleModel();
                    technicianvehicleModel.user_id = login.user_id;// "fafbbd01-f6ef-4763-be67-a8285c494fce";//App.user.user_id;
                    technicianvehicleModel.technician_id = TechnicianId;
                    technicianvehicleModel.workshop_id = login.agent.workshop.id;
                    var resp = await apiServices.AddTechnician(technicianvehicleModel);
                    App.associateVechicleId = resp.id;
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
