using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gibbit.Core.Models
{
	public class Owner
	{
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("login")]
        public string Name { get; set; }

		[JsonProperty("avatar_url")]
		public string AvatarUrl { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
	}
}
