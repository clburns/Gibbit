using Gibbit.Core.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace Gibbit.Core.Managers
{
    public class FetchManager
    {
        public async Task<string> GetJson(string url, Token token, bool isReadme = false)
        {
            HttpClient client = new HttpClient();

            if (isReadme == true)
            {
                client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3.html");
            }
            else
            {
                client.DefaultRequestHeaders.Add("Accept", "application/json");
            }
            
            client.DefaultRequestHeaders.Add("User-Agent", string.Format("{0}", token.TokenDescription));
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Token {0}", token.AccessToken));

            using (HttpResponseMessage response = await client.GetAsync(new Uri(url)))
            {
                string res = await response.Content.ReadAsStringAsync();
                return res;
            }
        }

        public async Task<string> DeleteJson(string url, Token token)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", string.Format("{0}", token.TokenDescription));
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Token {0}", token.AccessToken));

            using (HttpResponseMessage response = await client.DeleteAsync(new Uri(url)))
            {
                string res = await response.Content.ReadAsStringAsync();
                return res;
            }
        }

        public async Task<string> PutJson(string url, Token token, HttpContent content)
        {
            HttpClient client = new HttpClient();

            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", string.Format("{0}", token.TokenDescription));
            client.DefaultRequestHeaders.Add("Authorization", string.Format("Token {0}", token.AccessToken));

            using (HttpResponseMessage response = await client.PutAsync(new Uri(url), content))
            {
                string res = await response.Content.ReadAsStringAsync();
                return res;
            }
        }
    }
}
