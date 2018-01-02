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
using Xamarin.Controls;
using Android.Graphics;
using Newtonsoft.Json;
using RentACar.UI.Modals;
using System.Text.RegularExpressions;

namespace RentACar.UI
{
    [Activity(Label = "Inspection")]
    public class RentFlowSignatureActivity : Activity
    {
        RentRunningTrans rentRunningTrans;
        Spinner lstInspection;
        Spinner lstCleanedBy;
        Spinner lstEnteredBy;
        ProgressBar progressLayout;
        string[] strInspectionConditions;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar actionBar = ((Activity)this).ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetLogo(Resource.Drawable.ic_launcher);
            actionBar.SetDisplayUseLogoEnabled(true);
            // Create your application here
            SetContentView(Resource.Layout.RentFlowSignature);

            lstInspection = FindViewById<Spinner>(Resource.Id.lstInspection);
            strInspectionConditions = Resources.GetStringArray(Resource.Array.strInspectionConditions);
            var adpInspectionType = ArrayAdapter.CreateFromResource(this, Resource.Array.strInspectionConditions, Resource.Layout.list_item);
            lstInspection.Adapter = adpInspectionType;

            lstCleanedBy = FindViewById<Spinner>(Resource.Id.lstCleanedBy);
            var adpCleanedBy = ArrayAdapter.CreateFromResource(this, Resource.Array.strCleanedBy, Resource.Layout.list_item);
            lstCleanedBy.Adapter = adpCleanedBy;

            lstEnteredBy = FindViewById<Spinner>(Resource.Id.lstEnteredBy);
            var adpEnteredBy = ArrayAdapter.CreateFromResource(this, Resource.Array.strEnteredBy, Resource.Layout.list_item);
            lstEnteredBy.Adapter = adpEnteredBy;

            this.progressLayout = FindViewById<ProgressBar>(Resource.Id.progressLayout);
            this.progressLayout.Visibility = ViewStates.Gone;

