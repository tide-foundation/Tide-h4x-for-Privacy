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