using System;
using System.Collections.Generic;
using System.Text;
using WikitekMotorCycleMechanik.ViewModels;

namespace WikitekMotorCycleMechanik.Models
{
    public class MetaTags
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<MetaTagList> results { get; set; }
    }

    public class MetaTagList : BaseViewModel
    {
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
