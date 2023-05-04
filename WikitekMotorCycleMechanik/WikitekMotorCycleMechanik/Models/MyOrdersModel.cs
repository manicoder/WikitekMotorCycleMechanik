using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class MyOrdersModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<MyOrdersResults> results { get; set; }
        public HttpStatusCode status_code { get; set; }
    }

    public class MyOrdersResults
    {
        public string id { get; set; }
        public string user { get; set; }
        public string razorpay_payment_id { get; set; }
        public string razorpay_order_id { get; set; }
        public string razorpay_signature { get; set; }
        public string shipping_address { get; set; }
        public string billing_address { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public int total_amount { get; set; }
        public string currency { get; set; }
        public string gst_no { get; set; }
        public string pincode { get; set; }
        public string country { get; set; }
        public string billing_pincode { get; set; }
        public string billing_country { get; set; }
        public DateTime timestamp { get; set; }
        public List<OrderItem> order_items { get; set; }
    }
}
