using System;
using Tide.Encryption.AesMAC;

namespace Raziel.Library.Classes
{
    public class AesCrypto
    {
        public static string Encrypt(string plainText, string key)
        {
            return AesKey.Parse(key).EncryptStr(plainText);
        }

        public static string Decrypt(string cipherText, string key)
        {
            return AesKey.Parse(key).DecryptStr(cipherText);
        }
    }
}