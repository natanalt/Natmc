using System;
using System.Collections.Generic;
using System.Text;

namespace Natmc.Resources
{
    public class Language
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public bool IsBidirectional { get; set; }
        public Dictionary<string, string> Strings { get; protected set; }

        public Language()
        {
            Strings = new Dictionary<string, string>();
        }
    }
}
