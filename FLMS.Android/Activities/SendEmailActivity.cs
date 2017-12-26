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
using System.Net.Mail;
using SQLite;
using System.IO;

namespace RentACar.UI
{
    [Activity(Label = "Send Email")]
    public class SendEmailActivity : Activity
    {
        EditText txtSendTo;
        Spinner ddlEmailTemplate;
        EditText txtEmailTemplateContent;
        DataManager objDataManager;
        IList<EmailTemplate> etemp;
        RentRunningTrans rentRunningTrans;
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
            SetContentView(Resource.Layout.SendEmail);
            ddlEmailTemplate = FindViewById<Spinner>(Resource.Id.ddlEmailTemplate);
            txtSendTo = FindViewById<EditText>(Resource.Id.txtSendTo);
            txtEmailTemplateContent = FindViewById<EditText>(Resource.Id.txtEmailTemplateContent);
            txtSendTo.Text = Intent.GetStringExtra("EmailId");

            this.progressLayout = FindViewById<ProgressBar>(Resource.Id.progressLayout);
            this.progressLayout.Visibility = ViewStates.Gone;


            objDataManager = new DataManager();
            etemp = objDataManager.GetEmailTemplate();
            List<String> oEmailTitlelist = new List<string>();
            foreach (EmailTemplate e in etemp)
            {
                oEmailTitlelist.Add(e.EmailTitle);
            }
            var adpEmailTemplate = new ArrayAdapter<string>(this, Resource.Layout.list_item, oEmailTitlelist);
            ddlEmailTemplate.Adapter = adpEmailTemplate;

            ddlEmailTemplate.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(ddlEmailTemplate_ItemSelected);

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
                txtSendTo.Text = rentRunningTrans.Mobile;
            }
        }

        private void ddlEmailTemplate_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            string sTemplateCode = spinner.SelectedItem.ToString();
            if (!string.IsNullOrEmpty(sTemplateCode))
            {
                objDataManager = new DataManager();
                EmailTemplate oEmailTemplate = objDataManager.GetEmailTemplateByCode(sTemplateCode);
                if (oEmailTemplate != null)
                {
                    txtEmailTemplateContent.Text = oEmailTemplate.EmailBody;
                }
            }
        }

        public bool isValidPhone(string phone)
        {
            return Android.Util.Patterns.Phone.Matcher(phone).Matches();
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.dialog_complete_menu, menu);
            menu.FindItem(Resource.Id.menu_delete).SetVisible(false);
            return base.OnCreateOptionsMenu(menu);

            //MenuInflater.Inflate(Resource.Menu.dialog_complete_menu, menu);
            //menu.FindItem(Resource.Id.menu_delete).SetVisible(false);
            //return base.OnCreateOptionsMenu(menu);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {

            switch (item.ItemId)
            {
                case Resource.Id.menu_done:
                    if (!String.IsNullOrWhiteSpace(txtSendTo.Text.Trim()))
                    {
                        Regex regexEmail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                        if (!regexEmail.Match(txtSendTo.Text).Success)
                        {
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetMessage("Please enter valid email id.");
                            alert.SetNeutralButton("OK", delegate { });
                            alert.Create().Show();
                            //  Toast.MakeText(this, "Please enter valid email id.", ToastLength.Short).Show();
                            txtSendTo.RequestFocus();
                        }
                        else
                        {

                        }
                        try
                        {
                            this.progressLayout.Visibility = ViewStates.Gone;
                            var db = new SQLiteConnection(dbPath);
                            EmailToSend email = new EmailToSend();
                            email.EmailId = txtSendTo.Text;
                            email.EmailTemplate = ddlEmailTemplate.SelectedItem.ToString();
                            email.EmailBody = txtEmailTemplateContent.Text;
                            email.DateTime = DateTime.Now;
                            DataManager objData = new DataManager();
                            objData.SaveEmailToLocal(email);
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetMessage("Record has been saved successfully.");
                            alert.SetNeutralButton("OK", delegate
                            {
                                var intent = new Intent(this, typeof(MainMenuActivity));
                                StartActivity(intent);
                            });
                            alert.Create().Show();
                        }
                        catch (Exception ex)
                        {
                            Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
                        }

                        //this.progressLayout.Visibility = ViewStates.Gone;
                        //MailMessage mail = new MailMessage();
                        //SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        //// SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
                        //mail.From = new MailAddress("akashrajoriya94@gmail.com");
                        //mail.To.Add(txtSendTo.Text);
                        //mail.Subject = "Car Rental Agreement";
                        //mail.Body = (txtEmailTemplateContent.Text);
                        //    SmtpServer.Port = 587;
                        //    SmtpServer.Credentials = new System.Net.NetworkCredential("akashrajoriya94@gmail.com", "8273690066");
                        //    SmtpServer.EnableSsl = true;
                        //    SmtpServer.Send(mail);
                        //    // MessageBox.Show("mail Send");
                        //    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        //    alert.SetMessage("mail send.");
                        //    alert.SetNeutralButton("OK", delegate { });
                        //    alert.Create().Show();
                        //    //Toast.MakeText(this, "mail send", ToastLength.Short).Show();
                        //}
                        //catch (Exception ex)
                        //{

                        //}
                    }

                    else
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetMessage("Please fill all mandatory details.");
                        alert.SetNeutralButton("OK", delegate { });
                        alert.Create().Show();
                        // Toast.MakeText(this, "Please fill all mandatory details", ToastLength.Short).Show();
                    }
                    break;
                case Resource.Id.menu_cancel:
                    this.progressLayout.Visibility = ViewStates.Visible;
                    var intentVehicle = new Intent(this, typeof(RentFlowDriverSignatureActivity));
                    intentVehicle.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                    StartActivity(intentVehicle);
                    break;
            }
            //Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }
    }
}