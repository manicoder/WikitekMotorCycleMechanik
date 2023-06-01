using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using WikitekMotorCycleMechanik.ViewModels;

namespace WikitekMotorCycleMechanik.Models
{
    public class VehicleModelModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
        public List<VehicleModelRes> results { get; set; }
    }
    public class VehicleModelRes : BaseViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string segment_name { get; set; }
        public object model_year { get; set; }
        public string oem { get; set; }
        public List<VehicleSubModel> sub_models { get; set; }

        private double _row_height = 0;
        public double row_height
        {
            get => _row_height;
            set
            {
                _row_height = value;
                OnPropertyChanged("row_height");
            }
        }

        private string _arrow = "\U000F054F";
        public string arrow
        {
            get => _arrow;
            set
            {
                _arrow = value;
                OnPropertyChanged("arrow");
            }
        }
    }
}
