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
using System.Net;
using Org.Apache.Http.Protocol;
using SQLite;
using System.IO;

namespace RentACar.UI
{
    [Activity(Label = "Send SMS")]
    public class SendSMSActivity : Activity
    {
        RentRunningTrans rentRunningTrans;
        Spinner ddlSmsTemplate;
        EditText txtMobile;
        EditText txtSmsTemplateContent;
        DataManager objDataManager;
        IList<SmsTemplate> stemp;
        ProgressBar progressLayout;

        string dbPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "RentACar.db3");
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar actionBar = ((Activity)this).ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetLogo(Resource.Drawable.ic_launcher);
            actionBar.SetDisplayUseLogoEnabled(true);

            // Create your application here
            SetContentView(Resource.Layout.SendSMS);
            ddlSmsTemplate = FindViewById<Spinner>(Resource.Id.ddlSmsTemplate);
            txtSmsTemplateContent = FindViewById<EditText>(Resource.Id.txtSmsTemplateContent);
            txtMobile = FindViewById<EditText>(Resource.Id.txtMobile);
            txtMobile.Text = Intent.GetStringExtra("MobileNo");

            this.progressLayout = FindViewById<ProgressBar>(Resource.Id.progressLayout);
            this.progressLayout.Visibility = ViewStates.Gone;

            objDataManager = new DataManager();
            stemp = objDataManager.GetSmsTemplate();
            List<String> oSmsTitlelist = new List<string>();
            foreach (SmsTemplate s in stemp)
            {
                oSmsTitlelist.Add(s.SmsTitle);
            }
            var adpSmsTemplate = new ArrayAdapter<String>(this, Resource.Layout.list_item, oSmsTitlelist);
            ddlSmsTemplate.Adapter = adpSmsTemplate;
            //var adpSmsTitle = new ArrayAdapter<String>(this, Resource.Layout.list_item, oSmsTitlelist);
            //var adpVRegNo = new ArrayAdapter<String>(this, Resource.Layout.list_item, oVRegNolist);
            //ddlRegNo.Adapter = adpVRegNo;
            //ddlRegNo.ItemClick += ddlRegNo_ItemClick;
            ddlSmsTemplate.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(ddlSmsTemplate_ItemSelected);

