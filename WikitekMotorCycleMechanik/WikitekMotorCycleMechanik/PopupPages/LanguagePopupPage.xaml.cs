using Acr.UserDialogs;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.ViewModels;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using WikitekMotorCycleMechanik.Views.VehicleDiagnostics;
using Plugin.Multilingual;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WikitekMotorCycleMechanik.Views.Dashboad;

namespace WikitekMotorCycleMechanik.View.PopupPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LanguagePopupPage : ContentPage
    {
        LanguageViewModel viewModel;
        public LanguagePopupPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new LanguageViewModel();
        }

        private void CloseClick(object sender, EventArgs e)
        {
            txtSearch.Text = string.Empty;
            imgClose.IsVisible = false;
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtSearch.Text))
                {
                    imgClose.IsVisible = false;
                    Language_List.ItemsSource = viewModel.LaguageList;
                }
                else
                {
                    imgClose.IsVisible = true;
                    Language_List.ItemsSource = viewModel.LaguageList.Where(x => x.Language.ToLower().Contains(e.NewTextValue.ToLower()));
                }
            }
            catch (Exception ex)
            {
            }

        }

        private async void modelList_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                //var item = (e.Item as LanguageModel).Language;
                //var result = await DisplayAlert("Alert", $"Are you sure you want to change your APP language to {item}", "Ok", "Cancel");
                //{
                //    //ChangeLanguage();
                //}

                //var item = (e.Item as LanguageModel).Language;
                //var result = await DisplayAlert("Alert",$"Are you sure you want to change your APP language to {item}","Ok","Cancel");
                //using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                //{
                //    await Task.Delay(100);

                //    //var res = workShopGroupModel.CityList.FirstOrDefault(x => x.city == item);
                //    //MessagingCenter.Send<CityPopupPage, WorkCity>(this, "City", res);
                //    App.Current.MainPage = new NavigationPage(new LoginPage());
                //    await PopupNavigation.Instance.PopAsync();
                //}
            }
            catch (Exception ex)
            {
            }
        }

        public int CheckedLanguage = 0;
        private void check_Tapped(object sender, EventArgs e)
        {
            var selectedItem = (LanguageModel)((Grid)sender).BindingContext;

            foreach (var item in viewModel.LaguageList)
            {
                if (selectedItem.id == item.id)
                {
                    item.is_checked = true;
                    CheckedLanguage = item.id;
                }
                else
                {
                    item.is_checked = false;
                }
            }

        }

        public async void ChangeLanguage(int SelectedLag)
        {
            try
            {
                switch (SelectedLag)
                {
                    case 1:
                        CrossMultilingual.Current.CurrentCultureInfo = new System.Globalization.CultureInfo("en");
                        await DependencyService.Get<ISaveLocalData>().SaveData("LastSelectedLanguage", "en");
                        break;
                    case 2:
                        CrossMultilingual.Current.CurrentCultureInfo = new System.Globalization.CultureInfo("hi");
                        await DependencyService.Get<ISaveLocalData>().SaveData("LastSelectedLanguage", "hi");
                        break;
                    case 3:
                        CrossMultilingual.Current.CurrentCultureInfo = new System.Globalization.CultureInfo("kn");
                        await DependencyService.Get<ISaveLocalData>().SaveData("LastSelectedLanguage", "kn");
                        break;
                    case 4:
                        CrossMultilingual.Current.CurrentCultureInfo = new System.Globalization.CultureInfo("ml");
                        await DependencyService.Get<ISaveLocalData>().SaveData("LastSelectedLanguage", "ml");
                        break;
                    case 5:
                        CrossMultilingual.Current.CurrentCultureInfo = new System.Globalization.CultureInfo("te");
                        await DependencyService.Get<ISaveLocalData>().SaveData("LastSelectedLanguage", "te");
                        break;
                    case 6:
                        CrossMultilingual.Current.CurrentCultureInfo = new System.Globalization.CultureInfo("bg");
                        await DependencyService.Get<ISaveLocalData>().SaveData("LastSelectedLanguage", "bg");
                        break;
                    case 7:
                        CrossMultilingual.Current.CurrentCultureInfo = new System.Globalization.CultureInfo("ta");
                        await DependencyService.Get<ISaveLocalData>().SaveData("LastSelectedLanguage", "ta");
                        break;
                    default:
                        //top = 0;
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSubmit_Clicked(object sender, EventArgs e)
        {
            try
            {
                ChangeLanguage(CheckedLanguage);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    Application.Current.MainPage = new MasterDetailView(App.user) { Detail = new NavigationPage(new DashboadPage(App.user)) };
                });

                //App.Current.MainPage = new NavigationPage(new LoginPage());
            }
            catch (Exception ex)
            {
            }
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                Application.Current.MainPage = new MasterDetailView(App.user) { Detail = new NavigationPage(new DashboadPage(App.user)) };
            });
            return true;
        }


    }
}