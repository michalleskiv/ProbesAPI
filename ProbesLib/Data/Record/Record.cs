using System;

namespace ProbesLib.Data.Record
{
    public class Record
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public int Ver { get; set; }
        public Probe Fields { get; set; }
    }
}
