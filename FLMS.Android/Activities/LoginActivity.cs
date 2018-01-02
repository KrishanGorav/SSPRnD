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
using System.Collections.Generic;
using System.Threading.Tasks;

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

            btnLogin.Click += BtnLogin_Click;
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
                if (txtUsername.Text.Trim() == "Deepak" && txtPassword.Text.Trim() == "Test@123")
                {
                    if (CommonFunctions.IsNetworkConnected())
                    {
                        this.progressLayout.Visibility = ViewStates.Visible;
                        List<UserDetail> oUserDetail =  GetLogin(txtUsername.Text.Trim(),txtPassword.Text.Trim());

                        UserDetail userDetail = new UserDetail();
                        if (oUserDetail != null)
                        {
                            userDetail.userid = oUserDetail[0].userid;
                            userDetail.UserDefaultVehicle = 1;
                            userDetail.userName = oUserDetail[0].userName;
                            userDetail.token = oUserDetail[0].token;
                        }
                        else
                        {
                            userDetail.userid = 1;
                            userDetail.UserDefaultVehicle = 1;
                            userDetail.userName = txtUsername.Text.Trim();
                            userDetail.token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6WyIxIiwiZGVlcGFrIl0sIm5iZiI6MTUxNDg3MjM5MCwiZXhwIjoxNTE3NTUwNzkwLCJpYXQiOjE1MTQ4NzIzOTAsImlzcyI6IkV4YW1wbGVJc3N1ZXIiLCJhdWQiOiJFeGFtcGxlQXVkaWVuY2UifQ.IDzYN3gqwXfkM_bN0Br1spInlTuYqm4CDb15xwpObvI";
                        }
                        userDetail.Password = txtPassword.Text.Trim();
                        //Save user details to local storage
                        DataManager dataManager = new DataManager();
                        dataManager.UpdateUserDetailsToLocal(userDetail);

                        ApplicationClass.userId = userDetail.userid;
                        ApplicationClass.username = userDetail.userName;
                        ApplicationClass.UserDefaultVehicle = userDetail.UserDefaultVehicle;
                        ApplicationClass.ServiceEndPoint = "http://192.168.73.50:8000/api/";
                        var intent = new Intent(this, typeof(MainMenuActivity));
                        StartActivity(intent);
                        Finish();
                    }
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

        private async Task<List<UserDetail>> GetLoginAsync(string user)
        {
            List<UserDetail> obj = null;
            try
            {
                GetAPIResult<UserDetail> api = CommonFunctions.APIGet<UserDetail>("GetUser/" + user, string.Empty);
                if (api.HttpStatus == System.Net.HttpStatusCode.OK)
                {
                    obj = api.DataColl;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }

        private List<UserDetail> GetLogin(string user,string password)
        {
            List<UserDetail> obj = null;
            try
            {
                GetAPIResult<UserDetail> api = CommonFunctions.APIGet<UserDetail>("auth/login/" + user, string.Empty);
                if (api.HttpStatus == System.Net.HttpStatusCode.OK)
                {
                    obj = api.DataColl;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return obj;
        }
    }
}

