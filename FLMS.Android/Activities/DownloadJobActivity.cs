using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using RentACar.UI.Modals;
using System.Net;
using System.IO;
using System.Threading.Tasks;
using Android.Graphics;
using UniversalImageLoader.Core;
using Android;
using Android.Content.PM;
using DSoft.Datatypes.Grid.Data;
using DSoft.UI.Grid;
using DSoft.UI.Grid.Views;

namespace RentACar.UI
{
    [Activity(Label = "Download Job")]
    public class DownloadJobActivity : Activity
    {
        AutoCompleteTextView txtRegNo;
        Button btnSearch;
        VehicleUKData objVehicleImageList;
        ImageLoader imageLoader;
        const string permission = Manifest.Permission.WriteExternalStorage;
        const int RequestExternalStorageId = 0;
        DSGridView DataGrid;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar actionBar = ((Activity)this).ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetLogo(Resource.Drawable.ic_launcher);
            actionBar.SetDisplayUseLogoEnabled(true);

            // Create your application here
            SetContentView(Resource.Layout.DownloadJob);
            // Get singleton instance
            imageLoader = ImageLoader.Instance;
            DataGrid = FindViewById<DSGridView>(Resource.Id.DataGrid);
            txtRegNo = FindViewById<AutoCompleteTextView>(Resource.Id.txtRegNo);
            btnSearch = FindViewById<Button>(Resource.Id.btnSearch);

            btnSearch.Click += btnSearch_Click;
           
            DataGrid.DataSource = new DSDataTable();
            DataGrid.TableName = "DT1";
           
        }

        public void ViewDidLoad()
        {
            //create the data table object and set a name
            var aDataSource = new DSDataTable("ADT");
            //add a column
            var dc1 = new DSDataColumn("Title");
            dc1.Caption = "Title";
            dc1.ReadOnly = true;
            dc1.DataType = typeof(String);
            dc1.AllowSort = true;
            //dc1.Width = ColumnsDefs[aKey];

            aDataSource.Columns.Add(dc1);

            //add a row to the data table
            var dr = new DSDataRow();
            //dr["ID"] = loop;
            dr["Title"] = @"Test";
            dr["Description"] = @"Some description would go here";
            dr["Date"] = DateTime.Now.ToShortDateString();
            dr["Value"] = "10000.00";

            //set the value as an image
            //dr["Image"] = UIImage.FromFile("first.png")

             aDataSource.Rows.Add(dr); 

}

        //public void MyDataTable(String Name)
        //{
        //    var aDataSource = new DSDataTable("ADT");
        //    //create and add a column
        //    var dc1 = new DSDataColumn("Title");
        //    dc1.Caption = "Title";
        //    dc1.ReadOnly = true;
        //    dc1.DataType = typeof(String);
        //    dc1.AllowSort = true;
        //    // dc1.Width = ColumnsDefs[aKey];
        //    aDataSource.Columns.Add(dc1);
        //}

        //public  int GetRowCount()
        //{
        //    return 100000;
        //}

        //public  DSDataRow GetRow(int Index)
        //{
        //    DSDataRow aRow = null;

        //    ////check to see if we have a row for this index
        //    //if (Index < Rows.Count)
        //    //{
        //    //    aRow = Rows[Index];
        //    //}
        //    //else
        //    //{
        //    //    // create a new one
        //    //    aRow = new DSDataRow();
        //    //    aRow["Title"] = @"Test";
        //    //    Rows.Add(aRow);
        //    //}
        //    aRow["Title"] = @"Test";
        //    ///set the values
        //    aRow["Description"] = @"Some description would go here";
        //    aRow["Date"] = DateTime.Now.ToShortDateString();
        //    aRow["Value"] = "10000.00";

        //    //see if even or odd to pick an image from the array
        //    var pos = Index % 2;
        //   // aRow["Image"] = Icons[pos];
        //    aRow["Ordered"] = (pos == 0) ? true : false;

        //    return aRow;
        //}


