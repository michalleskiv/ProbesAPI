namespace ProbesLib.Configurations
{
    /// <summary>
    /// Configurations for ProbesWorker
    /// </summary>
    public interface IConfig
    {
        public string Url { get; set; }
        public string Endpoint { get; set; }
        public string FindProbeByIdEndpoint { get; set; }
        public string FindProbeByNameEndpoint { get; set; }
        public string FilterEndpoint { get; set; }
        public string ProbeEndpoint { get; set; }
        public string AppId { get; set; }
        public string TableId { get; set; }
        public string Token { get; set; }
    }
}
