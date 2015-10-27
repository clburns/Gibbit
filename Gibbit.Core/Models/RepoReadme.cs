using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gibbit.Core.Models
{
    public class RepoReadme
    {
        [JsonProperty("download_url")]
        public string ReadmeUrl { get; set; }
    }
}
