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
using SQLite;
using System.IO;
using Newtonsoft.Json;
using RentACar.UI.Modals;
using Android;
using RentACar.UI;

namespace RentACar.UI
{
    [Activity(Label = "Settings")]
    public class SettingsActivity : Activity
    {
        EditText txtService;
        ProgressBar progressLayout;
        CheckBox chkAutoSync;
        DataManager objDataManager;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar actionBar = ((Activity)this).ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetLogo(Resource.Drawable.ic_launcher);
            actionBar.SetDisplayUseLogoEnabled(true);
            // Create your application here
            SetContentView(Resource.Layout.Settings);
            txtService = FindViewById<EditText>(Resource.Id.txtService);
            chkAutoSync = FindViewById<CheckBox>(Resource.Id.chkAutoSync);


            objDataManager = new DataManager();
            //objDataManager.GetSetting();
            Setting objSetting = objDataManager.GetSetting();
            if (objSetting != null)
            {
                chkAutoSync.Checked = objSetting.AutoSync;
                txtService.Text = objSetting.ServiceEndPoint;
            }
            this.progressLayout = FindViewById<ProgressBar>(Resource.Id.progressLayout);
            this.progressLayout.Visibility = ViewStates.Gone;
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.dialog_complete_menu, menu);
            menu.FindItem(Resource.Id.menu_delete).SetVisible(false);
            return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_done:
                    if(!String.IsNullOrWhiteSpace(txtService.Text.Trim()))
                    {
                        try
                        {
                            this.progressLayout.Visibility = ViewStates.Gone;
                            // var db = new SQLiteConnection(dbPath);
                            Setting objSaveSetting = new Setting();
                            objSaveSetting.AutoSync = chkAutoSync.Checked;
                            objSaveSetting.ServiceEndPoint = txtService.Text;

                            objDataManager = new DataManager();
                            if (objDataManager.SaveSettingToLocal(objSaveSetting) > 0)
                            {
                                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                                alert.SetMessage("Settings has been saved successfully.");
                                alert.SetNeutralButton("OK", delegate
                                {
                                    var intent = new Intent(this, typeof(MainMenuActivity));
                                    StartActivity(intent);
                                });
                                alert.Create().Show();
                            }
                        }
                        catch (Exception ex)
                        {
                            Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
                        }
                    }
                    else
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetMessage("Please Enter Service End Point.");
                        alert.SetNeutralButton("OK", delegate { });
                        alert.Create().Show();
                        //  Toast.MakeText(this, "Please fill all mandatory details", ToastLength.Short).Show();
                        txtService.RequestFocus();
                    }
                    
                   
                    break;
                case Resource.Id.menu_cancel:
                    this.progressLayout.Visibility = ViewStates.Visible;
                    string FromActivity = Intent.GetStringExtra("FromActivity");
                    if (FromActivity == "Vehicle")
                    {
                       // // this.progressLayout.Visibility = ViewStates.Visible;
                       // //var intent_setting = new Intent(this, typeof(RentFlowVehicleActivity));
                       //// intent_setting.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                       // StartActivity(intent_setting);
                    }
                    else if (FromActivity == "MarkDamage")
                    {
                       // //  this.progressLayout.Visibility = ViewStates.Visible;
                       //// var intent_setting = new Intent(this, typeof(RentFlowMarkDamageActivity));
                       //// intent_setting.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                       // StartActivity(intent_setting);
                    }
                    else if (FromActivity == "CheckList")
                    {
                       // //this.progressLayout.Visibility = ViewStates.Visible;
                       //// var intent_setting = new Intent(this, typeof(RentFlowCheckListActivity));
                       //// intent_setting.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                       // StartActivity(intent_setting);
                    }
                    else if (FromActivity == "SignatureAvtivity")
                    {
                       // // this.progressLayout.Visibility = ViewStates.Visible;
                       // var intent_setting = new Intent(this, typeof(RentFlowSignatureActivity));
                       //// intent_setting.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                       // StartActivity(intent_setting);
                    }
                    else if (FromActivity == "DriverSignatureActivity")
                    {
                        //// this.progressLayout.Visibility = ViewStates.Visible;
                        //var intent_setting = new Intent(this, typeof(RentFlowDriverSignatureActivity));
                        ////intent_setting.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                        //StartActivity(intent_setting);
                    }
                    else if (FromActivity == "MainMenuActivity")
                    {
                        var intent_setting = new Intent(this, typeof(MainMenuActivity));
                       // intent_setting.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                        StartActivity(intent_setting);
                    }
                    else if (FromActivity == "Download")
                    {
                        var intent_setting = new Intent(this, typeof(MainMenuActivity));
                        //intent_setting.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                        StartActivity(intent_setting);
                    }
                    break;
                case Resource.Id.menu_sendsms:
                    //this.progressLayout.Visibility = ViewStates.Visible;
                    //var intent_sendsms = new Intent(this, typeof(SendSMSActivity));
                    //StartActivity(intent_sendsms);
                    break;
                case Resource.Id.menu_sendemail:
                    //this.progressLayout.Visibility = ViewStates.Visible;
                    //var intent_sendemail = new Intent(this, typeof(SendEmailActivity));
                    //StartActivity(intent_sendemail);
                    break;
                case Resource.Id.menu_settings:
                    this.progressLayout.Visibility = ViewStates.Visible;
                    var intent_settings = new Intent(this, typeof(SettingsActivity));
                    StartActivity(intent_settings);
                    break;
                case Resource.Id.menu_logout:
                    this.progressLayout.Visibility = ViewStates.Visible;
                    objDataManager = new DataManager();
                    objDataManager.Logout();
                    ApplicationClass.UserId = 0;
                    ApplicationClass.UserName = null;
                    ApplicationClass.CompanyId = 0;
                    var intent_logout = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent_logout);
                    break;
            }
            //Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }
    }
}