using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3Tagger
{
    public class ID3Constants
    {
        public const int ID3_HDR_SIZE = 10;
        public const int ID3_22FRAME_HDR_SIZE = 6;
        public const int ID3_23FRAME_HDR_SIZE = 10; // NOTE: 2.3 and 2.4 have same frame size
    }
}
