using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models1
{


    public class AssignTechnicianModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public  List<AssignTechnicianItem> results { get; set; }
    }

    public class AssignTechnicianItem
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
}
