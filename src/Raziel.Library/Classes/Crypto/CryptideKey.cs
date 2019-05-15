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
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace Raziel.Library.Classes.Crypto
{
    public class CryptideKey
    {
        public CryptideKey(bool secret, BigInteger p, BigInteger g, BigInteger key, BigInteger? id = null)
        {
            Secret = secret;
            P = p;
            G = g;
            Key = key;
            Id = id;
            Q = (p - 1) / 2;
        }

        public CryptideKey(string secret)
        {
            var bytes = Convert.FromBase64String(secret);
            var length = (bytes[0] & 1) == 1 ? 4 : 3;
            Secret = (bytes[0] & 0b10) == 1;

            var numbers = Utils.Decode(bytes.Skip(1), (bytes.Length - 1) * 8 / length);
            (P, G, Key) = (numbers[0], numbers[1], numbers[2]);
            Q = (P - 1) / 2;
            if (length > 3)
                Id = numbers[3];
        }

        public bool Secret { get; }
        public BigInteger P { get; }
        public BigInteger G { get; }
        public BigInteger Q { get; }
        public BigInteger Key { get; }
        public BigInteger? Id { get; }
        public int Bits { get => P.GetByteCount(true) * 8; }

        public static (CryptideKey sec, CryptideKey pub) Generate(int bitSize = 128)
        {
            var (p, q) = Utils.RandomPrime(bitSize);
            var g = Utils.getPrimitiveRoot(p);

            using (var rdmGen = new RandomField(q))
            {
                var x = rdmGen.Generate(BigInteger.One);
                var y = BigInteger.ModPow(g, x, p);

                return (new CryptideKey(true, p, g, x), new CryptideKey(false, p, g, y));
            }
        }

        public string Encrypt(string data)
        {
            var buffer = EncryptBuffer(data);
            return Convert.ToBase64String(buffer, 0, buffer.Length);
        }

        public byte[] EncryptBuffer(string data)
        {
            var ms = Utils.Decode(data, this.Bits);
            var nums = new List<BigInteger>();

            using (var rdm = new RandomField(P))
            {
                for (var i = 0; i < ms.Count; i++)
                {
                    var r = rdm.Generate();
                    var c1 = BigInteger.ModPow(G, r, P);
                    var c2 = BigInteger.Remainder(ms[i] * BigInteger.ModPow(this.Key, r, this.P), P);

                    nums.Add(c1);
                    nums.Add(c2);
                }
            }

            return Utils.EncodeByteArray(nums, this.Bits, true);
        }

        public string Decrypt(string cypher)
        {
            var buffer = DecryptBuffer(cypher);
            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }

        public byte[] DecryptBuffer(string cypher)
        {
            var chunks = Utils.Decode(cypher, Bits, isBase64: true);
            var numbers = new List<BigInteger>();
            for (var i = 0; i < chunks.Count; i += 2)
            {
                var c1 = chunks[i];
                var c2 = chunks[i + 1];

                var s = BigInteger.ModPow(c1, Key, P);
                var sInv = BigInteger.ModPow(s, P - 2, P);
                var m = BigInteger.Remainder(sInv * c2, P);
                numbers.Add(m);
            }

            return Utils.EncodeByteArray(numbers, Bits);
        }

        public override string ToString()
        {
            var values = new List<BigInteger>() { P, G, Key };
            if (Id.HasValue)
                values.Add(this.Id.Value);

            var bytes = Utils.EncodeByteArray(values, Bits, true);
            var falgs = (byte)((this.Secret ? 1 : 0) << 1 | (Id.HasValue ? 1 : 0));
            return Convert.ToBase64String((new byte[] { falgs }).Concat(bytes).ToArray());
        }
    }
}