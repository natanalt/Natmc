using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace Natmc.Resources
{
    public class ZipPackReader : IPackReader, IDisposable
    {
        public Stream ArchiveStream { get; protected set; }
        public ZipArchive Archive { get; protected set; }

        public ZipPackReader(string archivePath)
        {
            ArchiveStream = new FileStream(archivePath, FileMode.Open, FileAccess.Read);
            Archive = new ZipArchive(ArchiveStream, ZipArchiveMode.Read, false);
        }

        public ZipPackReader(Stream stream)
        {
            ArchiveStream = stream;
            Archive = new ZipArchive(ArchiveStream, ZipArchiveMode.Read, false);
        }

        public ZipPackReader(ZipArchive archive)
        {
            ArchiveStream = null;
            Archive = archive;
        }

        public bool FileExists(string path)
        {
            return Archive.GetEntry(path) != null;
        }

        public byte[] ReadFile(string path)
        {
            var entry = Archive.GetEntry(path);
            if (entry == null)
                return null;
            var data = new byte[entry.Length];
            using (var stream = entry.Open())
                stream.Read(data);
            return data;
        }

        public void Dispose()
        {
            if (Archive == null)
                throw new InvalidOperationException();
            Archive.Dispose();
            Archive = null;
        }
    }
}
