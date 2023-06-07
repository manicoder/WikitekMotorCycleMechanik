using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Models1;
using WikitekMotorCycleMechanik.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class DiassociateVehicleViewModel : ViewModelBase
    {
        ApiServices1 apiServices;
        public DiassociateVehicleViewModel(Page page, LoginResponse use) : base(page)
        {
            InitializeCommands();
            apiServices = new ApiServices1();
            Initialization = Init();
        }

        Task Initialization { get; }
        private string motp1;
        public string otp1
        {
            get { return motp1; }
            set
            {
                motp1 = value;
                OnPropertyChanged(nameof(otp1));
            }
        }

        private string motp2;
        public string otp2
        {
            get { return motp2; }
            set { motp2 = value; OnPropertyChanged(nameof(otp2)); }
        }

        private string motp3;
        public string otp3
        {
            get { return motp3; }
            set { motp3 = value; OnPropertyChanged(nameof(otp3)); }
        }

        private string motp4;
        public string otp4
        {
            get { return motp4; }
            set { motp4 = value; OnPropertyChanged(nameof(otp4)); }
        }





        private ObservableCollection<DisVechicleList> mVechicles;
        public ObservableCollection<DisVechicleList> Vehicles
        {
            get { return mVechicles; }
            set
            {
                mVechicles = value;
                OnPropertyChanged(nameof(Vehicles));
            }
        }

        private DisVechicleList mCurrentSelectedVehicle;

        public DisVechicleList CurrentSelectedVehicle
        {
            get { return mCurrentSelectedVehicle; }
            set
            {
                mCurrentSelectedVehicle = value;
                OnPropertyChanged(nameof(CurrentSelectedVehicle));
            }
        }




        public async Task Init()
        {
            try
            {
                var msgs = await apiServices.DisVehicleList();

                Vehicles = new ObservableCollection<DisVechicleList>(msgs.results);

            }
            catch (Exception ex)
            {

                throw;
            }
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


        #region Methods
        string json;
        public void InitializeCommands()
        {
            SendOTPCommand = new Command(async (obj) =>
            {
                try
                {
                    //var id = Preferences.Get("associatevehicle", null);
                    SentOtpVehicle sentOtpVehicle = new SentOtpVehicle()
                    {
                        associatevehicle_id = CurrentSelectedVehicle.id,
                        otp = otp1 + otp2 + otp3 + otp4
                    };

                    var msg = await apiServices.ConfirmDeAssociateVehicle(sentOtpVehicle);
                    await page.DisplayAlert(msg.message, msg.status, "OK");
                    otp1 = string.Empty;
                    otp2 = string.Empty;
                    otp3 = string.Empty;
                    otp4 = string.Empty;

                    if (msg.success)
                    {
                        await this.page.Navigation.PopToRootAsync();
                    }

                    //await this.page.Navigation.PushAsync(new Views.AssociateVehicleDetail.AssociateVehicleDetail());
                    ////api/v1/workshops/associate-vehicle0099

                }
                catch (Exception ex)
                {
                }
            });

            GetOtpCommand = new Command(async (obj) =>
            {
                try
                {
                    //var id = Preferences.Get("associatevehicle", null);
                    DeAssociatevehicleModel deAssociatevehicleModel = new DeAssociatevehicleModel()
                    {
                        associatevehicle_id = CurrentSelectedVehicle.id,
                    };

                    var msg = await apiServices.VechicleDeAssociate(deAssociatevehicleModel);
                    await page.DisplayAlert(msg.message, msg.status, "OK");

                    if (msg.success)
                    {
                        OTPOpenPanel = true;
                    }
                    //await this.page.Navigation.PushAsync(new Views.AssociateVehicleDetail.AssociateVehicleDetail());
                    ////api/v1/workshops/associate-vehicle0099
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
