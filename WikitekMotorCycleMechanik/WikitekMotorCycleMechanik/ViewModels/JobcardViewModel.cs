using Acr.UserDialogs;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Models;
using WikitekMotorCycleMechanik.Services;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class JobcardViewModel : ViewModelBase
    {
        ApiServices apiServices;
        public JobcardViewModel(Page page) : base(page)
        {
            apiServices = new ApiServices();
            GetJobcardList();
        }


        private ObservableCollection<JobcardResult> _jobcard_list;
        public ObservableCollection<JobcardResult> jobcard_list
        {
            get { return _jobcard_list; }
            set
            {
                _jobcard_list = value;
                OnPropertyChanged("jobcard_list");
            }
        }

        private JobcardResult _selected_jobcard;
        public JobcardResult selected_jobcard
        {
            get { return _selected_jobcard; }
            set
            {
                _selected_jobcard = value;
                OnPropertyChanged("selected_jobcard");
            }
        }

        public async void GetJobcardList()
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                using (UserDialogs.Instance.Loading("Loading...", null, null, true, MaskType.Black))
                {
                    await Task.Delay(100);

                    var result = await apiServices.GetJobcardList(Xamarin.Essentials.Preferences.Get("token", null));

                    if (!result.status)
                    {
                        //DependencyService.Get<Interfaces.IToasts>().Show($"{ mode.status}");
                        UserDialogs.Instance.Toast(result.message, new TimeSpan(0, 0, 0, 3));
                        return;
                    }

                    if (result.results == null || (!result.results.Any()))
                    {
                        UserDialogs.Instance.Toast("Jobcard not found", new TimeSpan(0, 0, 0, 3));
                        return;
                    }

                    jobcard_list = new ObservableCollection<JobcardResult>(result.results.ToList());
                }
            });
        }

        public ICommand CreateJobcardCommand => new Command(async (obj) =>
        {

            try
            {
                   await page.Navigation.PushAsync(new Views.Jobcard.JobcardDetailPage(selected_jobcard));
            }
            catch (Exception ex)
            {
            }
        });

        public ICommand GoToJobcardDetailCommand => new Command(async (obj) =>
        {

            try
            {
                selected_jobcard = (JobcardResult)obj;
                await page.Navigation.PushAsync(new Views.Jobcard.JobcardDetailPage(selected_jobcard));
            }
            catch (Exception ex)
            {
            }
        });
    }
}
