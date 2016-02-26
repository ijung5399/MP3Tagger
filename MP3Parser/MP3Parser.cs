using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP3Tagger
{    
    public class MP3Parser
    {
        public MP3Parser(string filename)
        {
            Load(filename);
        }

        public ID3Header ID3Header { get; set; }
		public List<Frame> Frames {
			get;
			set;
		}

        private void Load(string filename)
        {
            using (BinaryReader br = new BinaryReader(File.Open(filename, FileMode.Open)))
            {
                int length = (int)br.BaseStream.Length;
                if (length > 0)
                {
                    // 3.
                    // Read integer.
                    var bytes = br.ReadBytes(Constants.ID3HEADER_SIZE);
                    
                    ID3Header = ParseHeader(bytes);
					Frames = GetFrames(br, ID3Header.Size);
                }
            }
        }

        internal static ID3Header ParseHeader(byte[] bytes)
        {
            return new ID3Header(bytes);
        }

		private List<Frame> GetFrames(BinaryReader br, int size)
		{
			List<Frame> frames = new List<Frame> ();
			int frameBytes = size;
			while (frameBytes > 0) {
				var frame = Frame.ReadOneFrame (ref br);
				frames.Add(frame);
				frameBytes -= frame.Size;
			}
			return frames;
		}
        

    }

 
}
