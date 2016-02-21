using System;
using System.IO;
using System.Linq;

namespace MP3Tagger
{
	public class Frame
	{
		public string FrameID {
			get;
			set;
		}
		public int Size {
			get;
			set;
		}
		public byte[] Flags {
			get;
			set;
		}
		public byte[] Data {
			get;
			set;
		}

		public Frame ()
		{
			
		}
		public static Frame ReadOneFrame(ref BinaryReader br){
			var headerBytes = br.ReadBytes (Constants.FRAMEHEADER_SIZE);
			Frame frame = new Frame ();
			frame.FrameID = System.Text.Encoding.ASCII.GetString( headerBytes.Take (4).ToArray());
			frame.Size = GetFrameSize (headerBytes.Skip (4).Take (4).ToArray ());
			frame.Flags = headerBytes.Skip (8).Take (2).ToArray ();
			frame.Data = br.ReadBytes(frame.Size);
			return frame;
		}

	
		private static int GetFrameSize(byte[] bytes)
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


	}
}

