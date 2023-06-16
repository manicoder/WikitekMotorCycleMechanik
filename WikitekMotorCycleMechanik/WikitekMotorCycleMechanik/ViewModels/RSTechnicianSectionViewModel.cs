using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Text;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Models1;
using WikitekMotorCycleMechanik.Views.Otp;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class RSTechnicianSectionViewModel : ViewModelBase
    {
        ApiServices1 apiServices;
        public RSTechnicianSectionViewModel(Page page, LoginResponse use) : base(page)
        {
            InitializeCommands();
            apiServices = new ApiServices1();
            isVisibleButton = false;
            OTPOpenPanel = false;
            var selected_jobcard = App.selected_jobcard;
            if (selected_jobcard != null)
            {
                if (selected_jobcard.jobcard_pickupdrop.Count > 0)
                {
                    if (selected_jobcard.jobcard_pickupdrop[0].status == "Pickup-Open")
                    {
                        buttonText = "Start for Pickup";
                    }
                    else if (selected_jobcard.jobcard_pickupdrop[0].status == "Pickup_Closed")
                    {
                        buttonText = "Close Pickup";
                    }
                    else if (selected_jobcard.jobcard_pickupdrop[0].status == "Pickup-ReachedCustomer")
                    {
                        buttonText = "Reached Customer";
                    }
                    else if (selected_jobcard.jobcard_pickupdrop[0].status == "Pickup_VehPickedUp")
                    {
                        buttonText = "Vechicle Picked up";
                    }
                    if (selected_jobcard.jobcard_pickupdrop[0].status == "Pickup_VehPickedUp")
                    {
                        OTPOpenPanel = true;
                    }
                    else
                    {
                        OTPOpenPanel = false;
                    }


                    status = selected_jobcard.jobcard_pickupdrop[0].status;
                    id = selected_jobcard.jobcard_pickupdrop[0].id;
                    isVisibleButton = true;
                }
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

        private string mbuttonText;
        public string buttonText
        {
            get { return mbuttonText; }
            set { mbuttonText = value; OnPropertyChanged(nameof(buttonText)); }
        }

        private bool misVisibleButton;
        public bool isVisibleButton
        {
            get { return misVisibleButton; }
            set { misVisibleButton = value;
                OnPropertyChanged(nameof(isVisibleButton)); }
        }

        private string mid;
        public string id
        {
            get { return mid; }
            set { mid = value; OnPropertyChanged(nameof(id)); }
        }

        private string mstatus;
        public string status
        {
            get { return mstatus; }
            set { mstatus = value; OnPropertyChanged(nameof(status)); }
        }

        public void InitializeCommands()
        {
            PickUpCommand = new Command(async (obj) =>
            {
                try
                {
                    PickupModel model = new PickupModel();
                    model.id = id;
                    if (status == "Pickup-Open")
                    {
                        PickupStartedModel model1 = new PickupStartedModel();
                        model1.id = id;
                        model1.status = status;
                        var msg = await apiServices.PickupStarted(model1);
                        await page.DisplayAlert(msg.message, msg.status, "OK");
                    }
                    else if (status == "Pickup-ReachedCustomer")
                    {
                        var msg = await apiServices.PickupReachedcustomer(model);
                        await page.DisplayAlert(msg.message, msg.status, "OK");
                    }
                    else if (status == "Pickup_VehPickedUp")
                    {
                        model.otp = otp1 + otp2 + otp3 + otp4;
                        var msg = await apiServices.PickupVehicle(model);
                        await page.DisplayAlert(msg.message, msg.status, "OK");
                        otp1 = string.Empty;
                        otp2 = string.Empty;
                        otp3 = string.Empty;
                        otp4 = string.Empty;
                    }
                    else if (status == "Pickup_Closed")
                    {
                        var msg = await apiServices.PickupClose(model);
                        await page.DisplayAlert(msg.message, msg.status, "OK");
                    }
                    //await this.page.Navigation.PushAsync(new Views.AssociateVehicleDetail.AssociateVehicleDetail());

                }
                catch (Exception ex)
                {
                }
            });
        }
        #region ICommands
        public ICommand PickUpCommand { get; set; }
        #endregion
    }
}

