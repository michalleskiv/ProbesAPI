using System.Threading.Tasks;
using ProbesLib.Data.Definitions;
using ProbesLib.Data.Record;
using ProbesLib.Data.Count;

namespace ProbesLib.Interfaces
{
    public interface IProbesWorker
    {
        Task<ResultDefinition> GetDefinition();
        Task<ResultRecord> GetById(string idProbe);
        Task<FilteredCount> ExecuteProbe(string idProbe);
    }
}
