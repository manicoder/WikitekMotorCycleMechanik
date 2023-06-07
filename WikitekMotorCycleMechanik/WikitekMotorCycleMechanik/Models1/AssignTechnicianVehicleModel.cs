using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models1
{
    public class AssignTechnicianVehicleModel
    {
        public string associate_technician_id { get; set; }
        public int associate_vehicle_id { get; set; }
        public string user_id { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
    }
}
