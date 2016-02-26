using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

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
            string dataStr = System.Text.Encoding.ASCII.GetString(frame.Data);
            Trace.WriteLine("\n" + frame.FrameID + " [ASCII] : " + dataStr);

            string utf16 = Encoding.Unicode.GetString(frame.Data);
            Trace.WriteLine("\n" + frame.FrameID + " [UTF16] : " + utf16);

            string dataUtf = System.Text.Encoding.UTF8.GetString(frame.Data);
            Trace.WriteLine("\n" + frame.FrameID + " [UTF] : " + dataUtf);
            
            string dataKor20833 = System.Text.Encoding.GetEncoding(20833).GetString(frame.Data);
            Trace.WriteLine("\n" + frame.FrameID + " [20833] : " + dataKor20833);

            string dataKor20949 = System.Text.Encoding.GetEncoding(20949).GetString(frame.Data);
            Trace.WriteLine("\n" + frame.FrameID + " [20949] : " + dataKor20949);

            string dataKor50225 = System.Text.Encoding.GetEncoding(50225).GetString(frame.Data);
            Trace.WriteLine("\n" + frame.FrameID + " [50225] : " + dataKor50225);

            //string dataKor50933 = System.Text.Encoding.GetEncoding(50933).GetString(frame.Data);
            //Trace.WriteLine("\n" + frame.FrameID + " [50933] : " + dataUtf);

            string dataKor51949 = System.Text.Encoding.GetEncoding(51949).GetString(frame.Data);
            Trace.WriteLine("\n" + frame.FrameID + " [51949] : " + dataKor51949);

            string dataKor949 = System.Text.Encoding.GetEncoding(949).GetString(frame.Data);
            Trace.WriteLine("\n" + frame.FrameID + " [949] : " + dataKor949);

            string dataKor1361 = System.Text.Encoding.GetEncoding(1361).GetString(frame.Data);
            Trace.WriteLine("\n" + frame.FrameID + " [1361] : " + dataKor1361);

            string dataKor10003 = System.Text.Encoding.GetEncoding(10003).GetString(frame.Data);
            Trace.WriteLine("\n" + frame.FrameID + " [10003] : " + dataKor10003);

            

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

