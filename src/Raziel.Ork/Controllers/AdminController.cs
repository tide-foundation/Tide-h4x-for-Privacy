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

using Microsoft.AspNetCore.Mvc;
using Raziel.Library.Models;

// This controller is used strictly for account creation and should be disabled once all accounts are established for the hack
namespace Raziel.Ork.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase {
        private readonly IAdminTideAuthentication _tideAuthentication;

        public AdminController(IAdminTideAuthentication tideAuthentication)
        {
            _tideAuthentication = tideAuthentication;
        }

        [HttpGet("/CreateAccount")]
        public IActionResult CreateBlockchainAccount(string publicKey, string username)
        {
            return new JsonResult(_tideAuthentication.CreateAccount(publicKey, username, false));
        }

        [HttpPost("/PushFragment")]
        public IActionResult PushFragment(AuthenticationModel model)
        {
            model.SiteUrl = $"{Request.Scheme}://{Request.Host}";
            return new JsonResult(_tideAuthentication.AddFragment(model));
        }
    }
}