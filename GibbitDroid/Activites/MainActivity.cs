using Android.App;
using Android.OS;
using Android.Widget;

using Gibbit.Core.Managers;
using Gibbit.Core.Models;
using GibbitDroid.Helpers;
using GibbitDroid.Adapters;

using System.Collections.Generic;

namespace GibbitDroid
{
    [Activity (Label = "Gibbit", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        private readonly FetchHelper _fetch;
        private string tokenAppName;
        private string accessToken;
        public User User;
        public Activity context;
        public List<string> starredRepoList = new List<string>();

        public MainActivity()
        {
            _fetch = new FetchHelper();
        }

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Button signIn = FindViewById<Button>(Resource.Id.SignIn);
            ImageView userAvatar = FindViewById<ImageView>(Resource.Id.UserAvatar);
            Button getStarred = FindViewById<Button>(Resource.Id.GetStarred);
            TextView greeting = FindViewById<TextView>(Resource.Id.Greeting);
            ListView starredRepoListView = FindViewById<ListView>(Resource.Id.StarredRepoList);

            var token = await GetLocalStorage.GetLocalAccessToken(context);
            accessToken = string.Format("{0}", token.AccessToken);
            tokenAppName = string.Format("{0}", token.AppName);

            signIn.Click += async (sender, e) =>
            {
                var url = "https://api.github.com/user";
                var json = await _fetch.FetchJson(url, accessToken, tokenAppName);
                var user = await ParseManager.Parse<User>(json);

                User = user;

                if (user != null)
                {
                    greeting.Text = string.Format("Welcome {0}", user.UserName);
                    var bitmap = GetImageHelper.GetImageBitmapFromUrl(string.Format("{0}", user.avatarUrl));
                    userAvatar.SetImageBitmap(bitmap);
                    getStarred.Enabled = true;
                }
            };

            getStarred.Click += async (sender, e) =>
            {
                var url = "https://api.github.com/users/" +
                            User.UserName +
                            "/starred";
                var json = await _fetch.FetchJson(url, accessToken, tokenAppName);
                var repos = await ParseManager.Parse<List<Repo>>(json);

                starredRepoListView.Adapter = new StarredRepoListAdapter(this, repos);
            };
		}
	}
}


