using APDiagnosticAndroid.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APDiagnosticAndroid.Structures
{
    public struct flashconfig
    {
        public FLASHINDEXTYPE flash_index;
        public byte diag_mode;
        public byte sendseedbyte;
        public byte seedkeynumbytes;
        public SEEDKEYINDEXTYPE seedkeyindex;
        public byte addrdataformat;
        public UInt16 sectorframetransferlen;
        public byte septime;
        public EraseSectorEnum erasesector;
        public ChecksumSectorEnum checksumsector;
    }

    public enum EraseSectorEnum
    {
        None,
        ERASEALLATONCE,
        ERASEBYSECTOR
    }

    public enum ChecksumSectorEnum
    {
        None,
        COMPAREBYSECTOR,
        COMPUTEBYSECTOR,
        NOCHECKSUM
    }
}
