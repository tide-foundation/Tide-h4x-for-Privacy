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
using System.Net.Http;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Raziel.Library.Classes;
using Raziel.Library.Classes.Crypto;
using Raziel.Library.Models;

namespace Raziel.Ork.Classes {
    public class EosTideAuthentication : ITideAuthentication {
        private readonly ILogger _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly Settings _settings;

        public EosTideAuthentication(Settings settings, IMemoryCache memoryCache, ILoggerFactory logger) {
            _settings = settings;
            _memoryCache = memoryCache;
            _logger = logger.CreateLogger($"Ork-{settings.Account}");
        }

        public TideResponse GetUserNodes(AuthenticationModel model) {
            try {
                var usernameHash = model.Username.ConvertToUint64();

                // Fetch the item from the cache, otherwise get it from the blockchain
                if (!_memoryCache.GetCacheObject(CacheKeys.TideUser, out TideUser user, usernameHash)) {
                    user = GetTableRow<TideUser>(_settings.Onboarding, _settings.UsersTable, usernameHash);
                    if (user != null) _memoryCache.SetCacheObject(CacheKeys.TideUser, user);
                }

                if (user == null) {
                    _logger.LogMsg("Invalid user gathering nodes", model);
                    return new TideResponse(false, null, "Failed gathering nodes");
                }

                _logger.LogMsg("Gathered nodes", model);
                return new TideResponse(true, user.Nodes, null);
            }
            catch (Exception e) {
                _logger.LogMsg("Error gathering nodes", model, e);
                return new TideResponse(false, null, "Unable to gather nodes");
            }
        }

        public TideResponse Login(AuthenticationModel model) {
            try {
                var usernameHash = model.Username.ConvertToUint64();

                // Fetch the item from the cache, otherwise get it from the blockchain
                if (!_memoryCache.GetCacheObject(CacheKeys.Fragment, out Fragment fragment, usernameHash)) {
                    fragment = GetTableRow<Fragment>(_settings.Account, _settings.FragmentsTable, usernameHash);
                    if (fragment != null) _memoryCache.SetCacheObject(CacheKeys.Fragment, fragment);
                }

                if (fragment == null) {
                    _logger.LogMsg("Invalid user gathering fragment", model);
                    return new TideResponse(false, null, "That fragment does not exist");
                }

                // Process the request. Gather correct fragment if the password checks out, otherwise return junk data
                // Alternatively reject it if too many requests have been made
                var (success, result, minutes) = ProcessRequest(model, fragment);
                if (result == null) {
                    _logger.LogMsg("TideUser hit throttle", model);
                    return new TideResponse(false, null, $"Too many requests. Try again in: {minutes} minutes.");
                }

                // Convert to a transfer object
                var dto = new FragmentDto(fragment) {
                    CvkFragment = Cryptide.Instance.Encrypt(result, model.PublicKey)
                };

                _logger.LogMsg(success ? "Correct Fragment retrieved" : "Junk fragment retrieved", model);

                return new TideResponse(true, new {vendorFragment = dto}, null);
            }
            catch (Exception e) {
                _logger.LogMsg("Error gathering fragment", model, e);
                return new TideResponse(false, null, "Unable to gather fragment");
            }
        }

        // Performs a post request to an EOS smart-contract table
        private T GetTableRow<T>(string scope, string table, ulong index) {
            var tableRequest = new TableRequest {
                Code = _settings.Onboarding,
                Scope = scope,
                Table = table,
                Json = true,
                LowerBound = index.ToString(),
                UpperBound = (index + 1).ToString()
            };

            var postContent = new StringContent(JsonConvert.SerializeObject(tableRequest), Encoding.UTF8, "application/json");
            var client = new HttpClient {BaseAddress = new Uri(_settings.BlockchainEndpoint)};
            var response = client.PostAsync("/v1/chain/get_table_rows", postContent).Result;
            var content = response.Content.ReadAsStringAsync().Result;
            return JsonConvert.DeserializeObject<TableRows<T>>(content).Rows.FirstOrDefault();
        }

        #region Request Algorithm

        // Initialize a global dataset structure:
        // 	- key: end-point address
        //  - attempts: number of failed auth attempts
        //  - ban: epoch of expiry of latest ban
        private static readonly Dictionary<string, Tuple<int, double>> Records = new Dictionary<string, Tuple<int, double>>();

        private readonly DateTime _epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        // Reset a cleaner process
        private static DateTime _cleaner = DateTime.UtcNow;

        // This function runs on the ORK on every authentication request
        // epIP is the end-point IP/port address of the requester
        // Function returns the amount of minutes that requester is blocked from now - if the request failed. 0 is if the request is approved.
        private (bool success, string result, int minutes) ProcessRequest(AuthenticationModel model, Fragment fragment) {
            // initialize local variables
            string returnedValue = null;

            // check for current epoch time
            var epoch = (DateTime.UtcNow - _epoch).TotalSeconds;

            CleanDictionary(epoch);

            // check if record exists for that end-point
            if (!Records.ContainsKey(model.Ip)) Records.Add(model.Ip, new Tuple<int, double>(1, epoch - 1));

            var (item1, item2) = Records[model.Ip];

            ValidationResult validationResult = null;
            var success = false;
            // execute the authentication check only if ban expired or if still in the first 3 bans
            if (item1 < 4 || epoch > item2) {
                validationResult = ValidationManager.ValidatePass(model.PasswordHash, AesCrypto.Decrypt(fragment.PasswordHash, _settings.Password), AesCrypto.Decrypt(fragment.CvkFragment, _settings.Password), _settings.Key).Result;
                returnedValue = validationResult.Result;
                success = validationResult.Success;
            }

            var result = 0;

            if (validationResult != null && validationResult.Success) {
                // if authentication confirmed, reset record
                Records[model.Ip] = new Tuple<int, double>(1, epoch - 1);
            }
            else {
                // increase ban exponentially 
                result = (int) Math.Pow(2, item1 - 3);

                // if authentication failed:
                // increase Attempts counter for that record
                var additionalTime = epoch + 60 * result;
                Records[model.Ip] = new Tuple<int, double>(item1 + 1, additionalTime);
            }

            // return result 
            return (success, returnedValue, result);
        }

        private void CleanDictionary(double epoch) {
            try {
                // invoke clean-up every hour
                if (DateTime.UtcNow > _cleaner.AddSeconds(5)) {
                    // finding all records with expired bans
                    var deletableRecords = Records.Where(d => d.Value.Item2 < epoch);
                    foreach (var deletableRecord in deletableRecords) {
                        Records.Remove(deletableRecord.Key);
                    }

                    // reset clean-up time
                    _cleaner = DateTime.UtcNow;
                }
            }
            catch (Exception) {
                // Ignored
            }
        }

        #endregion
    }
}