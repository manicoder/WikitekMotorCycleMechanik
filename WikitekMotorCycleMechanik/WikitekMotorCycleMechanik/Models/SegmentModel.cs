using WikitekMotorCycleMechanik.ViewModels;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.Models
{
    public class SegmentModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<VehicleSegment> results { get; set; }
        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
    }

    public class VehicleSegment : BaseViewModel
    {
        public int id { get; set; }
        public string segment_name { get; set; }
        public string segment_icon { get; set; }


        private Color _selected_color = Color.FromHex("#FFF");
        public Color selected_color
        {
            get => _selected_color;
            set
            {
                _selected_color = value;
                OnPropertyChanged("selected_color");
            }
        }
    }
}
