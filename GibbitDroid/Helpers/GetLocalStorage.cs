using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.IO;
using Gibbit.Core.Managers;
using Gibbit.Core.Models;
using System.Threading.Tasks;

namespace GibbitDroid.Helpers
{
    public class GetLocalStorage
    {
        public static async Task<Token> GetLocalAccessToken()
        {
            StreamReader strm = new StreamReader(Application.Context.Assets.Open("LocalStorage.txt"));
            var json = await strm.ReadToEndAsync();
            var token = await ParseManager.Parse<Token>(json);
            return token;
        }
    }
}