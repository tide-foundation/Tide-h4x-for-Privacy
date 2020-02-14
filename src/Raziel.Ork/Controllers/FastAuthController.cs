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
using Tide.Encryption.Ecc;
using Tide.Encryption.Threshold.Authentication;
using Tide.Encryption.Threshold.FastAuthentication;
using Tide.Encryption.Tools;

namespace Raziel.Ork.Controllers
{
    [Route("api/fastauth")]
    [ApiController]
    public class FastAuthController : ControllerBase
    {
        private readonly IAccountManager _manager;
        private readonly ILogger _logger;
        private readonly Throttler _throttler;
        private readonly IHttpContextAccessor _accessor;
        private readonly TParams _config;
        private readonly Settings _settings;

        private string RemoteIp => _accessor.HttpContext.Connection.RemoteIpAddress.ToString();

        public FastAuthController(TParams config, IAccountManager manager, ILogger<SecureAuthController> logger, Throttler throttler, IHttpContextAccessor accessor, Settings settings)
        {
            _manager = manager;
            _logger = logger;
            _throttler = throttler;
            _accessor = accessor;
            _config = config;
            _settings = settings;
        }

        [HttpGet("ci/{user}/{pass}/{threshold}/{ids}")]
        public async Task<IEnumerable<string>> GetCi(string user, string pass, int threshold, string ids)
        {
            var cs = (await GetAuth(user).ShareCiAsync(new EcScalar(pass), EcScalarListSerializer.Deserialize(ids.DecodeBase64Url()), threshold));
            return cs.Select(ci => ci.ToString());
        }

        [HttpPost("rj/{user}")]
        public async Task<IEnumerable<string>> GetRj([FromRoute] string user, [FromBody] IEnumerable<string> cs)
        {
            var rs = await GetAuth(user).ShareRjAsync(cs.Select(ci => TResponseCi.Parse(ci)).ToList());
            return rs.Select(ri => ri.ToString());
        }

        [HttpPost("key/{user}/{pub}")]
        public async Task<FragmentDto> GetKey([FromRoute] string user, [FromRoute] string pub, [FromBody] IEnumerable<string> alphas)
        {
            var sharekey = (await GetAuth(user).ShareKeyAsync(alphas.Select(itm => TResponseRj.Parse(itm)))).ToString();

            var dto = new FragmentDto
            {
                CvkFragment = Cryptide.Instance.Encrypt(sharekey, pub.DecodeBase64Url()),
                CvkPublic = _manager.GetAccount(user).pub
            };

            return dto;
        }

        private TFastAuth GetAuth(string user)
        {
            var (password, key, pub) = _manager.GetAccount(user);
            return new TFastAuth(_config, key, password);
        }
    }
}
