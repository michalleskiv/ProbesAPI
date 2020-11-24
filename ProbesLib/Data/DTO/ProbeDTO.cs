using ProbesLib.Data.Record;

namespace ProbesLib.Data.DTO
{
    public class ProbeDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int DefaultMinValue { get; set; }
        public int DefaultMaxValue { get; set; }
        public string Note { get; set; }

        public ProbeDTO(Probe probe)
        {
            Id = probe.UniqueId;
            Name = probe.Name;
            DefaultMinValue = probe.LowValue;
            DefaultMaxValue = probe.MaxValue;
            Note = probe.Description;
        }
    }
}
