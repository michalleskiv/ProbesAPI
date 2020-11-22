using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ProbesLib.Interfaces;

namespace Probes.Controllers
{
    public class ProbesController : Controller
    {
        private readonly IProbesWorker _worker;

        public ProbesController(IProbesWorker probesWorker)
        {
            _worker = probesWorker;
        }

        [Route("/probes")]
        [HttpGet]
        public async Task<IActionResult> GetAllProbes()
        {
            return new OkObjectResult(await _worker.GetDefinition());
        }

        [Route("/probes/{idProbe}")]
        [HttpGet("{idProbe}")]
        public async Task<IActionResult> GetProbeById(string idProbe)
        {
            return new OkObjectResult(await _worker.GetById(idProbe));
        }

        [Route("/probes/{idProbe}/data")]
        [HttpGet("{idProbe}")]
        public async Task<IActionResult> ExecuteProbeById(string idProbe)
        {
            return new OkObjectResult(await _worker.ExecuteProbe(idProbe));
        }
    }
}
