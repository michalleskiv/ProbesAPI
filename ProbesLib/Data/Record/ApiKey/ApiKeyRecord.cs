using System;
using System.Collections.Generic;
using System.Text;

namespace ProbesLib.Data.Record.ApiKey
{
    public class ApiKeyRecord
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int Ver { get; set; }
        public ApiKey Fields { get; set; }
        public string Url { get; set; }
    }
}
