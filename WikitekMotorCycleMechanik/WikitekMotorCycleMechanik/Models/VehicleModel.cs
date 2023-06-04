using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using WikitekMotorCycleMechanik.ViewModels;

namespace WikitekMotorCycleMechanik.Models
{
    public class VehicleModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public bool success { get; set; }
        public List<VehicleModelResult> data { get; set; }
        public List<VehicleModelResult> results { get; set; }
        public string error { get; set; }
        public string detail { get; set; }
        public HttpStatusCode status_code { get; set; }
    }
    //public class Segment
    //{
    //    public int id { get; set; }
    //    public string segment_name { get; set; }
    //}
    public class Vehicle_Model
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class SubModel
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class ModelYear
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class VehOem
    {
        public int id { get; set; }
        public string name { get; set; }
    }
    public class Vehicle : ViewModels.BaseViewModel
    {
        public string id { get; set; }
        public string registration_id { get; set; }
        public string vin { get; set; }
        public Segment segment { get; set; }
        public Vehicle_Model vehicle_model { get; set; }
        public SubModel sub_model { get; set; }
        public ModelYear model_year { get; set; }
        public object vehicle_type { get; set; }
        public string picture { get; set; }
        public string user { get; set; }
        public VehOem oem { get; set; }
        public string vehicle_picture
        {
            get
            {
                return string.Format($"https://wikitek.io{picture}");
            }
        }
        public List<object> dongle { get; set; }

        private string _bg_color;
        public string bg_color
        {
            get => _bg_color;
            set
            {
                _bg_color = value;
                OnPropertyChanged("bg_color");
            }
        }

    }

    public class VehicleModelResult : BaseViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public object parent { get; set; }
        public object model_year { get; set; }
        public string model_file { get; set; }
        public byte[] model_file_lacal { get; set; }
        public List<VehicleSubModel> sub_models { get; set; }
        public Oem oem { get; set; }
        public AttachmentModel attachment { get; set; }
    }

    public class VehicleSubModel : BaseViewModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string model_year { get; set; }
        public List<VehicleEcu> ecus { get; set; }
        //public string sub_model_file { get; set; }
        //public string sub_model_name { get; set; }
        public string model_year_name { get; set; }
        public string connector_type { get; set; }
        public AttachmentModel attachment { get; set; }
        //public string injector_charecterization { get; set; }
        public int segment_name { get; set; }


        public bool _selected_sub_model = false;
        public bool selected_sub_model
        {
            get => _selected_sub_model;
            set
            {
                _selected_sub_model = value;
                OnPropertyChanged("selected_sub_model");
            }
        }
    }

    public class AttachmentModel
    {
        public int id { get; set; }
        public string attachment { get; set; }
        public byte[] attachmentfile_local { get; set; }
        public string media_type { get; set; }
        public string name { get; set; }
        public string status { get; set; }
    }

    public class VehicleEcu
    {
        public int id { get; set; }
        public string protocol { get; set; }
        public string name { get; set; }
        public string tx_header { get; set; }
        public string rx_header { get; set; }
        public string read_dtc_fn_index { get; set; }
        public string clear_dtc_fn_index { get; set; }
        public string read_data_fn_index { get; set; }
        public List<int> datasets { get; set; }
        public List<int> pid_datasets { get; set; }
        public bool? padding { get; set; }


        //public WriteDataFnIndex write_data_fn_index { get; set; }
        //public SeedkeyalgoFnIndex seedkeyalgo_fn_index { get; set; }
        //public List<VehicleEcu2> ecu { get; set; } = new List<VehicleEcu2>();
    }

    public class VehicleEcu2
    {
        public int id { get; set; }
        public string sequence_file_name { get; set; }
        public string flashsep_time { get; set; }
        public string flash_address_data_format { get; set; }
        public string flash_check_sum_type { get; set; }
        public string flash_diagnostic_mode { get; set; }
        public string flash_erase_type { get; set; }
        public string flash_frase_byte { get; set; }
        public string flash_nax_blkseqcntr { get; set; }
        public string flash_seed_key_length { get; set; }
        public string flash_status { get; set; }
        public string sequence_file { get; set; }
        public List<File> file { get; set; }

        public string sectorframetransferlen { get; set; }
        public string sendseedbyte { get; set; }
        public List<EcuMapFile> ecu_map_file { get; set; }
    }


    public class Dataset
    {
        public int id { get; set; }
        public string code { get; set; }
    }

    public class PidDataset
    {
        public int id { get; set; }
        public string code { get; set; }
    }

    public class Protocol
    {
        public string name { get; set; }
        public string elm { get; set; }
        public string autopeepal { get; set; }
    }

    public class ReadDtcFnIndex
    {
        public string value { get; set; }
    }

    public class ClearDtcFnIndex
    {
        public string value { get; set; }
    }

    public class ReadDataFnIndex
    {
        public string value { get; set; }
    }

    public class WriteDataFnIndex
    {
        public string value { get; set; }
    }

    public class SeedkeyalgoFnIndex
    {
        public string value { get; set; }
    }

    public class File
    {
        public int id { get; set; }
        public string data_file_name { get; set; }
        public string data_file { get; set; }
    }

    public class EcuMapFile
    {
        public int id { get; set; }
        public string end_address { get; set; }
        public string sector_name { get; set; }
        public string start_address { get; set; }
    }


    public class SubmodelList :BaseViewModel
    {
        public string submodel { get; set; }
        
        public bool _selected_sub_model = false;
        public bool selected_sub_model
        {
            get => _selected_sub_model;
            set
            {
                _selected_sub_model = value;
                OnPropertyChanged("selected_sub_model");
            }
        }

        public List<VehicleSubModel> model_year_list { get; set; }
    }
}
