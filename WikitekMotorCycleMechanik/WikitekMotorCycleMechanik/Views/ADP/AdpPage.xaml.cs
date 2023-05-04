using MultiEventController.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.ADP
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdpPage : TabbedPage
    {
        string dtc_code;
        int model_id;
        int submodel_id;
        public AdpPage(string dtc_code, int model_id, int submodel_id)
        {
            InitializeComponent();
            this.dtc_code = dtc_code;
            this.model_id = model_id;
            this.submodel_id = submodel_id;
            //Title = dtc_code;
            //Children.Add(new View.GdSection.InfoPage(dtc_code, model_id, submodel_id));
            //Children.Add(new Views.Colleborates.ColleboratePage(dtc_code) { Title = "Collaborate" });
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Title = dtc_code;
            App.controlEventManager.OnRecieved += ControlEventManager_OnRecieved;
            App.controlEventManager.OnRecievedData += ControlEventManager_OnRecievedData;
            Children.Clear();
            if (CurrentUserEvent.Instance.IsRemote)
            {
                Children.Add(new View.GdSection.InfoPage(dtc_code, model_id, submodel_id));
            }
            else
            {
                Children.Add(new View.GdSection.InfoPage(dtc_code, model_id, submodel_id));
                Children.Add(new Views.Colleborates.ColleboratePage(dtc_code) { Title = "Collaborate" });
            }

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            App.controlEventManager.OnRecieved -= ControlEventManager_OnRecieved;
            App.controlEventManager.OnRecievedData -= ControlEventManager_OnRecievedData;

            if (CurrentUserEvent.Instance.IsExpert)
            {
                App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                {
                    ElementName = "GoBackTP",
                    ElementValue = "GoBackTP",
                    ToUserId = CurrentUserEvent.Instance.ToUserId,
                    IsExpert = CurrentUserEvent.Instance.IsExpert
                });
            }
        }

        private void ControlEventManager_OnRecievedData(object sender, EventArgs e)
        {
            if (CurrentUserEvent.Instance.IsRemote && CurrentUserEvent.Instance.IsExpert)
            {
                //await Task.Delay(100);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    string data = (string)sender; //sender as string;
                    if (!string.IsNullOrEmpty(data))
                    {
                        string[] PairedData = new string[2];
                        if (data.Contains("SendStaticDataRD#"))
                        {
                            PairedData = data.Split('#');
                            //StaticMethods.ecu_info = JsonConvert.DeserializeObject<List<EcuDataSet>>(PairedData[1]);
                            //var DTCFound = new Popup.DisplayAlertPage("Remote Session started", "", "OK");
                            //await PopupNavigation.Instance.PushAsync(DTCFound);
                            //Acr.UserDialogs.UserDialogs.Instance.Toast("Static Data Recieved", new TimeSpan(0, 0, 0, 3));
                        }
                    }
                });
            }
        }

        private async void ControlEventManager_OnRecieved(object sender, EventArgs e)
        {
            string ReceiveValue = string.Empty;
            var elementEventHandler = (sender as ElementEventHandler);
            ReceiveValue = elementEventHandler.ElementValue;
            if (!CurrentUserEvent.Instance.IsExpert && ReceiveValue.Contains("GoBackTP"))
            {
                await this.Navigation.PopAsync();
            }
            else if (!CurrentUserEvent.Instance.IsExpert && elementEventHandler.ElementValue == "DTCBtnClicked")
            {
                //DependencyService.Get<ILodingPageService>().IsExpertShow_Active();
                //await Navigation.PushAsync(new Views.DtcList.DtcListPage());
                //App.PageFreezIsEnable = true;
                //DependencyService.Get<ILodingPageService>().HideLoadingPage();
            }
            //DependencyService.Get<ILodingPageService>().HideLoadingPage();
            App.controlEventManager.RecieveCallControlEvents(this, elementEventHandler, CurrentUserEvent.Instance.OwnerUserId);
        }

    }
}