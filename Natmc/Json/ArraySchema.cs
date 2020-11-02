using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Json
{
    public class ArraySchema
    {
        public object Schema;

        public ArraySchema(object schema)
        {
            Schema = schema;
        }

        public override string ToString() => "array";
    }
}
