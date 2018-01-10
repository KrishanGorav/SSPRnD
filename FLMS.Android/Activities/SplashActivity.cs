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
using System.Threading.Tasks;
using System.IO;
using SQLite;
using RentACar.UI.Modals;

namespace RentACar.UI
{
    
    [Activity(Theme = "@style/RentACarCustom.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
        }

        // Launches the startup task
        protected override void OnResume()
        {
            base.OnResume();
            Task startupWork = new Task(() => { SimulateStartup(); });
            startupWork.Start();
        }

        // Simulates background work that happens behind the splash screen
        async void SimulateStartup()
        {
            await Task.Delay(4000); // Simulate a bit of startup work.
            DoSomeDataAccess();
           // CommonFunctions.CreateDirectoryForApp();
            
            DataManager dataManager = new DataManager();
            var userDetail = dataManager.GetUser();
            if (userDetail == null)
            {
                StartActivity(new Intent(Application.Context, typeof(LoginActivity)));
            }
            else
            {
                ApplicationClass.userId = userDetail.userid;
                ApplicationClass.username = userDetail.userName;
                ApplicationClass.UserDefaultVehicle = 1;
                var dashBoard = new Intent(this, typeof(MainMenuActivity));
                StartActivity(dashBoard);
            }
        }

        public static void DoSomeDataAccess()
        {
            DataManager dataManager = new DataManager();
            dataManager.CreateUserTable();
            dataManager.CreateVehicleTable();
            dataManager.CreateUserVehicleTable();
            dataManager.CreateJourneyTable();
            dataManager.CreateJourneyDetailTable();
            dataManager.CreateSettingTable();
            //dataManager.AddDefaultVehicles();
            //dataManager.CreateVehicleRentTable();
            //dataManager.CreateVehicleMarkDamageDetailsTable();
            //dataManager.CreateVehicleMarkedDamageImageTable();
            //dataManager.CreateSmsTemplateTable();
            //dataManager.CreateEmailTemplateTable();
            //dataManager.AddDefaultSmsTemplate();
            //dataManager.AddDefaultEmailTemplate();
            //dataManager.createSmsToSendTable();
            //dataManager.CreateEmailToSendTable();
            //dataManager.CreateSettingTable();


            //Console.WriteLine("Creating database, if it doesn't already exist");
            //string dbPath = Path.Combine(
            //     System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),
            //     "PAYG.db3");
            //var db = new SQLiteConnection(dbPath);
            //db.CreateTable<Vehicle>();
            ////if (db.Table<VehicleMaster>().Count() == 0)
            //{
            //    // only insert the data if it doesn't already exist
            //    var obVehicleMaster = new Vehicle();
            //    obVehicleMaster.VehicleID = 1;
            //    obVehicleMaster.VehicleStatus = "test";
            //    db.Insert(obVehicleMaster);

            //}
            //Console.WriteLine("Reading data");
            //var table = db.Table<Stock>();
            //foreach (var s in table)
            //{
            //    Console.WriteLine(s.Id + " " + s.Symbol);
            //}
        }
    }
}