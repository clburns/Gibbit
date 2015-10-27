using Newtonsoft.Json;
using System;

namespace Gibbit.Core.Models
{
    public class User
    {
        [JsonProperty("login")]
        public string UserName { get; set; }

        [JsonProperty("avatar_url")]
        public string avatarUrl { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        public Token AccessToken { get; set; }

        public bool IsAuthenticated { get; set; }
    }
}

