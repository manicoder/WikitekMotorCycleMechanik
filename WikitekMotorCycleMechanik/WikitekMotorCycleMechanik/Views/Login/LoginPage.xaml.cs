    using System;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel vm;
        //DtcModel model;
        public LoginPage()
        {
            try
            {
                InitializeComponent();
                BindingContext = vm = new LoginViewModel(this);
                //var test = model.code;
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Device.BeginInvokeOnMainThread(async () =>
            //{
            //    try
            //    {
            //        bool isLatestVersion = await DependencyService.Get<ILatest>().IsUsingLatestVersion();
            //        if (!isLatestVersion)
            //        {
            //            bool res = await DisplayAlert("New Version", "New version Available,do you want to update it?", null, "Ok");
            //            if (!res)
            //            {
            //                await DependencyService.Get<ILatest>().OpenAppInStore();
            //            }
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //    }
            //});
        }
    }
}