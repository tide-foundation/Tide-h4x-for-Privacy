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
