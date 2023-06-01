using System;
using System.Collections.Generic;
using System.Text;
using WikitekMotorCycleMechanik.ViewModels;

namespace WikitekMotorCycleMechanik.Models
{
    public class JobcardModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<JobcardResult> results { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class JobcardResult
    {
        public string id { get; set; }
        public string job_card_name { get; set; }
        public string status { get; set; }
        public RegistrationNo registration_no { get; set; }
        public Workshop workshop { get; set; }
        public UserType user_type { get; set; }
        public CreatedBy created_by { get; set; }
        public List<JobcardSymptom> jobcard_symptom { get; set; }
        public List<JobcardService> jobcard_service { get; set; }
        public List<JobcardSparepart> jobcard_sparepart { get; set; }
        public List<JobcardPickupdrop> jobcard_pickupdrop { get; set; }
        public List<JobcardEntryexit> jobcard_entryexit { get; set; }
        public string pickup { get; set; }
        public string drop { get; set; }
        public string service_type { get; set; }
    }

    public class JobcardSymptom
    {
        public int id { get; set; }
        public string job_card { get; set; }
        public symptom symptom { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
        public string status { get; set; }
        public bool customer_enable{ get; set; }
        public bool service_enable { get; set; }
        public bool entry_enable { get; set; }
        public bool exit_enable { get; set; }

    }

    public class JobcardService
    {
        public string id { get; set; }
        public string job_card { get; set; }
        public Service service { get; set; }
        public int quantity { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
        public string status { get; set; }
        public int unit_price { get; set; }

        public bool customer_enable { get; set; }
        public bool service_enable { get; set; }
        public bool entry_enable { get; set; }
        public bool exit_enable { get; set; }
    }

    public class JobcardSparepart
    {
        public string id { get; set; }
        public string job_card { get; set; }
        public Sparepart sparepart { get; set; }
        public int quantity { get; set; }
        public string customer_check { get; set; }
        public string service_check { get; set; }
        public string entry_check { get; set; }
        public string exit_check { get; set; }
        public string status { get; set; }
        public int unit_price { get; set; }

        public bool customer_enable { get; set; }
        public bool service_enable { get; set; }
        public bool entry_enable { get; set; }
        public bool exit_enable { get; set; }
    }

    public class JobcardPickupdrop
    {
        public string id { get; set; }
        public string job_card { get; set; }
        public string type { get; set; }
        public string technician { get; set; }
        public string address { get; set; }
        public string latlong { get; set; }
        public string panned_time { get; set; }
        public string actual_time { get; set; }
        public string status { get; set; }
    }

    public class JobcardEntryexit
    {
        public string id { get; set; }
        public string job_card { get; set; }
        public string type { get; set; }
        public object technician { get; set; }
        public string panned_time { get; set; }
        public string actual_time { get; set; }
        public string status { get; set; }
    }

    public class Service
    {
        public int id { get; set; }
        public string service_name { get; set; }
    }

    public class Sparepart
    {
        public int id { get; set; }
        public string sparepart_no { get; set; }
    }


    public class symptom
    {
        public int id { get; set; }
        public string symptom_name { get; set; }
    }

    public class CreatedBy
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
    }

    public class RegistrationNo
    {
        public string id { get; set; }
        public string registration_id { get; set; }
        public string picture { get; set; }
    }

    
    public class UserType
    {
        public int id { get; set; }
        public string name { get; set; }
    }

    public class JobCardListModel : BaseViewModel
    {
        public string jobcard_name { get; set; }
        public string session_id { get; set; }
        public DateTime? start_date { get; set; }
        public string status { get; set; }
        public string job_card { get; set; }
        public DateTime? end_date { get; set; }
        public DateTime? modified { get; set; }
        public string session_type { get; set; }
        public string id { get; set; }
        public string vehicle_model { get; set; }
        public int model_id { get; set; }
        public string sub_model { get; set; }
        public int sub_model_id { get; set; }
        public string model_year { get; set; }
        public string chasis_id { get; set; }
        public string complaints { get; set; }
        public string fert_code { get; set; }
        public int km_covered { get; set; }
        public string registration_no { get; set; }
        public string vehicle_segment { get; set; }
        public string jobcard_status { get; set; }
        public string show_start_date { get; set; }
        public string source { get; set; }
        public string workshop { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        private string _model_with_submodel;
        public string model_with_submodel
        {
            get => _model_with_submodel;
            set
            {
                _model_with_submodel = value;
                OnPropertyChanged("model_with_submodel");
            }
        }

    }


    #region [Create Jobcard Model]
    public class CreateJobcardModel
    {
        public string job_card_name { get; set; }
        public string status { get; set; }
        public string registration_no { get; set; }
        public int workshop { get; set; }
        public string user_type { get; set; }
        public string created_by { get; set; }
        public string pickup { get; set; }
        public string drop { get; set; }
        public string service_type { get; set; }

    }

    public class CreateJobcardResponse
    {
        public string job_card_name_id { get; set; }
        public string job_card_name { get; set; }
        public string registration_no { get; set; }
        public int workshop { get; set; }
        public int user_type { get; set; }
        public string created_by { get; set; }
        public string service_type { get; set; }
        public bool success { get; set; }
        public string error { get; set; }
    }
    #endregion
}
