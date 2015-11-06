using Android.App;
using Android.Views;
using Android.Widget;
using Gibbit.Core.Managers;
using Gibbit.Core.Models;
using GibbitDroid.Helpers;
using System;
using System.Collections.Generic;

namespace GibbitDroid.Adapters
{
    [Activity(Label = "Repos")]

    public class RepoListAdapter : BaseAdapter<Repo>
    {
        private Token token;
        private User user;
        private List<Repo> repos;
        private Activity context;
        private readonly FetchManager _fetch;
        private readonly UrlManager _url;

        public RepoListAdapter(Activity context, Token token, User user, List<Repo> repos) : base()
        {
            this.context = context;
            this.user = user;
            this.token = token;
            this.repos = repos;

            _fetch = new FetchManager();
            _url = new UrlManager();
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override Repo this[int position]
        {
            get { return repos[position]; }
        }

        public override int Count
        {
            get { return repos.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var repo = repos[position];
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.RepoList, null);
                var starButton = view.FindViewById<ImageView>(Resource.Id.StarButton);
                if (repo.IsStarred == true)
                {
                    starButton.SetImageResource(Resource.Drawable.ic_star_white_24dp);
                    starButton.Click += (sender, e) =>
                    {
                        Unstar(sender, e, repo);
                        repos.RemoveAt(position);
                        NotifyDataSetChanged();
                    };
                }
                else if (repo.IsStarred == false)
                {
                    starButton.Click += (sender, e) =>
                    {
                        Star(sender, e, repo);
                        starButton.SetImageResource(Resource.Drawable.ic_star_white_24dp);
                    };
                };

            }
            view.FindViewById<TextView>(Resource.Id.Line1).Text = $"{repo.Owner.Name} - {repo.Name}";
            view.FindViewById<TextView>(Resource.Id.Line2).Text = $"Last Updated: {repo.Updated.ToLocalTime()}";
            view.FindViewById<ImageView>(Resource.Id.OwnerAvatar).SetImageBitmap(GetImageHelper.GetImageBitmapFromUrl(repo.Owner.AvatarUrl));
            return view;
        }

        private async void Unstar(object sender, EventArgs e, Repo repo)
        {
            await _fetch.DeleteJson(_url.Star(repo), token);
            Toast.MakeText(context, $"Unstarred the {repo.Name} repository", ToastLength.Short).Show();
        }

        private async void Star(object sender, EventArgs e, Repo repo)
        {
            await _fetch.PutJson(_url.Star(repo), token, null);
            Toast.MakeText(context, $"Starred the {repo.Name} repository", ToastLength.Short).Show();
        }
    }
}