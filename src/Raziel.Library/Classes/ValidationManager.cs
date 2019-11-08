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
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Raziel.Library.Classes {
    public static class ValidationManager {
        public static async Task<ValidationResult> ValidatePass(string input, string pass, string share, string randomKey, Task delay = null) {
            var inputBytes = Convert.FromBase64String(input + "==");
            var passBytes = Convert.FromBase64String(pass + "==");

            var passed = (new BigInteger(inputBytes, true) ^ new BigInteger(passBytes, true)) == 0;
            var hashValue = passed ? share : GetFakeShare(inputBytes, share, randomKey);
            if (delay != null) await delay;
            return new ValidationResult(hashValue, passed);
        }

        public static string GetFakeShare(byte[] input, string share, string randomKey) {
            var shareBytes = Convert.FromBase64String(share);
            var oAuthBytes = Convert.FromBase64String(randomKey);
            var size = (shareBytes.Length - 1) / 4;

            using (var hmac = size <= 32 ? new HMACSHA256(oAuthBytes) : new HMACSHA512(oAuthBytes) as HMAC) {
                var fakeSecret = new BigInteger(hmac.ComputeHash(input.Concat(shareBytes).ToArray()).Take(size).ToArray(), true);
                var fakeBytes = (fakeSecret >> 1).ToByteArray(true);

                var fakeShare = shareBytes.Take(shareBytes.Length - size * 2).Concat(fakeBytes).Concat(shareBytes.Skip(shareBytes.Length - size));
                return Convert.ToBase64String(fakeShare.ToArray());
            }
        }
    }

    public class ValidationResult {
        public ValidationResult(string result, bool success) {
            Result = result;
            Success = success;
        }

        public string Result { get; set; }
        public bool Success { get; set; }
    }
}