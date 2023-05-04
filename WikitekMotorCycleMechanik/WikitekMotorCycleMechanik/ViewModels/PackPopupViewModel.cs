using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WikitekMotorCycleMechanik.Views.Settings;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class PackPopupViewModel : BaseViewModel
    {
        public PackPopupViewModel(string pack)
        {
            this.pack = pack == "ADP" ? "Advanced Diagnostic Pack" : "Operational Pack";
            YesCommand = new Command( async () =>
            {
                MessagingCenter.Send<PackPopupViewModel>(this, "RemoveSelection");
                await PopupNavigation.Instance.PopAsync();
            });
            //NoCommand = new Command(async () =>
            //{
            //    await PopupNavigation.Instance.PopAsync();
            //});
            PackHelpCommand = new Command(async () =>
            {
                MessagingCenter.Send<PackPopupViewModel>(this, "NavigateToPackHelp");
                await PopupNavigation.Instance.PopAsync();
            });
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

        public ICommand YesCommand { get; set; }
        public ICommand NoCommand { get; set; }
        public ICommand PackHelpCommand { get; set; }
    }
}
