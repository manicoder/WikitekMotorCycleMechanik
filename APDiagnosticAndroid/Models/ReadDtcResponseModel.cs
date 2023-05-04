using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APDiagnosticAndroid.Models
{
    public class ReadDtcResponseModel
    {
        public string status { get; set; }
        public string[,] dtcs { get; set; }
        public UInt16 noofdtc { get; set; } 
    }
}
