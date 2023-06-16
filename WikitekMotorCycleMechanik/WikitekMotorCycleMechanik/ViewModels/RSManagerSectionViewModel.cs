using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Models1;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class RSManagerSectionViewModel : ViewModelBase
    {
        ApiServices1 apiServices;
        public RSManagerSectionViewModel(Page page, LoginResponse use) : base(page)
        {
            InitializeCommands();
            apiServices = new ApiServices1();
            Initialization = Init();
            var selected_jobcard = App.selected_jobcard;
            VehicleDropDownIsVisible = false;
            QuotationIsVisible = false;
            if (selected_jobcard != null)
            {
                if (selected_jobcard.status == "QuoteforTransport")
                {
                    buttonText = "Quote Transport";
                    VehicleDropDownIsVisible = false;
                    QuotationIsVisible = true;
                }
                else if (selected_jobcard.status == "ApprovedTransport")
                {
                    VehicleDropDownIsVisible = true;
                    QuotationIsVisible = false;
                    buttonText = "Arrange Transport";
                }
                status = selected_jobcard.status;
                id = selected_jobcard.id;
            }
        }
        Task Initialization { get; }
        private string mid;
        public string id
        {
            get { return mid; }
            set { mid = value; OnPropertyChanged(nameof(id)); }
        }

        private string _Quotation;
        public string Quotation
        {
            get { return _Quotation; }
            set { _Quotation = value; OnPropertyChanged(nameof(Quotation)); }
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

        private bool _VehicleDropDownIsVisible;
        public bool VehicleDropDownIsVisible
        {
            get { return _VehicleDropDownIsVisible; }
            set { _VehicleDropDownIsVisible = value; OnPropertyChanged(nameof(VehicleDropDownIsVisible)); }
        }

        private bool _QuotationIsVisible;
        public bool QuotationIsVisible
        {
            get { return _QuotationIsVisible; }
            set { _QuotationIsVisible = value; OnPropertyChanged(nameof(QuotationIsVisible)); }
        }


        private string mbuttonText;
        public string buttonText
        {
            get { return mbuttonText; }
            set { mbuttonText = value; OnPropertyChanged(nameof(buttonText)); }
        }
        private string mstatus;
        public string status
        {
            get { return mstatus; }
            set { mstatus = value; OnPropertyChanged(nameof(status)); }
        }
        public async Task Init()
        {
            try
            {
                int Workshopid = App.workshopid;
                var msgs = await apiServices.AvailableVehicleList(Workshopid);
                Vehicles = new ObservableCollection<DisVechicleList>(msgs.results);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public void InitializeCommands()
        {
            PickUpCommand = new Command(async (obj) =>
            {
                try
                {
                    PickupModel model = new PickupModel();
                    model.id = id;
                    if (status == "QuoteforTransport")
                    {
                        var msg = await apiServices.QuotedForTransport(model);
                        await page.DisplayAlert(msg.message, msg.status, "OK");
                        Quotation = string.Empty;
                    }
                    else if (status == "ApprovedTransport")
                    {
                        model.registration_no = CurrentSelectedVehicle.registration_id;
                        var msg = await apiServices.ArrangeTranport(model);
                        await page.DisplayAlert(msg.message, msg.status, "OK");
                        CurrentSelectedVehicle = null;
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
