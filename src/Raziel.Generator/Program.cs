using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Raziel.Library.Classes.Crypto;

namespace Raziel.Generator
{
    class Program
    {
        static void Main(string[] args)
        {
            var amount = 1;
            if (args.Any()) int.TryParse(args[0], out amount);

            for (var i = 0; i < amount; i++)
            {
                var pair = Cryptide.Instance.GetKey();
                Console.WriteLine($"\nPublic: {pair.Public}\nPrivate: {pair.Private}\nPassword: {GetKey()}\nKey: {GetKey()}");
            }
        }

        public static string GetKey(int byteSize = 32)
        {
            var buffer = new byte[byteSize];
            using var rdm = new RNGCryptoServiceProvider();
            rdm.GetBytes(buffer);
            return Convert.ToBase64String(buffer, 0, buffer.Length);
        }
    }
}