using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class MobileNumberModel
    {
        public string message { get; set; }
        public string success { get; set; }
        public string code { get; set; }
        public string description { get; set; }

        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
    }

    public class VerifyOtpModel
    {
        public string message { get; set; }
        public string reset_url { get; set; }
        public bool success { get; set; }

        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
    }
}
