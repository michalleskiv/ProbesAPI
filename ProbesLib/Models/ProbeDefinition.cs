using System;
using System.Collections.Generic;
using System.Text;

namespace ProbesLib.Models
{
    public class ProbeDefinition
    {
        public string id { get; set; }
        public string header { get; set; }
        public string type { get; set; }
        public Metadata metadata { get; set; }
    }
}
