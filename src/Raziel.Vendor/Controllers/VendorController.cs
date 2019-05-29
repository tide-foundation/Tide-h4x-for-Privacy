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

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Raziel.Library.Models;

namespace Raziel.Vendor.Controllers {
    [ApiController]
    public class VendorController : ControllerBase {
        private readonly IVendorService _service;
        private readonly IHttpContextAccessor _accessor;

        public VendorController(IVendorService service, IHttpContextAccessor accessor) {
            _service = service;
            _accessor = accessor;
        }

        [HttpPost("/PostUser/")]
        public TideResponse PostUser([FromBody] AuthenticationRequest user)
        {
            return _service.PostUser(user);
        }

        [HttpPost("/Token")]
        public AuthenticationRequest Token([FromBody] AuthenticationRequest request) {
            return _service.GenerateToken(AttachLogInformation(request));
        }

        [Authorize("Bearer")]
        [HttpPost("/GetDetails")]
        public User GetDetails([FromBody] AuthenticationRequest request) {
            return _service.GetDetails(AttachLogInformation(request));
        }

        [Authorize("Bearer")]
        [HttpPost("/Save")]
        public bool Save([FromBody] AuthenticationRequest request) {
            return _service.Save(AttachLogInformation(request));
        }

        private AuthenticationRequest AttachLogInformation(AuthenticationRequest model)
        {
            model.Ip = _accessor.HttpContext.Connection.RemoteIpAddress.ToString();
            return model;
        }
    }
}