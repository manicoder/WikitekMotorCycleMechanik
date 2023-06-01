using System;
using System.Collections.Generic;
using System.Text;
using WikitekMotorCycleMechanik.ViewModels;

namespace WikitekMotorCycleMechanik.Models
{
    public class ErrorModel:BaseViewModel
    {
        private bool _is_visible = true;
        public bool is_visible
        {
            get => _is_visible;
            set
            {
                _is_visible = value;
                OnPropertyChanged("is_visible");
            }
        }

        private bool _is_runing = true;
        public bool is_runing
        {
            get => _is_runing;
            set
            {
                _is_runing = value;
                OnPropertyChanged("is_runing");
            }
        }

        private string _error_message = "Loading...";
        public string error_message
        {
            get => _error_message;
            set
            {
                _error_message = value;
                OnPropertyChanged("error_message");
            }
        }
    }
}
