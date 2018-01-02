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
using Android.Graphics;
using Xamarin.Controls;
using Newtonsoft.Json;
using RentACar.UI.Modals;
using Android;

namespace RentACar.UI
{
    [Activity(Label = "Signature")]
    public class RentFlowDriverSignatureActivity : Activity
    {
        RentRunningTrans rentRunningTrans;
        ProgressBar progressLayout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar actionBar = ((Activity)this).ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetLogo(Resource.Drawable.ic_launcher);
            actionBar.SetDisplayUseLogoEnabled(true);
            // Create your application here
            SetContentView(Resource.Layout.RentFlowDriverSignature);

            this.progressLayout = FindViewById<ProgressBar>(Resource.Id.progressLayout);
            this.progressLayout.Visibility = ViewStates.Gone;

            var signatureDriver = FindViewById<SignaturePadView>(Resource.Id.signatureDriver);

            signatureDriver.Caption.Text = "Driver Signature";
            signatureDriver.Caption.SetTextColor(Color.Rgb(0, 0, 0));
            signatureDriver.Caption.SetTypeface(Typeface.Serif, TypefaceStyle.BoldItalic);
            signatureDriver.Caption.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 16f);
            signatureDriver.SignaturePrompt.Text = "";
            signatureDriver.BackgroundColor = Color.Rgb(255, 255, 255); // a light yellow.
            signatureDriver.StrokeColor = Color.Black;
            signatureDriver.StrokeWidth = 4;
            signatureDriver.ClearLabel.Text = "Refresh";
            signatureDriver.ClearLabel.SetTextColor(Color.Rgb(0, 0, 0));

