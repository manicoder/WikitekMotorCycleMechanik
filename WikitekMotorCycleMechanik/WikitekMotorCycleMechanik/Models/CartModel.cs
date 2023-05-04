using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Text;
using WikitekMotorCycleMechanik.ViewModels;

namespace WikitekMotorCycleMechanik.Models
{
    public class CartModel
    {
        public int count { get; set; }
        public object next { get; set; }
        public object previous { get; set; }
        public List<CartResult> results { get; set; }
        public HttpStatusCode status_code { get; set; }
        public string error { get; set; }

    }

    public class CartResult
    {
        public string user { get; set; }
        public int id { get; set; }
        public List<CartItem> cart_items { get; set; }
    }

    public class CartItem : BaseViewModel
    {
        public int id { get; set; }
        public PartDetail parts_id { get; set; }
        //public int quantity { get; set; }
        public bool fav { get; set; }
        public int cart_id { get; set; }
        public double unit_price { get; set; }
        //public string part_number { get; set; }

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

        private double _total_price;
        public double total_price
        {
            get => _total_price;
            set
            {
                _total_price = value;
                OnPropertyChanged("total_price");
            }
        }

        private bool _isVisible;
        public bool isVisible
        {
            get => _isVisible;
            set
            {
                _isVisible = value;
                OnPropertyChanged("isVisible");
            }
        }

        private double _discount;
        public double discount
        {
            get => _discount;
            set
            {
                _discount = value;
                OnPropertyChanged("discount");
            }
        }

        private Color _discountColor;
        public Color discountColor
        {
            get => _discountColor;
            set
            {
                _discountColor = value;
                OnPropertyChanged("discountColor");
            }
        }

        private int _quantity;
        public int quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged("quantity");
            }
        }
    }

    public class PartDetail
    {
        public string id { get; set; }
        public List<Price> prices { get; set; }
        public string part_number { get; set; }
        public string short_description { get; set; }
        public string long_description { get; set; }
        public int part_type { get; set; }
        public double mrp { get; set; }
        public bool serialization { get; set; }
    }

    public class Price
    {
        public string id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public bool is_active { get; set; }
        public double price { get; set; }
        public int min_quantity { get; set; }
        public int max_quantity { get; set; }
        public string discount { get; set; }
        public string part_id { get; set; }
    }
}
