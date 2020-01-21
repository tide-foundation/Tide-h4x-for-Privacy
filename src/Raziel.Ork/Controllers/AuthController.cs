// Tide Protocol - Infrastructure for the Personal Data economy
// Copyright (C) 2019 Tide Foundation Ltd
// 
// This program is free software and is subject to the terms of 
// the Tide Community Open Source License as published by the 
// Tide Foundation Limited. You may modify it and redistribute 
// it in accordance with and subject to the terms of that License.
// This program is distributed WITHOUT WARRANTY of any kind, 
// including without any implied warranty of MERCHANTABILITY or 
// FITNESS FOR A PARTICULAR PURPOSE.
// See the Tide Community Open Source License for more details.
// You should have received a copy of the Tide Community Open 
// Source License along with this program.
// If not, see https://tide.org/licenses_tcosl-1-0-en

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raziel.Library.Models;

namespace Raziel.Ork.Controllers {
    [ApiExplorerSettings(IgnoreApi = true)]
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