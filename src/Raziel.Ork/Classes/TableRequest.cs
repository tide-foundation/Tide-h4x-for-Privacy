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
using Newtonsoft.Json;

namespace Raziel.Ork.Classes {
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