using NUnit.Framework;
using System;
using System.IO;
using MP3Tagger;
using System.Diagnostics;


namespace MP3Tagger
{
	[TestFixture ()]
	public class NUnitTests
	{
		FileInfo[] samples = null;
		[TestFixtureSetUp] public void Init()
		{ 
			var path = Directory.GetCurrentDirectory();
			var di = new DirectoryInfo(path);
				samples = (new DirectoryInfo(Path.Combine(di.Parent.Parent.Parent.FullName, "Samples"))).GetFiles();           
		}
        [Test()]
        public void Should_Have_ID3()
        {
            foreach (var s in samples)
            {
                MP3Parser parser = new MP3Parser(s.FullName);
            }
        }
        [Test()]
        public void Version_Should_be_Valid()
        {
            foreach (var s in samples)
            {
                MP3Parser parser = new MP3Parser(s.FullName);
                Assert.IsTrue(parser.ID3Header.Version[0] >= 3 && parser.ID3Header.Version[1] >= 0);
            }
        }
        [Test()]
        public void Should_have_Flags()
        {
            
            foreach (var s in samples)
            {
                using (BinaryReader br = new BinaryReader(File.Open(s.FullName, FileMode.Open)))
                {
                    int length = (int)br.BaseStream.Length;
                    if (length > 0)
                    {
                        var bytes = br.ReadBytes(Constants.ID3HEADER_SIZE);

                        var ID3Header = MP3Parser.ParseHeader(bytes);
                        ID3Header.Flags.ShouldNotEqual(null);
                    }
                }
            }
        }

        [Test()]
        public void ID3Flags_Constructor()
        {
            byte b1 = 0x80;
            ID3Flags f1 = new ID3Flags(b1);
            f1.Unsynchronisation.ShouldBeTrue();

            byte b2 = 0x40;
            ID3Flags f2 = new ID3Flags(b2);
            f2.ExtendedHeader.ShouldBeTrue();

            byte b3 = 0x20;
            ID3Flags f3 = new ID3Flags(b3);
            f3.Experimental.ShouldBeTrue();
        }

        [Test()]
        public void HeaderInfo_String_Is_Not_Null()
        {
            foreach (var s in samples)
            {
                MP3Parser parser = new MP3Parser(s.FullName);
                Trace.WriteLine(parser.ID3Header.HeaderInfo);
                Assert.IsNotNull(parser.ID3Header.HeaderInfo);
            }
        }

        [Test()]
        public void ID3SizeComputation()
        {
            ID3Header header = new ID3Header();
            byte[] input = new byte[] { 0x00, 0x00, 0x02, 0x01 };
            var result = header.GetID3HeaderSize(input);
            Assert.AreEqual(257, result);
        }
        [Test ()]
		public void FrameID_Length_is_4 ()
		{
			foreach (var s in samples) {
				MP3Parser parser = new MP3Parser (s.FullName);
				if (parser.ID3Header.Size > 0) {
					foreach (var f in parser.Frames) {
						Assert.IsTrue (f.FrameID.Length == 4);
					}
				}
			}
		}     
    }
}

