using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class ShipmentAddressModel
    {
        public string user_name { get; set; }
        public string country { get; set; }
        public string pin_code { get; set; }
        public string address { get; set; }
        public string state { get; set; }
    }

    public class BillingAddressModel
    {
        public string user_name { get; set; }
        public string country { get; set; }
        public string pin_code { get; set; }
        public string address { get; set; }
        public string state { get; set; }
    }
}
