using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ID3Tagger
{
    /// <summary>
    /// Class used to parse/represent/serialize an ID3v2 header.  Supports reading ID3v2.2, ID3v2.3, and ID3v2.4
    /// headers and writing ID3v2.3 and ID3v2.4 headers
    /// </summary>
    public class ID3v2Header
    {
        public ID3Version Version { get; set; } = 0;
        public int Size { get; set; } = 0;
        public int TotalSize { get { return Size + ID3Constants.ID3_HDR_SIZE; } }
        public bool HasExtHeader { get; set; } = false;
        public bool HasFooter { get; set; } = false;
        public bool Unsynchronization { get; set; } = false;
        public bool Compression { get; set; } = false;
        public bool Experimental { get; set; } = false;

        public ID3v2Header() { }
        public ID3v2Header(byte[] buff)
        {
            if (buff.Length < ID3Constants.ID3_HDR_SIZE)
            {
                throw new Exception("ID3v2 header cannot fit in provided buffer");
            }

            // Is the "ID3" field present?
            string magic = Encoding.Latin1.GetString(buff, 0, 3);
            if (magic != "ID3") throw new Exception("ID3v2 identifier not found");
            Version = (ID3Version)buff[3];
            byte revision = buff[4];
            if (Version < ID3Version.ID3v2_2 || Version > ID3Version.ID3v2_4)
                throw new Exception("Unsupported ID3v2 version: " + Version.ToString());
            if (revision != 0) throw new Exception("Unsupported ID3v2 revision: " + revision.ToString());

            // Flags
            byte flags = buff[5];
            Unsynchronization = (flags & 0b10000000) != 0;
            switch (Version)
            {
                case ID3Version.ID3v2_2:
                    {
                        Compression = (flags & 0b01000000) != 0;
                        bool other = (flags & 0b10111111) != 0;
                        if (other) throw new Exception("Other flags in ID3v2 header not supported");
                        break;
                    }
                case ID3Version.ID3v2_3:
                    {
                        HasExtHeader = (flags & 0b01000000) != 0;
                        Experimental = (flags & 0b00100000) != 0;
                        bool other = (flags & 0b00011111) != 0;
                        if (other) throw new Exception("Other flags in ID3v2 header not supported");
                        break;
                    }
                case ID3Version.ID3v2_4:
                    {
                        HasExtHeader = (flags & 0b01000000) != 0;
                        Experimental = (flags & 0b00100000) != 0;
                        HasFooter = (flags & 0b00010000) != 0;
                        bool other = (flags & 0b00001111) != 0;
                        if (other) throw new Exception("Other flags in ID3v2 header not supported");
                        break;
                    }
            }

            // NOTE: Size is the total size taken up by the extended header (if present), frames, 
            // and padding (if present).  The header and footer size is not included in this size 
            // (10 bytes each)
            Size = SyncSafe.DecodeSyncsafeInt28(buff, 6);
        }

        public byte[] ToBytes(int size)
        {
            if (size == 0) return new byte[0];

            MemoryStream ms = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(Encoding.Latin1.GetBytes("ID3"));

                switch (Version)
                {
                    case ID3Version.ID3v2_3:
                    case ID3Version.ID3v2_4:
                        bw.Write((byte)Version); // Version major
                        break;
                    default:
                        throw new Exception("Unsupported version");
                }

                bw.Write((byte)0); // Version minor
                bw.Write((byte)0); // Flags
                bw.Write(SyncSafe.ToSyncSafe28(size));
            }

            return ms.ToArray();
        }
    }
}
