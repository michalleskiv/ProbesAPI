using System.Collections.Generic;
using System.Linq;
using ProbesLib.Data.Record;

namespace ProbesLib.Data.DTO
{
    public class ProbesDTO
    {
        public string Version { get; set; }
        public List<ProbeDTO> Probes { get; set; }

        public ProbesDTO(string version, List<Probe> probes)
        {
            Version = version;
            Probes = probes.Select(p => new ProbeDTO(p)).ToList();
        }
    }
}
