using System.Collections.Generic;
using System.Threading.Tasks;
using ProbesLib.Models;

namespace ProbesLib.Interfaces
{
    public interface IProbesWorker
    {
        Task<ResultDefinition> GetDefinition();
        Task<ResultRecord> GetById(string idProbe);
        Task<FilteredCount> ExecuteProbe(string idProbe);
    }
}
