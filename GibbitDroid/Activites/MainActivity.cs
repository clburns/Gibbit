using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Gibbit.Core.Managers;
using Gibbit.Core.Models;
using GibbitDroid.Activites;
using GibbitDroid.Adapters;
using GibbitDroid.Helpers;
using System.Collections.Generic;
using Android.Support.V4.View;

namespace GibbitDroid
{
    [Activity (Label = "Gibbit", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.AppCompat")]
	public class MainActivity : ActionBarActivity
	{
        private readonly FetchManager _fetch;
        private Android.Support.V7.Widget.SearchView _searchView;
        private ListView _listView;
        private StarredRepoListAdapter _adapter;
        
        public User user;
        public Activity context;
        public List<string> starredRepoList = new List<string>();
        public List<Repo> repos;

        public static Token token;
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
            _listView = FindViewById<ListView>(Resource.Id.StarredRepoList);

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
                var url = user.Url +
                          "/starred";

                var json = await _fetch.GetJson(url, token);
                repos = await ParseManager.Parse<List<Repo>>(json);

                _adapter = new StarredRepoListAdapter(this, token, user, repos);
                _listView.Adapter = _adapter;
            };

            _listView.ItemClick += (sender, e) =>
            {
                var listView = sender as ListView;
                repo = repos[e.Position];
                var intent = new Intent(this, typeof(RepoActivity));
                StartActivity(intent);
            };
		}

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.RepoSearch, menu);

            var item = menu.FindItem(Resource.Id.action_search);

            var searchView = MenuItemCompat.GetActionView(item);
            _searchView = searchView.JavaCast<Android.Support.V7.Widget.SearchView>();

            

            _searchView.QueryTextSubmit += async (sender, e) =>
            {
                var url = "https://api.github.com/search/repositories?q=" + e.Query;
                var json = await _fetch.GetJson(url, token);
                var searchedRepos = await ParseManager.Parse<Repos>(json);
                repos = searchedRepos.Data;
  

                _adapter = new StarredRepoListAdapter(this, token, user, repos);
                _listView.Adapter = _adapter;

                Toast.MakeText(this, "Searched for: " + e.Query, ToastLength.Short).Show();
                e.Handled = true;
            };

            return true;
        }
    }
}


