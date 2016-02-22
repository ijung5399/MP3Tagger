using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Diagnostics;
using MP3Tagger;
namespace MP3Tagger.Test
{
    [TestClass]
    public class MP3ParserTest
    {
        FileInfo[] samples = null;
        [TestInitialize]
        public void Initialize()
        {
            var path = Directory.GetCurrentDirectory();
            var di = new DirectoryInfo(path);
            samples = (new DirectoryInfo(Path.Combine(di.Parent.Parent.Parent.FullName, "Samples"))).GetFiles();           
        }
        [TestMethod]
        public void Should_Have_ID3()
        {
            foreach (var s in samples)
            {
                MP3Parser parser = new MP3Parser(s.FullName);
            }
        }
        [TestMethod]
        public void Version_Should_be_Valid()
        {
            foreach (var s in samples)
            {
                MP3Parser parser = new MP3Parser(s.FullName);
                Assert.IsTrue(parser.ID3Header.Version[0] >= 3 && parser.ID3Header.Version[1] >= 0);
            }
        }
        [TestMethod]
        public void Should_have_Flags()
        {
            foreach (var s in samples)
            {
                MP3Parser parser = new MP3Parser(s.FullName);
                Trace.WriteLine(parser.ID3Header.Flags);
            }
        }

        [TestMethod]
        public void ID3Flags_Constructor()
        {
            byte b1 = 0x80;
            ID3Flags f1 = new ID3Flags(b1);
            Assert.IsTrue(f1.Unsynchronisation);

            byte b2 = 0x40;
            ID3Flags f2 = new ID3Flags(b2);
            Assert.IsTrue(f2.ExtendedHeader);

            byte b3 = 0x20;
            ID3Flags f3 = new ID3Flags(b3);
            Assert.IsTrue(f3.Experimental);
        }

        [TestMethod]
        public void HeaderInfo_String_Is_Not_Null()
        {
            foreach (var s in samples)
            {
                MP3Parser parser = new MP3Parser(s.FullName);
                Trace.WriteLine(parser.ID3Header.HeaderInfo);
                Assert.IsNotNull(parser.ID3Header.HeaderInfo);
            }
        }

        [TestMethod]
        public void ID3SizeComputation()
        {
            PrivateObject po = new PrivateObject(typeof(ID3Header));
            byte[] input = new byte[] { 0x00, 0x00, 0x02, 0x01 };
            int result = (int) po.Invoke("GetID3HeaderSize", input);
            Assert.AreEqual(257, result);
        }

    }
}
