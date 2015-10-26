using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using Gibbit.Core.Managers;
using Gibbit.Core.Models;
using GibbitDroid.Activites;
using GibbitDroid.Adapters;
using GibbitDroid.Helpers;
using System.Collections.Generic;

namespace GibbitDroid
{
    [Activity (Label = "Gibbit", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
        private readonly FetchManager _fetch;
        public Token token;
        public User user;
        public Activity context;
        public List<string> starredRepoList = new List<string>();
        public List<Repo> repos;
        public ListView starredRepoListView;
        public static Repo repo;

        public MainActivity()
        {
            _fetch = new FetchManager();
        }

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Button signIn = FindViewById<Button>(Resource.Id.SignIn);
            ImageView userAvatar = FindViewById<ImageView>(Resource.Id.UserAvatar);
            Button getStarred = FindViewById<Button>(Resource.Id.GetStarred);
            TextView greeting = FindViewById<TextView>(Resource.Id.Greeting);
            starredRepoListView = FindViewById<ListView>(Resource.Id.StarredRepoList);

            token = await GetLocalStorage.GetLocalAccessToken(context);

            signIn.Click += async (sender, e) =>
            {
                var url = "https://api.github.com/user";
                var json = await _fetch.GetJson(url, token);
                user = await ParseManager.Parse<User>(json);

                if (user != null)
                {
                    greeting.Text = string.Format("Welcome {0}", user.UserName);
                    var bitmap = GetImageHelper.GetImageBitmapFromUrl(string.Format("{0}", user.avatarUrl));
                    userAvatar.SetImageBitmap(bitmap);
                    getStarred.Enabled = true;
                    signIn.Visibility = Android.Views.ViewStates.Gone;
                }
            };

            getStarred.Click += async (sender, e) =>
            {
                var url = "https://api.github.com/users/" +
                user.UserName +
                "/starred";

                var json = await _fetch.GetJson(url, token);
                repos = await ParseManager.Parse<List<Repo>>(json);

                starredRepoListView.Adapter = new StarredRepoListAdapter(this, token, user, repos);
            };

            starredRepoListView.ItemClick += (sender, e) =>
            {
                var listView = sender as ListView;
                repo = repos[e.Position];
                //Toast.MakeText(this, string.Format("The Id for this repo is: {0}", repo.Id), ToastLength.Short).Show();
                var intent = new Intent(this, typeof(RepoActivity));
                StartActivity(intent);
            };
		}
    }
}


