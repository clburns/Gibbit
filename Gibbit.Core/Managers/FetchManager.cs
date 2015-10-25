using Gibbit.Core.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gibbit.Core.Managers
{
    public class FetchManager
    {
        public async Task<string> GetJson(string url, Token token)
        {
             HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", string.Format("{0}", token.TokenDescription));
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Token {0}", token.AccessToken));

            using (HttpResponseMessage response = await client.GetAsync(new Uri(url)))
            {
                var stream = await response.Content.ReadAsStreamAsync();
                using (var sr = new StreamReader(stream))
                {
                    string res = await response.Content.ReadAsStringAsync();
                    return res;
                }
            }
        }
    }
}
