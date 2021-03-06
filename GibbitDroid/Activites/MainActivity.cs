﻿using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Gibbit.Core.Managers;
using Gibbit.Core.Models;
using GibbitDroid.Activites;
using GibbitDroid.Adapters;
using GibbitDroid.Helpers;
using System;
using System.Collections.Generic;

namespace GibbitDroid
{
    [Activity (Label = "Gibbit", MainLauncher = true, Icon = "@drawable/icon", Theme = "@style/Theme.AppCompat")]
	public class MainActivity : ActionBarActivity
	{
        private readonly FetchManager _fetch;
        private readonly UrlManager _url;
        private ListView listView;
        
        private int page;
        private int totalPages;
        private string query;

        private TextView pageInfo;
        private Button previousPage;
        private Button nextPage;
        private LinearLayout navigation;

        private User user;
        private List<Repo> repos;

        public static Token token;
        public static Repo repo;

        public MainActivity()
        {
            _fetch = new FetchManager();
            _url = new UrlManager();
        }

        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Main);

            Button signIn = FindViewById<Button>(Resource.Id.SignIn);
            ImageView userAvatar = FindViewById<ImageView>(Resource.Id.UserAvatar);
            Button getStarred = FindViewById<Button>(Resource.Id.GetStarred);
            TextView greeting = FindViewById<TextView>(Resource.Id.Greeting);

            pageInfo = FindViewById<TextView>(Resource.Id.PageInfo);
            navigation = FindViewById<LinearLayout>(Resource.Id.Navigation);
            previousPage = FindViewById<Button>(Resource.Id.PreviousPage);
            nextPage = FindViewById<Button>(Resource.Id.NextPage);

            listView = FindViewById<ListView>(Resource.Id.RepoList);

            token = await GetLocalStorage.GetLocalAccessToken();

            signIn.Click += async (sender, e) =>
            {
                var json = await _fetch.GetJson(_url.User, token);
                user = await ParseManager.Parse<User>(json);

                if (user != null)
                {
                    greeting.Text = $"Welcome {user.UserName}";
                    var bitmap = GetImageHelper.GetImageBitmapFromUrl(user.avatarUrl);
                    userAvatar.SetImageBitmap(bitmap);
                    getStarred.Enabled = true;
                    signIn.Visibility = ViewStates.Gone;
                }
            };

            getStarred.Click += async (sender, e) =>
            {
                var json = await _fetch.GetJson(_url.Starred(user), token);
                repos = await ParseManager.Parse<List<Repo>>(json);
                foreach(Repo repo in repos) 
                {
                    repo.IsStarred = true;
                };

                navigation.Visibility = ViewStates.Gone;

                listView.Adapter = new RepoListAdapter(this, token, user, repos);
            };

            listView.ItemClick += (sender, e) =>
            {
                var listView = sender as ListView;
                repo = repos[e.Position];
                var intent = new Intent(this, typeof(RepoActivity));
                StartActivity(intent);
            };

            previousPage.Click += (sender1, e1) =>
            {
                page--;
                Search();
            };

            nextPage.Click += (sender2, e2) =>
            {
                page++;
                Search();
            };
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            //TODO: look at switching to Support v7 Toolbar
            MenuInflater.Inflate(Resource.Menu.RepoSearch, menu);

            var item = menu.FindItem(Resource.Id.action_search);

            var menuItem = MenuItemCompat.GetActionView(item);
            var searchView = menuItem.JavaCast<Android.Support.V7.Widget.SearchView>();

            searchView.QueryTextSubmit += (sender, e) =>
            {
                page = 1;

                query = e.Query;

                Search();

                e.Handled = true;
            };

            return true;
        }

        public async void Search()
        {
            var json = await _fetch.GetJson(_url.Search(query, page), token);
            var searchedRepos = await ParseManager.Parse<Repos>(json);
            totalPages = (int)Math.Ceiling(searchedRepos.Total / 10);
            repos = searchedRepos.Data;

            if (page == 1)
            {
                previousPage.Enabled = false;
            }
            else
            {
                previousPage.Enabled = true;
            }

            if (totalPages == 1)
            {
                nextPage.Enabled = false;
                navigation.Visibility = ViewStates.Gone;
            }
            else
            {
                nextPage.Enabled = true;
                navigation.Visibility = ViewStates.Visible;
            }

            pageInfo.Text = $"Page: {page} of {totalPages}";
            
            listView.Adapter = new RepoListAdapter(this, token, user, repos);
        }
    }
}


