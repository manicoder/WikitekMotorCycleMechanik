﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Models1;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class DiassociateTechnicianViewModel : ViewModelBase
    {
        ApiServices1 apiServices;
        public DiassociateTechnicianViewModel(Page page, LoginResponse use) : base(page)
        {
            InitializeCommands();
            apiServices = new ApiServices1();
            Initialization = Init();
        }
        Task Initialization { get; }
        public async Task Init()
        {
            try
            {
                var msgs = await apiServices.TechnicianList();
                Technicians = new ObservableCollection<NewTechnicianList>(msgs.results.Take(100));
            }
            catch (Exception ex)
            {

                throw;
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

        private NewTechnicianList mCurrentSelectedTechnician;

        public NewTechnicianList CurrentSelectedTechnician
        {
            get { return mCurrentSelectedTechnician; }
            set
            {
                mCurrentSelectedTechnician = value;
                OnPropertyChanged(nameof(CurrentSelectedTechnician));
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
                    //var id = Preferences.Get("associatevehicle", null);
                    SentOtpVehicle sentOtpVehicle = new SentOtpVehicle()
                    {
                        associatevehicle_id = 3,
                        otp = otp1 + otp2 + otp3 + otp4
                    };

                    var msg = await apiServices.ConfirmDeAssociateTechnician(sentOtpVehicle);
                    await page.DisplayAlert("Success!", msg.message, "OK");
                    otp1 = string.Empty;
                    otp2 = string.Empty;
                    otp3 = string.Empty;
                    otp4 = string.Empty;

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
                        associatevehicle_id = 3,
                    };

                    var msg = await apiServices.Techniciandisassociate(deAssociatevehicleModel);
                    await page.DisplayAlert("Success!", msg.message, "OK");

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