using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using WikitekMotorCycleMechanik.StaticInfo;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class PackageViewModel : BaseViewModel
    {
        ApiServices apiServices;
        public event EventHandler CloseCountryPopup;

        public PackageViewModel(int segment_id, string pack)
        {
            apiServices = new ApiServices();
            package_list = new ObservableCollection<PackageResult>();
            selected_package = new PackageResult();
            this.segment_id = segment_id;
            this.pack = pack;
            Device.InvokeOnMainThreadAsync(async () =>
            {
                GetPackageList();
            });

            ClosePopupCommand = new Command(async (obj) =>
            {
                CloseCountryPopup?.Invoke("", new EventArgs());
                //StaticMethods.last_page = "CreateRSAgent";
                //await Rg.Plugins.Popup.Services.PopupNavigation.PushAsync(new PopupPages.CountyPopupPage());
            });
        }

        private int _segment_id;
        public int segment_id
        {
            get => _segment_id;
            set
            {
                _segment_id = value;
                OnPropertyChanged("segment_id");
            }
        }

        private string _search_key;
        public string search_key
        {
            get => _search_key;
            set
            {
                _search_key = value;

                if (!string.IsNullOrEmpty(search_key))
                {
                    package_list = new ObservableCollection<PackageResult>(package_static_list.Where(x => x.code.ToLower().Contains(search_key.ToLower())).ToList());
                }

                OnPropertyChanged("search_key");
            }
        }

        private string _pack;
        public string pack
        {
            get => _pack;
            set
            {
                _pack = value;
                OnPropertyChanged("pack");
            }
        }

        private ObservableCollection<PackageResult> _package_static_list;
        public ObservableCollection<PackageResult> package_static_list
        {
            get => _package_static_list;
            set
            {
                _package_static_list = value;
                OnPropertyChanged("package_static_list");
            }
        }

        private ObservableCollection<PackageResult> _package_list;
        public ObservableCollection<PackageResult> package_list
        {
            get => _package_list;
            set
            {
                _package_list = value;
                OnPropertyChanged("package_list");
            }
        }

        private PackageResult _selected_package;
        public PackageResult selected_package
        {
            get => _selected_package;
            set
            {
                _selected_package = value;

                if (selected_package != null)
                {
                    if (!string.IsNullOrEmpty(selected_package.code))
                    {
                        MessagingCenter.Send<PackageViewModel, PackageResult>(this, "selected_package", selected_package);
                    }

                    CloseCountryPopup?.Invoke("", new EventArgs());
                }
                OnPropertyChanged("selected_package");
            }
        }


        #region Methods

        public async void GetPackageList()
        {
            try
            {
                var response = await apiServices.GetPackageBySegment(Xamarin.Essentials.Preferences.Get("token", null), "wikitekMechanik", pack, App.country_id, segment_id);

                var api_status_code = StaticMethods.http_status_code(response.status_code);

                if (response.status_code == System.Net.HttpStatusCode.OK || response.status_code == System.Net.HttpStatusCode.Created)
                {


                    package_static_list = package_list = new ObservableCollection<PackageResult>(response.results);
                    //package_list = new ObservableCollection<PackageModel>(response.results);
                    //await page.DisplayAlert("Success!", "!", "Ok");
                    //await page.Navigation.PopAsync();
                }
                else
                {
                    //await page.DisplayAlert(api_status_code, "Create user Api  not working", "Ok");
                }
            }
            catch (Exception ex)
            {
            }
        }

        #endregion

        public ICommand ClosePopupCommand { get; set; }
    }
}
