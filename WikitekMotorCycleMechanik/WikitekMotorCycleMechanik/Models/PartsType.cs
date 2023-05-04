using System;
using System.Collections.Generic;
using System.Text;
using WikitekMotorCycleMechanik.ViewModels;

namespace WikitekMotorCycleMechanik.Models
{
    public class PartsType
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<PartsTypeList> results { get; set; }
    }

    public class PartsTypeList : BaseViewModel
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string name { get; set; }

        private bool _isSelected;
        public bool isSelected
        {
            get => _isSelected;
            set
            {
                _isSelected = value;
                OnPropertyChanged("isSelected");
            }
        }
    }

}
