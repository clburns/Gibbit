using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gibbit.Core.Models
{
    public class CommitRepo
    {
        [JsonProperty("sha")]
        public string Sha { get; set; }

        [JsonProperty("commit")]
        public Commit Commit { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("comments_url")]
        public string CommentsUrl { get; set; }
    }
}
