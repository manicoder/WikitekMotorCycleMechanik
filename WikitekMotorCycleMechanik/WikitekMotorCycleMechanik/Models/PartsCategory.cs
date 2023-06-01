using System;
using System.Collections.Generic;
using System.Text;
using WikitekMotorCycleMechanik.ViewModels;

namespace WikitekMotorCycleMechanik.Models
{
    public class PartsCategory
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<PartsCategoryList> results { get; set; }
    }

    public class PartsCategoryList
    {
        public PartCategory1 part_category { get; set; }
        public string part_number { get; set; }
    }

    public class PartCategory1 : BaseViewModel
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string name { get; set; }
        public List<SubCat> subs { get; set; }

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

        private bool _isExpanded;
        public bool isExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                OnPropertyChanged("isExpanded");
            }
        }

        private string _ic_updown = "ic_sort_down.png";
        public string ic_updown
        {
            get => _ic_updown;
            set
            {
                _ic_updown = value;
                OnPropertyChanged("ic_updown");
            }
        }
    }

    public class SubCat : BaseViewModel
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string name { get; set; }
        public int parent_cat { get; set; }

        private bool _Selected;
        public bool Selected
        {
            get => _Selected;
            set
            {
                _Selected = value;
                OnPropertyChanged("Selected");
            }
        }
    }
}
