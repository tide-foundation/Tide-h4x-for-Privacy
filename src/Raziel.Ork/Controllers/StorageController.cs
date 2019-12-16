using System;
using Microsoft.AspNetCore.Mvc;
using Raziel.Ork.Classes;
using Raziel.Ork.Models;
using Tide.Encryption.Ecc;
using Tide.Encryption.EcDSA;
using Tide.Encryption.Threshold.Authentication;

namespace Raziel.Ork.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StorageController : ControllerBase
    {
        private TParams _config;
        private readonly IAccountManager _manager;

        public StorageController(TParams config, IAccountManager manager)
        {
            _manager = manager;
            _config = config;
        }

        [HttpGet]
        public ActionResult<string> GetId()
        {

            return _config.Id.ToString();
        }

        [HttpPost("{user}")]
        public void SetAccount(string user, [FromBody] AccountModel value)
        {
            _manager.SetAccount(user, new EcScalar(value.Password), new EcDSAKey(value.Key));
        }

        [HttpGet("{user}")]
        public string GetAccount(string user)
        {
            return _manager.GetAccount(user).Key.Y.X.ToString();
        }
    }
}
