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

using Newtonsoft.Json;

namespace Raziel.Library.Models {
    public class Fragment : BaseModel {
        [JsonProperty("public_key")] public string CvkPublic { get; set; }

        [JsonProperty("private_key_frag")] public string CvkFragment { get; set; }

        [JsonProperty("pass_hash")] public string PasswordHash { get; set; }
    }

    public class FragmentDto {
        public FragmentDto() {}

        public FragmentDto(Fragment fragment) {
            CvkPublic = fragment.CvkPublic;
            CvkFragment = fragment.CvkFragment;
        }

        [JsonProperty("public_key")] public string CvkPublic { get; set; }

        [JsonProperty("private_key_frag")] public string CvkFragment { get; set; }
    }
}