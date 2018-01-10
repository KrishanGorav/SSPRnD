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
using RentACar.UI.Activities;
using RentACar.UI.Modals;

namespace RentACar.UI
{
    [Activity(Label = "PAYG")]
    public class MainMenuActivity : Activity
    {
        Button btnStartCover;
        Button btnStopCover;
        ProgressBar progressLayout;
        
        protected override void OnCreate(Bundle savedInstanceState)
        { 
            base.OnCreate(savedInstanceState);
           
            ActionBar actionBar = ((Activity)this).ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetLogo(Resource.Drawable.ic_launcher);
            actionBar.SetDisplayUseLogoEnabled(true);
            // Create your application here
            

            SetContentView(Resource.Layout.Menu);

            btnStartCover = FindViewById<Button>(Resource.Id.btnStartCover);
            btnStopCover = FindViewById<Button>(Resource.Id.btnStopCover);
            //loutAutoSync = FindViewById<LinearLayout>(Resource.Id.loutAutoSync);
            //AutoSync = FindViewById<Switch>(Resource.Id.AutoSync);
            //this.loutAutoSync.Visibility = ViewStates.Gone;
            //objDataManager.GetSetting();

            

        //DataManager objDataManager = new DataManager();
        //    Setting objSetting = objDataManager.GetSetting();
        //    if (objSetting != null)
        //    {
        //        if (objSetting.AutoSync)
        //        {
        //            this.loutAutoSync.Visibility = ViewStates.Gone;
        //        }
        //        else
        //        {
        //            this.loutAutoSync.Visibility = ViewStates.Visible;
        //        }
        //    }

            btnStartCover.Click += btnStartCover_Click;
            btnStopCover.Click += btnStopCover_Click;
            this.progressLayout = FindViewById<ProgressBar>(Resource.Id.progressLayout);
            this.progressLayout.Visibility = ViewStates.Gone;

            if (ApplicationClass.isJourneyRunning == true)
            {
                btnStartCover.Enabled = false;
                btnStartCover.SetTextColor(Android.Graphics.Color.Gray);
                btnStopCover.Enabled = true;
                btnStopCover.SetTextColor(Android.Graphics.Color.White);
            }
            else
            {
                btnStartCover.Enabled = true;
                btnStartCover.SetTextColor(Android.Graphics.Color.White);
                btnStopCover.Enabled = false;
                btnStopCover.SetTextColor(Android.Graphics.Color.Gray);
            }
        }

        private void btnAutoSync_Click(object sender, EventArgs e)
        {
            this.progressLayout.Visibility = ViewStates.Visible;
            //var intent = new Intent(this, typeof(RentFlowVehicleActivity));
            //intent.PutExtra("ShowVehicleForStatus", "IN");
            //intent.PutExtra("TransType", "PRE");

            //StartActivity(intent);
        }

        private void btnDownloadJobs_Click(object sender, EventArgs e)
        {
           this.progressLayout.Visibility = ViewStates.Visible;


           //// var intent_downloadjobs = new Intent(this, typeof(DownloadJobActivity));
           // StartActivity(intent_downloadjobs);
        }

        private void btnStartCover_Click(object sender, EventArgs e)
        {
            this.progressLayout.Visibility = ViewStates.Visible;
            StartService(new Intent(this, typeof(CoordinateService)));
            
            //if (ApplicationClass.locationProvider != null)
            {
                btnStartCover.Enabled = false;
                btnStartCover.SetTextColor(Android.Graphics.Color.Gray);
                btnStopCover.Enabled = true;
                btnStopCover.SetTextColor(Android.Graphics.Color.White);
                this.progressLayout.Visibility = ViewStates.Gone;
                ShowMessage("You cover has started.");
            }
            //else
            //{
            //    this.progressLayout.Visibility = ViewStates.Gone;
            //}
            
        }

        private void ShowMessage(string message)
        {
            var callDialog = new AlertDialog.Builder(this);
            callDialog.SetMessage(message);
            callDialog.SetNegativeButton("OK", delegate { });
            // Show the alert dialog to the user and wait for response.
            callDialog.Show();
        }

        private void RegisterService()
        {
            //_gpsServiceConnection = new GPSServiceConnection(_binder);
            //_gpsServiceIntent = new Intent(Android.App.Application.Context, typeof(GPSService));
            //BindService(_gpsServiceIntent, _gpsServiceConnection, Bind.AutoCreate);
        }

        private void btnStopCover_Click(object sender, EventArgs e)
        {
            this.progressLayout.Visibility = ViewStates.Visible;
            StopService(new Intent(this, typeof(CoordinateService)));
            btnStartCover.Enabled = true;
            btnStartCover.SetTextColor(Android.Graphics.Color.White);
            btnStopCover.Enabled = false;
            btnStopCover.SetTextColor(Android.Graphics.Color.Gray);
            this.progressLayout.Visibility = ViewStates.Gone;
            //ShowMessage("You cover has stopped.");
            var journeySummary = new Intent(this, typeof(JourneySummary));
            //intentSendSMS.PutExtra("MobileNo", rentRunningTrans.Mobile);
            //intentSendSMS.PutExtra("FromActivity", "MarkDamage");
            //intentSendSMS.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
            StartActivity(journeySummary);
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
                    //this.progressLayout.Visibility = ViewStates.Visible;
                    //var intent_sendsms = new Intent(this, typeof(SendSMSActivity));
                    //StartActivity(intent_sendsms);
                    break;
                case Resource.Id.menu_sendemail:
                   //this.progressLayout.Visibility = ViewStates.Visible;
                   // var intent_sendemail = new Intent(this, typeof(SendEmailActivity));
                   // StartActivity(intent_sendemail);
                    break;
                case Resource.Id.menu_dashboard:
                    var intent_dashboard = new Intent(this, typeof(MainMenuActivity));
                    StartActivity(intent_dashboard);
                    break;
                case Resource.Id.menu_settings:
                    this.progressLayout.Visibility = ViewStates.Visible;
                    var intent_settings = new Intent(this, typeof(SettingsActivity));
                    intent_settings.PutExtra("FromActivity", "MainMenuActivity");
                    StartActivity(intent_settings);
                    break;
                case Resource.Id.menu_logout:
                    this.progressLayout.Visibility = ViewStates.Visible;
                    DataManager objDataManager = new DataManager();
                    objDataManager.Logout();
                    ApplicationClass.userId = 0;
                    ApplicationClass.username = null;
                    ApplicationClass.UserDefaultVehicle = 0;
                    var intent_logout = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent_logout);
                    break;
            }
            //Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }
    }
}