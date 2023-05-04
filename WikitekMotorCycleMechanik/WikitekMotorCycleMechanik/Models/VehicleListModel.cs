using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class VehicleListModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<VehicleListResult> results { get; set; }
        public bool status { get; set; }
        public string message { get; set; }
    }

    public class VehicleListResult
    {
        public string id { get; set; }
        public string registration_id { get; set; }
        public string vin { get; set; }
        public User user { get; set; }
        public Oem oem { get; set; }    
        public string picture { get; set; }
        public bool status { get; set; }

        public int vehicle_model { get; set; }
        public int sub_model { get; set; }
        public int model_year { get; set; }
    }

    public class User
    {
        public string id { get; set; }
        public string email { get; set; }
    }
}
