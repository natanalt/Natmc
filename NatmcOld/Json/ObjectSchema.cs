using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Json
{
    public class ObjectSchema : Dictionary<string, object>
    {
        public override string ToString() => "object";
    }
}
