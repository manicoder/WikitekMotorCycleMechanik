using System;
using System.Collections.Generic;
using System.Text;

namespace WikitekMotorCycleMechanik.Models
{
    public class PartsSubcategory
    {
        public int count { get; set; }
        public string next { get; set; }
        public object previous { get; set; }
        public List<PartsSubcategoryList> results { get; set; }
    }

    public class PartsSubcategoryList
    {
        public int id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public string name { get; set; }
        public int parent_cat { get; set; }
    }
}
