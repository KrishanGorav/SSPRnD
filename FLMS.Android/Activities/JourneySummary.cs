using System;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Views;
using Android.Widget;
using RentACar.UI.Modals;

namespace RentACar.UI.Activities
{
    [Activity(Label = "Journey Summary")]
    public class JourneySummary : Activity
    {
        ProgressBar progressLayout;
        Geocoder geoCoder;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            geoCoder=new Geocoder(this);
            ActionBar actionBar = ((Activity)this).ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetLogo(Resource.Drawable.ic_launcher);
            actionBar.SetDisplayUseLogoEnabled(true);

            SetContentView(Resource.Layout.JourneySummary);

            this.progressLayout = FindViewById<ProgressBar>(Resource.Id.progressLayout);
            this.progressLayout.Visibility = ViewStates.Gone;
            // Create your application here
            if (ApplicationClass.currentRunningJourneyId != 0)
            {
                try
                {
                    var lblStartDate = FindViewById<TextView>(Resource.Id.lblStartDate);
                    var lblFrom = FindViewById<TextView>(Resource.Id.lblFrom);
                    var lblTo = FindViewById<TextView>(Resource.Id.lblTo);
                    var lblDuration = FindViewById<TextView>(Resource.Id.lblDuration);
                    var lblCost = FindViewById<TextView>(Resource.Id.lblCost);
                    DataManager dataManager = new DataManager();
                    Journey lastJourney = dataManager.GetLastJourney();
                    JourneyDetail journeyDetailStart = dataManager.GetStaringPointForJourney();
                    if (lblStartDate != null)
                    {
                        lblStartDate.Text = "Start at : " + lastJourney.StartDate.ToString("dd/MM/yyyy hh:mm");
                        //string fromLocation = "";
                        //string toLocation = "";
                        GetLocationNameAsync(Convert.ToDouble(journeyDetailStart.Latitude), Convert.ToDouble(journeyDetailStart.Longitude), lblFrom,"From : ");
                        //lblFrom.Text = "From: " + fromLocation;
                        JourneyDetail journeyDetailend = dataManager.GetEndPointForJourney();
                        GetLocationNameAsync(Convert.ToDouble(journeyDetailend.Latitude), Convert.ToDouble(journeyDetailend.Longitude), lblTo, "To : ");
                        //lblTo.Text = "To: " + toLocation,"From : "
                        lblDuration.Text = "Duration: " + (lastJourney.EndDate - lastJourney.StartDate).Minutes.ToString();
                        lblCost.Text = "Cost: £" + CalculateCost((lastJourney.EndDate - lastJourney.StartDate).Minutes).ToString();
                    }
                    else
                    {
                        ShowMessage("No view found to load");
                    }
                }
                catch (Exception ex)
                {
                    ShowMessage(ex.Message.ToString());
                }
            }
        }
        private void ShowMessage(string message)
        {
            var callDialog = new AlertDialog.Builder(this);
            callDialog.SetMessage(message);
            callDialog.SetNegativeButton("OK", delegate { });
            // Show the alert dialog to the user and wait for response.
            callDialog.Show();
        }
        private double CalculateCost(int minsToCalculate)
        {
            //double dBaseRate = 187.0;
            double dPerHourRate = 1.20;
            return minsToCalculate * (dPerHourRate / 60.0);
        }

        private async void GetLocationNameAsync(double latitude, double longitude,TextView textViewToDisplay,string staticText)
        {
            try
            {
                var addresses =await geoCoder.GetFromLocationAsync(latitude, longitude, 1);
                if (addresses.Count > 0)
                {
                    textViewToDisplay.Text= staticText+ addresses[0].GetAddressLine(0).ToString()+" " +addresses[0].GetAddressLine(1).ToString();
                }
                else
                    textViewToDisplay.Text= staticText;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private async System.Threading.Tasks.Task<string> GetLocationNameToAsync(double latitude, double longitude)
        {
            try
            {
                var addresses = await geoCoder.GetFromLocationAsync(latitude, longitude, 1);
                if (addresses.Count > 0)
                {
                    return addresses[0].GetAddressLine(0).ToString() + " " + addresses[0].GetAddressLine(1).ToString();
                }
                else
                    return string.Empty;
            }
            catch (Exception ex)
            {
                //throw ex;
                return ex.Message.ToString();
            }
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