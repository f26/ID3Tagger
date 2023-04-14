using System.Text;

namespace ID3Tagger
{
    public class ID3v2Frame
    {
        public string ID { get; set; } = "";
        public int Size { get; set; } = 0;
        public int TotalSize { get { return Size + ID3Constants.ID3_23FRAME_HDR_SIZE; } }
        public bool TagAlterPreservation { get; set; } = false;
        public bool FileAlterPreservation { get; set; } = false;
        public bool ReadOnly { get; set; } = false;
        public bool Compression { get; set; } = false;
        public bool Encryption { get; set; } = false;
        public bool GroupingIdentity { get; set; } = false;
        public bool DataLengthIndicator { get; set; } = false;
        public bool Unsynchronization { get; set; } = false;

        private ID3Version _version = ID3Version.Unknown;

        public ID3v2Frame(ID3Version ver, byte[] buff, int offset)
        {
            if (ver == ID3Version.ID3v2_3)
            {
                ID = Encoding.Latin1.GetString(buff, offset, 4).TrimEnd('\0');
                offset += 4;
                Size = Utils.ReadBigEndian32Bit(buff, offset);
                offset += 4;

                TagAlterPreservation = (buff[offset] & 0b10000000) != 0;
                FileAlterPreservation = (buff[offset] & 0b01000000) != 0;
                ReadOnly = (buff[offset] & 0b00100000) != 0;
                offset++;

                Compression = (buff[offset] & 0b10000000) != 0;
                Encryption = (buff[offset] & 0b01000000) != 0;
                GroupingIdentity = (buff[offset] & 0b00100000) != 0;
            }
            else if (ver == ID3Version.ID3v2_4)
            {
                ID = Encoding.Latin1.GetString(buff, offset, 4).TrimEnd('\0');
                offset += 4;
                Size = SyncSafe.DecodeSyncsafeInt28(buff, offset);
                offset += 4;

                TagAlterPreservation = (buff[offset] & 0b01000000) != 0;
                FileAlterPreservation = (buff[offset] & 0b00100000) != 0;
                ReadOnly = (buff[offset] & 0b00010000) != 0;
                offset++;

                GroupingIdentity = (buff[offset] & 0b01000000) != 0;
                Compression = (buff[offset] & 0b00001000) != 0;
                Encryption = (buff[offset] & 0b00000100) != 0;
                Unsynchronization = (buff[offset] & 0b00000010) != 0;
                DataLengthIndicator = (buff[offset] & 0b00000001) != 0;
            }
            else
            {
                throw new Exception("Unsupported version supplied to ID3v2FrameHeader constructor: " + ver.ToString());
            }

            _version = ver;
        }

        public string AsText(byte[] buff, int offset)
        {
            if (!this.ID.StartsWith("T"))
                throw new Exception("Unable to interpret a non-text frame as text");

            string str = "";
            if (_version == ID3Version.ID3v2_3)
            {
                ID3Encoding encoding = (ID3Encoding)buff[offset + 10];

                if (encoding == ID3Encoding.ISO_8859_1)
                    str = Encoding.Latin1.GetString(buff, offset + 11, Size - 1).TrimEnd('\0');
                else if (encoding == ID3Encoding.UTF_16)
                    str = Encoding.Unicode.GetString(buff, offset + 11, Size - 1).TrimEnd('\0');
            }
            else if (_version == ID3Version.ID3v2_4)
            {
                ID3Encoding encoding = (ID3Encoding)buff[offset + 10];

                if (encoding == ID3Encoding.ISO_8859_1)
                    str = Encoding.Latin1.GetString(buff, offset + 11, Size - 1).TrimEnd('\0');
                else if (encoding == ID3Encoding.UTF_16 || encoding == ID3Encoding.UTF_16BE)
                    str = Encoding.Unicode.GetString(buff, offset + 11, Size - 1).TrimEnd('\0');
                else if (encoding == ID3Encoding.UTF_8)
                    str = Encoding.UTF8.GetString(buff, offset + 11, Size - 1).TrimEnd('\0');
            }

            return str;
        }

