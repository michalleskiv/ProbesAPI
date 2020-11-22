using ProbesLib.Interfaces;

namespace ProbesLib.Models
{
    public class Config : IConfig
    {
        public string Url { get; set; }
        public string DefinitionsEndpoint { get; set; }
        public string ProbeEndpoint { get; set; }
        public string IdApp { get; set; }
        public string IdSchema { get; set; }
        public string Token { get; set; }
    }
}
