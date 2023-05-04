using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class HotPartsModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<HotPartsModelList> results { get; set; }
    }

    public class HotPartsModelList
    {
        public string id { get; set; }
        public string marketplace_name { get; set; }
        public string org { get; set; }
        public List<MarketplaceBanner> marketplace_banner { get; set; }
        public List<MarketplaceHotproduct> marketplace_hotproducts { get; set; }
        public bool is_active { get; set; }
    }

    public class MarketplaceHotproduct
    {
        public string id { get; set; }
        public PartsList part_no { get; set; }
        public int priority { get; set; }
        public bool is_active { get; set; }
    }

    public class MarketplaceBanner
    {
        public string id { get; set; }
        public string banner_name { get; set; }
        public Attachment attachment { get; set; }
        public int priority { get; set; }
        public string action { get; set; }
        public object url { get; set; }
        public string part_no { get; set; }
        public bool is_active { get; set; }
    }

}
