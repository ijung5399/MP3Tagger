using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Parser
{    
    public class MP3Parser
    {
        public MP3Parser(string filename)
        {
            Load(filename);
        }

        public ID3Header ID3Header { get; set; }

        private void Load(string filename)
        {
            using (BinaryReader br = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                // 2.
                // Position and length variables.
                int pos = 0;
                // 2A.
                // Use BaseStream.
                int length = (int)br.BaseStream.Length;
                while (pos < length)
                {
                    // 3.
                    // Read integer.
                    var bytes = br.ReadBytes(Constants.ID3HEADER_SIZE);
                    
                    ID3Header = ParseHeader(bytes);
                    break; // Only for header parsing for the time being.
                    // 4.
                    // Advance our position variable.
                    pos += sizeof(int);
                }
            }
        }

        private ID3Header ParseHeader(byte[] bytes)
        {
            return new ID3Header(bytes);
        }

        
    }

 
}
