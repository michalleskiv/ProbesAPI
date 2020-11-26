using System;
using System.Collections.Generic;
using System.Text;

namespace ProbesLib.Data.Record.ApiKey
{
    public class ApiKey
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public ProbesPerKeyCount Probes { get; set; }
    }
}
