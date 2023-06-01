using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.ViewModels;
using WikitekMotorCycleMechanik.Views.Dashboad;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.MarketPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MarketPlacePage : ContentPage
    {
        MarketPlaceViewModel viewModel;
        public MarketPlacePage(ObservableCollection<PartsList> marketList)
        {
            InitializeComponent();
            BindingContext = viewModel = new MarketPlaceViewModel(marketList, this);
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                //InitializeComponent();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    try
                    {
                        ApiServices apiServices = new ApiServices();
                        var res = await apiServices.GetCartList(Xamarin.Essentials.Preferences.Get("token", null), App.user_id);
                        if (res.results.Count > 0)
                        {
                            if (res.results.FirstOrDefault().cart_items.Count > 0)
                            {
                                var count = res.results.FirstOrDefault().cart_items.Count;

                                if (count > 0)
                                {
                                    viewModel.badge_count = $"{count}";
                                    viewModel.badge_count_visible = true;
                                }
                                else
                                {
                                    viewModel.badge_count_visible = false;
                                }
                            }
                        }
                        //await Task.Delay(500);
                        //DependencyService.Get<IToolbarItemBadgeService>().SetBadge(this, ToolbarItems.FirstOrDefault(), $"{count}", Color.Red, Color.White);
                    }
                    catch (Exception ex)
                    {
                    }
                });
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnDisappearing()
        {
            App.isFilter = false;
            App.selectedBannerId = string.Empty;
            base.OnDisappearing();
            //ToolbarItems.Clear();
        }

        protected override bool OnBackButtonPressed()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                Application.Current.MainPage = new MasterDetailView(App.user) { Detail = new NavigationPage(new DashboadPage()) };
            });
            return true;
        }
    }
}