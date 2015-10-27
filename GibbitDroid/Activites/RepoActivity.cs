using Android.App;
using Android.OS;
using Android.Widget;
using Gibbit.Core.Models;
using Gibbit.Core.Managers;
using System.Collections.Generic;
using GibbitDroid.Adapters;
using Android.Views;
using Android.Support.V4.Widget;
using Android.Text;
using CommonMark;

namespace GibbitDroid.Activites
{
    [Activity(Label = "Gibbit")]
    public class RepoActivity : MainActivity
    {
        private readonly FetchManager _fetch;
        public List<CommitRepo> commits;

        public RepoActivity()
        {
            _fetch = new FetchManager();
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
            var readmeUrl = repo.Url +
                            "/readme";
            var readmeJson = await _fetch.GetJson(readmeUrl, token);
            var readme = await ParseManager.Parse<RepoReadme>(readmeJson);
            var readmeJson2 = await _fetch.GetJson(readme.ReadmeUrl, token);

            readMeView.TextFormatted = Html.FromHtml(CommonMarkConverter.Convert(readmeJson2));


            //Gets the commits and binds to the ListView
            var commitsUrl = repo.Url +
                                 "/commits"; 

            var commitsJson = await _fetch.GetJson(commitsUrl, token);
            commits = await ParseManager.Parse<List<CommitRepo>>(commitsJson);

            commitList.Adapter = new CommitListAdapter(this, commits);

        }
    }
}