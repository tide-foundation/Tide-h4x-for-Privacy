using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raziel.Library.Models;

namespace Raziel.Ork.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase {
        private readonly IHttpContextAccessor _accessor;
        private readonly ITideAuthentication _tideAuthentication;

        public AuthController(ITideAuthentication tideAuthentication, IHttpContextAccessor accessor) {
            _tideAuthentication = tideAuthentication;
            _accessor = accessor;
        }

        // Gather the nodes associated to a user
        [HttpPost("/Nodes")]
        public IActionResult GetUserNodes(AuthenticationModel model) {
            var response = _tideAuthentication.GetUserNodes(AttachLogInformation(model));
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        // Retrieve the fragment for the user belonging to this node
        [HttpPost("/Login")]
        public IActionResult Login(AuthenticationModel model) {
            var response = _tideAuthentication.Login(AttachLogInformation(model));
            if (response.Success) return Ok(response);
            return BadRequest(response);
        }

        private AuthenticationModel AttachLogInformation(AuthenticationModel model) {
            model.Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            return model;
        }
    }
}