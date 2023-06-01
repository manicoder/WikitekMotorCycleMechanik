using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using WikitekMotorCycleMechanik.ViewModels;

namespace WikitekMotorCycleMechanik.Models
{
    public class PartsModel
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<PartsList> results { get; set; }
        public HttpStatusCode status_code { get; set; }
    }

    public class MetaTag
    {
        public string name { get; set; }
    }

    public class PartsList : BaseViewModel
    {
        public string id { get; set; }
        public List<Prices> prices { get; set; }
        public List<MetaTag> meta_tags { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string internal_part_no { get; set; }
        public string part_number { get; set; }
        public bool bom { get; set; }
        public string short_description { get; set; }
        public string long_description { get; set; }
        public double? mrp { get; set; }
        public string weight { get; set; }
        public string length { get; set; }
        public string breadth { get; set; }
        public string height { get; set; }
        public bool serialization { get; set; }
        public bool is_active { get; set; }
        public int warranty_period { get; set; }
        public string warranty_terms { get; set; }
        public PartType part_type { get; set; }
        public string manufacturer { get; set; }
        public PartCategory part_category { get; set; }
        public string gst_itm { get; set; }
        public SubCategory sub_category { get; set; }
        public List<Document> documents { get; set; }
        public Default @default { get; set; }
    }

    public class Default
    {
        public string attachment { get; set; }
    }

    public class Document
    {
        public string id { get; set; }
        public string name { get; set; }
        public Attachment attachment { get; set; }
        public string parts { get; set; }
    }

    public class Attachment
    {
        public string media_type { get; set; }
        public string attachment { get; set; }
        public string name { get; set; }
        public bool status { get; set; }
        public string parts { get; set; }

    }

    public class Prices
    {
        public string id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public bool is_active { get; set; }
        public double? price { get; set; }
        public int min_quantity { get; set; }
        public int max_quantity { get; set; }
        public string discount { get; set; }
        public string part_id { get; set; }
    }

    public class PartCategory
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string name { get; set; }
    }

    public class PartType
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string name { get; set; }
    }

    public class SubCategory
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string name { get; set; }
        public int parent_cat { get; set; }
    }
}
