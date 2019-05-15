using System;

namespace Raziel.Library.Models {
    public class Challenge {
        public Challenge(string key, string value) {
            Key = key;
            Value = value;
            Entered = DateTimeOffset.Now;
        }

        public string Key { get; set; }
        public string Value { get; set; }
        public DateTimeOffset Entered { get; set; }
    }
}