            var actionToolbar = FindViewById<Toolbar>(Resource.Id.action_toolbar);
            actionToolbar.SetNavigationIcon(Resource.Drawable.ic_action_account_circle);
            actionToolbar.Title = ApplicationClass.username;
            actionToolbar.SetPadding(0, 0, 0, 00);
            actionToolbar.InflateMenu(Resource.Menu.action_menus);
            actionToolbar.Menu.FindItem(Resource.Id.menu_save).SetVisible(false);
            actionToolbar.Menu.FindItem(Resource.Id.menu_sendemail).SetVisible(false);
            actionToolbar.MenuItemClick += (sender, e) =>
            {
                if (e.Item.ItemId == Resource.Id.menu_next)
                {
                    //if (lstInspection.SelectedItem.ToString() != "(Selact Inspection Conditions)" && lstCleanedBy.SelectedItem.ToString() != "(Select CleanedBy)" && lstEnteredBy.SelectedItem.ToString() != "(Select EnteredBy)")
                    //{
                    Regex regxlstInspection = new Regex("(Select Inspection Conditions)");
                        Regex regxlstCleanedBy = new Regex("(Select CleanedBy)");
                        Regex regxlstEnteredBy = new Regex("(Select EnteredBy)");
                        if (regxlstInspection.Match(lstInspection.SelectedItem.ToString()).Success)
                        {
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetMessage("Please select inspection conditions.");
                            alert.SetNeutralButton("OK", delegate { });
                            alert.Create().Show();
                            lstInspection.Focusable = true;
                            lstInspection.FocusableInTouchMode = true;
                            lstInspection.RequestFocus();
                            lstInspection.PerformClick();
                        }
                        else if (regxlstCleanedBy.Match(lstCleanedBy.SelectedItem.ToString()).Success)
                        {
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetMessage("Please select select cleaned by.");
                            alert.SetNeutralButton("OK", delegate { });
                            alert.Create().Show();
                            lstCleanedBy.Focusable = true;
                            lstCleanedBy.FocusableInTouchMode = true;
                            lstCleanedBy.RequestFocus();
                            lstCleanedBy.PerformClick();
                        }
                        else if (regxlstEnteredBy.Match(lstEnteredBy.SelectedItem.ToString()).Success)
                        {
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetMessage("Please select entered by.");
                            alert.SetNeutralButton("OK", delegate { });
                            alert.Create().Show();
                            lstEnteredBy.Focusable = true;
                            lstEnteredBy.FocusableInTouchMode = true;
                            lstEnteredBy.RequestFocus();
                            lstEnteredBy.PerformClick();
                        }
                        else
                        {
                        this.progressLayout.Visibility = ViewStates.Visible;
                        var intent = new Intent(this, typeof(RentFlowDriverSignatureActivity));
                            //assing extra data
                            rentRunningTrans.InspectionCondition = Convert.ToInt32(lstInspection.SelectedItemId);
                            rentRunningTrans.CleanedBy = Convert.ToInt32(lstCleanedBy.SelectedItemId);
                            rentRunningTrans.CheckedoutBy = Convert.ToInt32(lstEnteredBy.SelectedItemId);
                            //rentRunningTrans. = chkAlloyWheel.Activated;
                            intent.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                            StartActivity(intent);
                        }
                    //}
                    //else
                    //{
                    //    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    //    alert.SetMessage("Please fill all mandatory details.");
                    //    alert.SetNeutralButton("OK", delegate { });
                    //    alert.Create().Show();
                    //    // Toast.MakeText(this, "Please fill all mandatory details", ToastLength.Short).Show();
                    //}
                }
                if (e.Item.ItemId == Resource.Id.menu_back)
                {
                    var intent = new Intent(this, typeof(RentFlowCheckListActivity));
                    intent.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                    StartActivity(intent);
                }
                if (e.Item.ItemId == Resource.Id.menu_sendsms)
                {
                    this.progressLayout.Visibility = ViewStates.Visible;
                    var intentSendSMS = new Intent(this, typeof(SendSMSActivity));
                    intentSendSMS.PutExtra("MobileNo", rentRunningTrans.Mobile);
                    intentSendSMS.PutExtra("FromActivity", "SignatureAvtivity");
                    intentSendSMS.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                    StartActivity(intentSendSMS);
                }
                if (e.Item.ItemId == Resource.Id.menu_sendemail)
                {
                    this.progressLayout.Visibility = ViewStates.Visible;
                    var intentMainMenu = new Intent(this, typeof(SendEmailActivity));
                    StartActivity(intentMainMenu);
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
                //Toast.MakeText(this, "Bottom toolbar tapped: " + e.Item.TitleFormatted, ToastLength.Short).Show();
            };

            //Button btnDriverSignature = FindViewById<Button>(Resource.Id.btnDriverSignature);
            //Button btnCustomerSignature = FindViewById<Button>(Resource.Id.btnCustomerSignature);

            //btnDriverSignature.Click += btnDriverSignature_Click;
            //btnCustomerSignature.Click += btnCustomerSignature_Click;

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
                actionBar.Title = "Inspection for " + rentRunningTrans.TransType;
                lstInspection.SetSelection(rentRunningTrans.InspectionCondition);
            }
        }


        //private void btnDriverSignature_Click(object sender, EventArgs e)
        //{
        //    var intent = new Intent(this, typeof(RentFlowDriverSignatureActivity));
        //    intent.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
        //    StartActivity(intent);
        //}

        //private void btnCustomerSignature_Click(object sender, EventArgs e)
        //{
        //    var intent= new Intent(this, typeof(RentFlowCustomerSignatureActivity));
        //    intent.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
        //    StartActivity(intent);
        //}

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
                    intent_settings.PutExtra("FromActivity", "SignatureAvtivity");
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
            //Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }

    }
}