using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Resources
{
    public interface IPackReader
    {
        public bool FileExists(string path);
        public byte[] ReadFile(string path);
    }
}
