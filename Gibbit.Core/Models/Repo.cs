using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gibbit.Core.Models
{

    public class Repos
    {
        [JsonProperty("total_count")]
        public double Total { get; set; }

        [JsonProperty("items")]
        public List<Repo> Data { get; set; }
    }

    public class Repo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("updated_at")]
        public DateTime Updated { get; set; }

        [JsonProperty("owner")]
        public Owner Owner { get; set; }

        public bool IsStarred { get; set; }
    }
}
