using Acr.UserDialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class DtcFinderViewModel : ViewModelBase
    {
        ApiServices apiServices;
        public static ObservableCollection<DtcCode> dtcListFixed;
        public DtcFinderViewModel(Page page) : base(page)
        {
            apiServices = new ApiServices();
            Device.BeginInvokeOnMainThread(async () =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await GetDtcList();
                }
            });

        }

        private ObservableCollection<DtcCode> _dtcList;
        public ObservableCollection<DtcCode> dtcList
        {
            get { return _dtcList; }
            set
            {
                _dtcList = value;

                OnPropertyChanged("dtcList");
            }
        }

        private string _txtSearch;
        public string txtSearch
        {
            get { return _txtSearch; }
            set
            {
                _txtSearch = value;
                if (!string.IsNullOrWhiteSpace(_txtSearch))
                {
                    if (dtcListFixed.Count > 0)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            dtcList = new ObservableCollection<DtcCode>(dtcListFixed.
                                Where(x => x.code.ToLower().Contains(txtSearch.ToLower())).ToList());
                            //dtcList = new ObservableCollection<DtcCode>(dtcListFixed
                            //    .Where(string.Compare("",_txtSearch,true)==1).ToList());
                        });
                    }
                }
                OnPropertyChanged("txtSearch");
            }
        }

        public async Task GetDtcList()
        {
            var dtc_li = await apiServices.GetDtc(Xamarin.Essentials.Preferences.Get("token", null), 10663, App.is_update);

            dtcListFixed = dtcList = new ObservableCollection<DtcCode>(dtc_li.codes.ToList());
        }
    }
}
