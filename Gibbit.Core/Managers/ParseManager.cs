using Gibbit.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gibbit.Core.Managers
{
	public static class ParseManager
	{
        public static async Task<T> Parse<T> (string json) where T : class
        {
            var data = await Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(json));
            return data;
        }
	}
}
