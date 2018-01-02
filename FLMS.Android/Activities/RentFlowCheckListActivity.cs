using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using RentACar.UI.Modals;

namespace RentACar.UI
{
    [Activity(Label = "Check List")]
    public class RentFlowCheckListActivity : Activity
    {
        RentRunningTrans rentRunningTrans;
        CheckBox chkAlloyWheel;
        LinearLayout progressLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar actionBar = ((Activity)this).ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetLogo(Resource.Drawable.ic_launcher);
            actionBar.SetDisplayUseLogoEnabled(true);
            // Create your application here
            SetContentView(Resource.Layout.RentFlowCheckList);
            this.progressLayout = FindViewById<LinearLayout>(Resource.Id.progressLayout);
            this.progressLayout.Visibility = ViewStates.Gone;

            chkAlloyWheel = FindViewById<CheckBox>(Resource.Id.chkAlloyWheel);
            CheckBox chkBonet = FindViewById<CheckBox>(Resource.Id.chkBonet);
            CheckBox chkBrakeFluid = FindViewById<CheckBox>(Resource.Id.chkBrakeFluid);
            CheckBox chkCoolant = FindViewById<CheckBox>(Resource.Id.chkCoolant);
            CheckBox chkDisc = FindViewById<CheckBox>(Resource.Id.chkDisc);
            CheckBox chkEngine = FindViewById<CheckBox>(Resource.Id.chkEngine);
            CheckBox chkFrontBumper = FindViewById<CheckBox>(Resource.Id.chkFrontBumper);
            CheckBox chkInterior = FindViewById<CheckBox>(Resource.Id.chkInterior);
            CheckBox chkNSDoor = FindViewById<CheckBox>(Resource.Id.chkNSDoor);
            CheckBox chkNSFWheel = FindViewById<CheckBox>(Resource.Id.chkNSFWheel);
            CheckBox chkNSRWheel = FindViewById<CheckBox>(Resource.Id.chkNSRWheel);
            CheckBox chkOil = FindViewById<CheckBox>(Resource.Id.chkOil);
            CheckBox chkOSDoor = FindViewById<CheckBox>(Resource.Id.chkOSDoor);
            CheckBox chkOSFWheel = FindViewById<CheckBox>(Resource.Id.chkOSFWheel);
            CheckBox chkOSRWheel = FindViewById<CheckBox>(Resource.Id.chkOSRWheel);
            CheckBox chkRearBumper = FindViewById<CheckBox>(Resource.Id.chkRearBumper);
            CheckBox chkRoof = FindViewById<CheckBox>(Resource.Id.chkRoof);
            CheckBox chkSpareTyre = FindViewById<CheckBox>(Resource.Id.chkSpareTyre);
            CheckBox chkTaligate = FindViewById<CheckBox>(Resource.Id.chkTaligate);
            CheckBox chkTools = FindViewById<CheckBox>(Resource.Id.chkTools);
            CheckBox chkTyres = FindViewById<CheckBox>(Resource.Id.chkTyres);
            CheckBox chkWasherFluid = FindViewById<CheckBox>(Resource.Id.chkWasherFluid);
            CheckBox chkWindScreen = FindViewById<CheckBox>(Resource.Id.chkWindScreen);
            EditText txtDamageDetails = FindViewById<EditText>(Resource.Id.txtDamageDetails);
            EditText txtLooseItems = FindViewById<EditText>(Resource.Id.txtLooseItems);

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
                    this.progressLayout.Visibility = ViewStates.Visible;
                    var intent = new Intent(this, typeof(RentFlowSignatureActivity));
                    //Assign values from screen to running object
                    
