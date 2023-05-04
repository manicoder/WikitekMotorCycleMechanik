using System;
using System.Collections.Generic;
using System.Text;
using WikitekMotorCycleMechanik.ViewModels;

namespace WikitekMotorCycleMechanik.Models
{
    public class InvoiceModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<InvoiceResult> results { get; set; }
    }

    public class InvoiceResult
    {
        public string id { get; set; }
        public List<PartsInvoice> parts_invoice { get; set; }
        public OrgInvoice org { get; set; }
        public InvoiceType invoice_type { get; set; }
        public BillingAddressInvoice billing_address { get; set; }
        public ShippingAddressInvoice shipping_address { get; set; }
        public PaymentTerm payment_term { get; set; }
        public CreatedByInvoice created_by { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string invoice_number { get; set; }
        public object po_number { get; set; }
        public string payment_date { get; set; }
        public string delivery_term { get; set; }
        public string invoice_date { get; set; }
        public object docket_no { get; set; }
        public bool approved { get; set; }
        public bool assigned { get; set; }
        public object invoice_comment { get; set; }
        public string order_id { get; set; }
        public object project { get; set; }
        public object billing_org { get; set; }
        public object po_no { get; set; }
        public string sale_order { get; set; }
        public object user { get; set; }
        public object picking_list { get; set; }
        public object courier { get; set; }
        public object approved_by { get; set; }
        public object status { get; set; }
    }

    public class PartsInvoice
    {
        public string id { get; set; }
        public PartsNo parts_no { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public int quantity { get; set; }
        public string customer_part_no { get; set; }
        public double price { get; set; }
        public int warranty { get; set; }
        public string short_description { get; set; }
        public string invoice { get; set; }
        public string part { get; set; }
    }

    public class InvoiceType
    {
        public string id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string name { get; set; }
    }

    public class OrgInvoice
    {
        public string id { get; set; }
        public List<object> banks { get; set; }
        public CountryInvoice country { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string org_code { get; set; }
        public string company_type { get; set; }
        public string company_name { get; set; }
        public object logo { get; set; }
        public string address { get; set; }
        public object pan_no { get; set; }
        public object pan_cert { get; set; }
        public string pincode { get; set; }
        public string contact_person { get; set; }
        public int payment_term { get; set; }
        public List<object> departments { get; set; }
        public List<object> meta_tags { get; set; }
    }

    public class BillingAddressInvoice
    {
        public string id { get; set; }
        public Org2 org { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string address { get; set; }
        public object gst_no { get; set; }
        public object gst_cert { get; set; }
        public object address_type { get; set; }
        public string pincode { get; set; }
        public string country { get; set; }
    }

    public class ShippingAddressInvoice
    {
        public string id { get; set; }
        public Org2 org { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string address { get; set; }
        public object gst_no { get; set; }
        public object gst_cert { get; set; }
        public object address_type { get; set; }
        public string pincode { get; set; }
        public string country { get; set; }
    }

    public class ContactPerson
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
    }

    public class Org2
    {
        public string company_name { get; set; }
        public ContactPerson contact_person { get; set; }
    }

    public class PaymentTerm
    {
        public int id { get; set; }
        public string term { get; set; }
    }

    public class CreatedByInvoice
    {
        public string id { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public Org2 org { get; set; }
    }

    public class PartsNo
    {
        public string id { get; set; }
        public PartType part_type { get; set; }
        public object uom { get; set; }
        public GstItm gst_itm { get; set; }
        public List<SerializedPart> serialized_parts { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string internal_part_no { get; set; }
        public string part_number { get; set; }
        public bool bom { get; set; }
        public string short_description { get; set; }
        public string long_description { get; set; }
        public double mrp { get; set; }
        public string weight { get; set; }
        public string length { get; set; }
        public string breadth { get; set; }
        public string height { get; set; }
        public bool serialization { get; set; }
        public bool is_active { get; set; }
        public int warranty_period { get; set; }
        public string warranty_terms { get; set; }
        public string manufacturer { get; set; }
        public int part_category { get; set; }
        public int sub_category { get; set; }
        public List<int> meta_tags { get; set; }
        public List<object> market_place { get; set; }
    }

    public class SerializedPart : BaseViewModel
    {
        public string id { get; set; }
        public string serial_number { get; set; }
        public string imageUrl { get; set; }
        
        private bool _isExpanded;
        public bool isExpanded
        {
            get => _isExpanded;
            set
            {
                _isExpanded = value;
                OnPropertyChanged("isExpanded");
            }
        }

        private string _ic_updown = "ic_sort_down.png";
        public string ic_updown
        {
            get => _ic_updown;
            set
            {
                _ic_updown = value;
                OnPropertyChanged("ic_updown");
            }
        }
    }

    public class GstItm
    {
        public string id { get; set; }
        public string hsn_or_sac { get; set; }
        public string description { get; set; }
    }

    public class CountryInvoice
    {
        public string id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string name { get; set; }
        public object slug { get; set; }
        public string region { get; set; }
        public string code { get; set; }
        public string currency_name { get; set; }
        public object postal_code_format { get; set; }
        public object postal_code_regex { get; set; }
        public string currency { get; set; }
    }
}
