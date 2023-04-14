using BrightIdeasSoftware;
using Microsoft.VisualBasic.ApplicationServices;
using System.CodeDom;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ID3Tagger
{
    /// <summary>
    /// Class to parse/represent/serialize ID3 tags.  Supports parsing of ID3v1, ID3v2.3, and ID3v2.4 tags.  Will parse
    /// tags from a provided buffer containing the raw bytes to an MP3 file with ID3 tag(s).  Only a subset of all
    /// possible tags are parsed and saved.  Any other tags are skipped/ignored and will effectively be stripped by
    /// using this class to parse and re-serialize the ID3 tag.
    /// </summary>
    public class ID3Tag
    {
        string[] GenresID3v1 = { "Blues", "Classic Rock", "Country", "Dance", "Disco", "Funk", "Grunge", "Hip-Hop", "Jazz", "Metal", "New Age", "Oldies", "Other", "Pop", "Rhythm and Blues", "Rap", "Reggae", "Rock", "Techno", "Industrial", "Alternative", "Ska", "Death Metal", "Pranks", "Soundtrack", "Euro-Techno", "Ambient", "Trip-Hop", "Vocal", "Jazz & Funk", "Fusion", "Trance", "Classical", "Instrumental", "Acid", "House", "Game", "Sound clip", "Gospel", "Noise", "Alternative Rock", "Bass", "Soul", "Punk", "Space", "Meditative", "Instrumental Pop", "Instrumental Rock", "Ethnic", "Gothic", "Darkwave", "Techno-Industrial", "Electronic", "Pop-Folk", "Eurodance", "Dream", "Southern Rock", "Comedy", "Cult", "Gangsta", "Top 40", "Christian Rap", "Pop/Funk", "Jungle music", "Native US", "Cabaret", "New Wave", "Psychedelic", "Rave", "Showtunes", "Trailer", "Lo-Fi", "Tribal", "Acid Punk", "Acid Jazz", "Polka", "Retro", "Musical", "Rock ’n’ Roll", "Hard Rock", "Folk", "Folk-Rock", "National Folk", "Swing", "Fast Fusion", "Bebop", "Latin", "Revival", "Celtic", "Bluegrass", "Avantgarde", "Gothic Rock", "Progressive Rock", "Psychedelic Rock", "Symphonic Rock", "Slow Rock", "Big Band", "Chorus", "Easy Listening", "Acoustic", "Humour", "Speech", "Chanson", "Opera", "Chamber Music", "Sonata", "Symphony", "Booty Bass", "Primus", "Porn Groove", "Satire", "Slow Jam", "Club", "Tango", "Samba", "Folklore", "Ballad", "Power Ballad", "Rhythmic Soul", "Freestyle", "Duet", "Punk Rock", "Drum Solo", "A cappella", "Euro-House", "Dance Hall", "Goa music", "Drum & Bass", "Club-House", "Hardcore Techno", "Terror", "Indie", "BritPop", "Negerpunk", "Polsk Punk", "Beat", "Christian Gangsta Rap", "Heavy Metal", "Black Metal", "Crossover", "Contemporary Christian", "Christian Rock", "Merengue", "Salsa", "Thrash Metal", "Anime", "Jpop", "Synthpop", "Christmas", "Art Rock", "Baroque", "Bhangra", "Big beat", "Breakbeat", "Chillout", "Downtempo", "Dub", "EBM", "Eclectic", "Electro", "Electroclash", "Emo", "Experimental", "Garage", "Global", "IDM", "Illbient", "Industro-Goth", "Jam Band", "Krautrock", "Leftfield", "Lounge", "Math Rock", "New Romantic", "Nu-Breakz", "Post-Punk", "Post-Rock", "Psytrance", "Shoegaze", "Space Rock", "Trop Rock", "World Music", "Neoclassical", "Audiobook", "Audio Theatre", "Neue Deutsche Welle", "Podcast", "Indie-Rock", "G-Funk", "Dubstep", "Garage Rock", "Psybient" };

        ILogger _logger = Logger.GetGlobalLogger();

        // Backing variables
        private string _title = "";
        private string _artist = "";
        private string _album = "";
        private string _track = "";
        private string _year = "";
        private string _genre = "";
        private string _rating = "";
        private string _comment = "";
        public byte[] _pictureData = new byte[0];

        // Properties
        public string Version { get; set; } = "";
        public string Fullname { get; set; } = "";
        public bool Modified { get; set; } = false;
        public string Title { get { return _title; } set { _title = value; Modified = true; } }
        public string Artist { get { return _artist; } set { _artist = value; Modified = true; } }
        public string Album { get { return _album; } set { _album = value; Modified = true; } }
        public string Track { get { return _track; } set { _track = value; Modified = true; } }
        public string Year { get { return _year; } set { _year = value; Modified = true; } }
        public string Genre { get { return _genre; } set { _genre = value; Modified = true; } }
        public string Rating { get { return _rating; } set { _rating = value; Modified = true; } }
        public string Comment { get { return _comment; } set { _comment = value; Modified = true; } }
        public byte[] PictureData { get { return _pictureData; } set { _pictureData = value; Modified = true; } }
        public bool WriteEmptyFrames { get; set; } = false;

        // Settings


        // Read only properties
        public string Filename { get { return Path.GetFileName(Fullname); } }
        public string PicInfo
        {
            get
            {
                if (PictureData.Length > 0)
                    return (PictureData.Length / 1024).ToString() + " kB";
                else
                    return "---";
            }
        }

        private ID3v2Header _hdr = new ID3v2Header();

        public ID3Tag() { }

        public ID3Tag(string filename)
        {
            byte[] buff = File.ReadAllBytes(filename);
            Fullname = filename;

            try
            {
                // Attempt to parse an ID3v2
                _hdr = new ID3v2Header(buff);

                if (_hdr.TotalSize > buff.Length)
                    throw new Exception("Length in ID3v2 header is longer than file, file possibly corrupt");

                switch (_hdr.Version)
                {
                    case ID3Version.ID3v2_2:
                    case ID3Version.ID3v2_3:
                    case ID3Version.ID3v2_4:
                        ParseID3v2(_hdr, buff);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogErr(Path.GetFileName(Fullname) + ": " + ex.Message);

                // ID3v2 tag is either not present, corrupted, or not supported.  Try ID3v1:
                try
                {
                    ParseID3v1(buff);
                }
                catch (Exception ex2)
                {
                    _logger.LogErr(Path.GetFileName(Fullname) + ": " + ex2.Message);
                }

            }

            this.Modified = false;
        }

        public void ParseID3v1(byte[] data)
        {
            // Can an ID3v1 tag fit?
            if (data.Length < 128) throw new Exception("ID3v1 cannot fit in provided buffer");

            // Is the "TAG" field present?
            int tagOffset = data.Length - 128;
            string magic = Encoding.UTF8.GetString(data, tagOffset, 3);
            if (magic != "TAG") throw new Exception("ID3v1 identifier not found");

            // Parse the data.  Note that the parsed strings will have trailing nulls up to the length, so 
            // they need to be trimmed to the correct length.
            // Ex: Title will be 30 chars long regardless of actual title length, with null padding up to 30
            Title = Encoding.UTF8.GetString(data, tagOffset + 3, 30).TrimEnd('\0');
            Artist = Encoding.UTF8.GetString(data, tagOffset + 33, 30).TrimEnd('\0');
            Album = Encoding.UTF8.GetString(data, tagOffset + 63, 30).TrimEnd('\0');
            Year = Encoding.UTF8.GetString(data, tagOffset + 93, 4);
            int genreID = data[tagOffset + 127];
            if (genreID >= GenresID3v1.Length) throw new Exception("Invalid ID3v1 genre: " + genreID.ToString());
            Genre = GenresID3v1[genreID];

            // Determine if this is ID3v1 or ID3v1.1 and parse accordingly
            // NOTE: Encoding is not specified for ID3v1, assume UTF8
            if (data[tagOffset + 125] == 0x00 && data[tagOffset + 126] != 0x00)
            {
                Comment = Encoding.UTF8.GetString(data, tagOffset + 97, 28);
                Track = data[tagOffset + 126].ToString();
                Version = "1.1";
            }
            else
            {
                Comment = Encoding.UTF8.GetString(data, tagOffset + 97, 30);
                Version = "1";
            }
        }

        public void ParseID3v2(ID3v2Header hdr, byte[] data)
        {
            if (hdr.Version != ID3Version.ID3v2_3 && hdr.Version != ID3Version.ID3v2_4)
            {
                // NOTE: Unable to find an example of an ID3v2.2 file, so no code was written to parse these...
                throw new Exception("ID3v2.2 tag not supported");
            }

            // NOTE: All logic from here on assumes version is 2.3 or 2.4...

            // Minimum size for an ID3v2.3/4 tag is 10 byte header + a single frame that's composed of
            // a 10 byte header and 1 byte payload = 10 + 10 + 1 = 21 bytes
            if (data.Length < 21) throw new Exception("ID3v2 tag does not fit");

            // Frames start after the header, unless there's an extended header.  If there is an
            // extended header, skip it.  FUTURE: Parse ext header and do something with it?
            int offset = ID3Constants.ID3_HDR_SIZE;
            if (hdr.HasExtHeader)
            {
                if (hdr.Version == ID3Version.ID3v2_3)
                    offset += SyncSafe.DecodeSyncsafeInt28(data, offset);
                else
                    offset += Utils.ReadBigEndian32Bit(data, offset) + 4;
            }

            while (offset < hdr.TotalSize)
            {
                // Standard states that if we encounter padding bytes, there is no more frame data
                if (data[offset] == 0) break;

                // Read this frame's header
                ID3v2Frame frame;
                if (hdr.Version == ID3Version.ID3v2_3)
                    frame = new ID3v2Frame(ID3Version.ID3v2_3, data, offset);
                else
                    frame = new ID3v2Frame(ID3Version.ID3v2_4, data, offset);

                // Skip frames that are not supported
                if (frame.Compression)
                {
                    _logger.LogWarn(Path.GetFileName(Fullname) + " Skipping frame due to compression: " + frame.ID);
                    offset += frame.TotalSize;
                    continue;
                }
                if (frame.Encryption)
                {
                    _logger.LogWarn(Path.GetFileName(Fullname) + "Skipping frame due to encryption: " + frame.ID);
                    offset += frame.TotalSize;
                    continue;
                }
                if (frame.Unsynchronization)
                {
                    _logger.LogWarn(Path.GetFileName(Fullname) + "Skipping frame due to unsyncrhonization: " + frame.ID);
                    offset += frame.TotalSize;
                    continue;
                }

                try
                {
                    switch (frame.ID)
                    {
                        case "TPE1": Artist = frame.AsText(data, offset); break;
                        case "TIT2": Title = frame.AsText(data, offset); break;
                        case "TALB": Album = frame.AsText(data, offset); break;
                        case "TRCK": Track = frame.AsText(data, offset); break;
                        case "TDRC": Year = frame.AsText(data, offset); break; // ID3v2.3
                        case "TYER": Year = frame.AsText(data, offset); break; // ID3v2.4
                        case "TCON": Genre = frame.AsText(data, offset); break;
                        case "APIC": PictureData = frame.GetPictureData(data, offset); break;
                        case "POPM": Rating = frame.AsRating(data, offset); break;
                        case "COMM": Comment = frame.AsComment(data, offset); break;
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogErr("Error parsing frame " + frame.ID + ": " + ex.Message);
                }

                // Jump to next frame
                offset += frame.TotalSize;
            }

            if (hdr.Version == ID3Version.ID3v2_3) Version = "2.3";
            else Version = "2.4";
        }

        public byte[] ToBytes(ID3Version v)
        {
            // First write all frames first so we can determine size of frame data
            MemoryStream tagPayload = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(tagPayload))
            {
                WriteTextFrame(v, bw, "TPE1", Artist);
                WriteTextFrame(v, bw, "TIT2", Title);
                WriteTextFrame(v, bw, "TALB", Album);
                WriteTextFrame(v, bw, "TRCK", Track);
                WriteTextFrame(v, bw, "TCON", Genre);

                if (v == ID3Version.ID3v2_3)
                    WriteTextFrame(v, bw, "TYER", Year);
                else if (v == ID3Version.ID3v2_4)
                    WriteTextFrame(v, bw, "TDRC", Year);

                WritePicture(v, bw);
                WriteRating(v, bw);
                WriteComment(v, bw);
            }

            // Determine the size of the frame data and prepend the header to it
            byte[] payloadBuff = tagPayload.ToArray();
            MemoryStream entireTag = new MemoryStream();
            using (BinaryWriter bw = new BinaryWriter(entireTag))
            {
                _hdr.Version = v;
                bw.Write(_hdr.ToBytes(payloadBuff.Length));
                bw.Write(payloadBuff);
            }

            return entireTag.ToArray();
        }

        private void WriteComment(ID3Version v, BinaryWriter bw)
        {
            if (Comment.Length == 0) return;

            WriteFrameHeader(v, bw, "COMM",
                1 + // Encoding 
                3 + // Language
                1 + // Description (empty string, just a null)
                Comment.Length + 1); // Comment + null terminator

            if (v == ID3Version.ID3v2_3) bw.Write((byte)ID3Encoding.ISO_8859_1);
            else if (v == ID3Version.ID3v2_4) bw.Write((byte)ID3Encoding.UTF_8);

            bw.Write(Encoding.Latin1.GetBytes("ENG")); // Language descriptor
            bw.Write((byte)0); // Empty description
            bw.Write(Encoding.UTF8.GetBytes(Comment));
            bw.Write((byte)0); // Null terminator
        }

        private void WriteTextFrame(ID3Version v, BinaryWriter bw, string id, string text)
        {
            if (!WriteEmptyFrames && text.Length == 0) return;

            WriteFrameHeader(v, bw, id, text.Length + 1);

            switch (v)
            {
                case ID3Version.ID3v2_3:
                    bw.Write((byte)0);
                    bw.Write(Encoding.Latin1.GetBytes(text));
                    break;
                case ID3Version.ID3v2_4:
                    bw.Write((byte)0);
                    bw.Write(Encoding.UTF8.GetBytes(text));
                    break;
                default:
                    throw new Exception("Unsupported ID3 version when writing frame");
            }
        }

        private void WritePicture(ID3Version v, BinaryWriter bw)
        {
            if (PictureData.Length == 0) return;
            WriteFrameHeader(v, bw, "APIC", PictureData.Length + 14);
            bw.Write((byte)ID3Encoding.ISO_8859_1); // Encoding
            bw.Write(Encoding.Latin1.GetBytes("image/jpeg")); // MIME type
            bw.Write((byte)0);
            bw.Write((byte)3); // Picture type (album cover)
            bw.Write((byte)0); // Description
            bw.Write(PictureData); // Picture data
        }

        private void WriteRating(ID3Version v, BinaryWriter bw)
        {
            if (Rating == "" && !WriteEmptyFrames) return;

            // Empty rating results in writing a zero
            if (Rating == "") Rating = "0";

            byte r = 0;
            try
            {
                r = Convert.ToByte(Rating);
            }
            catch (Exception ex)
            {
                _logger.LogErr("Error attempting to write rating: " + ex.Message);
                return;
            }

            WriteFrameHeader(v, bw, "POPM", 6);
            bw.Write((byte)0); // Email
            bw.Write(r); // Rating
            bw.Write((UInt32)0); // Play counter
        }

        private void WriteFrameHeader(ID3Version v, BinaryWriter bw, string id, int size)
        {

            bw.Write(Encoding.Latin1.GetBytes(id)); // Identifier

            switch (v)
            {
                case ID3Version.ID3v2_3:
                    bw.Write((byte)((size & 0xFF000000) >> 24)); // Big endian 32 bit integer
                    bw.Write((byte)((size & 0x00FF0000) >> 16));
                    bw.Write((byte)((size & 0x0000FF00) >> 8));
                    bw.Write((byte)((size & 0x000000FF) >> 0));
                    break;
                case ID3Version.ID3v2_4:
                    bw.Write(SyncSafe.ToSyncSafe28(size));
                    break;
                default:
                    throw new Exception("Invalid ID3 version when writing frame header");
            }

            // Flags
            bw.Write((byte)0);
            bw.Write((byte)0);
        }

        /// <summary>
        /// Strips ID3v1 tag from the supplied buffer, if present.
        /// </summary>
        /// <param name="buff">Buffer to strip</param>
        /// <returns>Buffer with tag removed, or original buffer if no tag found</returns>
        public static byte[] StripID3v1(byte[] buff)
        {
            if (buff.Length < 128) return buff;
            string magic = Encoding.UTF8.GetString(buff, buff.Length - 128, 3);
            if (magic != "TAG") return buff;
            byte[] newBuff = new byte[buff.Length - 128];
            Buffer.BlockCopy(buff, 0, newBuff, 0, newBuff.Length);
            return newBuff;
        }

        /// <summary>
        /// Strips the ID3v2 tag from the supplied buffer, if present.  Note that while this class does not currently
        /// support parsing ID3v2.2 tags, this function will strip them.  Very little validation is performed
        /// on the tag, just the bare minimum that the identifier is present and the size isn't nonsensical.
        /// </summary>
        /// <param name="buff">Buffer to strip</param>
        /// <returns>Buffer with tag removed, or original buffer if no tag found</returns>
        /// <exception cref="Exception"></exception>
        public static byte[] StripID3v2(byte[] buff)
        {
            string magic = Encoding.UTF8.GetString(buff, 0, 3);
            if (magic != "ID3") return buff;

            // Assumption: If the "ID3" identifier is found this is an ID3 tag and the 10-byte header format
            // will not change regardless of the version.  Versions up to 2.4 have the same header so this
            // code assumes that any and all future versions will have the same structure.

            // Size of tag is header size + tag size
            int sizeOfTag = 10 + SyncSafe.DecodeSyncsafeInt28(buff, 6);

            // v2.4 may have an additional footer that is not included in the size
            int version = buff[3];
            if (version == (int)ID3Version.ID3v2_4)
            {
                bool footerPresent = (buff[5] & 0b00010000) != 0;
                if (footerPresent) sizeOfTag += 10;
            }

            // Sanity check the parsed size
            if (sizeOfTag > buff.Length)
                throw new Exception("Tag size is greater than file size, possible corruption detected");

            byte[] newBuff = new byte[buff.Length - sizeOfTag];
            Buffer.BlockCopy(buff, sizeOfTag, newBuff, 0, newBuff.Length);
            return newBuff;

        }
    }
}
