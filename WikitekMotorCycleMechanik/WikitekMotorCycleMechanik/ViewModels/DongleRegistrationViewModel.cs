using Acr.UserDialogs;
using WikitekMotorCycleMechanik.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using WikitekMotorCycleMechanik.Views;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class DongleRegistrationViewModel : ViewModelBase
    {
        ApiServices apiServices;
        readonly INavigation navigationService;
        readonly Page page;
        public string dongle_type = string.Empty;
        //MediaFile file = null;
        public DongleRegistrationViewModel(Page page, string title, string part_number, string image) : base(page)
        {
            try
            {
                apiServices = new ApiServices();
                this.page = page;
                this.navigationService = page.Navigation;
                this.image = image;
                dongle_type = title;
                page.Title = $"Dongle ({title}) Registration";
                header = $"Registration of your WikitekMotorCycleMechanik {title} Dongle ({part_number})";
                InitializeCommands();
            }
            catch
            { }
        }

        #region Properties

        private string _title;
        public string title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("title");
            }
        }

        private string _image;
        public string image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged("image");
            }
        }

        private string _header;// = $"Registration of your RSAngel {title} Dongle";
        public string header
        {
            get => _header;
            set
            {
                _header = value;
                OnPropertyChanged("header");
            }
        }

        private bool _visible_done_button = false;// = $"Registration of your RSAngel {title} Dongle";
        public bool visible_done_button
        {
            get => _visible_done_button;
            set
            {
                _visible_done_button = value;
                OnPropertyChanged("visible_done_button");
            }
        }
        #endregion

        #region Methods
        [Obsolete]
        public void InitializeCommands()
        {
            YesCommand = new Command(async (obj) =>
            {
                visible_done_button = true;
            });

            NoCommand = new Command(async (obj) =>
            {
                var result = await page.DisplayAlert("Alert", "Do you want to puchase a new dongle", "Ok", "Cancel");
                if (result)
                {
                    await page.Navigation.PushAsync(new Views.MarketPlace.MarketPlacePage(null));
                }
                else
                {
                    await page.Navigation.PopAsync();
                }
                //await page.DisplayAlert("Alert", "Please contact your area manager to purchase this dongle.", "Ok");
                //await page.Navigation.PopAsync();
            });

            DoneCommand = new Command(async (obj) =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(200);
                    //StaticMethods.last_page = "registration";
                    string dongle = string.Empty;
                    if (dongle_type.Contains("24"))
                    {
                        dongle = "WKBTD001-HD";
                    }
                    else
                    {
                        dongle = "wkbtd001";
                    }
                    await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.DongleListPopupPage(dongle));
                }
            });

        }
        #endregion


        #region ICommands
        public ICommand YesCommand { get; set; }
        public ICommand NoCommand { get; set; }
        public ICommand DoneCommand { get; set; }
        #endregion
    }
}
