using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Locations;
using Android.OS;
using Android.Runtime;
using Android.Util;

namespace RentACar.UI
{
    [Service]
    public class CoordinateService : Service, ILocationListener
    {
        static readonly string TAG = "X:" + typeof(CoordinateService).Name;
        //static readonly int TimerWait = 10000;
        //Timer timer;
        //DateTime startTime;

        Location _currentLocation;
        LocationManager _locationManager;
        string _locationProvider;

        public override void OnCreate()
        {
            base.OnCreate();
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            //Log.Debug(TAG, $"OnStartCommand called at {startTime}, flags={flags}, startid={startId}");
            if (!ApplicationClass.isJourneyRunning)
            {
                //startTime = DateTime.UtcNow;
                //Log.Debug(TAG, $"Starting the service, at {startTime}.");
                //timer = new Timer(HandleTimerCallback, startTime, 0, TimerWait);
                
                try
                {
                    InitializeLocationManager();
                    ApplicationClass.isJourneyRunning = true;
                    DataManager dataManager = new DataManager();
                    ApplicationClass.currentRunningJourneyId = dataManager.AddRecordForStartJourney();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return StartCommandResult.NotSticky;
        }

        public override IBinder OnBind(Intent intent)
        {
            // This is a started service, not a bound service, so we just return null.
            return null;
        }


        public override void OnDestroy()
        {
            //timer.Dispose();
            //timer = null;
            try
            {
                DataManager dataManager = new DataManager();
                Location lastKnownLocation = _locationManager.GetLastKnownLocation(_locationProvider);
                dataManager.AddLocationToRunningJourney(lastKnownLocation.Longitude.ToString(), lastKnownLocation.Latitude.ToString());
                dataManager.StopCurrentRunningJourney();
                ApplicationClass.isJourneyRunning = false;
                
                //TimeSpan runtime = DateTime.UtcNow.Subtract(startTime);
                //Log.Debug(TAG, $"Simple Service destroyed at {DateTime.UtcNow} after running for {runtime:c}.");
                _locationManager.RemoveUpdates(this);
                //Redirect to show summary of current journey
            }
            catch (Exception ex)
            {
                throw ex;
            }
            base.OnDestroy();
        }


        void HandleTimerCallback(object state)
        {
            //TimeSpan runTime = DateTime.UtcNow.Subtract(startTime);
            ////_locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
            //_currentLocation = _locationManager.GetLastKnownLocation(_locationProvider);
            //Log.Debug(TAG, $"This service has been running for {runTime:c} (since ${state}).");
        }


        public void OnLocationChanged(Location location)
        {
            _currentLocation = location;
            if (_currentLocation == null)
            {
                var callDialog = new AlertDialog.Builder(this);
                callDialog.SetMessage("Unable to trace current location.");
                callDialog.SetNegativeButton("OK", delegate { });
                // Show the alert dialog to the user and wait for response.
                callDialog.Show();
            }
            else
            {
                DataManager dataManager = new DataManager();
                dataManager.AddLocationToRunningJourney(_currentLocation.Latitude.ToString(), _currentLocation.Longitude.ToString());
                //txtlatitu.Text = _currentLocation.Latitude.ToString();
                //txtlong.Text = _currentLocation.Longitude.ToString();
            }
            //timer.Dispose();
            //timer = null;
        }

        public void OnProviderDisabled(string provider)
        {

        }

        public void OnProviderEnabled(string provider)
        {

        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {

        }

        private void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteriaForLocationService = new Criteria
            {
                Accuracy = Accuracy.Fine
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteriaForLocationService, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
                _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
            }
            else
            {
                _locationProvider = string.Empty;
            }
            Log.Debug(TAG, "Using " + _locationProvider + ".");
        }
    }
}