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
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Raziel.Library.Models;
using Raziel.Ork.Classes;
using Raziel.Ork.Models;
using Tide.Encryption.Ecc;
using Tide.Encryption.Threshold.Authentication;
using Tide.Encryption.Tools;

namespace Raziel.Ork.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]
    public class Auth2Controller //: ControllerBase
    {
        private readonly AccountManager _manager;
        private readonly TParams _config;

        public Auth2Controller(TParams config, Settings settings, IMemoryCache memoryCache)
        {
            _manager = new AccountManager(new OrkRepo(settings, memoryCache));
            _config = config;
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

        [HttpPost("Si/{user}/{beta}")]
        public async Task<string> GetSi([FromRoute] string user, [FromRoute] string beta, [FromBody] IEnumerable<string> alphas)
        {
            return (await GetAuth(user).FinishMtA(beta.DecodeBase64Url(), alphas.Select(itm => TResponseAlpha.Parse(itm)))).ToString();
        }

        // POST api/values
        private TAuth GetAuth(string user)
        {
            var (password, key) = _manager.GetAccount(user);
            return new TAuth(_config, key, password);
        }
    }
}
