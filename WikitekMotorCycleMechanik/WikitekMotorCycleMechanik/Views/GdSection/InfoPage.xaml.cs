using Acr.UserDialogs;
using WikitekMotorCycleMechanik.Models;
using MultiEventController.Models;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WikitekMotorCycleMechanik.Services;
using Rg.Plugins.Popup.Services;

namespace WikitekMotorCycleMechanik.View.GdSection
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    #region Old Code
    public partial class InfoPage : ContentPage
    {
        ApiServices services;
        List<ResultGD> gd_data = new List<ResultGD>();
        //List<GdImage> gd_image = new List<GdImage>();
        bool IsPageRemove = false;
        //public InfoPage(List<ResultGD> gd_data)
      
        string dtc_code = string.Empty; 
        int model_id = 0; 
        int submodel_id = 0;
        
        public InfoPage(string dtc_code, int model_id, int submodel_id)
        {
            try
            {
                InitializeComponent();
                this.dtc_code = dtc_code;
                this.model_id = model_id;
                this.submodel_id = submodel_id;
                services = new ApiServices();
                GetGD();
                //this.gd_data = gd_data;
                //this.firstListClass = firstListClass;
                //UserId = "M387190";
                //ChessisId = "123456";

                //trees = firstListClass.tree;
            }
            catch (Exception ex)
            {
            }
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            try
            {
                App.controlEventManager.OnRecievedData += ControlEventManager_OnRecievedData;
                App.controlEventManager.OnRecieved += ControlEventManager_OnRecieved;
                IsPageRemove = true;
            }
            catch (Exception ex)
            {

            }
        }

        public async Task GetGD()
        {
            try
            {
                var res = await services.Get_gd(Xamarin.Essentials.Preferences.Get("token", null), dtc_code, submodel_id);
                if (res != null)
                {
                    if (res.results.Count() != 0)
                    {
                        this.gd_data = res.results;
                        Title = "Guided Diagnostics";// gd_data.FirstOrDefault().gd_id;
                        txtDescription.Text = gd_data.FirstOrDefault().gd_description;
                        txtCauses.Text = gd_data.FirstOrDefault().causes;
                        txtRemAction.Text = gd_data.FirstOrDefault().remedial_actions;
                        txtEffOnVehicle.Text = gd_data.FirstOrDefault().effects_on_vehicle;
                    }
                    else
                    {
                        //await this.DisplayAlert("", "JD Not Found for this DTC", "OK");

                        if (!CurrentUserEvent.Instance.IsExpert)
                        {
                            App.controlEventManager.SendRequestData("JDNotFound_");
                            await Task.Delay(200);
                        }

                        var errorpage = new Views.Popup.DisplayAlertPage("", "Gd not found for this DTC", "OK");
                        await PopupNavigation.Instance.PushAsync(errorpage);
                    }
                }
                else
                {
                    if (!CurrentUserEvent.Instance.IsExpert)
                    {
                        App.controlEventManager.SendRequestData("JDNotFound_");
                        await Task.Delay(200);
                    }

                    var errorpage = new Views.Popup.DisplayAlertPage("", "Gd not found for this DTC", "OK");
                    await PopupNavigation.Instance.PushAsync(errorpage);
                }
            }
            catch (Exception ex)
            {
            }
        }


        private void ControlEventManager_OnRecievedData  (object sender, EventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                #region Check Internet Connection
                if (CurrentUserEvent.Instance.IsRemote && CurrentUserEvent.Instance.IsExpert)
                {
                    await Task.Delay(100);
                    bool InsternetActive = true;

                    Device.StartTimer(new TimeSpan(0, 0, 01), () =>
                    {
                        // do something every 5 seconds
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            var _isReachable = await CrossConnectivity.Current.IsRemoteReachable("https://www.google.com/");
                            if (_isReachable)
                            {
                                string json = sender as string;
                                if (!string.IsNullOrEmpty(json))
                                {

                                }
                                InsternetActive = false;
                            }
                        });
                        return InsternetActive; // runs again, or false to stop
                    });
                }
                #endregion
            });
        }
        public string ReceiveValue = string.Empty;
        private async void ControlEventManager_OnRecieved(object sender, EventArgs e)
        {
            var elementEventHandler = (sender as ElementEventHandler);
            this.ReceiveValue = elementEventHandler.ElementValue;
            App.controlEventManager.RecieveCallControlEvents(this, elementEventHandler, CurrentUserEvent.Instance.OwnerUserId);
        }

        private async void BtnStart_Clicked(object sender, EventArgs e)
        {
            //IsPageRemove = false;
            if (CurrentUserEvent.Instance.IsExpert)
            {
                App.INFOPage = false;
                App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                {
                    ElementName = "BtnStart",
                    ElementValue = "BtnStart_Clicked",
                    ToUserId = CurrentUserEvent.Instance.ToUserId,
                    IsExpert = CurrentUserEvent.Instance.IsExpert
                });
            }

            await Navigation.PushAsync(new TreeListPage(gd_data, txtDescription.Text, Title));
        }
        protected override void OnDisappearing()
        {
            try
            {
                base.OnDisappearing();
                if (IsPageRemove)
                {
                    if (CurrentUserEvent.Instance.IsExpert)
                    {
                        //App.DCTPage = true;

                        App.controlEventManager.SendRequestControlEvents(new ElementEventHandler()
                        {
                            ElementName = "GoBack",
                            ElementValue = "GoBack",
                            ToUserId = CurrentUserEvent.Instance.ToUserId,
                            IsExpert = CurrentUserEvent.Instance.IsExpert
                        });
                    }
                }

                App.controlEventManager.OnRecievedData -= ControlEventManager_OnRecievedData;
                App.controlEventManager.OnRecieved -= ControlEventManager_OnRecieved;
            }
            catch (Exception ex)
            {
            }
        }
        private void gd_image_clicked(object sender, EventArgs e)
        {
            try
            {
                //this.Navigation.PushAsync(new GdImagePage(firstListClass.main_list[0].gd_image, this.Title));
            }
            catch (Exception ex)
            {
            }
        }

        public void show_alert(string title, string message, bool btnCancel, bool btnOk)
        {
            //Working = true;
            //TitleText = title;
            //MessageText = message;
            //OkVisible = btnOk;
            //CancelVisible = btnCancel;
            //CancelCommand = new Command(() =>
            //{
            //    Working = false;
            //});
        }
    }
    #endregion
}