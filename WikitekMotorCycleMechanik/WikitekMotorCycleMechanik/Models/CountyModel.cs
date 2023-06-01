
using System.Collections.Generic;
using System.Net;

namespace WikitekMotorCycleMechanik.Models
{
    public class CountyModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<Countries> results { get; set; }
        public string error { get; set; }
        public HttpStatusCode status_code { get; set; }
    }

    public class Countries
    {
        public string name { get; set; }
        public List<RsUserTypeCountry> rs_user_type_country { get; set; }
    }

    public class RsUserTypeCountry
    {
        public int id { get; set; }
        public string name { get; set; }
    }
}