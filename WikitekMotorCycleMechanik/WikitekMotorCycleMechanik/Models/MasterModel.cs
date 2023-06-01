using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace WikitekMotorCycleMechanik.Models
{
    public class MasterModel
    {
        public string Title { get; set; }
        public string IconSource { get; set; }
        public Thickness IconMargin { get; set; }
        public Type TargetType { get; set; }
        public object[] args { get; set; }
    }
}
