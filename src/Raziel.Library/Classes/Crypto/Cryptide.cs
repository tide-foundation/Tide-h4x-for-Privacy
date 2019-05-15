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

namespace Raziel.Library.Classes.Crypto {
    public class Cryptide {
        public static readonly Cryptide Instance = new Cryptide();

        public (string Private, string Public) GetKey() {
            var (sec, pub) = CryptideKey.Generate();
            return (sec.ToString(), pub.ToString());
        }

        [Obsolete("This method is not yet implemented.", true)]
        public string[] Share(int points, int threshold, string key) {
            throw new NotImplementedException();
        }

        [Obsolete("This method is not yet implemented.", true)]
        public string ReconstructText(string[] shares) {
            throw new NotImplementedException();
        }

        [Obsolete("This method is not yet implemented.", true)]
        public string[] ShareText(int points, int threshold, string message) {
            throw new NotImplementedException();
        }

        public string Encrypt(string data, string key) {
            return new CryptideKey(key).Encrypt(data);
        }

        public string Decrypt(string data, string key) {
            return new CryptideKey(key).Decrypt(data);
        }

        [Obsolete("This method is not yet implemented.", true)]
        public string DecryptPartial(string data, string key) {
            throw new NotImplementedException();
        }

        [Obsolete("This method is not yet implemented.", true)]
        public string ReconstructPartial(string[] data, string key) {
            throw new NotImplementedException();
        }
    }
}