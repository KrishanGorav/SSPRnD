using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Views;
using Android.Content;
using System.IO;
using SQLite;
using RentACar.UI.Modals;
using Android;

namespace RentACar.UI
{
    [Activity(Label = "PAYG", Icon = "@drawable/ic_launcher")]
    public class LoginActivity : Activity
    {
        EditText txtUsername;
        EditText txtPassword;
        Button btnLogin;
        ProgressBar progressLayout;
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            ActionBar actionBar = ((Activity)this).ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetLogo(Resource.Drawable.ic_launcher);
            actionBar.SetDisplayUseLogoEnabled(true);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            // Get the UI controls from the loaded layout:
            this.progressLayout = FindViewById<ProgressBar>(Resource.Id.progressLayout);
            this.progressLayout.Visibility = ViewStates.Gone;

            txtUsername = FindViewById<EditText>(Resource.Id.txtUsername);
            txtPassword = FindViewById<EditText>(Resource.Id.txtPassword);
            btnLogin = FindViewById<Button>(Resource.Id.btnLogin);

            btnLogin.Click += BtnLogin_Click ;
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            ValidateLogin();
            //Toast.MakeText(this, "Not checked", ToastLength.Short).Show();
        }



        protected void ValidateLogin()
        {
            // On "Login" button click, try to ask for enter login details.
            var callDialog = new AlertDialog.Builder(this);
            if (!String.IsNullOrWhiteSpace(txtUsername.Text.Trim()) && !String.IsNullOrWhiteSpace(txtPassword.Text.Trim()))
            {
                if (txtUsername.Text.Trim() == "demo" && txtPassword.Text.Trim() == "demo")
                {
                    this.progressLayout.Visibility = ViewStates.Visible;
                    //Save user details to local storage
                    UserDetail userDetail = new UserDetail();
                    userDetail.UserId = 1;
                    userDetail.CompanyId = 1;
                    userDetail.UserName = txtUsername.Text.Trim();
                    userDetail.Password = txtPassword.Text.Trim();
                    DataManager dataManager = new DataManager();
                    dataManager.UpdateUserDetailsToLocal(userDetail);

                    ApplicationClass.UserId = userDetail.UserId;
                    ApplicationClass.UserName = userDetail.UserName;
                    ApplicationClass.CompanyId = userDetail.CompanyId;
                    
                    var intent = new Intent(this, typeof(MainMenuActivity));
                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    callDialog.SetMessage("Invalid login details.Please enter demo,demo");
                    callDialog.SetNegativeButton("OK", delegate { });
                    // Show the alert dialog to the user and wait for response.
                    callDialog.Show();
                }
            }
            else
            {  
                callDialog.SetMessage("Enter Login Details");
                callDialog.SetNegativeButton("OK", delegate { });
                // Show the alert dialog to the user and wait for response.
                callDialog.Show();
            }
        }
    }
}

