using System.Collections.Generic;
using System.Linq;
using ProbesLib.Data.Record;

namespace ProbesLib.Data.DTO
{
    public class DtoProbes
    {
        public string Version { get; set; }
        public List<DtoProbe> Probes { get; set; }

        public DtoProbes(string version, List<Probe> probes)
        {
            Version = version;
            Probes = probes.Select(p => new DtoProbe(p)).ToList();
        }
    }
}
