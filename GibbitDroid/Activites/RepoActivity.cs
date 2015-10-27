using Android.App;
using Android.OS;
using Android.Widget;
using Gibbit.Core.Models;
using Gibbit.Core.Managers;
using System.Collections.Generic;
using GibbitDroid.Adapters;
using Android.Views;
using Android.Support.V4.Widget;

namespace GibbitDroid.Activites
{
    [Activity(Label = "Gibbit")]
    public class RepoActivity : MainActivity
    {
        private readonly FetchManager _fetch;

        public RepoActivity()
        {
            _fetch = new FetchManager();
        }
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Repo);

            TextView repoName = FindViewById<TextView>(Resource.Id.RepoName);
            ListView commitList = FindViewById<ListView>(Resource.Id.CommitList);

            repoName.Text = string.Format("{0}", repo.Name);

            var url = "https://api.github.com/repos/" +
                repo.Owner.Name +
                "/" +
                repo.Name +
                "/commits";

            var json = await _fetch.GetJson(url, token);
            var commits = await ParseManager.Parse<List<CommitRepo>>(json);

            commitList.Adapter = new CommitListAdapter(this, commits);
        }
    }
}