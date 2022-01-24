using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using challenge.Services;
using challenge.Models;

namespace challenge.Controllers
{
    [Route("api/reportingstructure")]
    public class ReportingStructureController : Controller
    {
        private readonly ILogger _logger;
        private readonly IReportingStructureService _reportingStructureService;

        public ReportingStructureController(ILogger<ReportingStructureController> logger, IReportingStructureService reportingStructureService)
        {
            _logger = logger;
            _reportingStructureService = reportingStructureService;
        }

        [HttpPost]
        public IActionResult CreateReportingStructure(string employeedId)
        {
            _logger.LogDebug($"Received employee create request for '{employeedId}'");

            var reportingStructure = _reportingStructureService.CreateReportingStructure(employeedId);

            return CreatedAtRoute("createReportingStructure", new { id = employeedId }, reportingStructure);
        }
    }
}
