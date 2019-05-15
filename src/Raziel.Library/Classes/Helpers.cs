using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Caching.Memory;
using Raziel.Library.Models;

namespace Raziel.Library.Classes {
    public static class Helpers {
        public static string RandomString(int length) {
            const string chars = "abcdefghijklmnopqrstuvwxyz";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public static ulong ConvertToUint64(this string input, bool needToHash = true) {
            var hashed = needToHash ? Sha256(input) : input;
            var bytes = Encoding.UTF8.GetBytes(hashed);

            return BitConverter.ToUInt64(bytes, 0);

            string Sha256(string value) {
                using (var hash = SHA256.Create()) {
                    return string.Concat(hash
                        .ComputeHash(Encoding.UTF8.GetBytes(value))
                        .Select(item => item.ToString("x2")));
                }
            }
        }


        #region Cache Repo

        private static readonly MemoryCacheEntryOptions MemoryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromDays(1));

        // Fetches a valid indexed item from the cache
        public static bool GetCacheObject<T>(this IMemoryCache cache, string key, out T item, ulong index = 0, string username = null) where T : BaseModel {
            item = default;

            // Fetch the list from the cache
            if (cache.TryGetValue(key, out List<T> itemFromCache)) {
                // Then search for the correct index and return it
                item = index != 0 ? itemFromCache.FirstOrDefault(i => i.Id == index) : itemFromCache.FirstOrDefault(i => i.Username == username);
                return true;
            }

            return false;
        }

        // Sets an item to the cache
        public static void SetCacheObject<T>(this IMemoryCache cache, string key, T item) where T : BaseModel {
            // Gather the list from cache
            var list = GetList<T>(cache, key);

            // Add the entry
            list.Add(item);

            // Commit to cache
            cache.Set(key, list, MemoryOptions);
        }

        private static List<T> GetList<T>(IMemoryCache cache, string key) {
            var list = (List<T>) cache.Get(key);
            if (list == null) {
                list = new List<T>();
                cache.Set(key, list, MemoryOptions);
            }

            return list;
        }

        #endregion
    }
}