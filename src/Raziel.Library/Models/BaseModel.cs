using Newtonsoft.Json;

namespace Raziel.Library.Models {
    public class BaseModel {
        [JsonProperty("id")] public ulong Id { get; set; }
        public string Username { get; set; }
    }
}