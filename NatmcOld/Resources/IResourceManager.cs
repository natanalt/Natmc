using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Resources
{
    public interface IResourceManager
    {
        public void Init();
        public void Deinit();
        public void UnloadResources();
        public void LoadResources();
    }
}
