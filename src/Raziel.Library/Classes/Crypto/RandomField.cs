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

namespace Raziel.Library.Classes.Crypto {
    public class RandomField : IDisposable {
        private readonly RNGCryptoServiceProvider _rdm;

        public RandomField(int field) : this(new BigInteger(field)) {
        }

        public RandomField(BigInteger field) {
            Field = field;
            _rdm = new RNGCryptoServiceProvider();
        }

        public BigInteger Field { get; }
        private int Bytes => Field.GetByteCount(true);

        public void Dispose() {
            _rdm.Dispose();
        }

        public BigInteger Generate(BigInteger? min = null) {
            var bytes = new byte[Bytes];
            BigInteger number;

            do {
                _rdm.GetBytes(bytes);
                number = new BigInteger(bytes, true, true);
            } while (number >= Field || min.HasValue && number < min.Value);

            return number;
        }

        public List<BigInteger> GenerateUniqueSet(int num) {
            var numbers = new List<BigInteger>();
            while (numbers.Count < num) {
                var rdm = Generate();
                if (!numbers.Any(n => n == rdm))
                    numbers.Add(rdm);
            }

            return numbers;
        }
    }
}