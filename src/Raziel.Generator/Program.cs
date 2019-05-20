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
using System.Security.Cryptography;
using Raziel.Library.Classes.Crypto;

namespace Raziel.Generator
{
    internal class Program
    {
        private static void Main(string[] args)
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