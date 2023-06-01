using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class DPPackViewModel : BaseViewModel
    {
        public DPPackViewModel(String vehicle)
        {
            switch (vehicle)
            {
                case "bike":
                    this.vehicle = "Two Wheeler";
                    break;
                case "rikshaw":
                    this.vehicle = "Three Wheeler";
                    break;
                case "car":
                    this.vehicle = "Passenger Vehicle";
                    break;
                case "truck":
                    this.vehicle = "Commercial Vehicle";
                    break;
            }
            message = $"No subscription for {this.vehicle} Diagnostic Pack";
            ContinueCommand = new Command(async () =>
            {
                MessagingCenter.Send<DPPackViewModel>(this, "NavigateToOBD2");
                await PopupNavigation.Instance.PopAsync();
            });
            PurchaseCommand = new Command(async () =>
            {
                MessagingCenter.Send<DPPackViewModel>(this, "NavigateToMarketplace");
                await PopupNavigation.Instance.PopAsync();
            });
            PackHelpCommand = new Command(async () =>
            {
                MessagingCenter.Send<DPPackViewModel>(this, "NavigateToPackHelp");
                await PopupNavigation.Instance.PopAsync();
            });
        }

        private string _message;
        public string message
        {
            get => _message;
            set
            {
                _message = value;
                OnPropertyChanged("message");
            }
        }

        private string _vehicle;
        public string vehicle
        {
            get => _vehicle;
            set
            {
                _vehicle = value;
                OnPropertyChanged("vehicle");
            }
        }


        public ICommand ContinueCommand { get; set; }
        public ICommand PurchaseCommand { get; set; }
        public ICommand PackHelpCommand { get; set; }
    }
}
