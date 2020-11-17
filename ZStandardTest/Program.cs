using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.IO.Compression;
using System.Reflection;

using Zstandard.Net;

namespace ZStandardTest
{
    class Program
    {
        static void Main(string[] args)
        {
			byte[] input = File.ReadAllBytes(Assembly.GetExecutingAssembly().Location);
			byte[] compressed = null;
			byte[] output = null;

			// load a dictionary that is trained for the data (optional).
			// var dictionary = new ZstandardDictionary("loremipsum.zdict");

			// compress
			using (var memoryStream = new MemoryStream())
			using (var compressionStream = new ZstandardStream(memoryStream, CompressionMode.Compress))
			{
				compressionStream.CompressionLevel = 11;               // optional!!
				// compressionStream.CompressionDictionary = dictionary;  // optional!!
				compressionStream.Write(input, 0, input.Length);
				compressionStream.Close();
				compressed = memoryStream.ToArray();
			}

			// decompress
			using (var memoryStream = new MemoryStream(compressed))
			using (var compressionStream = new ZstandardStream(memoryStream, CompressionMode.Decompress))
			using (var temp = new MemoryStream())
			{
				// compressionStream.CompressionDictionary = dictionary;  // optional!!
				compressionStream.CopyTo(temp);
				output = temp.ToArray();
			}

			// test output
			if (output.SequenceEqual(input) == false)
			{
				throw new Exception("Output is different from input!");
			}
		}
    }
}
