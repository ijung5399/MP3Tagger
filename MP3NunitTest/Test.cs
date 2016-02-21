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
		[Test ()]
		public void TestCase ()
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

