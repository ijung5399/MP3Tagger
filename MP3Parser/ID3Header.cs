using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace MP3Tagger
{
    /*
       The ID3v2 tag header, which should be the first information in the
   file, is 10 bytes as follows:

     ID3v2/file identifier      "ID3"
     ID3v2 version              $03 00
     ID3v2 flags                %abc00000
     ID3v2 size             4 * %0xxxxxxx
     */
    public class ID3Header
    {

        public byte[] Version { get; set; }
        public ID3Flags Flags { get; set; }
        public int Size { get; set; }

        public ID3Header() { }
        public ID3Header(byte[] headerBytes)
        {
            if (headerBytes.Length != Constants.ID3HEADER_SIZE)
                throw new ArgumentException("ID3 header size should be 10.");
            var bytes = new List<Byte>(headerBytes);
            var identifier = System.Text.Encoding.Default.GetString(bytes.Take(3).ToArray());
            Trace.WriteLine(identifier);
            if (identifier != "ID3")
                throw new Exception("No ID3 Header identifier is found.");

            Version = bytes.Skip(3).Take(2).ToArray();
            Flags = new ID3Flags(bytes.Skip(5).Take(1).FirstOrDefault());
            Size = GetID3HeaderSize(bytes.Skip(6).Take(4).ToArray());            
        }

        private int GetID3HeaderSize(byte[] bytes)
        {
            byte mask = 0x7F;
            int totalSize = 0;
            for (int i = 0; i < 4; i++)
            {
                if (bytes[i] >> 7 != 0)
                    throw new Exception("Invalid size field. The first bit of each byte should be zero.");
                var size = bytes[i] & mask; // delete the first bit
                totalSize = totalSize * 128 + size;
            }
            return totalSize;
        }

        public string VersionString
        {
            get
            {
                return Version[0].ToString() + Version[1].ToString();
            }
        }
        public string FlagsString
        {
            get
            {
                return (Flags.Unsynchronisation == true ? "a": "") +
                    (Flags.ExtendedHeader == true ? "b": "") +
                    (Flags.Experimental == true ? "c": "");
            }
        }
        public string HeaderInfo
        {
            get
            {
                return "Version: " + VersionString + ", Flags: " + FlagsString + ", Size: " + Size; 
            }
        }

    }
}