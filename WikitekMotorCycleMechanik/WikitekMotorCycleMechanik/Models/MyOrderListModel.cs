using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class MyOrderListModel
    {
        public string orderId { get; set; }
        public int total_amount { get; set; }
        public string shipping_address { get; set; }
        public string billing_address { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string gst_no { get; set; }
        public string pruductType { get; set; }
        public List<SerializedPart> serialized_parts { get; set; }
        public string short_description { get; set; }
        public string long_description { get; set; }
        public double price { get; set; }
        public int quantity { get; set; }
        public string part_number { get; set; }
        public int warranty_period { get; set; }
        public string invoice_number { get; set; }
        public string payment_date { get; set; }
        public bool shipped_status { get; set; }
    }
}
