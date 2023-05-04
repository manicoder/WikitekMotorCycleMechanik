using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.ViewModels
{
    public class ViewModelBase : INotifyPropertyChanged//,INavigation,
    {

        protected Page page { get; private set; }
        //protected IPageDialog PageDialogService { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public ViewModelBase(Page page)
        {
            this.page = page;
        }

        protected bool SetProperty<T>(
          ref T backingStore, T value,
          [CallerMemberName] string propertyName = "",
          Action onChanged = null)
        {


            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;

            if (onChanged != null)
                onChanged();

            OnPropertyChanged(propertyName);
            return true;
        }

        public void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}