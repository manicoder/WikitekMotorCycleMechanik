using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class AgentModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<AgentResult> results { get; set; }
        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
    }

    public class AgentResult
    {
        public int id { get; set; }
        public string name { get; set; }
        public List<AgentUserType> agent_user_type { get; set; }
    }

    public class AgentUserType
    {
        public string name { get; set; }
        public string code { get; set; }
        public bool status { get; set; }
        public string mobile { get; set; }
        public string country { get; set; }
        public string state { get; set; }
        public string district { get; set; }
        public string pincode { get; set; }
    }
}
