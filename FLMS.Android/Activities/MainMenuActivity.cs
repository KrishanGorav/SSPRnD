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
using RentACar.UI.Modals;

namespace RentACar.UI
{
    [Activity(Label = "PAYG")]
    public class MainMenuActivity : Activity
    {
        Button btnStartCover;
        Button btnStopCover;
        ProgressBar progressLayout;
        bool isServiceRunning = false;
        //LinearLayout loutAutoSync;

        GPSServiceBinder _binder;
        GPSServiceConnection _gpsServiceConnection;
        //Intent _gpsServiceIntent;
        //private GPSServiceReciever _receiver;

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

            if (isServiceRunning == true)
            {
                btnStartCover.Enabled = false;
                btnStopCover.Enabled = true;
            }
            else
            {
                btnStartCover.Enabled = true;
                btnStopCover.Enabled = false;
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
            StartService(new Intent(this, typeof(SimpleStartedService)));
            isServiceRunning = true;
            btnStartCover.Enabled = false;
            btnStopCover.Enabled = true;
            this.progressLayout.Visibility = ViewStates.Invisible;
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
            StopService(new Intent(this, typeof(SimpleStartedService)));
            isServiceRunning = false;
            btnStartCover.Enabled = true;
            btnStopCover.Enabled = false;
            this.progressLayout.Visibility = ViewStates.Invisible;
        }

        private void btnRentIN_Click(object sender, EventArgs e)
        {
           //this.progressLayout.Visibility = ViewStates.Visible;
           // var intent = new Intent(this, typeof(RentFlowVehicleActivity));
           // intent.PutExtra("ShowVehicleForStatus", "OUT");
           // intent.PutExtra("TransType", "IN");
            
           // StartActivity(intent);
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