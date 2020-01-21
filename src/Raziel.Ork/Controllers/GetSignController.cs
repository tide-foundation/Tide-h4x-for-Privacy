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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Raziel.Ork.Classes;
using Raziel.Ork.Models;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Tide.Encryption.Threshold.Authentication;
using Tide.Encryption.ThresSign;
using Tide.Encryption.Tools;

namespace Raziel.Ork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetSignController : ControllerBase
    {
        private readonly IAccountManager _manager;
        private readonly ILogger<SignController> _logger;
        private readonly TParams _config;

        public GetSignController(TParams config, IAccountManager manager, ILogger<SignController> logger)
        {
            _manager = manager;
            this._logger = logger;
            _config = config;
        }

        [HttpGet("Alpha/{user}")]
        public async Task<ResponseModel> GetAlpha([FromRoute] string user, [FromQuery] IEnumerable<string> ks)
        {
            if (ks == null || !ks.Any())
                return null;

            var mta = await GetSign(user).StartMtA(ks.Select(ki => ResponseKi.Parse(ki.DecodeBase64Url())).ToList());
            return new ResponseModel
            {
                Public = mta.Public.Select(itm => itm.ToString()),
                Private = mta.Private
            };
        }

        [HttpGet("Delta/{user}/{beta}")]
        public async Task<SingleResponseModel> GetDelta([FromRoute] string user, [FromRoute] string beta, [FromQuery] IEnumerable<string> alphas)
        {
            if (alphas == null || !alphas.Any())
                return null;

            var endMta = (await GetSign(user).FinishMtA(beta.DecodeBase64Url(), alphas.Select(itm => ResponseAlpha.Parse(itm.DecodeBase64Url()))));
            return new SingleResponseModel
            {
                Public = endMta.Public.ToString(),
                Private = endMta.Private
            };
        }

        [HttpGet("RSi/{user}/{sigma}")]
        public async Task<string> GetRSi([FromRoute] string user, [FromRoute] string sigma, [FromQuery] IEnumerable<string> deltas)
        {
            if (deltas == null || !deltas.Any())
                return null;

            var sign = await GetSign(user).ComputeSign(sigma.DecodeBase64Url(), deltas.Select(itm => ResponseDelta.Parse(itm.DecodeBase64Url())));
            return sign.ToString();
        }

        private ITSign GetSign(string user)
        {
            var (_, key, pub) = _manager.GetAccount(user);
            return new OrkSignLoger(new TSign(_config, key), new WebBasicLoger(_logger));
        }
    }
}
