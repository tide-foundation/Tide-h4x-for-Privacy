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

using Raziel.Library.Classes;
using System.Collections.Generic;
using Tide.Encryption.Ecc;
using Tide.Encryption.EcDSA;
using Tide.Encryption.ElGamal;

namespace Raziel.Ork.Classes
{
    public interface IAccountManager
    {
        (EcScalar Pwd, EcDSAKey Key, string pub) GetAccount(string user);
        void SetAccount(string user, EcScalar password, EcDSAKey key);
    }

    public class AccountManager : IAccountManager
    {
        private readonly OrkRepo repo;

        public AccountManager(OrkRepo repo)
        {
            this.repo = repo;
        }

        public (EcScalar Pwd, EcDSAKey Key, string pub) GetAccount(string user)
        {
            var share = repo.GetShare(user.ConvertToUint64());
            return (new EcScalar(share.PasswordHash), EcDSAKey.FromPrivate(share.CvkFragment), share.CvkPublic);
        }

        public void SetAccount(string user, EcScalar password, EcDSAKey key)
        {
            throw new System.NotImplementedException();
        }
    }


    public class EnvAccountManager : IAccountManager
    {
        private readonly EcDSAKey share;
        private readonly EcScalar pass;
        private readonly ElGamalKey pub;

        public EnvAccountManager(EcDSAKey share, ElGamalKey pub, EcScalar pass)
        {
            this.share = share;
            this.pass = pass;
            this.pub = pub;
        }

        public EnvAccountManager(string share, string pub, string pass) 
            : this(EcDSAKey.FromPrivate(share), new ElGamalKey(pub),new EcScalar(pass)) { }

        public (EcScalar Pwd, EcDSAKey Key, string pub) GetAccount(string user)
        {
            return (pass, share, pub.ToString());
        }

        public void SetAccount(string user, EcScalar password, EcDSAKey key)
        {
            throw new System.NotImplementedException();
        }
    }

    public class MemoryAccountManager : IAccountManager
    {
        private readonly Dictionary<string, (EcScalar, EcDSAKey, string)> _items;

        public MemoryAccountManager()
        {
            _items = new Dictionary<string, (EcScalar, EcDSAKey, string)>();
        }

        public void SetAccount(string user, EcScalar password, EcDSAKey key)
        {
            _items[user] = (password, key, string.Empty);
        }

        public (EcScalar Pwd, EcDSAKey Key, string pub) GetAccount(string user)
        {
            if (!_items.ContainsKey(user))
                return (EcScalar.Random(), new EcDSAKey(), string.Empty);

            return _items[user];
        }
    }
}