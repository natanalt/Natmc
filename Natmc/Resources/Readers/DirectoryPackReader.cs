using System.IO;

namespace Natmc.Resources.Readers
{
    public class DirectoryPackReader : IPackReader
    {
        public string BasePath { get; set; }

        public DirectoryPackReader(string basePath)
        {
            BasePath = basePath;
        }

        public bool FileExists(string path)
        {
            return Filesystem.FileExists(Path.Combine(BasePath, path));
        }

        public Stream OpenFile(string path)
        {
            if (!FileExists(path))
                return null;
            return Filesystem.OpenFile(Path.Combine(BasePath, path), FileMode.Open, FileAccess.Read);
        }

        public void Dispose()
        {
        }
    }
}
