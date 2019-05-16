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
using System.Security.Cryptography;
using System.Text;

namespace Raziel.Library.Classes.Crypto {
    public class Utils {
        public static List<BigInteger> Decode(string text, int bits, bool isBase64 = false) {
            var data = isBase64 ? Convert.FromBase64String(text) : Encoding.UTF8.GetBytes(text);
            return Decode(data, bits);
        }

        public static List<BigInteger> Decode(IEnumerable<byte> data, int bits) {
            var j = 0;
            var blockSize = bits / 8;
            return (from byt in data
                group byt by j++ / blockSize
                into blk
                select new BigInteger(blk.ToArray(), true, true)).ToList();
        }

        public static string Encode(IReadOnlyList<BigInteger> numbers, int bits, bool isBase64 = false, bool fill = false) {
            var buffer = EncodeByteArray(numbers, bits, fill);
            return isBase64
                ? Convert.ToBase64String(buffer, 0, buffer.Length)
                : Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }

        public static byte[] EncodeByteArray(IReadOnlyList<BigInteger> numbers, int bits, bool fill = false) {
            var blockSize = (int) Math.Ceiling(bits / 8.0);
            var arrayBuffer = new List<List<byte>>();

            for (var i = 0; i < numbers.Count; i++) {
                var bytes = new List<byte>(numbers[i].ToByteArray(true, true));

                if (fill) {
                    //fill the empty bits
                    var diff = blockSize - bytes.Count;
                    for (var j = 0; j < diff; j++) {
                        bytes.Insert(0, 0);
                    }
                }

                arrayBuffer.Add(bytes);
            }

            return arrayBuffer.SelectMany(itm => itm).ToArray();
        }

        public static (BigInteger, BigInteger) RandomPrime(int bits, int confidence = 160) {
            //(2^(bits-2))+1 -> 100...001 <- length bits-1
            var minMask = (new BigInteger(2) << (bits - 3)) + 1;
            var bytes = new byte[(int) Math.Ceiling(bits / 8.0)];

            using (var rdmGen = new RNGCryptoServiceProvider()) {
                while (true) {
                    rdmGen.GetBytes(bytes);
                    var q = (new BigInteger(bytes, true, true) >> 1) | minMask;
                    if (!IsProbablePrime(q, confidence))
                        continue;

                    // p = (q * 2) + 1
                    var p = (q << 1) + BigInteger.One;
                    if (!IsProbablePrime(p, confidence))
                        continue;

                    return (p, q);
                }
            }
        }

        public static BigInteger getPrimitiveRoot(BigInteger p) {
            if (p == new BigInteger(2))
                return BigInteger.One;
            var min = new BigInteger(3); // Avoid g=2 because of Bleichenbacher's attack
            var p1 = new BigInteger(2); // The prime divisors of p-1 are 2 and (p-1)/2
            var p2 = (p - 1) / 2; // Because: p = 2q + 1 where q is a prime

            using (var rdmGen = new RandomField(p)) {
                while (true) {
                    var g = rdmGen.Generate(min);
                    if (BigInteger.ModPow(g, p1, p) != BigInteger.One &&
                        BigInteger.ModPow(g, p2, p) != BigInteger.One &&
                        BigInteger.Remainder(p - 1, g) != BigInteger.Zero && // g|p-1
                        BigInteger.Remainder(p - 1, BigInteger.ModPow(g, p - 2, p)) != BigInteger.Zero) // g^(-1)|p-1 (evades Khadir's attack)
                        return g;
                }
            }
        }

        public static bool IsProbablePrime(BigInteger source, int certainty = 160) {
            if (source == 2 || source == 3)
                return true;
            if (source < 2 || source % 2 == 0)
                return false;

            var d = source - 1;
            var s = 0;

            while (d % 2 == 0) {
                d /= 2;
                s += 1;
            }

            // There is no built-in method for generating random BigInteger values.
            // Instead, random BigIntegers are constructed from randomly generated
            // byte arrays of the same length as the source.
            var rng = RandomNumberGenerator.Create();
            var bytes = new byte[source.ToByteArray().LongLength];
            BigInteger a;

            for (var i = 0; i < certainty; i++) {
                do {
                    rng.GetBytes(bytes);
                    a = new BigInteger(bytes);
                } while (a < 2 || a >= source - 2);

                var x = BigInteger.ModPow(a, d, source);
                if (x == 1 || x == source - 1)
                    continue;

                for (var r = 1; r < s; r++) {
                    x = BigInteger.ModPow(x, 2, source);
                    if (x == 1)
                        return false;
                    if (x == source - 1)
                        break;
                }

                if (x != source - 1)
                    return false;
            }

            return true;
        }
    }
}