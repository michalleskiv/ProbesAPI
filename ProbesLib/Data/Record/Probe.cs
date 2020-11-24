namespace ProbesLib.Data.Record
{
    public class Probe
    {
        public int UniqueId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string AppId { get; set; }
        public string TableId { get; set; }
        public string UserFilter { get; set; }
        public string FilterBody { get; set; }
        public int LowValue { get; set; }
        public int MaxValue { get; set; }
        public string Description { get; set; }
        public Documentation Documentation { get; set; }
    }
}
