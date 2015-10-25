using Newtonsoft.Json;
using System;

namespace Gibbit.Core
{
    public class User
    {
        [JsonProperty("login")]
        public string UserName { get; set; }

        [JsonProperty("avatar_url")]
        public string avatarUrl { get; set; }
        public string AccessToken { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}

