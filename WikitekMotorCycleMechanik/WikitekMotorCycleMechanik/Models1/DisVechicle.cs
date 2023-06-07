using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models1
{

    public class DisVechicle
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<DisVechicleList> results { get; set; }
    }

    public class DisVechicleList
    {
        public string id { get; set; }
        public string registration_id { get; set; }
        public string vin { get; set; }
        public int vehicle_model { get; set; }
        public int sub_model { get; set; }
        public int model_year { get; set; }
        public Oem oem { get; set; }
        public User user { get; set; }
        public string picture { get; set; }
        public int associate_workshop { get; set; }
        public string vehicle_status { get; set; }
        public bool status { get; set; }
    }

    public class Oem
    {
        public int id { get; set; }
        public string name { get; set; }
        public object slug { get; set; }
        public Attachment attachment { get; set; }
    }

    public class Attachment
    {
        public string media_type { get; set; }
        public string attachment { get; set; }
        public string name { get; set; }
        public bool status { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string email { get; set; }
        public string status { get; set; }
    }



}
