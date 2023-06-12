using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models1
{
    public class StartTripModel
    {
        public int associatevehicletechnician_id { get; set; }
        public DateTime start_date { get; set; }
        public string latlong { get; set; }
    }
}
