using Natmc.Core;
using System;
using System.IO;
using System.IO.Compression;

namespace Natmc
{
    class Program
    {
        static void Main(string[] args)
        {
            ZipArchive archive = new ZipArchive(new FileStream(@"C:\Users\Nat\Desktop\test.zip", FileMode.Open, FileAccess.Read));
            foreach (var e in archive.Entries)
            {
                Console.WriteLine(e.FullName);
            }
            //Engine.Start();
        }
    }
}
