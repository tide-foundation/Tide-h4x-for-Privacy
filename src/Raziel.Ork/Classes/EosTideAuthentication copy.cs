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

namespace Raziel.Ork.Classes {
    public class Throttler {
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
        public (bool result, int minutes) SlowDown(string ip)
        {
            // check for current epoch time
            var epoch = (DateTime.UtcNow - _epoch).TotalSeconds;

            CleanDictionary(epoch);

            // check if record exists for that end-point
            if (!Records.ContainsKey(ip)) Records.Add(ip, new Tuple<int, double>(1, epoch - 1));

            var (attempts, banTime) = Records[ip];

            // execute the authentication check only if ban expired or if still in the first 3 bans
            var result = attempts < 4 || epoch > banTime;

            // increase ban exponentially 
            var extraTime = (int)Math.Pow(2, attempts - 3);

            // if authentication failed:
            // increase Attempts counter for that record
            var additionalTime = epoch + 60 * extraTime;
            Records[ip] = new Tuple<int, double>(attempts + 1, additionalTime);

            // return result 
            return (result, extraTime);
        }

        private void CleanDictionary(double epoch) {
            try {
                // invoke clean-up every hour
                if (DateTime.UtcNow > _cleaner.AddHours(2)) {
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
    }
}