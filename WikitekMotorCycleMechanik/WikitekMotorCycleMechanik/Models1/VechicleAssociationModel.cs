using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models1
{
    public class VechicleTechician
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public VechicleTechicianList[] results { get; set; }
    }

    public class VechicleTechicianList
    {
        public int id { get; set; }
        public Vehicle_Association vehicle_association { get; set; }
        public Technician_Association technician_association { get; set; }
        public string user { get; set; }
        public DateTime creation_date { get; set; }
        public DateTime from_date { get; set; }
        public DateTime to_date { get; set; }
        public string status { get; set; }
        public object[] technician_vehicle_activity { get; set; }
    }

    public class Vehicle_Association
    {
        public string id { get; set; }
        public string registration_id { get; set; }
        public string vin { get; set; }
        public string vehicle_status { get; set; }
        public bool status { get; set; }
    }

    public class Technician_Association
    {
        public string id { get; set; }
        public string email { get; set; }
        public string status { get; set; }
    }
}
