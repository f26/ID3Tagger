using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ID3Tagger
{
    public class SyncSafe
    {
        public static int DecodeSyncsafeInt28(byte[] data, int offset)
        {
            int byte1 = ((data[offset + 0] & 0b01111111) << 28);
            int byte2 = ((data[offset + 1] & 0b01111111) << 14);
            int byte3 = ((data[offset + 2] & 0b01111111) << 7);
            int byte4 = ((data[offset + 3] & 0b01111111) << 0);
            return byte1 + byte2 + byte3 + byte4;
        }

        public static int DecodeSyncsafeInt21(byte[] data, int offset)
        {
            int byte1 = ((data[offset + 0] & 0b01111111) << 14);
            int byte2 = ((data[offset + 1] & 0b01111111) << 7);
            int byte3 = ((data[offset + 2] & 0b01111111) << 0);
            return byte1 + byte2 + byte3;
        }

        public static byte[] ToSyncSafe28(uint num)
        {
            byte[] bytes = new byte[4];
            bytes[0] = (byte)((num & 0b01111111000000000000000000000) >> 21);
            bytes[1] = (byte)((num & 0b00000000111111100000000000000) >> 14);
            bytes[2] = (byte)((num & 0b00000000000000011111110000000) >> 7);
            bytes[3] = (byte)((num & 0b00000000000000000000001111111) >> 0);
            return bytes;
        }

        public static byte[] ToSyncSafe28(int num)
        {
            return ToSyncSafe28((uint)num);
        }
    }
}