            if (savedInstanceState != null)
            {
                rentRunningTrans = JsonConvert.DeserializeObject<RentRunningTrans>(savedInstanceState.GetString("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans)));
            }
            else if (Intent.GetStringExtra("RentRunningTrans") != null)
            {
                rentRunningTrans = JsonConvert.DeserializeObject<RentRunningTrans>(Intent.GetStringExtra("RentRunningTrans"));
            }
            //Load details from existing object
            if (rentRunningTrans != null)
            {
                txtMobile.Text = rentRunningTrans.Mobile;
            }
        }

        protected override void OnSaveInstanceState(Bundle savedInstanceState)
        {
            savedInstanceState.PutString("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
            // always call the base implementation!
            base.OnSaveInstanceState(savedInstanceState);
        }

        private void ddlSmsTemplate_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            //string toast = string.Format("The planet is {0}", spinner.GetItemAtPosition(e.Position));
            //Toast.MakeText(this, toast, ToastLength.Long).Show();
            string sTemplateCode = spinner.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(sTemplateCode))
            {
                objDataManager = new DataManager();
                SmsTemplate oSmsTemplate = objDataManager.GetSmsTemplateByCode(sTemplateCode);
                if (oSmsTemplate != null)
                {
                    txtSmsTemplateContent.Text = oSmsTemplate.SmsBody;
                }
            }
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
                    if (!String.IsNullOrEmpty(txtMobile.Text.Trim()))
                    {
                        Regex regxMobile = new Regex(@"[0-9]{10}");
                        if (!regxMobile.Match(txtMobile.Text).Success)
                        {
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetMessage("Please enter valid mobile number.");
                            alert.SetNeutralButton("OK", delegate { });
                            alert.Create().Show();
                            //  Toast.MakeText(this, "Please enter valid mobile number", ToastLength.Short).Show();
                            txtMobile.RequestFocus();
                        }
                    }
                    else
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetMessage("Please enter mobile number.");
                        alert.SetNeutralButton("OK", delegate { });
                        alert.Create().Show();
                        //  Toast.MakeText(this, "Please enter mobile number", ToastLength.Short).Show();
                        txtMobile.RequestFocus();
                    }
                    try
                    {
                        this.progressLayout.Visibility = ViewStates.Gone;
                        var db = new SQLiteConnection(dbPath);
                        SmsToSend sms = new SmsToSend();
                        sms.MobileNumber = txtMobile.Text;
                        sms.SmsTemplate = ddlSmsTemplate.SelectedItem.ToString();
                        sms.SmsBody = txtSmsTemplateContent.Text;
                        sms.DateTime = DateTime.Now;
                        DataManager objData = new DataManager();
                        objData.SaveSmsToLocal(sms);
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetMessage("Record has been saved successfully.");
                        alert.SetNeutralButton("OK", delegate
                        {
                            var intent = new Intent(this, typeof(MainMenuActivity));
                            StartActivity(intent);
                        });
                        alert.Create().Show();
                    }
                    ////    this.progressLayout.Visibility = ViewStates.Visible;
                    ////    int iMaxSmsAllowedLength = 150;
                    ////    string strMessage = txtSmsTemplateContent.Text;
                    ////    if (strMessage.Length > 0)
                    ////    {
                    ////        int MessageCount = 0;
                    ////        if ((Convert.ToInt32(strMessage.Length) % iMaxSmsAllowedLength) != 0)
                    ////        {
                    ////            MessageCount = Convert.ToInt32(Convert.ToInt32(strMessage.Length) / iMaxSmsAllowedLength) + 1;
                    ////        }
                    ////        else
                    ////        {
                    ////            MessageCount = Convert.ToInt32(Convert.ToInt32(strMessage.Length) / iMaxSmsAllowedLength);
                    ////        }
                    ////        for (int i = 0; i <= MessageCount - 1; i++)
                    ////        {
                    ////            string NewStrMessage;
                    ////            if (i == 0)
                    ////            {
                    ////                if (strMessage.Length > iMaxSmsAllowedLength)
                    ////                {
                    ////                    NewStrMessage = "1/" + Convert.ToString(i + 1) + "- " + strMessage.Substring(i * iMaxSmsAllowedLength, iMaxSmsAllowedLength);
                    ////                }
                    ////                else
                    ////                {
                    ////                    NewStrMessage = strMessage;
                    ////                }
                    ////            }
                    ////            else
                    ////            {
                    ////                int istringLength = Convert.ToInt32(strMessage.Length) - (i * iMaxSmsAllowedLength);
                    ////                int iMsglength = istringLength > iMaxSmsAllowedLength ? iMaxSmsAllowedLength : istringLength;
                    ////                NewStrMessage = "1/" + Convert.ToString(i + 1) + "- " + strMessage.Substring((i * iMaxSmsAllowedLength), iMsglength);
                    ////            }
                    ////            if (rentRunningTrans != null)
                    ////            {
                    ////                NewStrMessage = NewStrMessage.Replace("<<Customer Name>>", "Richard");
                    ////                NewStrMessage = NewStrMessage.Replace("<<Manufacturer>>", "BMW");
                    ////                NewStrMessage = NewStrMessage.Replace("<<Registration>>", rentRunningTrans.RegNo);
                    ////            }
                    ////            //cm direct
                    ////            string strMobileNo = txtMobile.Text.TrimStart('0');
                    ////            if (!CommonFunctions.SendSms(strMobileNo, NewStrMessage))
                    ////            {
                    ////                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    ////                alert.SetMessage("Unable to send SMS. Please try again later.");
                    ////                alert.SetNeutralButton("OK", delegate { });
                    ////                alert.Create().Show();
                    ////               // Toast.MakeText(this, "Unable to send SMS. Please try again later.", ToastLength.Short).Show();
                    ////                break;
                    ////            }
                    ////            else
                    ////            {
                    ////                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    ////                alert.SetMessage("SMS is sent successfully.");
                    ////                alert.SetNeutralButton("OK", delegate { });
                    ////                alert.Create().Show();
                    ////                //Toast.MakeText(this, "SMS is sent successfully. ", ToastLength.Short).Show();
                    ////            }
                    ////        }
                    ////    }
                    ////}
                    //catch (Exception ex)
                    //{
                    //    throw ex;
                    //}
                    catch (Exception ex)
                    {
                        Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
                    }
                    break;
                case Resource.Id.menu_cancel:

                    string FromActivity = Intent.GetStringExtra("FromActivity");
                    if (FromActivity == "Vehicle")
                    {
                        this.progressLayout.Visibility = ViewStates.Visible;
                        var intentVehicle = new Intent(this, typeof(RentFlowVehicleActivity));
                        intentVehicle.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                        StartActivity(intentVehicle);
                    }
                    else if (FromActivity == "MarkDamage")
                    {
                        this.progressLayout.Visibility = ViewStates.Visible;
                        var intentVehicle = new Intent(this, typeof(RentFlowMarkDamageActivity));
                        intentVehicle.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                        StartActivity(intentVehicle);
                    }
                    else if (FromActivity == "CheckList")
                    {
                        this.progressLayout.Visibility = ViewStates.Visible;
                        var intentVehicle = new Intent(this, typeof(RentFlowCheckListActivity));
                        intentVehicle.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                        StartActivity(intentVehicle);
                    }
                    else if (FromActivity == "SignatureAvtivity")
                    {
                        this.progressLayout.Visibility = ViewStates.Visible;
                        var intentVehicle = new Intent(this, typeof(RentFlowSignatureActivity));
                        intentVehicle.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                        StartActivity(intentVehicle);
                    }
                    else if (FromActivity == "DriverSignatureActivity")
                    {
                        this.progressLayout.Visibility = ViewStates.Visible;
                        var intentVehicle = new Intent(this, typeof(RentFlowDriverSignatureActivity));
                        intentVehicle.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                        StartActivity(intentVehicle);
                    }
                    break;
            }
            //Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }

    }
}