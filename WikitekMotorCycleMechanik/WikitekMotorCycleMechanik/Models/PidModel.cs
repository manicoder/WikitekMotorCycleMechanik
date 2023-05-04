using WikitekMotorCycleMechanik.ViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using System.Collections.ObjectModel;
using WikitekMotorCycleMechanik.Extensions;
using MultiEventController.Models;
using WikitekMotorCycleMechanik.Services;

namespace WikitekMotorCycleMechanik.Models
{
    public class PidModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<PidResult> results { get; set; }
    }

    public class PidResult
    {
        public int id { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        public List<PidCode> codes { get; set; }
        public string model_file { get; set; }
    }

    public class PidCode : BaseViewModel
    {
        //------ Tab Section

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

        //------ List Section
        public int id { get; set; }
        public string code { get; set; }
        private string _short_name;
        public string short_name
        {
            get { return StringExtensions.Translate(_short_name.ToLower()); }
            set { _short_name = value; }
        }
        public string long_name { get; set; }
        public int total_len { get; set; }
        public int byte_position { get; set; }
        public int length { get; set; }
        public bool bitcoded { get; set; }
        public int? start_bit_position { get; set; }
        public int? end_bit_position { get; set; }
        public double? resolution { get; set; }
        public double? offset { get; set; }
        public double? min { get; set; }
        public double? max { get; set; }
        public bool read { get; set; }
        public bool write { get; set; }
        public string message_type { get; set; }
        public string unit { get; set; }
        public string write_pid { get; set; }
        public bool reset { get; set; }
        public string reset_value { get; set; }
        public List<Message> messages { get; set; } = new List<Message>();

        private string _show_resolution;
        public string show_resolution
        {
            get => _show_resolution;
            set
            {
                _show_resolution = value;
                OnPropertyChanged("show_resolution");
            }
        }

        public Color bg_color { get; set; }

        private bool _list_visible;
        public bool list_visible
        {
            get => _list_visible;
            set
            {
                _list_visible = value;
                OnPropertyChanged("list_visible");
            }
        }

        private bool selected = false;
        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                OnPropertyChanged("Selected");
            }
        }
    }

    public class Message
    {
        public string code { get; set; }
        public string message { get; set; }
    }

    public class ReadParameterPID : BaseViewModel
    {
        public string pid { get; set; }
        public int totalLen { get; set; }
        public int totalBytes { get; set; }
        public int startByte { get; set; }
        public int noOfBytes { get; set; }
        public bool IsBitcoded { get; set; }
        public int startBit { get; set; }
        public int noofBits { get; set; }
        public string datatype { get; set; }
        public double? resolution { get; set; }
        public double? offset { get; set; }
        public string unit { get; set; }
        public int pidNumber { get; set; }
        public string pidName { get; set; }

        private string _show_resolution;
        public string show_resolution
        {
            get => _show_resolution;
            set
            {
                _show_resolution = value;
                OnPropertyChanged("show_resolution");
            }
        }

        public List<Message> messages { get; set; }
    }

    public class ReadPidPresponseModel
    {
        public string Status { get; set; }
        public string DataArray { get; set; }
        public int pidNumber { get; set; }
        public string pidName { get; set; }
        public string responseValue { get; set; }
    }

    public class EcusModel : BaseViewModel
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
        public List<PidCode> pid_list { get; set; }
    }
}
