using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Natmc.Resources
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
            return File.Exists(Path.Combine(BasePath, path));
        }

        public Stream OpenFile(string path)
        {
            if (!FileExists(path))
                return null;

            return new FileStream(path, FileMode.Open, FileAccess.Read);
        }
    }
}
