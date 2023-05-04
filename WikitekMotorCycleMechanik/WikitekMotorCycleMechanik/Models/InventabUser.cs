using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class InventabUser
    {
        public bool success { get; set; }
        public DataIT data { get; set; }
        public string message { get; set; }
    }

    public class DataIT
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public Org org { get; set; }
        public List<Role> role { get; set; }
        public bool online { get; set; }
        public bool active { get; set; }
        public AuthToken auth_token { get; set; }
        public string user_id { get; set; }
    }

    public class AuthToken
    {
        public string access { get; set; }
        public string refresh { get; set; }
    }

    public class Org
    {
        public string id { get; set; }
        public List<object> banks { get; set; }
        public object country { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string org_code { get; set; }
        public string company_type { get; set; }
        public string company_name { get; set; }
        public object logo { get; set; }
        public object address { get; set; }
        public string pan_no { get; set; }
        public object pan_cert { get; set; }
        public object pincode { get; set; }
        public string contact_person { get; set; }
        public object payment_term { get; set; }
        public List<object> departments { get; set; }
        public List<object> meta_tags { get; set; }
    }

    public class Role
    {
        public string id { get; set; }
        public string name { get; set; }
        public string display_name { get; set; }
        public string slug { get; set; }
    }
}
