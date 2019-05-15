using Newtonsoft.Json;

namespace Raziel.Library.Models {
    public class Fragment : BaseModel {
        [JsonProperty("public_key")] public string CvkPublic { get; set; }

        [JsonProperty("private_key_frag")] public string CvkFragment { get; set; }

        [JsonProperty("pass_hash")] public string PasswordHash { get; set; }
    }

    public class FragmentDto {
        public FragmentDto(Fragment fragment) {
            CvkPublic = fragment.CvkPublic;
            CvkFragment = fragment.CvkFragment;
        }

        [JsonProperty("public_key")] public string CvkPublic { get; set; }

        [JsonProperty("private_key_frag")] public string CvkFragment { get; set; }
    }
}