        public Image AsImage(byte[] imgData)
        {
            using (var ms = new MemoryStream(imgData))
            {
                return Image.FromStream(ms);
            }
        }

        public byte[] GetPictureData(byte[] buff, int offset)
        {
            if (ID != "APIC")
                throw new Exception("Unable to interpret a non-image frame as an image");

            if (_version != ID3Version.ID3v2_3 && _version != ID3Version.ID3v2_4)
                throw new Exception("Unsupported ID3v2 version");

            offset += ID3Constants.ID3_23FRAME_HDR_SIZE;
            ID3Encoding encoding = (ID3Encoding)buff[offset];
            if (encoding != ID3Encoding.ISO_8859_1 && encoding != ID3Encoding.UTF_8)
                throw new Exception("Unsupported encoding for picture description");

            string mime = ReadISO88591String(buff, ++offset);
            offset += mime.Length + 1;
            byte type = buff[offset];
            if (type != 3)
                throw new Exception("Unsupported image type, only album cover (type: 3) is supported");

            string desc = "";
            if (encoding == ID3Encoding.ISO_8859_1)
                desc = ReadISO88591String(buff, ++offset);
            else
                desc = ReadUTF8String(buff, ++offset);

            // The remaining data is the img size:
            int imgSize = Size
                - desc.Length - 1 // Description + null
                - 1               // Picture type
                - mime.Length - 1 // Mime type + null
                - 1;              // Encoding byte

            offset += desc.Length + 1;
            byte[] imgData = new byte[imgSize];
            Buffer.BlockCopy(buff, offset, imgData, 0, imgSize);

            return imgData;
        }

        public string AsComment(byte[] buff, int offset)
        {
            if (this.ID != "COMM")
                throw new Exception("Unable to interpret non-comment field as a comment");

            if (_version == ID3Version.ID3v2_3 || _version == ID3Version.ID3v2_4)
            {
                offset += ID3Constants.ID3_23FRAME_HDR_SIZE;
                int startOffset = offset;

                ID3Encoding encoding = (ID3Encoding)buff[offset];
                if (encoding != ID3Encoding.ISO_8859_1 && encoding != ID3Encoding.UTF_8)
                    throw new Exception("Unsupported encoding for comment description");

                // Language descriptor.  No details in the standard, assumption is
                // that it's a 3 character designator like "ENG"?
                offset += 3;

                // Null-terminated content description of indeterminate size.
                // Scan for the null termination so we can skip it
                while (buff[offset++] != 0) ;

                int length = Size - (offset - startOffset);
                string comment = Encoding.Latin1.GetString(buff, offset, length).TrimEnd('\0');

                return comment;
            }

            return "";
        }

        public string AsRating(byte[] buff, int offset)
        {
            if (this.ID != "POPM")
                throw new Exception("Unable to interpret non-rating field as a rating");

            if (_version == ID3Version.ID3v2_3 || _version == ID3Version.ID3v2_4)
            {
                offset += ID3Constants.ID3_23FRAME_HDR_SIZE;

                // First field of this frame is a null-terminated email of indeterminate size.
                // Scan for the null termination so we can skip it
                while (buff[offset++] != 0) ; // Skip email
                return buff[offset].ToString();
            }

            return "";
        }

        /// <summary>
        /// Reads a ISO-8859-1 encoded string of indeterminate length at the specified offset.
        /// String end is found by searching for null termination.
        /// </summary>
        /// <param name="buff">Buffer to read</param>
        /// <param name="offset">The offset at which the string starts</param>
        /// <returns>The string read from the buffer</returns>
        private string ReadISO88591String(byte[] buff, int offset)
        {
            string str = "";
            int len = 0;
            while (buff[offset + len] != 0) { len++; }
            str = Encoding.Latin1.GetString(buff, offset, len);
            return str;
        }

        private string ReadUTF8String(byte[] buff, int offset)
        {
            string str = "";
            int len = 0;
            while (buff[offset + len] != 0) { len++; }
            str = Encoding.UTF8.GetString(buff, offset, len);
            return str;
        }
    }
}
