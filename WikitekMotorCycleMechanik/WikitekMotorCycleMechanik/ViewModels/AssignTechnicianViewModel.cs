﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Models1;
using WikitekMotorCycleMechanik.Views.AssociateVehicle;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class AssignTechnicianViewModel : ViewModelBase
    {
        ApiServices1 apiServices;
        public AssignTechnicianViewModel(Page page, LoginResponse use) : base(page)
        {
            InitializeCommands();
            apiServices = new ApiServices1();
            // Initialization = Init();
        }

        //   Task Initialization { get; }
        public async Task Init()
        {
            try
            {
                var json = Preferences.Get("LoginResponse", null);
                LoginResponse login = JsonSerializer.Deserialize<LoginResponse>(json);
                int WorkShopId = login.agent.workshop.id;
                var msgs = await apiServices.AssignTechnicianList(WorkShopId);
                Technicians = new ObservableCollection<AssignTechnicianItem>(msgs.results); 
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        private ObservableCollection<AssignTechnicianItem> mTechnicians;
        public ObservableCollection<AssignTechnicianItem> Technicians
        {
            get { return mTechnicians; }
            set
            {
                mTechnicians = value;
                OnPropertyChanged(nameof(Technicians));
            }
        }

        private AssignTechnicianItem mCurrentSelectedTechnician;

        public AssignTechnicianItem CurrentSelectedTechnician
        {
            get { return mCurrentSelectedTechnician; }
            set
            {
                mCurrentSelectedTechnician = value;
                OnPropertyChanged(nameof(CurrentSelectedTechnician));
            }
        }

        private string mstartDate;
        public string startDate
        {
            get { return mstartDate; }
            set { mstartDate = value;
                OnPropertyChanged(nameof(startDate));
            }
        }

        private string mendDate;
        public string endDate
        {
            get { return mendDate; }
            set { mendDate = value;
                OnPropertyChanged(nameof(endDate));
            }
        }
        private string mstartTime;

        public string startTime
        {
            get { return mstartTime; }
            set { mstartTime = value;
                OnPropertyChanged(nameof(startTime));
            }
        }
        private string mendTime;

        public string endTime
        {
            get { return mendTime; }
            set { mendTime = value;
                OnPropertyChanged(nameof(endTime));
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
            if (string.IsNullOrEmpty(startDate))
            {
                startDate = DateTime.Now.ToString();
            }
            if (string.IsNullOrEmpty(endDate))
            {
                endDate = DateTime.Now.ToString();
            }
            SendOTPCommand = new Command(async (obj) =>
            {
                try
                {
                    //var id = Preferences.Get("associatevehicle", null);
                    SentOtpVehicle sentOtpVehicle = new SentOtpVehicle()
                    {
                        // associatevehicletechnician_id = 2,
                        otp = otp1 + otp2 + otp3 + otp4
                    };

                    var msg = await apiServices.ConfirmVehicleTechnicianAssociateVehicle(sentOtpVehicle);
                    await page.DisplayAlert("Success!", msg.message, "OK");
                    otp1 = string.Empty;
                    otp2 = string.Empty;
                    otp3 = string.Empty;
                    otp4 = string.Empty;

                    //await this.page.Navigation.PushAsync(new Views.AssociateVehicleDetail.AssociateVehicleDetail());
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
                    AssignTechnicianVehicleModel assignTechnicianVehicleModel = new AssignTechnicianVehicleModel();
                    assignTechnicianVehicleModel.associate_technician_id = App.SelectedTechnician.id;
                    assignTechnicianVehicleModel.associate_vehicle_id = App.associateVechicleId;
                    assignTechnicianVehicleModel.user_id = login.user_id;// "fafbbd01-f6ef-4763-be67-a8285c494fce";//App.user.user_id;
                    assignTechnicianVehicleModel.start_date = startDate;
                    assignTechnicianVehicleModel.end_date = endDate;

                    var resp = await apiServices.AssignTechnician(assignTechnicianVehicleModel);
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
        public ICommand DisassociateVehicleCommand { get; set; }

        #endregion
    }
}
