using System.Collections.Generic;

namespace ProbesLib.Data.Definitions
{
    public class DefinitionData
    {
        public string id { get; set; }
        public string shortid { get; set; }
        public string header { get; set; }
        public List<ProbeDefinition> items { get; set; }
    }
}
