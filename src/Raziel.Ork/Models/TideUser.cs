using System.Collections.Generic;
using Newtonsoft.Json;
using Raziel.Library.Models;

namespace Raziel.Ork.Models {
    public class TideUser : BaseModel {
        [JsonProperty("nodes")] public List<Node> Nodes { get; set; }
    }

    public class Node {
        [JsonProperty("ork_node")] public string OrkNode { get; set; }

        [JsonProperty("ork_url")] public string OrkUrl { get; set; }

        [JsonProperty("ork_public")] public string PublicKey { get; set; }
    }
}