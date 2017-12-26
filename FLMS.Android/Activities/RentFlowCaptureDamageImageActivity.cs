using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Provider;
using Java.IO;
using Environment = Android.OS.Environment;
using Uri = Android.Net.Uri;
using Android.Graphics;
using RentACar.UI.Modals;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Android;
using Android.Content.PM;

namespace RentACar.UI
{
    [Activity(Label = "Capture Damage Image")]
    public class RentFlowCaptureDamageImageActivity : Activity
    {

        ImageView imgVehicle;
        Spinner lstDamageType;
        Uri _ImagePath;
        VehicleMarkDamageDetails objMarkDamageDetail;
        String strLocationX;
        String strLocationY;
        int iLastDamageNumber = 0;
        int iDamageImageId;
        const string permission = Manifest.Permission.Camera;
        const int RequestLocationId = 0;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar actionBar = ((Activity)this).ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetLogo(Resource.Drawable.ic_launcher);
            actionBar.SetDisplayUseLogoEnabled(true);
            // Create your application here
            SetContentView(Resource.Layout.RentFlowCaptureDamageImage);
            CreateDirectoryForPictures();
            strLocationX = Intent.GetStringExtra("TapImageLocationX");
            strLocationY = Intent.GetStringExtra("TapImageLocationY");
            iDamageImageId = Convert.ToInt32(Intent.GetStringExtra("CurrentDamageImageId"));
            iLastDamageNumber = Convert.ToInt32(Intent.GetStringExtra("DamageCounter"));
            lstDamageType = FindViewById<Spinner>(Resource.Id.lstDamageType);
            var adpDamageType = ArrayAdapter.CreateFromResource(
                    this, Resource.Array.strArrDamageType, Resource.Layout.list_item);
            lstDamageType.Adapter = adpDamageType;

            Button btnTakeImage = FindViewById<Button>(Resource.Id.btnTakeImage);
            imgVehicle = FindViewById<ImageView>(Resource.Id.imgVehicle);
            Button btnSelectImage = FindViewById<Button>(Resource.Id.btnSelectImage);

            btnTakeImage.Click += btnTakeImage_Click;
            btnSelectImage.Click += btnSelectImage_Click;

            if (savedInstanceState != null)
            {
                //objMarkDamageDetail = JsonConvert.DeserializeObject<VehicleMarkDamageDetails>(savedInstanceState.GetString("objMarkDamageDetail", JsonConvert.SerializeObject(objMarkDamageDetail)));
            }
            else //if(objMarkDamageDetail != null)
            {
                if (Intent.GetStringExtra("objMarkDamageDetail") != null)
                {
                   // objMarkDamageDetail = JsonConvert.DeserializeObject<VehicleMarkDamageDetails>(Intent.GetStringExtra("objMarkDamageDetail"));
                }
            }
            //Load details from existing object

            if (objMarkDamageDetail != null)
            {
                lstDamageType.SetSelection(Convert.ToInt32(objMarkDamageDetail.DamageType));
                imgVehicle.SetImageURI(Uri.Parse(objMarkDamageDetail.ImagePath));
                _ImagePath = Uri.Parse(objMarkDamageDetail.ImagePath);
            }
            //Toast.MakeText(this, "Image tapped: " + Intent.GetStringExtra("TapImageLocationX") +","+ Intent.GetStringExtra("TapImageLocationY"), ToastLength.Short).Show();
        }

        async Task TryGetCameraAsync()
        {
            if ((int)Build.VERSION.SdkInt < 23)
            {
                GetCameraAsync();
                return;
            }

            await GetCameraPermissionAsync();
        }
        readonly string[] PermissionsCamera =
          {
              Manifest.Permission.Camera,
              Manifest.Permission.WriteExternalStorage
            };


