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

namespace GibbitDroid.Adapters
{
    [Activity(Label = "Starred Repos")]

    public class StarredRepoListAdapter : BaseAdapter<Repo>
    {
        List<Repo> repos;
        Activity context;

        public StarredRepoListAdapter(Activity context, List<Repo> repos) : base()
            {
                this.context = context;
                this.repos = repos;
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
                view = context.LayoutInflater.Inflate(Resource.Layout.StarredRepoList, null);
                view.FindViewById<ImageView>(Resource.Id.StarButton).Click += (sender, e) =>
                {
                    Toast.MakeText(this.context, string.Format("Unstar the {0} repository", repo.Name), ToastLength.Short).Show();
                };
            }
            view.FindViewById<TextView>(Resource.Id.Line1).Text = string.Format("{0} - {1}", repo.Owner.Name, repo.Name);
            view.FindViewById<TextView>(Resource.Id.Line2).Text = string.Format("Last Updated: {0}", repo.Updated.ToLocalTime());
            view.FindViewById<ImageView>(Resource.Id.OwnerAvatar).SetImageBitmap(GetImageHelper.GetImageBitmapFromUrl(string.Format("{0}", repo.Owner.AvatarUrl)));
            return view;
        }

        //void starButtonClick(object sender, AdapterView.ItemClickEventArgs e)
        //{
        //    Toast.MakeText(this.context, string.Format("Unstar the {0} repository"), ToastLength.Short).Show();
        //}
    }
}