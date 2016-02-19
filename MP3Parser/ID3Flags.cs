using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Parser
{
    public class ID3Flags
    {
        public bool Unsynchronisation = false;
        public bool ExtendedHeader = false;
        public bool Experimental = false;

        public ID3Flags(byte data)
        {
            Unsynchronisation = (data >> 7) == 1;
            ExtendedHeader = (data << 1) >> 7 == 1;
            Experimental = (data << 2) >> 7 == 1;
        }
    }
}