                    rentRunningTrans.AlloyWheel= chkAlloyWheel.Checked;
                    rentRunningTrans.Bonet= chkBonet.Checked;
                    rentRunningTrans.BrakeFluid= chkBrakeFluid.Checked;
                    rentRunningTrans.Coolant= chkCoolant.Checked;
                    rentRunningTrans.Disc= chkDisc.Checked;
                    rentRunningTrans.Engine= chkEngine.Checked;
                    rentRunningTrans.FrontBumper= chkFrontBumper.Checked;
                    rentRunningTrans.Interior= chkInterior.Checked;
                    rentRunningTrans.NSDoor= chkNSDoor.Checked;
                    rentRunningTrans.NSFWheel= chkNSFWheel.Checked;
                    rentRunningTrans.NSRWheel= chkNSRWheel.Checked;
                    rentRunningTrans.Oil= chkOil.Checked;
                    rentRunningTrans.OSDoor= chkOSDoor.Checked;
                    rentRunningTrans.OSFWheel= chkOSFWheel.Checked;
                    rentRunningTrans.OSRWheel= chkOSRWheel.Checked;
                    rentRunningTrans.RearBumper= chkRearBumper.Checked;
                    rentRunningTrans.Roof= chkRoof.Checked;
                    rentRunningTrans.SpareTyre= chkSpareTyre.Checked;
                    rentRunningTrans.Taligate= chkTaligate.Checked;
                    rentRunningTrans.Tools= chkTools.Checked;
                    rentRunningTrans.Tyres= chkTyres.Checked;
                    rentRunningTrans.WasherFluid= chkWasherFluid.Checked;
                    rentRunningTrans.WindScreen= chkWindScreen.Checked;
                    rentRunningTrans.DamageDetail = txtDamageDetails.Text;
                    rentRunningTrans.LooseItemDetail = txtLooseItems.Text;
                    intent.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                    StartActivity(intent);
                }
                if (e.Item.ItemId == Resource.Id.menu_back)
                {
                    var intent = new Intent(this, typeof(RentFlowMarkDamageActivity));
                    intent.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                    StartActivity(intent);
                }
                if (e.Item.ItemId == Resource.Id.menu_sendsms)
                {
                    this.progressLayout.Visibility = ViewStates.Visible;
                    var intentSendSMS = new Intent(this, typeof(SendSMSActivity));
                    intentSendSMS.PutExtra("MobileNo", rentRunningTrans.Mobile);
                    intentSendSMS.PutExtra("FromActivity", "CheckList");
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
                // Toast.MakeText(this, "Bottom toolbar tapped: " + e.Item.TitleFormatted, ToastLength.Short).Show();
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
                actionBar.Title = "Check List for " + rentRunningTrans.TransType;
                chkAlloyWheel.Checked = rentRunningTrans.AlloyWheel;
                chkBonet.Checked = rentRunningTrans.Bonet;
                chkBrakeFluid.Checked = rentRunningTrans.BrakeFluid;
                chkCoolant.Checked = rentRunningTrans.Coolant;
                chkDisc.Checked = rentRunningTrans.Disc;
                chkEngine.Checked = rentRunningTrans.Engine;
                chkFrontBumper.Checked = rentRunningTrans.FrontBumper;
                chkInterior.Checked = rentRunningTrans.Interior;
                chkNSDoor.Checked = rentRunningTrans.NSDoor;
                chkNSFWheel.Checked = rentRunningTrans.NSFWheel;
                chkNSRWheel.Checked = rentRunningTrans.NSRWheel;
                chkOil.Checked = rentRunningTrans.Oil;
                chkOSDoor.Checked = rentRunningTrans.OSDoor;
                chkOSFWheel.Checked = rentRunningTrans.OSFWheel;
                chkOSRWheel.Checked = rentRunningTrans.OSRWheel;
                chkRearBumper.Checked = rentRunningTrans.RearBumper;
                chkRoof.Checked = rentRunningTrans.Roof;
                chkSpareTyre.Checked = rentRunningTrans.SpareTyre;
                chkTaligate.Checked = rentRunningTrans.Taligate;
                chkTools.Checked = rentRunningTrans.Tools;
                chkTyres.Checked = rentRunningTrans.Tyres;
                chkWasherFluid.Checked = rentRunningTrans.WasherFluid;
                chkWindScreen.Checked = rentRunningTrans.WindScreen;
                txtDamageDetails.Text = rentRunningTrans.DamageDetail;
                txtLooseItems.Text = rentRunningTrans.LooseItemDetail;
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
                    intent_settings.PutExtra("FromActivity", "CheckList");
                    intent_settings.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                    StartActivity(intent_settings);
                    break;
                case Resource.Id.menu_logout:
                    DataManager objDataManager = new DataManager();
                    objDataManager.Logout();
                    ApplicationClass.userId = 0;
                    ApplicationClass.username = null;
                    ApplicationClass.UserDefaultVehicle = 0;
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