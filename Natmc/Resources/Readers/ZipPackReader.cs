using System;
using System.IO;
using System.IO.Compression;

namespace Natmc.Resources.Readers
{
    public class ZipPackReader : IPackReader
    {
        public Stream ArchiveStream { get; protected set; }
        public ZipArchive Archive { get; protected set; }

        public ZipPackReader(string archivePath)
        {
            ArchiveStream = Filesystem.OpenFile(archivePath, FileMode.Open, FileAccess.Read);
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

        public Stream OpenFile(string path)
        {
            var entry = Archive.GetEntry(path);
            if (entry == null)
                return null;
            return entry.Open();
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
