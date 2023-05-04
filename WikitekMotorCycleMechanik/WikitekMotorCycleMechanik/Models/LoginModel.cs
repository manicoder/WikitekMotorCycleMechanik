using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class LoginModel
    {
        public string email { get; set; }
        public string password { get; set; }
        public string device_type { get; set; }
        public string mac_id { get; set; }
    }

    /// <summary>
    /// // Response Model
    /// </summary>

    public class LoginResponse
    {
        public bool success { get; set; }
        public string email { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string user_id { get; set; }
        public string user_type { get; set; }
        public string mobile { get; set; }
        public string picture { get; set; }
        public byte[] picture_local { get; set; }
        public string dongle { get; set; }
        public string last_login { get; set; }
        public Token token { get; set; }
        public List<Subscription> subscriptions { get; set; }
        public string role { get; set; }
        public string error { get; set; }
        public string mac_id { get; set; }
        public List<Dongles> dongles { get; set; }
        public Agent agent { get; set; }
        public HttpStatusCode status_code { get; set; }
        public string segment { get; set; }
        public int? segment_id { get; set; }
    }

    public class Dongles
    {
        public string mac_id { get; set; }
        public object parent { get; set; }
        public string serial_number { get; set; }
        public string device_type { get; set; }
        public bool is_active { get; set; }
    }

    public class Token
    {
        public string refresh { get; set; }
        public string access { get; set; }
        public int expire { get; set; }
    }

    public class Workshop
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string mobile { get; set; }
        public int country { get; set; }
    }

    public class Agent
    {
        public Workshop workshop { get; set; }
    }

    public class Subscription
    {
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public string code { get; set; }
        public int country { get; set; }
        public Segments segments { get; set; }
        public int user_type { get; set; }
        public int grace_period { get; set; }
        public int recurrence_period { get; set; }
        public string package_visibility { get; set; }
        public bool oem_specific { get; set; }
        public string usability { get; set; }
        public string package_name { get; set; }
        public List<string> runtime_params { get; set; }
        public List<OemsSpecific> oem { get; set; }
        public string show_end_date { get; set; }
        public string show_start_date { get; set; }
        public string txt_is_oem_speci { get; set; }
    }

    public class Package
    {
        public Segments segments { get; set; }
        public string code { get; set; }
        public List<string> runtime_params { get; set; }
    }

    public class Segments
    {
        public int id { get; set; }
        public string segment_name { get; set; }
    }

    public class OemsSpecific
    {
        public int id { get; set; }
        public string name { get; set; }
        public string oem_file { get; set; }
        public AttachmentModel attachment { get; set; }
        public byte[] oem_file_local { get; set; }
    }


    //public class RunTimeParams
    //{
    //    public List<int> runtime_params { get; set; }
    //}


}