using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GP_API
{
    public class Test
    {

        public static void TestStreams()
        {
            var dir = new System.IO.FileInfo(@"C:\Users\moame\Desktop\spotlight\1b297b81cc08eaa62023ea55394fbed82c3bb8c9fe81b5b6f3b338e19aabfc64.jpg");
            Console.WriteLine(dir.Exists);
            using FileStream filestream = dir.OpenRead();
            using MemoryStream m = new MemoryStream();
            filestream.CopyTo(m);
            using var file2 = File.Create(@$"D:\temp\{Guid.NewGuid()}.jpg");
            m.CopyTo(file2);
            using var file3 = File.Create(@$"D:\temp\{Guid.NewGuid()}.jpg");
            m.WriteTo(file3);
            using var file4 = File.Create(@$"D:\temp\{Guid.NewGuid()}.jpg");
            m.WriteTo(file4);
        }


    }
}
