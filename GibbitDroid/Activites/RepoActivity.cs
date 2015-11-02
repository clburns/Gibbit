using Android.App;
using Android.OS;
using Android.Widget;
using Gibbit.Core.Models;
using Gibbit.Core.Managers;
using System.Collections.Generic;
using GibbitDroid.Adapters;
using Android.Views;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Text;
using CommonMark;

namespace GibbitDroid.Activites
{
    [Activity(Label = "Gibbit", Theme = "@style/Theme.AppCompat")]
    public class RepoActivity : MainActivity
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

            repoName.Text = string.Format("{0} - {1}", repo.Owner.Name, repo.Name);

            readMeView.MovementMethod = new Android.Text.Method.ScrollingMovementMethod();

            //Gets the readme and binds to the TextView
            //TODO: need to cleanup and figure out a way to do this in fewer calls if possible.

            var readmeJson = await _fetch.GetJson(_url.Readme(repo), token);
            var readme = await ParseManager.Parse<RepoReadme>(readmeJson);
            var readmeJson2 = await _fetch.GetJson(readme.ReadmeUrl, token);

            readMeView.TextFormatted = Html.FromHtml(CommonMarkConverter.Convert(readmeJson2));


            //Gets the commits and binds to the ListView

            var commitsJson = await _fetch.GetJson(_url.Commits(repo), token);
            commits = await ParseManager.Parse<List<CommitRepo>>(commitsJson);

            commitList.Adapter = new CommitListAdapter(this, commits);

        }
    }
}