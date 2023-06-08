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
            SelectedTechnician = App.SelectedTechnician;
            // Initialization = Init();
        }

        private NewTechnicianList mSelectedTechnician;
        public NewTechnicianList SelectedTechnician
        {
            get { return mSelectedTechnician; }
            set
            {
                mSelectedTechnician = value;
                OnPropertyChanged(nameof(SelectedTechnician));
            }
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
        public async Task Init1()
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
                App.CurrentSelectedTechnician = CurrentSelectedTechnician;
                OnPropertyChanged(nameof(CurrentSelectedTechnician));
            }
        }

        private string mstartDate;
        public string startDate
        {
            get { return mstartDate; }
            set
            {
                mstartDate = value;
                OnPropertyChanged(nameof(startDate));
            }
        }

        private string mendDate;
        public string endDate
        {
            get { return mendDate; }
            set
            {
                mendDate = value;
                OnPropertyChanged(nameof(endDate));
            }
        }
        private string mstartTime;

        public string startTime
        {
            get { return mstartTime; }
            set
            {
                mstartTime = value;
                OnPropertyChanged(nameof(startTime));
            }
        }
        private string mendTime;

        public string endTime
        {
            get { return mendTime; }
            set
            {
                mendTime = value;
                OnPropertyChanged(nameof(endTime));
            }
        }
        private string mSelectedStartDate;
        public string SelectedStartDate
        {
            get { return mSelectedStartDate; }
            set
            {
                mSelectedStartDate = value;
                OnPropertyChanged(nameof(SelectedStartDate));
            }
        }

        private string mSelectedEndDate;
        public string SelectedEndDate
        {
            get { return mSelectedEndDate; }
            set
            {
                mSelectedEndDate = value;
                OnPropertyChanged(nameof(SelectedEndDate));
            }
        }


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
            set
            {
                motp2 = value; OnPropertyChanged(nameof(otp2));
            }
        }

        private string motp3;
        public string otp3
        {
            get { return motp3; }
            set
            {
                motp3 = value; OnPropertyChanged(nameof(otp3));
            }
        }

        private string motp4;
        public string otp4
        {
            get { return motp4; }
            set
            {
                motp4 = value; OnPropertyChanged(nameof(otp4));
            }
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
                    SentOtpAssignTechnician sentOtpVehicle = new SentOtpAssignTechnician()
                    {
                         associatevehicletechnician_id = App.associateVechicleId,
                        otp = otp1 + otp2 + otp3 + otp4
                    };

                    var msg = await apiServices.ConfirmVehicleTechnicianAssociateVehicle2(sentOtpVehicle);
                    await page.DisplayAlert(msg.status, msg.message, "OK");
                    otp1 = string.Empty;
                    otp2 = string.Empty;
                    otp3 = string.Empty;
                    otp4 = string.Empty;
                    await this.page.Navigation.PopToRootAsync();
                    //await this.page.Navigation.PushAsync(new Views.AssociateVehicleDetail.AssociateVehicleDetail());
                    //api/v1/workshops/associate-vehicle0099

                }
                catch (Exception ex)
                {
                }
            });
            //totomanoj CurrentSelectedTechnician
            GetOtpCommand1 = new Command(async (obj) =>
            {
                try
                {
                    json = Preferences.Get("LoginResponse", null);
                    LoginResponse login = JsonSerializer.Deserialize<LoginResponse>(json);
                    AssignTechnicianVehicleModel assignTechnicianVehicleModel = new AssignTechnicianVehicleModel();
                    assignTechnicianVehicleModel.associate_technician_id = App.SelectedTechnician.email;
                    assignTechnicianVehicleModel.associate_vehicle_id = App.associateVechicleId;
                    assignTechnicianVehicleModel.user_id = login.user_id;// "fafbbd01-f6ef-4763-be67-a8285c494fce";//App.user.user_id;
                    assignTechnicianVehicleModel.start_date = startDate;
                    assignTechnicianVehicleModel.end_date = endDate;

                    var msg = await apiServices.AssignTechnician(assignTechnicianVehicleModel);
                    App.associateVechicleId = msg.id;
                    await page.DisplayAlert(msg.status, msg.message, "OK");

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
                    AssignTechnicianVehicleModel1 assignTechnicianVehicleModel = new AssignTechnicianVehicleModel1();
                    assignTechnicianVehicleModel.associate_technician_id = App.SelectedTechnician.email;
                  
                   //get otp todo
                    assignTechnicianVehicleModel.associate_vehicle_id = App.CurrentSelectedTechnician.registration_id;
                    assignTechnicianVehicleModel.user_id = login.user_id;// "fafbbd01-f6ef-4763-be67-a8285c494fce";//App.user.user_id;
                    assignTechnicianVehicleModel.start_date = SelectedStartDate;
                    assignTechnicianVehicleModel.end_date = SelectedStartDate;

                    var msg = await apiServices.AssignTechnician1(assignTechnicianVehicleModel);
                    App.associateVechicleId = msg.id;
                    await page.DisplayAlert(msg.status, msg.message, "OK");

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
        public ICommand GetOtpCommand1 { get; set; }

        public ICommand DisassociateVehicleCommand { get; set; }

        #endregion
    }
}
