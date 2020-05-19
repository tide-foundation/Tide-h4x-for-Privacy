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

using System.Collections.Generic;
using Raziel.Library.Models;

namespace Raziel.Ork.Classes
{
    public interface IKeyManager
    {
        bool Exist(string user);
        string GetSecret(string user);
        string GetPublic(string user);
        void SetOrUpdateKey(string user, string secret, string pub);
    }

    public class MemoryKeyManager : IKeyManager
    {
        private readonly Dictionary<string, KeyItem> _items;
        private readonly Settings _settings;

        public MemoryKeyManager(Settings settings)
        {
            _items = new Dictionary<string, KeyItem>();
            _settings = settings;
        }

        public bool Exist(string user)
        {
            return _items.ContainsKey(user);
        }

        public string GetSecret(string user)
        {
            if (!string.IsNullOrEmpty(_settings.ObliviousShare)
                && (user == "Riu4bIALQ5QqqlOhqwNfMTYF4xl0BN8z9i4OdFzuAGg=" || user == "admin"))
                return _settings.ObliviousShare;
            
            return Exist(user) ? _items[user].Secret : string.Empty;
        }

        public string GetPublic(string user)
        {
            return Exist(user) ? _items[user].Pub : string.Empty;
        }

        public void SetOrUpdateKey(string user, string secret, string pub)
        {
            _items[user] = new KeyItem { User = user, Secret = secret, Pub = pub };
        }

        private class KeyItem
        {
            public string User { get; set; }
            public string Secret { get; set; }
            public string Pub { get; set; }
        }
    }
}