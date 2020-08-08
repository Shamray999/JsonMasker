using System;
using System.Collections.Generic;
using System.Text;

namespace JsonMasker
{
    public class MaskerConfig
    {
        public string JsonPath { get; set; }
        public int StartWith { get; set; }
        public int EndWith { get; set; }
    }
}
