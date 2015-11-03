using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Text;
using Android.Widget;
using Gibbit.Core.Managers;
using Gibbit.Core.Models;
using GibbitDroid.Adapters;
using System.Collections.Generic;

namespace GibbitDroid.Activites
{
    [Activity(Label = "Gibbit", Icon = "@drawable/icon", Theme = "@style/Theme.AppCompat")]
    public class RepoActivity : Activity
    {
        private readonly UrlManager _url;
        private readonly FetchManager _fetch;
        public List<CommitRepo> commits;

        public RepoActivity()
        {
            _fetch = new FetchManager();
            _url = new UrlManager();
        }
        protected override async void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Repo);

            TextView repoName = FindViewById<TextView>(Resource.Id.RepoName);
            TextView readMeView = FindViewById<TextView>(Resource.Id.Readme);
            ListView commitList = FindViewById<ListView>(Resource.Id.CommitList);

            repoName.Text = string.Format("{0} - {1}", MainActivity.repo.Owner.Name, MainActivity.repo.Name);

            readMeView.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();

            //Gets the readme and binds to the TextView
            var readme = await _fetch.GetJson(_url.Readme(MainActivity.repo), MainActivity.token, true);
            readMeView.TextFormatted = Html.FromHtml(readme);

            //Gets the commits and binds to the ListView
            var commitsJson = await _fetch.GetJson(_url.Commits(MainActivity.repo), MainActivity.token);
            commits = await ParseManager.Parse<List<CommitRepo>>(commitsJson);

            commitList.Adapter = new CommitListAdapter(this, commits);

        }

        //TODO: use either support v7 to do a toolbar menu with actions or a regular one for repo view
    }
}