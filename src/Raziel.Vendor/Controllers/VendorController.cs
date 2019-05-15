using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Raziel.Library.Models;

namespace Raziel.Vendor.Controllers {
    [ApiController]
    public class VendorController : ControllerBase {
        private readonly IVendorService _service;

        public VendorController(IVendorService service) {
            _service = service;
        }

        [HttpPost("/Token")]
        public AuthenticationRequest Token([FromBody] AuthenticationRequest request) {
            return _service.GenerateToken(request);
        }

        [Authorize("Bearer")]
        [HttpPost("/GetDetails")]
        public User GetDetails([FromBody] AuthenticationRequest request) {
            return _service.GetDetails(request);
        }

        [Authorize("Bearer")]
        [HttpPost("/Save")]
        public bool Save([FromBody] AuthenticationRequest request) {
            return _service.Save(request);
        }
    }
}