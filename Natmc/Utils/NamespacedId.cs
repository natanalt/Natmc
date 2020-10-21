using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Utils
{
    public struct NamespacedId
    {
        public string Namespace;
        public string Id;

        public NamespacedId(string formatted)
        {
            if (formatted.Contains(':'))
            {
                var split = formatted.Split(':');
                if (split.Length != 2)
                    throw new ArgumentException("Invalid namespaced ID");

                Namespace = split[0];
                Id = split[1];
            }
            else
            {
                Namespace = "minecraft";
                Id = formatted;
            }
        }

        public NamespacedId(string @namespace, string id)
        {
            Namespace = @namespace;
            Id = id;
        }

        public override string ToString()
        {
            return Namespace + ":" + Id;
        }
    }
}
