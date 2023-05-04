using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Interfaces;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.ViewModels;
using WikitekMotorCycleMechanik.Views.MasterDetail;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WikitekMotorCycleMechanik.Views.MarketPlace
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MarketPlaceDetailPage : ContentPage
    {
        MarketPlaceDetailViewModel viewModel;
        PartsList selected_market_place;
        public MarketPlaceDetailPage(PartsList selected_market_place)
        {
            InitializeComponent();
            BindingContext = viewModel = new MarketPlaceDetailViewModel(this, selected_market_place);
            this.selected_market_place = selected_market_place;
        }

        protected override void OnAppearing()
        {
            try
            {
                base.OnAppearing();
                //InitializeComponent();
                Device.BeginInvokeOnMainThread(async () =>
                {
                    using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                    {
                        await Task.Delay(100);
                        try
                        {

                            ApiServices apiServices = new ApiServices();
                            var res = await apiServices.GetCartList(Xamarin.Essentials.Preferences.Get("token", null), App.user_id);

                            if (res.results.Count > 0)
                            {
                                if (res.results.FirstOrDefault().cart_items.Count > 0)
                                {
                                    //viewModel.quantities = res.results.FirstOrDefault().cart_items
                                    //                       .FirstOrDefault(x => x.parts_id.part_number == selected_market_place.part_number)
                                    //                       .quantity;

                                    //if (selected_market_place.prices.Any())
                                    //{
                                    //    foreach (var pr in selected_market_place.prices)
                                    //    {
                                    //        if (viewModel.quantities >= pr.min_quantity && viewModel.quantities <= pr.max_quantity)
                                    //        {
                                    //            viewModel.extended_price = (pr.price.HasValue) ? pr.price.Value : 0 * viewModel.quantities;
                                    //        }
                                    //    }
                                    //}
                                    //else
                                    //    viewModel.extended_price = (selected_market_place.mrp.HasValue) ? selected_market_place.mrp.Value : 0 * viewModel.quantities;

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
                        }
                        catch (Exception ex)
                        {
                        }

                    }
                    //DependencyService.Get<IToolbarItemBadgeService>().SetBadge(this, ToolbarItems.FirstOrDefault(), $"{count}", Color.Red, Color.White);
                });
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            //ToolbarItems.Clear();
        }

        protected override bool OnBackButtonPressed()
        {
            Navigation.PushAsync(new MasterDetailView(App.user) { Detail = new NavigationPage(new MarketPlacePage(null)) });
            return true;
        }
    }
}