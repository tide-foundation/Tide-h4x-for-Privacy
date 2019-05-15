using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;

namespace Raziel.Library.Classes {
    public class AesCrypto {
        public static string Encrypt(string plainText, string key) {
            using (var aesAlg = Aes.Create()) {
                aesAlg.Key = Convert.FromBase64String(key);
                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream()) {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) {
                        using (var swEncrypt = new StreamWriter(csEncrypt)) {
                            swEncrypt.Write(plainText);
                        }

                        return Convert.ToBase64String(aesAlg.IV.Concat(msEncrypt.ToArray()).ToArray());
                    }
                }
            }
        }

        public static string Decrypt(string cipherText, string key) {
            var data = Convert.FromBase64String(cipherText);
            using (var aesAlg = Aes.Create()) {
                aesAlg.Key = Convert.FromBase64String(key);
                aesAlg.IV = data.Take(aesAlg.IV.Length).ToArray();

                using (var msDecrypt = new MemoryStream(data.Skip(aesAlg.IV.Length).ToArray())) {
                    using (var csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read)) {
                        using (var srDecrypt = new StreamReader(csDecrypt)) {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}