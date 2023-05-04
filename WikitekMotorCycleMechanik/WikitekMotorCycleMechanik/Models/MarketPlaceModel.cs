using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using WikitekMotorCycleMechanik.ViewModels;

namespace WikitekMotorCycleMechanik.Models
{
    public class MarketPlaceModel//:BaseViewModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<MarketPlaceResult> results { get; set; }
        public HttpStatusCode status_code { get; set; }
        public string error { get; set; }
    }

    public class MarketPlaceResult : BaseViewModel
    {
        public int id { get; set; }
        public string sparepart_id { get; set; }
        public string part_description { get; set; }
        public string part_number { get; set; }
        public string brand { get; set; }
        public double? price { get; set; }
        public string types { get; set; }
        public string gst { get; set; }
        public string picture { get; set; }
        public object equi_oe_part { get; set; }
        public List<SparePart> spare_parts { get; set; }

        private double _basic_price;
        public double basic_price
        {
            get => _basic_price;
            set
            {
                _basic_price = value;
                OnPropertyChanged("basic_price");
            }
        }

        private double _extended_price;
        public double extended_price
        {
            get => _extended_price;
            set
            {
                _extended_price = value;
                OnPropertyChanged("extended_price");
            }
        }

        private int _quantity = 1;
        public int quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged("quantity");
            }
        }
        //public string extended_price { get; set; }
        //public string show_description{ get; set; }
        //public string show_quantity_control { get; set; }
    }

    public class SparePart
    {
        public int name { get; set; }
    }

    public class AddCartModel
    {
        public string parts_id { get; set; }
        public int qty { get; set; }
        public bool fav { get; set; }
        public double unit_price { get; set; }
    }

    public class FilterModel : BaseViewModel
    {
        public string filter_by { get; set; }
        //public bool is_ckecked { get; set; }

        private bool _is_ckecked;
        public bool is_ckecked
        {
            get => _is_ckecked;
            set
            {
                _is_ckecked = value;
                OnPropertyChanged("is_ckecked");
            }
        }
    }
}
