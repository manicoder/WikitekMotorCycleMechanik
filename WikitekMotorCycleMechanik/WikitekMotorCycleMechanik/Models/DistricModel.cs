using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class DistricModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
        public List<DistricResult> results { get; set; }
    }

    public class DistricResult
    {
        public string name { get; set; }
        public List<RsUserTypePincode> rs_user_type_pincode { get; set; }
    }
    public class RsUserTypePincode
    {
        public int id { get; set; }
        public string name { get; set; }
        public District district { get; set; }
    }
    public class District
    {
        public int id { get; set; }
        public string name { get; set; }
        public State state { get; set; }
    }


    public class State
    {
        public int id { get; set; }
        public string name { get; set; }
        public RsUserTypeCountry country { get; set; }
    }
}
