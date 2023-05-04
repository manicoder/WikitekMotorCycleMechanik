using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class ExpertModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<ExpertModel> results { get; set; }
    }

    public class Expert
    {
        public int id { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string status { get; set; }
        public string workshop { get; set; }
        public string workshop_city { get; set; }
        public string remote_session_id { get; set; }
        public string remote_session_internal_id { get; set; }
        public object auto_session_internal_id { get; set; }
        public object auto_session_id { get; set; }
        public string status_color { get; set; }
        public bool btnIsActive { get; set; }
    }

    public class RemoteRequestModel
    {
        public string id { get; set; }
        public string remote_session_id { get; set; }
        public string job_card_session { get; set; }
        public object case_id { get; set; }
        public string status { get; set; }
        public int expert_user { get; set; }
        public object expert_device { get; set; }
        public string expert_email { get; set; }
        public bool request_status { get; set; }
    }
}
