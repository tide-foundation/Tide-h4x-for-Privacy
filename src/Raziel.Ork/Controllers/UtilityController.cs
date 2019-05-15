using Microsoft.AspNetCore.Mvc;
using Raziel.Library.Models;

namespace Raziel.Ork.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class UtilityController : ControllerBase {
        private readonly Settings _settings;

        public UtilityController(Settings settings) {
            _settings = settings;
        }

        [HttpGet("/discover")]
        public TideResponse Discover() {
            var content = new {
                account = _settings.Account,
                url = $"{Request.Scheme}://{Request.Host}",
                publicKey = _settings.PublicKey
            };
            return new TideResponse(true, content, null);
        }
    }
}