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

namespace RentACar.UI
{
    public class ApplicationClass : Application
    {
        public static int userId;
        public static string username;
        public static int UserDefaultVehicle;
        public static string ServiceEndPoint;
        public static string SecurityToken;
        public static int currentRunningJourneyId;
        public static bool isJourneyRunning;
    }
   
}