﻿// Tide Protocol - Infrastructure for the Personal Data economy
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
using Microsoft.Extensions.Caching.Memory;
using Raziel.Library.Models;
using Raziel.Ork.Classes;
using Raziel.Ork.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tide.Encryption.DSA;
using Tide.Encryption.Threshold.Authentication;
using Tide.Encryption.ThresSign;
using Tide.Encryption.Tools;

namespace Raziel.Ork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignController : ControllerBase
    {
        private readonly IAccountManager _manager;
        private readonly TParams _config;

        public SignController(TParams config, Settings settings, IMemoryCache memoryCache)
        {
            _manager = new AccountManager(new OrkRepo(settings, memoryCache));
            _config = config;
        }

        [HttpPost("Ki/{user}")]
        public async Task<string> GetKi([FromRoute] string user, [FromBody] string message)
        {
            return (await GetSign(user).PrepareSign(message)).ToString();
        }

        [HttpPost("Alpha/{user}")]
        public async Task<ResponseModel> GetAlpha([FromRoute] string user, [FromBody] IEnumerable<string> ks)
        {
            var mta = await GetSign(user).StartMtA(ks.Select(ki => ResponseKi.Parse(ki)).ToList());
            return new ResponseModel
            {
                Public = mta.Public.Select(itm => itm.ToString()),
                Private = mta.Private
            };
        }

        [HttpPost("Delta/{user}/{beta}")]
        public async Task<SingleResponseModel> GetDelta([FromRoute] string user, [FromRoute] string beta, [FromBody] IEnumerable<string> alphas)
        {
            var endMta = (await GetSign(user).FinishMtA(beta.DecodeBase64Url(), alphas.Select(itm => ResponseAlpha.Parse(itm))));
            return new SingleResponseModel
            {
                Public = endMta.Public.ToString(),
                Private = endMta.Private
            };
        }

        [HttpPost("RSi/{user}/{sigma}")]
        public async Task<string> GetRSi([FromRoute] string user, [FromRoute] string sigma, [FromBody] IEnumerable<string> deltas)
        {
            var (r, s) = (await GetSign(user).ComputeSign(sigma.DecodeBase64Url(), deltas.Select(itm => ResponseDelta.Parse(itm))));
            return DSAFormat.EncodeSignature(r, s);
        }

        private ITSign GetSign(string user)
        {
            var (_, key) = _manager.GetAccount(user);
            return new TSign(_config, key);
        }
    }
}
