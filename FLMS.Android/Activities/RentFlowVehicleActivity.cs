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
using Newtonsoft.Json;
using RentACar.UI.Modals;
using System.Text.RegularExpressions;
using System.Timers;
using System.Net;
using Android.Net;

namespace RentACar.UI
{
    [Activity(Label = "Select Vehicle")]
    public class RentFlowVehicleActivity : Activity
    {
        RentRunningTrans rentRunningTrans;
        string[] strVehicleType;
        string[] strArrFuelLevel;
        IList<Vehicle> oVehicles;
        Spinner ddlVehicleType;
        Spinner ddlFuelLevel;
        AutoCompleteTextView ddlRegNo;
        EditText txtEmail;
        EditText txtMobile;
        EditText txtMileage;
        DataManager objDataManager;
        string strShowVehicleForStatus;
        string strTransType;
        ProgressBar progressLayout;
        // WebClient webClient;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar actionBar = ((Activity)this).ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetLogo(Resource.Drawable.ic_launcher);
            actionBar.SetDisplayUseLogoEnabled(true);

            // Create your application here
            SetContentView(Resource.Layout.RentFlowVehicle);
            strShowVehicleForStatus = Intent.GetStringExtra("ShowVehicleForStatus");
            strTransType = Intent.GetStringExtra("TransType");
            actionBar.Title = "Select Vehicle for " + strTransType;
            ddlRegNo = FindViewById<AutoCompleteTextView>(Resource.Id.ddlRegNo);
            //string[] strArrRegNo = Resources.GetStringArray(Resource.Array.strArrRegNo);
            BindVehicleRegNoList(strShowVehicleForStatus);
            //objDataManager = new DataManager();
            //oVehicles = objDataManager.GetVehicles(strShowVehicleForStatus);
            //List<String> oVRegNolist = new List<String>();
            //foreach (Vehicle v in oVehicles)
            //{
            //    oVRegNolist.Add(v.RegNumber);
            //}
            //var adpVRegNo = new ArrayAdapter<String>(this, Resource.Layout.list_item, oVRegNolist);
            //ddlRegNo.Adapter = adpVRegNo;
            //ddlRegNo.ItemClick += ddlRegNo_ItemClick;
            ddlVehicleType = FindViewById<Spinner>(Resource.Id.ddlVehicleType);
            strVehicleType = Resources.GetStringArray(Resource.Array.strArrVehicleType);
            //var adpVehicleType = ArrayAdapter.CreateFromResource(
            //        this, Resource.Array.strArrVehicleType, Android.Resource.Layout.SimpleSpinnerItem);
            var adpVehicleType = ArrayAdapter.CreateFromResource(
                   this, Resource.Array.strArrVehicleType, Resource.Layout.list_item);
            //adpVehicleType.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            ddlVehicleType.Adapter = adpVehicleType;

            ddlFuelLevel = FindViewById<Spinner>(Resource.Id.ddlFuelLevel);
            strArrFuelLevel = Resources.GetStringArray(Resource.Array.strArrFuelLevel);
            var adapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, strArrFuelLevel);
            ddlFuelLevel.Adapter = adapter;

            this.progressLayout = FindViewById<ProgressBar>(Resource.Id.progressLayout);
            this.progressLayout.Visibility = ViewStates.Gone;
            //ConnectivityManager connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);

            txtEmail = FindViewById<EditText>(Resource.Id.txtEmail);
            txtMileage = FindViewById<EditText>(Resource.Id.txtMileage);
            txtMobile = FindViewById<EditText>(Resource.Id.txtMobile);

