using System.Threading.Tasks;
using ProbesLib.Data.Definitions;
using ProbesLib.Data.Record;
using ProbesLib.Data.Count;

namespace ProbesLib.Interfaces
{
    /// <summary>
    /// Gets information about probes
    /// </summary>
    public interface IProbesWorker
    {
        /// <summary>
        /// Gets definition of a probe
        /// </summary>
        /// <returns>Probe definition</returns>
        Task<ResultDefinition> GetDefinition();

        /// <summary>
        /// Get a certain probe by id
        /// </summary>
        /// <param name="idProbe">Probe id</param>
        /// <returns>Probe</returns>
        Task<ResultRecord> GetById(string idProbe);

        /// <summary>
        /// Gets count of records matching the probe
        /// </summary>
        /// <param name="idProbe">Probe id</param>
        /// <returns>FilteredCount object containing count of records and time of execution</returns>
        Task<FilteredCount> ExecuteProbe(string idProbe);
    }
}
