using Microsoft.AspNetCore.Mvc;
using Raziel.Library.Classes;
using Raziel.Library.Models;

namespace Raziel.Logger.Controllers
{
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ITideLogger _logger;

        public LogController(ITideLogger logger) {
            _logger = logger;
        }

        [HttpGet("/hey")]
        public string Hey() {
            return "Hello there";
        }

        [HttpPost("/log")]
        public void Log(TideLog log)
        {
            _logger.Log(log);
        }
    }
}
