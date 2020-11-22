﻿using System;

namespace ProbesLib.Data.Record
{
    public class Record
    {
        public string id { get; set; }
        public DateTime created { get; set; }
        public DateTime modified { get; set; }
        public int ver { get; set; }
        public Probe fields { get; set; }
    }
}