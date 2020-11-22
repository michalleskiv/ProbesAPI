namespace ProbesLib.Data.Record
{
    /// <summary>
    /// Class for processing JSON object from /api/v2/apps/{idApp}/schemas/{idSchema}/data/{idProbe}
    /// </summary>
    public class ResultRecord
    {
        public Record data { get; set; }
    }
}
