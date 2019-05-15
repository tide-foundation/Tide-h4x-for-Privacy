using System;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Raziel.Library.Classes {
    public static class ValidationManager {
        public static async Task<ValidationResult> ValidatePass(string input, string pass, string share, string randomKey, Task delay = null) {
            var inputBytes = Convert.FromBase64String(input + "==");
            var passBytes = Convert.FromBase64String(pass + "==");

            var passed = (inputBytes[0] ^ passBytes[0]) == 0;
            var hashValue = passed ? share : GetFakeShare(inputBytes, share, randomKey);
            if (delay != null) await delay;
            return new ValidationResult(hashValue, passed);
        }

        public static string GetFakeShare(byte[] input, string share, string randomKey) {
            var shareBytes = Convert.FromBase64String(share);
            var oAuthBytes = Convert.FromBase64String(randomKey);
            var size = (shareBytes.Length - 1) / 4;

            using (var hmac = size <= 32 ? new HMACSHA256(oAuthBytes) : new HMACSHA512(oAuthBytes) as HMAC) {
                var fakeSecret = hmac.ComputeHash(input.Concat(shareBytes).ToArray()).Take(size);
                var fakeShare = shareBytes.Take(shareBytes.Length - size * 2).Concat(fakeSecret).Concat(shareBytes.Skip(shareBytes.Length - size));
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