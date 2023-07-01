using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.Models
{
    /// <summary>
    /// Request Model
    /// </summary>
    public class UserModel
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string password { get; set; }
        public string device_type { get; set; }
        public string mac_id { get; set; }
        public ImageSource user_profile_pic { get; set; }
        public string pin_code { get; set; }
        public string rs_agent_id { get; set; }
        public string segment { get; set; }
        public int segment_id { get; set; }
        public string user_type { get; set; }
        public string role { get; set; }
        public string country_name { get; set; }
        public string userType_name { get; set; }
        public string vehicleSegment_name { get; set; }
        public string org { get; set; }
    }

    /// <summary>
    /// Response Model
    /// </summary>
    /// 

    public class RegistrationResponseModel
    {
        public HttpStatusCode status_code { get; set; }
        public string null_error { get; set; }
        public UserResponse userResponse { get; set; }
        public RegistrationError registrationError{ get; set; }
    }
    public class UserResponse
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public string message { get; set; }
        public RegToken token { get; set; }
        public string user_profile_pic { get; set; }
        public string mac_id { get; set; }
    }

    public class RegistrationError
    {
        public string detail { get; set; }
        public string error { get; set; }
        public string null_error { get; set; }
        public List<string> email { get; set; }
        public List<string> mobile { get; set; }
        public List<string> mac_id { get; set; }
        public List<string> country { get; set; }
        public List<string> pincode { get; set; }
    }

    public class RegToken
    {
        public string refresh { get; set; }
        public string access { get; set; }
        public int expire { get; set; }
    }
}
