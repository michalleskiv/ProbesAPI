﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProbesLib.Interfaces;

namespace ProbesAPI.Controllers
{
    public class ProbesController : Controller
    {
        private readonly IProbesWorker _worker;

        public ProbesController(IProbesWorker probesWorker)
        {
            _worker = probesWorker;
        }

        /// <summary>
        /// Shows probes definition table
        /// </summary>
        /// <returns>Probes definition table</returns>
        /// <response code="200">Returns probes definition table</response>
        [Route("/probes")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllProbes()
        {
            return new OkObjectResult(await _worker.GetDefinition());
        }

        /// <summary>
        /// Returns probe by id
        /// </summary>
        /// <param name="idProbe">Id of a probe</param>
        /// <returns>A probe</returns>
        /// <response code="200">Returns a probe</response>
        /// <response code="400">Bad request</response>
        [Route("/probes/{idProbe}")]
        [HttpGet("{idProbe}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetProbeById(string idProbe)
        {
            return new OkObjectResult(await _worker.GetById(idProbe));
        }

        /// <summary>
        /// Execute probe and get result
        /// </summary>
        /// <param name="idProbe">Id of a probe</param>
        /// <returns>Count of records matching the probe and time of execution</returns>
        /// <response code="200">Returns result</response>
        /// <response code="400">Bad request</response>
        [Route("/probes/{idProbe}/data")]
        [HttpGet("{idProbe}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ExecuteProbeById(string idProbe)
        {
            return new OkObjectResult(await _worker.ExecuteProbe(idProbe));
        }
    }
}
