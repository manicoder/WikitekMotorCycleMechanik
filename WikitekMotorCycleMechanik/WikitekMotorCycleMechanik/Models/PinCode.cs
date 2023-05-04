using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class PinCode
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<PincodeInfo> results { get; set; }
        public HttpStatusCode status_code { get; set; }
    }

    public class PincodeInfo
    {
        public int id { get; set; }
        public string name { get; set; }
        public DistrictNew district { get; set; }
        public object slug { get; set; }
    }

    public class CountryNew
    {
        public int id { get; set; }
        public string name { get; set; }
        public object country_code { get; set; }
        public object currency_code { get; set; }
        public string is_active { get; set; }
        public object slug { get; set; }
    }

    public class DistrictNew
    {
        public int id { get; set; }
        public string name { get; set; }
        public StateNew state { get; set; }
        public object slug { get; set; }
    }

    public class StateNew
    {
        public int id { get; set; }
        public string name { get; set; }
        public CountryNew country { get; set; }
        public object slug { get; set; }
    }
}
