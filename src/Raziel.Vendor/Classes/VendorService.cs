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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Raziel.Library.Classes;
using Raziel.Library.Classes.Crypto;
using Raziel.Library.Models;

namespace Raziel.Vendor.Classes {
    public class VendorService : IVendorService {
        private readonly RazielContext _context;
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly Settings _settings;

        public VendorService(RazielContext context, Settings settings, IMemoryCache memoryCache, ILoggerFactory logger) {
            _context = context;
            _settings = settings;
            _memoryCache = memoryCache;
            _logger = logger.CreateLogger("Vendor");
        }

        public TideResponse PostUser(AuthenticationRequest user) {
            try {
                if (user.Token != _settings.Password) return new TideResponse("Incorrect password");
                _context.Add(user.User);
                _context.SaveChanges();
                return  new TideResponse(true);
            }
            catch (Exception e) {
                return new TideResponse(e.Message);
            }
          
        }

        public AuthenticationRequest GenerateToken(AuthenticationRequest request) {
            var user = FetchUser(request.User.Username);
            if (user == null) return null;

            // Encrypt the token with the end users public key. If they're able to decrypt it we know they're valid
            request.Token = Cryptide.Instance.Encrypt(GenerateToken(request.User.Username), user.VendorPublicKey);

            _logger.LogMsg("Created token for user", new AuthenticationModel {Username = request.User.Username});
            return request;
        }

        public User GetDetails(AuthenticationRequest request) {
            _logger.LogMsg("Returned details for user", new AuthenticationModel {Username = request.User.Username});
            return FetchUser(request.User.Username);
        }

        public bool Save(AuthenticationRequest request) {
            try {
                var user = FetchUser(request.User.Username);
                if (user == null) return false;

                user.BitcoinPrivateKey = request.User.BitcoinPrivateKey;
                user.FirstName = request.User.FirstName;
                user.LastName = request.User.LastName;
                user.Note = request.User.Note;

                _logger.LogMsg("Updated details for user", new AuthenticationModel {Username = request.User.Username});
                _context.SaveChanges();
                return true;
            }
            catch (Exception) {
                return false;
            }
        }

        private User FetchUser(string username) {
            // Fetch the item from the cache, otherwise get it from the blockchain
            if (_memoryCache.GetCacheObject(CacheKeys.VendorUser, out User user, 0, username)) return user;
            user = _context.Users.FirstOrDefault(u => u.Username == username);
            if (user != null) _memoryCache.SetCacheObject(CacheKeys.VendorUser, user);

            return user;
        }

        private string GenerateToken(string publicKey) {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_settings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(new[] {
                    new Claim(ClaimTypes.Name, publicKey)
                }),
                Expires = DateTime.UtcNow.AddHours(5),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}