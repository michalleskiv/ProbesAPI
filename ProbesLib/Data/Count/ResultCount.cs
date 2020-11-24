namespace ProbesLib.Data.Count
{
    /// <summary>
    /// Class for processing JSON object from /api/v2/apps/{idApp}/schemas/{idSchema}/data/count
    /// </summary>
    public class ResultCount
    {
        public DataCount Data { get; set; }
    }
}
