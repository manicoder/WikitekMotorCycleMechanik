using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    /// <summary>
    /// Request Model
    /// </summary>
    public class CreateRSAgentModel
    {
        public string name { get; set; }
        public bool status { get; set; }
        public string mobile { get; set; }
        public string gps_location { get; set; }
        public int country { get; set; }
        public int state { get; set; }
        public int segment { get; set; }
        public int district { get; set; }
        public int pincode { get; set; }
        public List<int> user_type { get; set; } = new List<int>();
        //public string rs_country { get; set; }
    }


    /// <summary>
    /// Response Model
    /// </summary>
    public class CreateRsAgentResponse
    {
        public string name { get; set; }
        public bool status { get; set; }
        public string mobile { get; set; }
        public string gps_location { get; set; }
        public int country { get; set; }
        public int state { get; set; }
        public int district { get; set; }
        public int pincode { get; set; }
        public List<int> user_type { get; set; } = new List<int>();
        public string detail { get; set; }
        public HttpStatusCode status_code { get; set; }
    }
}