            var signatureCustomer = FindViewById<SignaturePadView>(Resource.Id.signatureCustomer);
            signatureCustomer.Caption.Text = "Customer Signature";
            signatureCustomer.Caption.SetTextColor(Color.Rgb(0, 0, 0));
            signatureCustomer.Caption.SetTypeface(Typeface.Serif, TypefaceStyle.BoldItalic);
            signatureCustomer.Caption.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 16f);
            signatureCustomer.SignaturePrompt.Text = "";
            signatureCustomer.BackgroundColor = Color.Rgb(255, 255, 255); // a light yellow.
            signatureCustomer.StrokeColor = Color.Black;
            signatureCustomer.StrokeWidth = 4;
            signatureCustomer.ClearLabel.Text = "Refresh";
            signatureCustomer.ClearLabel.SetTextColor(Color.Rgb(0, 0, 0));
            //signatureCustomer.LoadPoints()
            var actionToolbar = FindViewById<Toolbar>(Resource.Id.action_toolbar);
            actionToolbar.SetNavigationIcon(Resource.Drawable.ic_action_account_circle);
            actionToolbar.Title = ApplicationClass.username;
            actionToolbar.SetPadding(00, 0, 0, 00);
            //actionToolbar.SetContentInsetsAbsolute(0, 0);
            actionToolbar.InflateMenu(Resource.Menu.action_menus);
            actionToolbar.Menu.FindItem(Resource.Id.menu_next).SetVisible(false);
            actionToolbar.MenuItemClick += (sender, e) =>
            {
                if (e.Item.ItemId == Resource.Id.menu_save)
                {
                    if (signatureDriver.IsBlank)
                    {
                        // display the base line for the user to sign on.
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetMessage("Driver signature is required.");
                        alert.SetNeutralButton("OK", delegate { });
                        alert.Create().Show();
                    }
                    else
                    {
                        if (signatureCustomer.IsBlank)
                        {
                            // display the base line for the user to sign on.
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetMessage("Customer signature is required.");
                            alert.SetNeutralButton("OK", delegate { });
                            alert.Create().Show();
                        }
                        else
                        {
                            this.progressLayout.Visibility = ViewStates.Visible;
                            //rentRunningTrans.DriverSignature = (byte[])signatureDriver.GetImage();
                            rentRunningTrans.DriverSignatureData = signatureDriver.Points.ToString();
                            //rentRunningTrans.CustomerSignature = (byte[])signatureCustomer.GetImage();
                            rentRunningTrans.CustomerSignatureData = signatureCustomer.Points.ToString();
                            DataManager dataManager = new DataManager();
                            if (dataManager.SaveRentRunningTransToLocal(rentRunningTrans) > 0)
                            {
                                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                                alert.SetMessage("Vehicle transaction has been saved successfully.");
                                alert.SetNeutralButton("OK", delegate
                                {
                                    var intent = new Intent(this, typeof(MainMenuActivity));
                                    StartActivity(intent);
                                });
                                alert.Create().Show();
                            }
                        }
                    }
                }
                if (e.Item.ItemId == Resource.Id.menu_next)
                {

                }
                if (e.Item.ItemId == Resource.Id.menu_back)
                {
                    //rentRunningTrans = null;
                    var intent = new Intent(this, typeof(RentFlowSignatureActivity));
                    intent.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                    StartActivity(intent);
                }
                if (e.Item.ItemId == Resource.Id.menu_sendsms)
                {
                    this.progressLayout.Visibility = ViewStates.Visible;
                    var intentSendSMS = new Intent(this, typeof(SendSMSActivity));
                    intentSendSMS.PutExtra("MobileNo", rentRunningTrans.Mobile);
                    intentSendSMS.PutExtra("FromActivity", "DriverSignatureActivity");
                    intentSendSMS.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                    StartActivity(intentSendSMS);
                }
                if (e.Item.ItemId == Resource.Id.menu_sendemail)
                {
                    if (signatureDriver.IsBlank)
                    {
                        // display the base line for the user to sign on.
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetMessage("Driver signature is required.");
                        alert.SetNeutralButton("OK", delegate { });
                        alert.Create().Show();
                    }
                    else
                    {

                        if (signatureCustomer.IsBlank)
                        {
                            // display the base line for the user to sign on.
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetMessage("Customer signature is required.");
                            alert.SetNeutralButton("OK", delegate { });
                            alert.Create().Show();
                        }
                        else
                        {
                            this.progressLayout.Visibility = ViewStates.Visible;
                            //rentRunningTrans.DriverSignature = (byte[])signatureDriver.GetImage();
                            rentRunningTrans.DriverSignatureData = signatureDriver.Points.ToString();
                            //rentRunningTrans.CustomerSignature = (byte[])signatureCustomer.GetImage();
                            rentRunningTrans.CustomerSignatureData = signatureCustomer.Points.ToString();
                            DataManager dataManager = new DataManager();
                            dataManager.SaveRentRunningTransToLocal(rentRunningTrans);
                            var intentSendEmail = new Intent(this, typeof(SendEmailActivity));
                            intentSendEmail.PutExtra("EmailId", rentRunningTrans.Email);
                            StartActivity(intentSendEmail);
                        }
                    }
                }
                if (e.Item.ItemId == Resource.Id.menu_video)
                {
                    this.progressLayout.Visibility = ViewStates.Visible;
                    var intentVideo = new Intent(this, typeof(CaptureVideoActivity));
                    StartActivity(intentVideo);
                }
                if (e.Item.ItemId == Resource.Id.menu_dashboard)
                {
                    this.progressLayout.Visibility = ViewStates.Visible;
                    rentRunningTrans = null;
                    var intentMainMenu = new Intent(this, typeof(MainMenuActivity));
                    StartActivity(intentMainMenu);
                }
            };
            //Recreate Object
            if (savedInstanceState != null)
            {
                rentRunningTrans = JsonConvert.DeserializeObject<RentRunningTrans>(savedInstanceState.GetString("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans)));
            }
            else
            {
                rentRunningTrans = JsonConvert.DeserializeObject<RentRunningTrans>(Intent.GetStringExtra("RentRunningTrans"));
            }
            //Load details from existing object
            if (rentRunningTrans != null)
            {
                actionBar.Title = "Signature for " + rentRunningTrans.TransType;
            }
        }

        protected override void OnSaveInstanceState(Bundle savedInstanceState)
        {
            savedInstanceState.PutString("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
            // always call the base implementation!
            base.OnSaveInstanceState(savedInstanceState);
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
                case Resource.Id.menu_dashboard:
                    var intent_dashboard = new Intent(this, typeof(MainMenuActivity));
                    StartActivity(intent_dashboard);
                    break;
                case Resource.Id.menu_settings:
                    var intent_settings = new Intent(this, typeof(SettingsActivity));
                    intent_settings.PutExtra("FromActivity", "DriverSignatureActivity");
                    intent_settings.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                    StartActivity(intent_settings);
                    break;
                case Resource.Id.menu_logout:
                    //Clear existing application variables and remove user record from table
                    DataManager objDataManager = new DataManager();
                    objDataManager.Logout();
                    ApplicationClass.userId = 0;
                    ApplicationClass.username = null;
                    ApplicationClass.SecurityToken = null;
                    var intent_logout = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent_logout);
                    break;
            }
            return base.OnOptionsItemSelected(item);
        }
    }
}