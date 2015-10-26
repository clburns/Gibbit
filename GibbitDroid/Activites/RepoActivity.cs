using Android.App;
using Android.OS;
using Android.Widget;

namespace GibbitDroid.Activites
{
    [Activity(Label = "Gibbit")]
    public class RepoActivity : MainActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.Repo);

            TextView text1 = FindViewById<TextView>(Resource.Id.Text1);

            text1.Text = string.Format("{0}", repo.Name);
        }
    }
}