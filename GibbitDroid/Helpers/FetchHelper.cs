using System;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace GibbitDroid.Helpers
{
    public class FetchHelper
    {
        public async Task<string> FetchJson(string url, string accessToken, string tokenAppName )
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
            request.UserAgent = tokenAppName;
            request.Headers.Add("Authorization", string.Format("Token {0}", accessToken));
            request.ContentType = "application/json";
            request.Method = "GET";

            using (WebResponse response = await request.GetResponseAsync())
            {
                var stream = response.GetResponseStream();
                using (var sr = new StreamReader(stream))
                {
                    var res = await sr.ReadToEndAsync();
                    return res;
                }
            }
        }
    }
}