using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Models1;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.Views.Popup;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class AssociateVehicleViewModel : ViewModelBase
    {
        ApiServices1 apiServices;
        public AssociateVehicleViewModel(Page page, LoginResponse use) : base(page)
        {
            InitializeCommands();
            apiServices = new ApiServices1();

            OTPOpenPanel = false;
            OpenButton = false;
        }

        private string mAssociateVehicleId = "MP094321";
        public string AssociateVehicleId
        {
            get { return mAssociateVehicleId; }
            set
            {
                mAssociateVehicleId = value;
                OnPropertyChanged(nameof(AssociateVehicleId));
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
        private bool mOTPOpenPanel;

        public bool OTPOpenPanel
        {
            get { return mOTPOpenPanel; }
            set
            {
                mOTPOpenPanel = value;
                OnPropertyChanged(nameof(OTPOpenPanel));
            }
        }

        private bool mOpenButton;

        public bool OpenButton
        {
            get { return mOpenButton; }
            set
            {
                mOpenButton = value;
                OnPropertyChanged(nameof(OpenButton));
            }
        }

        #region Methods
        string json;
        public void InitializeCommands()
        {
            SendOTPCommand = new Command(async (obj) =>
            {
                try
                {
                    try
                    {
                        // var id = Preferences.Get("associatevehicle", null);
                        SentOtpVehicle sentOtpVehicle = new SentOtpVehicle()
                        {
                            associatevehicle_id = App.associateVechicleId,
                            otp = otp1 + otp2 + otp3 + otp4
                        };

                        var msg = await apiServices.ConfirmAssociateVehicle(sentOtpVehicle);
                        await page.DisplayAlert("Success!", msg.message, "OK");

                        await this.page.Navigation.PushAsync(new Views.AssociateVehicleDetail.AssociateVehicleDetail());
                        //api/v1/workshops/associate-vehicle0099

                    }
                    catch (Exception ex)
                    {
                    }

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

                    AssociatevehicleModel associatevehicleModel = new AssociatevehicleModel();
                    associatevehicleModel.user_id = login.user_id;// "fafbbd01-f6ef-4763-be67-a8285c494fce";//App.user.user_id;
                    associatevehicleModel.vehicle_id = AssociateVehicleId;
                    associatevehicleModel.workshop_id = login.agent.workshop.id;
                    var resp = await apiServices.AssociateVehicle(associatevehicleModel);
                    await page.DisplayAlert("", resp?.message, "OK");

                    App.associateVechicleId = resp.id;
                    if (resp.id < 1)
                    {
                        OTPOpenPanel = false;
                        OpenButton = true;
                    }
                    else
                    {
                        OTPOpenPanel = true;
                        OpenButton = false;
                    }
                    //api/v1/workshops/associate-vehicle0099

                }
                catch (Exception ex)
                {
                }
            });

            DisassociateVehicleCommand = new Command(async (obj) =>
            {
                try
                {
                    await this.page.Navigation.PushAsync(new Views.DiassociateVehicle.DiassociateVehicle());

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
        public ICommand DisassociateVehicleCommand { get; set; }


        #endregion
    }
}
