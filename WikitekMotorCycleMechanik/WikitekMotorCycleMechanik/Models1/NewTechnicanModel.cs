using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models1
{
    public class NewTechnicanModel
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public  List<NewTechnicianList> results { get; set; }
    }

    public class NewTechnicianList
    {
        public string id { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string user_type { get; set; }
        public string role { get; set; }
        public bool is_active { get; set; }
        public DateTime created_at { get; set; }
        public object[] meta_tags { get; set; }
        public string status { get; set; }
    }

}
