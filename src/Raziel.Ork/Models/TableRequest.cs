using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Raziel.Ork.Models {
    [Serializable]
    public class TableRequest {
        [JsonProperty("json")] public bool? Json { get; set; } = false;

        [JsonProperty("code")] public string Code { get; set; }

        [JsonProperty("scope")] public string Scope { get; set; }

        [JsonProperty("table")] public string Table { get; set; }

        [JsonProperty("lower_bound")] public string LowerBound { get; set; } = "0";

        [JsonProperty("upper_bound")] public string UpperBound { get; set; } = "-1";

        [JsonProperty("limit")] public uint? Limit { get; set; } = 10U;
    }

    [Serializable]
    public class TableRows<TRowType> {
        [JsonProperty("rows")] public List<TRowType> Rows { get; set; }

        [JsonProperty("more")] public bool? More { get; set; }
    }
}