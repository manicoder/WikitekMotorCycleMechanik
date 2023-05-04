using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class RegisterDongleModel
    {
        public object mac_id { get; set; }
        public object device_type { get; set; }
    }
    public class RegisterDongleRespons
    {
        public RegDongleRespons errorRes { get; set; }
        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
    }

    //public class ErrorRes
    //{
    //    public string error { get; set; }
    //    public bool is_active { get; set; }
    //}
    public class RegDongleRespons
    {
        public string oem { get; set; }
        public string device_type { get; set; }
        public string message { get; set; }
        public string mac_id { get; set; }
        public string user { get; set; }
        public bool is_active { get; set; }
        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
    }

    public class DongleStatusModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<DongleResult> results { get; set; }
        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
    }

    public class DongleResult
    {
        public bool is_active { get; set; }
        public string mac_id { get; set; }
    }
}

