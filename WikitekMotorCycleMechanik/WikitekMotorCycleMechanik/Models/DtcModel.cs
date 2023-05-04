using System;
using System.Collections.Generic;
using System.Text;
using WikitekMotorCycleMechanik.Extensions;
using WikitekMotorCycleMechanik.ViewModels;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.Models
{
    public class DtcModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<DtcResult> results { get; set; }
    }

    public class DtcResult
    {
        public int id { get; set; }
        public string code { get; set; }
        public object description { get; set; }
        public string is_active { get; set; }
        public List<DtcCode> codes { get; set; }
    }


    public class DtcCode
    {
        public int id { get; set; }
        public string code { get; set; }
        //public string description { get; set; }

        private string _description;
        public string description
        {
            get { return StringExtensions.Translate(_description.ToLower()); }
            set { _description = value; }
        }

        public string is_active { get; set; }
        public string status_activation { get; set; }
        public string lamp_activation { get; set; }
        public Color status_activation_color { get; set; }
        public Color lamp_activation_color { get; set; }
    }

    public class DtcEcusModel : BaseViewModel
    {
        public string ecu_name { get; set; }

        private double _opacity;
        public double opacity
        {
            get => _opacity;
            set
            {
                _opacity = value;
                OnPropertyChanged("opacity");
            }
        }
        public List<DtcCode> dtc_list { get; set; }
    }





    public class ReadDtcResponseModel
    {
        public string status { get; set; }
        public string[,] dtcs { get; set; }
    }

    public class ClearDtcResponseModel
    {
        public string ECUResponseStatus { get; set; }
        public string ECUResponse { get; set; }
        public string ActualDataBytes { get; set; }
    }
}