        void btnSearch_Click(object sender, EventArgs e)
        {
            DownLoadVehicleImages("km12akk");
            //objDataManager = new DataManager();
            //oVehicles = objDataManager.GetVehicleDetailsFromOnline();
            //List<String> oVRegNoList = new List<String>();
            //foreach (Vehicle v in oVehicles)
            //{
            //    oVRegNoList.Add(v.RegNumber);
            //}
            //var adpVRegNo = new ArrayAdapter<String>(this, Resource.Layout.list_item, oVRegNoList);
            //txtRegNo.Adapter = adpVRegNo;

            //strVehicle = Resources.GetStringArray(Resource.Array.strVehicle);
            //var adapter = new ArrayAdapter<String>(this, Resource.Layout.list_item, strVehicle);
            //gridview.Adapter = adapter;
        }
        async Task TryGetCameraAsync()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                GetExternalStorageAsync();
                return;
            }

            GetExternalStoragePermissionAsync();
        }

        readonly string[] PermissionsExternalStorage =
         {
              //Manifest.Permission.Camera,
              Manifest.Permission.WriteExternalStorage
            };

        private void GetExternalStoragePermissionAsync()
        {
            if (CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                //GetExternalStorageAsync();
                return;
            }

            //need to request permission
            if (ShouldShowRequestPermissionRationale(permission))
            {

                // Explain to the user why we need to read the contacts
                ////Snackbar.Make(layout, "Location access is required to show coffee shops nearby.", Snackbar.LengthIndefinite)
                ////        .SetAction("OK", v => RequestPermissions(PermissionsLocation, RequestLocationId))
                ////        .Show();
                //return;
            }
            //Finally request permissions with the list of permissions and Id
            RequestPermissions(PermissionsExternalStorage, RequestExternalStorageId);
        }

        private void GetExternalStorageAsync()
        {

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
                case Resource.Id.menu_cancel:
                    // this.progressLayout.Visibility = ViewStates.Visible;
                    var intent_sendsms = new Intent(this, typeof(MainMenuActivity));
                    StartActivity(intent_sendsms);
                    break;

                case Resource.Id.menu_settings:
                    //this.progressLayout.Visibility = ViewStates.Visible;
                    var intent_settings = new Intent(this, typeof(SettingsActivity));
                    intent_settings.PutExtra("FromActivity", "Download");
                    StartActivity(intent_settings);
                    break;
                case Resource.Id.menu_logout:
                    // this.progressLayout.Visibility = ViewStates.Visible;
                    DataManager objDataManager = new DataManager();
                    objDataManager.Logout();
                    ApplicationClass.UserId = 0;
                    ApplicationClass.UserName = null;
                    ApplicationClass.CompanyId = 0;
                    var intent_logout = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent_logout);
                    break;
            }
            //Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }

        private void DisplayVehicleDetails(RentRunningTrans rentRunningTrans)
        {
            txtRegNo.Text = rentRunningTrans.RegNo;
        }


        private async void DownLoadVehicleImages(string strRegNo)
        {
            if (CommonFunctions.IsNetworkConnected())
            {
                //km12akk
                // Get the latitude and longitude entered by the user and create a query.km12akk";//
                string url = "https://uk1.ukvehicledata.co.uk/api/datapackage/VehicleImageData?v=2&api_nullitems=1&auth_apikey=a418b0ad-33ab-4953-9a00-9d3ea8a12319&key_VRM=" + strRegNo;

                // Fetch the vehicle information asynchronously, 
                // parse the results, then update the screen:
                objVehicleImageList = await GetVehicleImagesfromAPIAsync(url);

                if (objVehicleImageList != null)
                {
                    if (objVehicleImageList.Response.StatusCode.Equals("Success"))
                    {
                        if (CheckSelfPermission(permission) == (int)Permission.Granted)
                        {
                            CommonFunctions.CreateDirectoryForApp();
                            CommonFunctions.CreateDirectoryForRegNo(strRegNo);

                            //Needed to uncommment
                            for (int i = 0; i < objVehicleImageList.Response.DataItems.VehicleImages.ImageDetailsList.Count(); i++)
                            {
                                try
                                {
                                    string _RealVehicleImageName = String.Format("_{0}.png", i + 1);
                                    string _RealVehicleImagePath = Android.OS.Environment.ExternalStorageDirectory + "/RentACar" + "/" + strRegNo + "/" + _RealVehicleImageName;
                                    // Load image synchronously
                                    //// Bitmap bmp = imageLoader.LoadImageSync(objVehicleImageList.Response.DataItems.VehicleImages.ImageDetailsList[i].ImageUrl);
                                    Bitmap imageBitmap = CommonFunctions.GetBitmapFromUrl(objVehicleImageList.Response.DataItems.VehicleImages.ImageDetailsList[i].ImageUrl);
                                    var stream = new System.IO.FileStream(_RealVehicleImagePath, System.IO.FileMode.Create);
                                    imageBitmap.Compress(Bitmap.CompressFormat.Png, 100, stream);
                                    stream.Flush();
                                    stream.Close();
                                }
                                catch (Exception ex)
                                {
                                    // TODO: handle exception
                                }
                                finally
                                {

                                }

                            }
                        }
                        else
                        {
                            GetExternalStoragePermissionAsync();
                        }

                    }
                    else
                    {

                    }
                }
            }
            else
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetMessage("Please enable internet to get vehicle images from API.");
                alert.SetNeutralButton("OK", delegate { });
                alert.Create().Show();
            }
            //this.progressLayout.Visibility = ViewStates.Gone;
        }

        // Gets vehicle images data from the passed URL.
        private async Task<VehicleUKData> GetVehicleImagesfromAPIAsync(string url)
        {
            try
            {
                // Create an HTTP web request using the URL:
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";

                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync())
                {
                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        string responseString = string.Empty;
                        StreamReader reader = new StreamReader(stream);
                        responseString = reader.ReadToEnd();
                        stream.Flush();

                        VehicleUKData obj = null;
                        if (responseString != "")
                        {
                            //Converting JSON Array Objects into generic list  
                            obj = JsonConvert.DeserializeObject<VehicleUKData>(responseString);
                        }

                        return obj;
                    }
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                //return null;
            }
            return null;
        }

        // Gets vehicle details data from the online Database Service End Point passed URL.
        private async Task<Vehicle> GetVehicleDetailsFromOnlineAsync(string url)
        {
            try
            {
                // Create an HTTP web request using the URL:
                HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(new Uri(url));
                request.ContentType = "application/json";
                request.Method = "GET";

                // Send the request to the server and wait for the response:
                using (WebResponse response = await request.GetResponseAsync())
                {
                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        string responseString = string.Empty;
                        StreamReader reader = new StreamReader(stream);
                        responseString = reader.ReadToEnd();
                        stream.Flush();

                        Vehicle obj = null;
                        if (responseString != "")
                        {
                            //Converting JSON Array Objects into generic list  
                            obj = JsonConvert.DeserializeObject<Vehicle>(responseString);
                        }

                        return obj;
                    }
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                //return null;
            }
            return null;
        }

        
    }
}