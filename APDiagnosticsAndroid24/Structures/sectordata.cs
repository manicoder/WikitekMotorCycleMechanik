using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APDiagnosticsAndroid24.Structures
{
    struct sectordata
    {
        public UInt32 startaddress;
        public UInt32 noofbytes;
        public byte[] bytearray;
        public UInt16 sectorchecksum;
    }

}
