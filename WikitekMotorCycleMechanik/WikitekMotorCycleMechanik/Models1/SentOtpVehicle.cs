﻿using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models1
{

    public class SentOtpVehicle
    {
        public string associatevehicle_id { get; set; }
        public string otp { get; set; }
        //public int associatevehicletechnician_id { get; set; }
        //public int associatetechnician_id { get; set; }
    }
    public class SentOtpAssignTechnician
    {
        public int associatevehicletechnician_id { get; set; }
        public string otp { get; set; }
        //public int associatevehicletechnician_id { get; set; }
        //public int associatetechnician_id { get; set; }
    }
    public class SentOtpTechnician
    {
        public int associatetechnician_id { get; set; }
        public string otp { get; set; }
        //public int associatevehicletechnician_id { get; set; }
        //public int associatetechnician_id { get; set; }
    }
    public class AssociateSentOtpVehicle
    {
        public int associatevehicle_id { get; set; }
        public string otp { get; set; }

    };
}
