using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class MibudUserModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<MibudUser> results { get; set; }

        public bool status { get; set; }
        public string message { get; set; }
    }

    public class MibudUser
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
        public List<object> meta_tags { get; set; }
    }

}
