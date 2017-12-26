using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using RentACar.UI;

namespace RentACar.UI
{
    [Activity(Label = "Capture Video")]
    public class CaptureVideoActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar actionBar = ((Activity)this).ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetLogo(Resource.Drawable.ic_launcher);
            actionBar.SetDisplayUseLogoEnabled(true);
            // Create your application here
            SetContentView(Resource.Layout.CaptureVideo);
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.common_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_sendsms:
                    var intent_sendsms = new Intent(this, typeof(SendSMSActivity));
                    StartActivity(intent_sendsms);
                    break;
                case Resource.Id.menu_sendemail:
                    var intent_sendemail = new Intent(this, typeof(SendEmailActivity));
                    StartActivity(intent_sendemail);
                    break;
                case Resource.Id.menu_settings:
                    //var intent_settings = new Intent(this, typeof(SettingsActivity));
                    //StartActivity(intent_settings);
                    break;
                case Resource.Id.menu_logout:
                    var intent_logout = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent_logout);
                    break;
            }
            //Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }
    }
}