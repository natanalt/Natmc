using System;
using System.IO;

namespace Natmc.Resources.Readers
{
    public interface IPackReader : IDisposable
    {
        public bool FileExists(string path);
        public Stream OpenFile(string path);

        public byte[] ReadBinaryFile(string path)
        {
            using var stream = OpenFile(path);
            var data = new byte[stream.Length];
            stream.Read(data);
            return data;
        }

        public string ReadTextFile(string path)
        {
            using var reader = new StreamReader(OpenFile(path), leaveOpen: false);
            return reader.ReadToEnd();
        }
    }
}
