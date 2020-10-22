using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Natmc.Resources
{
    public interface IPackReader
    {
        public bool FileExists(string path);
        public Stream OpenFile(string path);
    }
}
