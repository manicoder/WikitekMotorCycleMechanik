using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APDongleCommAnroid.Models
{
    public class ResponseArrayStatus
    {
        public string ECUResponseStatus { get; set; }
        public byte[] ECUResponse { get; set; }
        public byte[] ActualDataBytes { get; set; }
    }
}
