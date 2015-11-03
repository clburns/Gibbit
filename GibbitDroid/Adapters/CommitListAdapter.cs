using Android.App;
using Android.Views;
using Android.Widget;
using Gibbit.Core.Models;
using System;
using System.Collections.Generic;

namespace GibbitDroid.Adapters
{
    [Activity(Label = "Commits")]
    public class CommitListAdapter : BaseAdapter<CommitRepo>
    {
        private List<CommitRepo> commits;
        private Activity context;

        public CommitListAdapter(Activity context, List<CommitRepo> commits) : base()
        {
            this.context = context;
            this.commits = commits;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override CommitRepo this[int position]
        {
            get { return commits[position]; }
        }

        public override int Count
        {
            get { return commits.Count; }
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var commit = commits[position];
            View view = convertView;
            if (view == null)
            {
                view = context.LayoutInflater.Inflate(Resource.Layout.CommitList, null);

                //TODO: make this menu actually do something.
                view.FindViewById<ImageView>(Resource.Id.EllipsesButton).Click += (sender, e) =>
                {
                    PopupMenu menu = new PopupMenu(Application.Context, view, GravityFlags.Right);
                    menu.Inflate(Resource.Menu.CommitMenu);

                    menu.MenuItemClick += (sender1, e1) =>
                    {
                        Console.WriteLine("{0} selected", e1.Item.TitleFormatted);
                    };

                    menu.Show();
                };
            }
            view.FindViewById<TextView>(Resource.Id.Message).Text = string.Format("{0}", commit.Commit.Message);
            view.FindViewById<TextView>(Resource.Id.Commiter).Text = string.Format("{0} commited on {1}", commit.Commit.Committer.Name, commit.Commit.Committer.Date.ToLocalTime());
            return view;
        }
    }
}