            var actionToolbar = FindViewById<Toolbar>(Resource.Id.action_toolbar);
            actionToolbar.SetNavigationIcon(Resource.Drawable.ic_action_account_circle);
            actionToolbar.Title = ApplicationClass.UserName;
            actionToolbar.SetPadding(00, 0, 0, 00);
            //actionToolbar.SetContentInsetsAbsolute(0, 0);
            actionToolbar.InflateMenu(Resource.Menu.action_menus);
            actionToolbar.Menu.FindItem(Resource.Id.menu_save).SetVisible(false);
            actionToolbar.Menu.FindItem(Resource.Id.menu_sendemail).SetVisible(false);
            actionToolbar.MenuItemClick += (sender, e) =>
            {
                if (e.Item.ItemId == Resource.Id.menu_next)
                {
                    if (!String.IsNullOrWhiteSpace(ddlRegNo.Text.Trim()) && !String.IsNullOrWhiteSpace(txtEmail.Text.Trim()) && !String.IsNullOrWhiteSpace(txtMobile.Text.Trim()) && !String.IsNullOrWhiteSpace(txtMileage.Text.Trim()))
                    {
                        Regex regexEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                        Regex regxMobile = new Regex(@"[0-9]{10}");
                        Regex regxFuelLevel = new Regex("Select Fuel Level");
                        Regex regxVehicleType = new Regex("Select Vehicle Type");
                        if (!regxMobile.Match(txtMobile.Text).Success)
                        {
                            // Toast.MakeText(this, "Please enter valid mobile number.", ToastLength.Short).Show();
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetMessage("Please enter valid mobile number.");
                            alert.SetNeutralButton("OK", delegate { });
                            alert.Create().Show();
                            txtMobile.RequestFocus();
                        }
                        else if (!regexEmail.Match(txtEmail.Text).Success)
                        {
                            // Toast.MakeText(this, "Please enter valid email id.", ToastLength.Short).Show();
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetMessage("Please enter valid email id.");
                            alert.SetNeutralButton("OK", delegate { });
                            alert.Create().Show();
                            txtEmail.RequestFocus();
                        }
                        else if (regxVehicleType.Match(ddlVehicleType.SelectedItem.ToString()).Success)
                        {
                            // Toast.MakeText(this, "Please select vehicle type.", ToastLength.Short).Show();
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetMessage("Please select vehicle type.");
                            alert.SetNeutralButton("OK", delegate { });
                            alert.Create().Show();
                            ddlFuelLevel.RequestFocus();
                        }
                        else if (regxFuelLevel.Match(ddlFuelLevel.SelectedItem.ToString()).Success)
                        {
                            // Toast.MakeText(this, "Please select fuel level.", ToastLength.Short).Show();
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetMessage("Please select fuel level.");
                            alert.SetNeutralButton("OK", delegate { });
                            alert.Create().Show();
                            ddlFuelLevel.Focusable = true;
                            ddlFuelLevel.FocusableInTouchMode = true;
                            ddlFuelLevel.RequestFocus();
                            ddlFuelLevel.PerformClick();
                        }
                        else if (CommonFunctions.IsNetworkConnected())
                        {
                            this.progressLayout.Visibility = ViewStates.Visible;
                            Vehicle objVehicle = oVehicles.Where(x => x.RegNumber == ddlRegNo.Text).FirstOrDefault();
                            if (objVehicle != null)
                            {
                                var intentMarkDamage = new Intent(this, typeof(RentFlowMarkDamageActivity));

                                if (rentRunningTrans == null)
                                {
                                    rentRunningTrans = new RentRunningTrans();
                                }
                                rentRunningTrans.VehicleId = objVehicle.ID;
                                rentRunningTrans.VehicleType = strVehicleType[ddlVehicleType.SelectedItemPosition];
                                rentRunningTrans.TransType = strTransType;
                                rentRunningTrans.RegNo = ddlRegNo.Text.Trim();
                                rentRunningTrans.Email = txtEmail.Text.Trim();
                                rentRunningTrans.Mobile = txtMobile.Text.Trim();
                                rentRunningTrans.Mileage = Convert.ToInt32(txtMileage.Text);
                                rentRunningTrans.FuelLevel = strArrFuelLevel[ddlFuelLevel.SelectedItemPosition];
                                intentMarkDamage.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                                StartActivity(intentMarkDamage);
                            }
                            else
                            {
                                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                                alert.SetMessage("Please select valid Reg Number.");
                                alert.SetNeutralButton("OK", delegate { });
                                alert.Create().Show();
                                // Toast.MakeText(this, "Please select valid Reg Number.", ToastLength.Short).Show();
                            }
                        }
                        else
                        {
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetMessage("Please enable internet to get vehicle images from API.");
                            alert.SetNeutralButton("OK", delegate { });
                            alert.Create().Show();
                        }
                    }
                    else
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetMessage("Please fill all mandatory details.");
                        alert.SetNeutralButton("OK", delegate { });
                        alert.Create().Show();
                        //  Toast.MakeText(this, "Please fill all mandatory details", ToastLength.Short).Show();
                        txtMileage.RequestFocus();
                    }
                }
                if (e.Item.ItemId == Resource.Id.menu_back)
                {
                    rentRunningTrans = null;
                    var intentMainMenu = new Intent(this, typeof(MainMenuActivity));
                    StartActivity(intentMainMenu);
                }
                if (e.Item.ItemId == Resource.Id.menu_sendsms)
                {
                    if (!String.IsNullOrWhiteSpace(txtMobile.Text.Trim()))
                    {
                        this.progressLayout.Visibility = ViewStates.Visible;
                        var intentSendSMS = new Intent(this, typeof(SendSMSActivity));
                        intentSendSMS.PutExtra("MobileNo", txtMobile.Text);
                        intentSendSMS.PutExtra("FromActivity", "Vehicle");
                        intentSendSMS.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                        StartActivity(intentSendSMS);
                    }
                    else
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetMessage("Please enter mobile no.");
                        alert.SetNeutralButton("OK", delegate { });
                        alert.Create().Show();
                        // Toast.MakeText(this, "Please enter mobile no", ToastLength.Short).Show();
                        txtMobile.RequestFocus();
                    }
                }
                if (e.Item.ItemId == Resource.Id.menu_sendemail)
                {
                    this.progressLayout.Visibility = ViewStates.Visible;
                    var intentSendEmail = new Intent(this, typeof(SendEmailActivity));
                    intentSendEmail.PutExtra("EmailId", txtEmail.Text);
                    StartActivity(intentSendEmail);
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
            //load existing details (if available)
            if (savedInstanceState != null)
            {
                rentRunningTrans = JsonConvert.DeserializeObject<RentRunningTrans>(savedInstanceState.GetString("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans)));
            }
            else if (Intent.GetStringExtra("RentRunningTrans") != null)
            {
                rentRunningTrans = JsonConvert.DeserializeObject<RentRunningTrans>(Intent.GetStringExtra("RentRunningTrans"));
            }
            if (rentRunningTrans != null)
            {
                actionBar.Title = "Select Vehicle for " + rentRunningTrans.TransType;
                if (rentRunningTrans.TransType.Equals("PRE"))
                {
                    BindVehicleRegNoList("IN");
                }
                else if (rentRunningTrans.TransType.Equals("OUT"))
                {
                    BindVehicleRegNoList("PRE");
                }
                else if (rentRunningTrans.TransType.Equals("IN"))
                {
                    BindVehicleRegNoList("OUT");
                }
                DisplayVehicleDetails(rentRunningTrans);
                
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
                    intent_sendsms.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                    StartActivity(intent_sendsms);
                    break;
                case Resource.Id.menu_sendemail:
                    var intent_sendemail = new Intent(this, typeof(SendEmailActivity));
                    intent_sendemail.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                    StartActivity(intent_sendemail);
                    break;
                case Resource.Id.menu_dashboard:
                    var intent_dashboard = new Intent(this, typeof(MainMenuActivity));
                    StartActivity(intent_dashboard);
                    break;
                case Resource.Id.menu_settings:
                    var intent_settings = new Intent(this, typeof(SettingsActivity));
                    intent_settings.PutExtra("FromActivity", "Vehicle");
                    intent_settings.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                    StartActivity(intent_settings);
                    break;
                case Resource.Id.menu_logout:
                    DataManager objDataManager = new DataManager();
                    objDataManager.Logout();
                    ApplicationClass.UserId = 0;
                    ApplicationClass.UserName = null;
                    ApplicationClass.CompanyId = 0;
                    var intent_logout = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent_logout);
                    Finish();
                    break;
            }
            //Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }

        //Bind Autocomplete List to vehicel reg no bsed on required status
        private void BindVehicleRegNoList(string strShowVehicleForStatus)
        {
            objDataManager = new DataManager();
            oVehicles = objDataManager.GetVehicles(strShowVehicleForStatus);
            List<String> oVRegNolist = new List<String>();
            foreach (Vehicle v in oVehicles)
            {
                oVRegNolist.Add(v.RegNumber);
            }
            var adpVRegNo = new ArrayAdapter<String>(this, Resource.Layout.list_item, oVRegNolist);
            ddlRegNo.Adapter = adpVRegNo;
            ddlRegNo.ItemClick += ddlRegNo_ItemClick;
        }

        void ddlRegNo_ItemClick(object sender, AdapterView.ItemClickEventArgs e)
        {
            Vehicle vehicle = oVehicles.Where(x => x.RegNumber == ddlRegNo.Text).First();
            objDataManager = new DataManager();
            rentRunningTrans = objDataManager.GetVehicleRentLastTransactionDetails(vehicle.ID, strShowVehicleForStatus);
            if (rentRunningTrans != null)
            {
                DisplayVehicleDetails(rentRunningTrans);
            }
            else
            {
                ddlVehicleType.SetSelection(Array.IndexOf(strVehicleType, vehicle.VehicleType));
                ddlVehicleType.Enabled = false;
                txtMileage.RequestFocus();
            }
            
        }
        //dispaly vehicle Details
        private void DisplayVehicleDetails(RentRunningTrans rentRunningTrans)
        {
            ddlRegNo.Text = rentRunningTrans.RegNo;
            ddlVehicleType.SetSelection(Array.IndexOf(strVehicleType, rentRunningTrans.VehicleType));
            ddlVehicleType.Enabled = false;
            ddlFuelLevel.SetSelection(Array.IndexOf(strArrFuelLevel, rentRunningTrans.FuelLevel));
            txtEmail.Text = rentRunningTrans.Email;
            txtMileage.Text = rentRunningTrans.Mileage.ToString();
            txtMobile.Text = rentRunningTrans.Mobile;           
        }
    }
}