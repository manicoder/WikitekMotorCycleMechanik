using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class PaymentModel
    {
    }

    public class PaymentResponseModel
    {
        public object Data { get; set; }
        public object ExternalWallet { get; set; }
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Signature { get; set; }
        public string UserContact { get; set; }
        public string UserEmail { get; set; }
    }

    public class GenerateOrderIdModel
    {
        public int amount { get; set; }
        public string currency { get; set; }
        public string receipt { get; set; }
    }

    public class GenerateOrderIdResponseModel
    {
        public string id { get; set; }
        public string entity { get; set; }
        public int amount { get; set; }
        public int amount_paid { get; set; }
        public int amount_due { get; set; }
        public string currency { get; set; }
        public string receipt { get; set; }
        public object offer_id { get; set; }
        public string status { get; set; }
        public int attempts { get; set; }
        public List<object> notes { get; set; }
        public int created_at { get; set; }

        public HttpStatusCode status_code { get; set; }
        public string error { get; set; }
    }

    //////
    /// Send data to server
    /////
    ///

    public class PaymentServerRequestModel
    {
        public ShipmentAddress shipment_address { get; set; }
        public BillingAddress billing_address { get; set; }
        public Paymentdetail payment_detail { get; set; }
        public List<CartList> cart_list { get; set; } = new List<CartList>();
    }
    public class ShipmentAddress
    {
        public string address { get; set; }
        public string country { get; set; }
        public string pin_code { get; set; }
        public string user_id { get; set; }
    }

    public class BillingAddress
    {
        public string address { get; set; }
        public string country { get; set; }
        public string pin_code { get; set; }
        public string user_id { get; set; }
    }

    public class Paymentdetail
    {
        public string order_id { get; set; }
        public string payment_id { get; set; }
        public string signature { get; set; }
        public string user_contact { get; set; }
        public string user_email { get; set; }
        public string user_id { get; set; }
        public double extended_price { get; set; }
        public string currency { get; set; }
        public PackageResult package_detail { get; set; }
        public string gst_no { get; set; }
    }

    public class CartList
    {
        public string part_id { get; set; }
        public string part_number { get; set; }
        public double unit_price { get; set; }
        public int quantity { get; set; }
    }


    //////
    /// Send data to server
    /////
    /////


    #region Market place(order) server response models
    public class PaymentServerResponseModel
    {
        public string id { get; set; }
        public string user { get; set; }
        public string razorpay_payment_id { get; set; }
        public string razorpay_order_id { get; set; }
        public string razorpay_signature { get; set; }
        public string shipping_address { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public int total_amount { get; set; }
        public string currency { get; set; }
        public DateTime timestamp { get; set; }
        public List<OrderItem> order_items { get; set; }
        public HttpStatusCode status_code { get; set; }
        public string error { get; set; }
    }

    public class OrderItem
    {
        public int id { get; set; }
        public string order_id { get; set; }
        public string parts_id { get; set; }
        public int price { get; set; }
        public int quantity { get; set; }
    }
    #endregion


    #region Subscription server response models
    public class SubscriprionPaymentServerResponseModel
    {
        public int id { get; set; }
        public string user { get; set; }
        public string razorpay_payment_id { get; set; }
        public string razorpay_order_id { get; set; }
        public string razorpay_signature { get; set; }
        public int validity_period { get; set; }
        public double amount { get; set; }
        public List<PaySubscription> subscriptions { get; set; }
        public HttpStatusCode Status_code { get; set; }
        public string error { get; set; }
    }

    public class PaySubscription
    {
        public string id { get; set; }
        public string user { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public DateTime start_date { get; set; }
        public DateTime end_date { get; set; }
        public double cost { get; set; }
        public int validity_period { get; set; }
        public string status { get; set; }
        public string package { get; set; }
        public int order_id { get; set; }
        public bool is_active { get; set; }
    }
    #endregion

    ///////
    ///  Purchase package request model
    //////
    ///

    public class PurchasePackageModel
    {
        public string user { get; set; }
        public string razorpay_payment_id { get; set; }
        public string razorpay_order_id { get; set; }
        public string razorpay_signature { get; set; }
        public int validity_period { get; set; }
        public double amount { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public DateTime start_date { get; set; }
        public string code { get; set; }
        public int country { get; set; }
        public int segments { get; set; }
        public string user_type { get; set; }
        public int grace_period { get; set; }
        public string recurrence_unit { get; set; }
        public int recurrence_period { get; set; }
        public int package_visibility { get; set; }
        public string package_name { get; set; }
        public bool oem_specific { get; set; }
        public int usability { get; set; }
        public List<int> oem { get; set; }
        public List<int> runtime_params { get; set; }

        //public string order_id { get; set; }
        //public string payment_id { get; set; }
        //public string signature { get; set; }
        //public string user_contact { get; set; }
        //public string user_email { get; set; }
        //public string user_id { get; set; }
        //public double extended_price { get; set; }
        //public string currency { get; set; }
        //public int segments { get; set; }
        //public string user_type { get; set; }
        //public bool oem_specific { get; set; }
        //public int package_visibility { get; set; }
        //public List<int> oem { get; set; }
        //public int usability { get; set; }
        //public PackageDetail package_detail { get; set; }
    }

    public class PackageDetail
    {
        public string id { get; set; }
        public object slug { get; set; }
        public string code { get; set; }
        public Country country { get; set; }
        public List<string> runtime_params { get; set; }
        public object package_type { get; set; }
        public string segments { get; set; }
        public double amount { get; set; }
        public object cost { get; set; }
        public int recurrence_period { get; set; }
        public string recurrence_unit { get; set; }
        public string description { get; set; }
        public int grace_period { get; set; }
        public bool is_active { get; set; }
    }


}
