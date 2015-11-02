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
using Gibbit.Core.Models;
using GibbitDroid.Helpers;
using Gibbit.Core.Managers;
using System.Threading.Tasks;

namespace GibbitDroid.Adapters
{
    [Activity(Label = "Repos")]

    public class RepoListAdapter : BaseAdapter<Repo>
    {
        private Token token;
        private User user;
        private List<Repo> repos;
        private Activity context;
        private FetchManager _fetch;

        public RepoListAdapter(Activity context, Token token, User user, List<Repo> repos) : base()
        {
            this.context = context;
            this.user = user;
            this.token = token;
            this.repos = repos;

            _fetch = new FetchManager();
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
            view.FindViewById<TextView>(Resource.Id.Line1).Text = string.Format("{0} - {1}", repo.Owner.Name, repo.Name);
            view.FindViewById<TextView>(Resource.Id.Line2).Text = string.Format("Last Updated: {0}", repo.Updated.ToLocalTime());
            view.FindViewById<ImageView>(Resource.Id.OwnerAvatar).SetImageBitmap(GetImageHelper.GetImageBitmapFromUrl(string.Format("{0}", repo.Owner.AvatarUrl)));
            return view;
        }

        private async void Unstar(object sender, EventArgs e, Repo repo)
        {
            var url = "https://api.github.com/user/starred/" +
                        repo.Owner.Name +
                        "/" +
                        repo.Name;
            await _fetch.DeleteJson(url, token);
            Toast.MakeText(context, string.Format("Unstar the {0} repository", repo.Name), ToastLength.Short).Show();
        }

        private async void Star(object sender, EventArgs e, Repo repo)
        {
            var url = "https://api.github.com/user/starred/" +
                        repo.Owner.Name +
                        "/" +
                        repo.Name;
            await _fetch.PutJson(url, token, null);
            Toast.MakeText(context, string.Format("Starred the {0} repository", repo.Name), ToastLength.Short).Show();
        }
    }
}