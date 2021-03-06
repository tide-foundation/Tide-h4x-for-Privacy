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
    public class SignController : ControllerBase
    {
        private readonly IAccountManager _manager;
        private readonly ILogger<SignController> _logger;
        private readonly TParams _config;

        public SignController(TParams config, IAccountManager manager, ILogger<SignController> logger)
        {
            _manager = manager;
            this._logger = logger;
            _config = config;
        }

        [HttpGet("Ki/{user}/{m}")]
        public async Task<string> GetKi([FromRoute] string user, [FromRoute] string m)
        {
            return (await GetSign(user).PrepareSign(BigInteger.Parse(m))).ToString();
        }

        [HttpPost("Ki/{user}")]
        public async Task<string> PostKi([FromRoute] string user, [FromBody] string message)
        {
            return (await GetSign(user).PrepareSign(message)).ToString();
        }

        [HttpPost("Alpha/{user}")]
        public async Task<ResponseModel> GetAlpha([FromRoute] string user, [FromBody] IEnumerable<string> ks)
        {
            try {

                _logger.LogWarning($"<<<<<<<<----------{string.Join(',', ks)}");
                var mta = await GetSign(user).StartMtA(ks.Select(ResponseKi.Parse).ToList());
                return new ResponseModel
                {
                    Public = mta.Public.Select(itm => itm.ToString()),
                    Private = mta.Private
                };
            }
            catch (Exception e) {
                _logger.LogError(e.ToString());
                throw;
            }
           
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
            var sign = await GetSign(user).ComputeSign(sigma.DecodeBase64Url(), deltas.Select(itm => ResponseDelta.Parse(itm)));
            return sign.ToString();
        }

        private ITSign GetSign(string user)
        {
            var (_, key, pub) = _manager.GetAccount(user);
            return new OrkSignLoger(new TSign(_config, key), new WebBasicLoger(_logger));
        }
    }

    public class WebBasicLoger : IBasicLoger
    {
        private readonly ILogger _logger;
        public WebBasicLoger(ILogger _logger) => this._logger = _logger;

        public void WriteLine(string message) => _logger.LogInformation(message);
    }

}