        public override async void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            switch (requestCode)
            {
                case RequestLocationId:
                    {
                        if (grantResults[0] == Permission.Granted)
                        {

                            //Permission granted
                            //var snack = Snackbar.Make(layout, "Location permission is available, getting lat/long.", Snackbar.LengthShort);
                            //snack.Show();

                            GetCameraAsync();
                        }
                        else
                        {
                            //Permission Denied 🙁
                            //Disabling location functionality
                            //var snack = Snackbar.Make(layout, "Location permission is denied.", Snackbar.LengthShort);
                            //snack.Show();
                        }
                    }
                    break;
            }
        }

        async Task GetCameraPermissionAsync()
        {
            //Check to see if any permission in our group is available, if one, then all are

            if (CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                GetCameraAsync();
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
            RequestPermissions(PermissionsCamera, RequestLocationId);
        }
        void GetCameraAsync()
        {

            Intent intent = new Intent(MediaStore.ActionImageCapture);
            App._file = new File(App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
            intent.PutExtra(MediaStore.ExtraOutput, Uri.FromFile(App._file));
            StartActivityForResult(intent, 0);
            //await ;
        }

        protected override void OnSaveInstanceState(Bundle savedInstanceState)
        {
           // savedInstanceState.PutString("objMarkDamageDetail", JsonConvert.SerializeObject(objMarkDamageDetail));
            // always call the base implementation!
            base.OnSaveInstanceState(savedInstanceState);
        }
        //Manifest.Permission.Camera
        private async void btnTakeImage_Click(object sender, EventArgs e)
        {
            if (CheckSelfPermission(permission) == (int)Permission.Granted)
            {
                GetCameraAsync();
                return;
            }
            else
            {
                await GetCameraPermissionAsync();
            }
        }

        private void btnSelectImage_Click(object sender, EventArgs e)
        {
            Intent = new Intent();
            Intent.SetType("image/*");
            Intent.SetAction(Intent.ActionGetContent); 
            StartActivityForResult(Intent.CreateChooser(Intent, "Select Picture"), 1);
        }

        private void CreateDirectoryForPictures()
        {
            Environment.DirectoryPictures = "Images";
            App._dir = new File(
                Environment.GetExternalStoragePublicDirectory(
                    Environment.DirectoryPictures), "RentACarDemo");
            if (!App._dir.Exists())
            {
                App._dir.Mkdirs();
            }
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if ((requestCode == 1) && (resultCode == Result.Ok) && (data != null))
            {
                Uri uri = data.Data;
                _ImagePath = uri;
                imgVehicle.SetImageURI(uri);
            }
            else
            {
                // Make it available in the gallery

                Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                Uri contentUri = Uri.FromFile(App._file);
                _ImagePath = contentUri;
                mediaScanIntent.SetData(contentUri);
                SendBroadcast(mediaScanIntent);

                // Display in ImageView. We will resize the bitmap to fit the display.
                // Loading the full sized image will consume to much memory
                // and cause the application to crash.

                int height = Resources.DisplayMetrics.HeightPixels;
                int width = imgVehicle.Height;
                App.bitmap = App._file.Path.LoadAndResizeBitmap(width, height);
                if (App.bitmap != null)
                {
                    imgVehicle.SetImageBitmap(App.bitmap);
                    App.bitmap = null;
                }

                // Dispose of the Java side bitmap.
                GC.Collect();
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.dialog_complete_menu, menu);
            if (objMarkDamageDetail == null)
            {
                menu.FindItem(Resource.Id.menu_delete).SetVisible(false);
            }
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var intent_MarkDamage = new Intent(this, typeof(RentFlowMarkDamageActivity));
            switch (item.ItemId)
            {
                case Resource.Id.menu_done:
                    Regex regxDamageType = new Regex("(Select damage type)");
                    if (!regxDamageType.Match(lstDamageType.SelectedItem.ToString()).Success)
                    {
                        if (_ImagePath!=null)
                        {
                            if (objMarkDamageDetail == null)
                            {
                                objMarkDamageDetail = new VehicleMarkDamageDetails();
                                objMarkDamageDetail.VehicleTransID = Convert.ToInt32(Intent.GetStringExtra("VehicleTransId"));
                                objMarkDamageDetail.DamageLocationX = Convert.ToInt32(strLocationX);
                                objMarkDamageDetail.DamageLocationY = Convert.ToInt32(strLocationY);
                                objMarkDamageDetail.DamageNumber = iLastDamageNumber + 1;
                                objMarkDamageDetail.DamageType = lstDamageType.SelectedItemId.ToString();
                                objMarkDamageDetail.ImagePath = _ImagePath.ToString();
                                objMarkDamageDetail.DamageImageId = iDamageImageId;
                                //objMarkDamageDetail.DamageImageId = imgVehicle.Id = 102;
                                // objMarkDamageDetail.DamageImageId = imgVehicle.Id = 102;
                            }
                            else
                            {
                                objMarkDamageDetail.DamageType = lstDamageType.SelectedItemId.ToString();
                                objMarkDamageDetail.ImagePath = _ImagePath.ToString();
                            }
                           // intent_MarkDamage.PutExtra("MarkDamage", JsonConvert.SerializeObject(objMarkDamageDetail));
                           // SetResult(Result.Ok, intent_MarkDamage);
                            //Finish();
                        }
                        else
                        {
                            AlertDialog.Builder alert = new AlertDialog.Builder(this);
                            alert.SetMessage("Please select image from gallery or take from camera.");
                            alert.SetNeutralButton("OK", delegate { });
                            alert.Create().Show();                            
                        }
                    }
                    else
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetMessage("Please select damage type.");
                        alert.SetNeutralButton("OK", delegate { });
                        alert.Create().Show();
                        lstDamageType.RequestFocus();
                    }
                    
                    break;
                case Resource.Id.menu_cancel:
                    SetResult(Result.Canceled, intent_MarkDamage);
                    Finish();
                    break;
                case Resource.Id.menu_delete:
                   // intent_MarkDamage.PutExtra("MarkDamage", JsonConvert.SerializeObject(objMarkDamageDetail));
                  //  SetResult(Result.FirstUser, intent_MarkDamage);
                  //  Finish();
                    break;
            }
            //Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }
    }

    public static class App
    {
        public static File _file;
        public static File _dir;
        public static Bitmap bitmap;
    }

    public static class BitmapHelpers
    {
        public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            BitmapFactory.DecodeFile(fileName, options);

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;

            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);

            return resizedBitmap;
        }
    }
}