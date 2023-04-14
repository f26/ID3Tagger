using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3Tagger
{
    public class Utils
    {
        public static int ReadBigEndian32Bit(byte[] buff, int offset)
        {
            return (buff[offset] << 24) +
                   (buff[offset + 1] << 16) +
                   (buff[offset + 2] << 8) +
                    buff[offset + 3];
        }
    }
}
