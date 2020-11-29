using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Core
{
    public class EngineObject
    {
        public Engine Parent { get; set; }

        public virtual void OnAdd() { }
        public virtual void OnRemove() { }
        public virtual void Update(float delta) { }
        public virtual void PostUpdate(float delta) { }
    }
}
