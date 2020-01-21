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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Raziel.Library.Classes.Crypto;
using Raziel.Library.Models;
using Raziel.Ork.Classes;
using Raziel.Ork.Models;
using Tide.Encryption.Ecc;
using Tide.Encryption.Threshold.Authentication;
using Tide.Encryption.Tools;

namespace Raziel.Ork.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class SecureAuthController : ControllerBase
    {
        private readonly IAccountManager _manager;
        private readonly ILogger _logger;
        private readonly Throttler _throttler;
        private readonly IHttpContextAccessor _accessor;
        private readonly TParams _config;
        private readonly Settings _settings;

        private string RemoteIp => _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

        public SecureAuthController(TParams config, IAccountManager manager, ILogger<SecureAuthController> logger, Throttler throttler, IHttpContextAccessor accessor, Settings settings)
        {
            _manager = manager;
            _logger = logger;
            _throttler = throttler;
            _accessor = accessor;
            _config = config;
            _settings = settings;
        }

        [HttpGet("Ri/{user}")]
        public async Task<string> GetRi(string user)
        {
            return (await GetAuth(user).ShareRi()).ToString();
        }

        [HttpPost("Alpha/{user}/{pass}")]
        public async Task<ResponseModel> GetAlpha([FromRoute] string user, [FromRoute] string pass, [FromBody] IEnumerable<string> rs)
        {
            var mta = await GetAuth(user).StartMtA(new EcScalar(pass), rs.Select(ri => TResponseRi.Parse(ri)).ToList());
            return new ResponseModel
            {
                Public = mta.Public.Select(itm => itm.ToString()),
                Private = mta.Private
            };
        }

        [HttpPost("Si/{user}/{beta}/{pub}")]
        public async Task<IActionResult> GetSi([FromRoute] string user, [FromRoute] string beta, [FromRoute] string pub, [FromBody] IEnumerable<string> alphas)
        {
            /*
            var (result, minutes) = _throttler.SlowDown(RemoteIp);
            if (result)
            {
                _logger.LogInformation($"TideUser {user} hit throttle", RemoteIp);
                return BadRequest(new TideResponse(false, null, $"Too many requests. Try again in: {minutes} minutes."));
            }
            //*/
            var shareSi  = (await GetAuth(user).FinishMtA(beta.DecodeBase64Url(), alphas.Select(itm => TResponseAlpha.Parse(itm)))).ToString();
            var dto = new FragmentDto
            {
                CvkFragment = Cryptide.Instance.Encrypt(shareSi, pub.DecodeBase64Url()),
                CvkPublic = _manager.GetAccount(user).pub
            };

            _logger.LogInformation($"fragment sent to the user: {user}", RemoteIp);

            return Ok(new TideResponse(true, new { vendorFragment = dto }, null));
        }

        // POST api/values
        private TAuth GetAuth(string user)
        {
            var (password, key, pub) = _manager.GetAccount(user);
            return new TAuth(_config, key, password);
        }
    }
}
