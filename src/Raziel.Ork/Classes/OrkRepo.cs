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

using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using Raziel.Library.Classes;
using Raziel.Library.Models;
using Raziel.Ork.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Raziel.Ork.Classes
{
    public class OrkRepo
    {
        private readonly IMemoryCache _memoryCache;
        private readonly Settings _settings;

        public OrkRepo(Settings settings, IMemoryCache cache)
        {
            _settings = settings;
            _memoryCache = cache;
        }

        public TideUser GetUser(ulong username)
        {
            if (!_memoryCache.GetCacheObject(CacheKeys.TideUser, out TideUser user, username))
            {
                user = GetTableRow<TideUser>(_settings.Onboarding, _settings.UsersTable, username);
                if (user != null) _memoryCache.SetCacheObject(CacheKeys.TideUser, user);
            }

            return user;
        }
        public Fragment GetShare(ulong username)
        {
            if (!_memoryCache.GetCacheObject(CacheKeys.Fragment, out Fragment fragment, username))
            {
                fragment = GetTableRow<Fragment>(_settings.Account, _settings.FragmentsTable, username);
                if (fragment != null) _memoryCache.SetCacheObject(CacheKeys.Fragment, fragment);
            }

            return fragment;
        }

        // Performs a post request to an EOS smart-contract table
        private T GetTableRow<T>(string scope, string table, ulong index)
        {
            var tableRequest = new TableRequest
            {
                Code = _settings.Onboarding,
                Scope = scope,
                Table = table,
                Json = true,
                LowerBound = index.ToString(),
                UpperBound = (index + 1).ToString()
            };

            var postContent = new StringContent(JsonConvert.SerializeObject(tableRequest), Encoding.UTF8, "application/json");
            var client = new HttpClient { BaseAddress = new Uri(_settings.BlockchainEndpoint) };
            var response = client.PostAsync("/v1/chain/get_table_rows", postContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<TableRows<T>>(content).Rows.FirstOrDefault();
        }
    }
}
