using System.Threading.Tasks;
using ProbesLib.Data.DTO;

namespace ProbesLib.Interfaces
{
    /// <summary>
    /// Gets information about probes
    /// </summary>
    public interface IProbesWorker
    {
        /// <summary>
        /// Returns JSON with Swagger documentation
        /// </summary>
        /// <returns>Swagger documentation</returns>
        string Bonjour();

        /// <summary>
        /// Gets definition of a probe
        /// </summary>
        /// <returns>Probe definition</returns>
        Task<DtoProbes> GetDefinition();

        /// <summary>
        /// Get a certain probe by id or name
        /// </summary>
        /// <param name="idProbe">Probe id or name</param>
        /// <returns>Probe</returns>
        Task<DtoProbe> GetProbeDto(string idProbe);

        /// <summary>
        /// Gets count of records matching the probe
        /// </summary>
        /// <param name="idProbe">Probe id or name</param>
        /// <returns>FilteredCount object containing count of records and time of execution</returns>
        Task<DtoCount> ExecuteProbe(string idProbe);
    }
}
