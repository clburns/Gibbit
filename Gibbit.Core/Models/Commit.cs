using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gibbit.Core.Models
{
    public class Commit
    {
        [JsonProperty("committer")]
        public CommitCommitter Committer { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
