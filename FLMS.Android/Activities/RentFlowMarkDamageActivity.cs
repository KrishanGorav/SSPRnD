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
using Android.Graphics;
using Newtonsoft.Json;
using RentACar.UI.Modals;
//using Java.IO;
//using System.IO;
using System.Net;
using System.IO;
//using System.Json;
using System.Threading.Tasks;
using Android.Graphics.Drawables;

namespace RentACar.UI
{
    [Activity(Label = "Mark Damage")]
    public class RentFlowMarkDamageActivity : Activity, View.IOnTouchListener, View.IOnLongClickListener, View.IOnDragListener, GestureDetector.IOnGestureListener, View.IOnClickListener
    {
        //ImageView imgVehicle;
        FrameLayout _markdamage;
        LinearLayout _layoutVehicleImages;
        VehicleMarkDamageDetails objVehicleMarkDamageDetails;
        RentRunningTrans rentRunningTrans;
        int iLastDamageNumber = 0;
        int iCurrentDamageImageId = 0;
        private GestureDetector _gestureDetector;
        //VehicleUKData objVehicleImageList;
        ProgressBar progressLayout;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);


            ActionBar actionBar = ((Activity)this).ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetLogo(Resource.Drawable.ic_launcher);
            actionBar.SetDisplayUseLogoEnabled(true);
            // Create your application here
            SetContentView(Resource.Layout.RentFlowMarkDamage);
            this.progressLayout = FindViewById<ProgressBar>(Resource.Id.progressLayout);
            this.progressLayout.Visibility = ViewStates.Gone;
            _gestureDetector = new GestureDetector(BaseContext, this);
            try
            {
                var actionToolbar = FindViewById<Toolbar>(Resource.Id.action_toolbar);
                actionToolbar.SetNavigationIcon(Resource.Drawable.ic_action_account_circle);
                actionToolbar.Title = ApplicationClass.UserName;
                actionToolbar.SetPadding(0, 0, 0, 00);
                actionToolbar.InflateMenu(Resource.Menu.action_menus);
                actionToolbar.Menu.FindItem(Resource.Id.menu_save).SetVisible(false);
                actionToolbar.Menu.FindItem(Resource.Id.menu_sendemail).SetVisible(false);
                actionToolbar.MenuItemClick += (sender, e) =>
                {
                    if (e.Item.ItemId == Resource.Id.menu_next)
                    {
                        this.progressLayout.Visibility = ViewStates.Visible;
                        SaveAndContinue();
                    }
                    if (e.Item.ItemId == Resource.Id.menu_back)
                    {
                        var intentVehicle = new Intent(this, typeof(RentFlowVehicleActivity));
                        intentVehicle.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                        StartActivity(intentVehicle);
                    }
                    if (e.Item.ItemId == Resource.Id.menu_sendsms)
                    {
                        this.progressLayout.Visibility = ViewStates.Visible;
                        var intentSendSMS = new Intent(this, typeof(SendSMSActivity));
                        intentSendSMS.PutExtra("MobileNo", rentRunningTrans.Mobile);
                        intentSendSMS.PutExtra("FromActivity", "MarkDamage");
                        intentSendSMS.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                        StartActivity(intentSendSMS);
                    }
                    if (e.Item.ItemId == Resource.Id.menu_sendemail)
                    {
                        this.progressLayout.Visibility = ViewStates.Visible;
                        var intentMainMenu = new Intent(this, typeof(SendEmailActivity));
                        StartActivity(intentMainMenu);
                    }
                    if (e.Item.ItemId == Resource.Id.menu_video)
                    {
                        this.progressLayout.Visibility = ViewStates.Visible;
                        var intentVideo = new Intent(this, typeof(CaptureVideoActivity));
                        StartActivity(intentVideo);
                    }
                    if (e.Item.ItemId == Resource.Id.menu_dashboard)
                    {
                        this.progressLayout.Visibility = ViewStates.Visible;
                        rentRunningTrans = null;
                        var intentMainMenu = new Intent(this, typeof(MainMenuActivity));
                        StartActivity(intentMainMenu);
                    }
                    //Toast.MakeText(this, "Bottom toolbar tapped: " + e.Item.TitleFormatted, ToastLength.Short).Show();
                };


                _markdamage = FindViewById<FrameLayout>(Resource.Id.markdamage);
                // imgVehicle = FindViewById<ImageView>(Resource.Id.imgVehicle);
                //Recreate Object
                if (savedInstanceState != null)
                {
                    rentRunningTrans = JsonConvert.DeserializeObject<RentRunningTrans>(savedInstanceState.GetString("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans)));
                }
                else
                {
                    rentRunningTrans = JsonConvert.DeserializeObject<RentRunningTrans>(Intent.GetStringExtra("RentRunningTrans"));
                }
                //Load details from existing object

                if (rentRunningTrans != null)
                {
                    actionBar.Title = "Mark Damage for " + rentRunningTrans.TransType;
                    //have to code here to call vehicle image API and bind thumbnail images
                    BindVehicleImages();
                    
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void SaveAndContinue()
        {
            string _MarkDamageImagePath = String.Format("MarkDamageScreen_{0}.png", rentRunningTrans.RegNo+"_"   +iCurrentDamageImageId);
            saveMarkDamageScreen(_markdamage, Android.OS.Environment.ExternalStorageDirectory + "/RentACar" + "/" + _MarkDamageImagePath);
            rentRunningTrans.MarkDamageImagePath = _MarkDamageImagePath;
            var intentCheckList = new Intent(this, typeof(RentFlowCheckListActivity));
            intentCheckList.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
            StartActivity(intentCheckList);
        }

        protected override void OnSaveInstanceState(Bundle savedInstanceState)
        {
            //rentRunningTrans.DamageDetail = objVehicleMarkDamageDetails;
            savedInstanceState.PutString("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
            // always call the base implementation!
            base.OnSaveInstanceState(savedInstanceState);
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            _gestureDetector.OnTouchEvent(e);
            return false;
        }
        public void OnLongPress(MotionEvent e)
        {
            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetMessage("Please select valid Reg Number.");
            alert.SetNeutralButton("OK", delegate { });
            alert.Create().Show();
            // Toast.MakeText(this, "Please select valid Reg Number", ToastLength.Short).Show();
        }
        public bool OnDown(MotionEvent e)
        {
            return false;
        }

        public bool OnFling(MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
        {
            //_textView.Text = String.Format("Fling velocity: {0} x {1}", velocityX, velocityY);
            return false;
        }



        public bool OnScroll(MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
        {
            return false;
        }

        public void OnShowPress(MotionEvent e) { }

        public bool OnSingleTapUp(MotionEvent e)
        {
            return false;
        }
        public bool OnTouch(View v, MotionEvent e)
        {

            //float _viewX=0;
            // int x = (int)e.GetX();
            // int y = (int)e.GetY();
            switch (e.Action)
            {
                case MotionEventActions.Down:
                    //_viewX = e.GetX();

                    break;
                case MotionEventActions.Move:

                    //var left = (int)(e.RawX - _viewX);
                    //var right = (int)(left + v.Width);
                    //v.Layout(left, v.Top, right, v.Bottom);
                    break;
                case MotionEventActions.Up:
                    if (v.GetType() == typeof(TextView))
                    {
                        //var intent_CaptureDamageImage = new Intent(this, typeof(RentFlowCaptureDamageImageActivity));
                        //objVehicleMarkDamageDetails = rentRunningTrans.RentVehicleDamage.Where(x => x.DamageNumber == Convert.ToInt32(((TextView)v).Text)).First();
                        //intent_CaptureDamageImage.PutExtra("objMarkDamageDetail", JsonConvert.SerializeObject(objVehicleMarkDamageDetails));
                        //StartActivityForResult(intent_CaptureDamageImage, 1);
                    }
                    else
                    {
                        var intent_CaptureDamageImage = new Intent(this, typeof(RentFlowCaptureDamageImageActivity));
                        intent_CaptureDamageImage.PutExtra("VehicleTransId", rentRunningTrans.VehicleTransID.ToString());
                        intent_CaptureDamageImage.PutExtra("TapImageLocationX", ((int)e.GetX()).ToString());
                        intent_CaptureDamageImage.PutExtra("TapImageLocationY", ((int)e.GetY()).ToString());
                        intent_CaptureDamageImage.PutExtra("DamageCounter", iLastDamageNumber.ToString());
                        intent_CaptureDamageImage.PutExtra("CurrentDamageImageId", iCurrentDamageImageId.ToString());
                        StartActivityForResult(intent_CaptureDamageImage, 0);
                    }
                    break;
            }
            return true;
        }

        public bool OnDrag(View view, DragEvent e)
        {

            switch (e.Action)
            {
                case DragAction.Started:

                    return true;
                case DragAction.Entered:

                    return true;
                case DragAction.Exited:

                    return true;
                case DragAction.Drop:

                    objVehicleMarkDamageDetails = rentRunningTrans.RentVehicleDamage.Where(x => x.DamageNumber == Convert.ToInt32(e.ClipData.GetItemAt(0).Text)).First();
                    objVehicleMarkDamageDetails.DamageLocationX = (int)e.GetX();
                    objVehicleMarkDamageDetails.DamageLocationY = (int)e.GetY();
                    this.DrawDamageCircle(objVehicleMarkDamageDetails);
                    //view.Visibility = (ViewStates.Invisible);
                    //droppedIndex = Convert.ToInt16(view.GetTag(Resource.String.keyval)); ;
                    return true;
                case DragAction.Ended:

                    return true;

            }
            return false;

        }
        private void MarkDamageNumber_Click(object sender, EventArgs e)
        {
            //View view = (View)sender;
            
            var intent_CaptureDamageImage = new Intent(this, typeof(RentFlowCaptureDamageImageActivity));
            objVehicleMarkDamageDetails = rentRunningTrans.RentVehicleDamage.Where(x => x.DamageNumber == Convert.ToInt32(((TextView)sender).Text)).First();
            intent_CaptureDamageImage.PutExtra("objMarkDamageDetail", JsonConvert.SerializeObject(objVehicleMarkDamageDetails));
            // intent_CaptureDamageImage.PutExtra("VehicleTransId", rentRunningTrans.VehicleTransID.ToString());
            //intent_CaptureDamageImage.PutExtra("TapImageLocationX", ((int)e.GetX()).ToString());
            //intent_CaptureDamageImage.PutExtra("TapImageLocationY", ((int)e.GetY()).ToString());
            //intent_CaptureDamageImage.PutExtra("DamageCounter", ((TextView)sender).Text);
            StartActivityForResult(intent_CaptureDamageImage, 1);

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
                    var intent_sendsms = new Intent(this, typeof(SendSMSActivity));
                    StartActivity(intent_sendsms);
                    break;
                case Resource.Id.menu_sendemail:
                    var intent_sendemail = new Intent(this, typeof(SendEmailActivity));
                    StartActivity(intent_sendemail);
                    break;
                case Resource.Id.menu_settings:
                    var intent_settings = new Intent(this, typeof(SettingsActivity));
                    intent_settings.PutExtra("FromActivity", "MarkDamage");
                    intent_settings.PutExtra("RentRunningTrans", JsonConvert.SerializeObject(rentRunningTrans));
                    StartActivity(intent_settings);
                    break;
                case Resource.Id.menu_logout:
                    var intent_logout = new Intent(this, typeof(LoginActivity));
                    StartActivity(intent_logout);
                    break;
            }
            //Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                if (data.GetStringExtra("MarkDamage") != null)
                {
                    objVehicleMarkDamageDetails = JsonConvert.DeserializeObject<VehicleMarkDamageDetails>(data.GetStringExtra("MarkDamage"));
                    this.DrawDamageCircle(objVehicleMarkDamageDetails);
                    if (rentRunningTrans.RentVehicleDamage == null)
                    {
                        rentRunningTrans.RentVehicleDamage = new List<VehicleMarkDamageDetails>();

                    }
                    if (rentRunningTrans.RentVehicleDamage.Where(x => x.DamageNumber == objVehicleMarkDamageDetails.DamageNumber).ToList().Count > 0)
                    {
                        rentRunningTrans.RentVehicleDamage.Where(x => x.DamageNumber == objVehicleMarkDamageDetails.DamageNumber).Any(c => { c.DamageType = objVehicleMarkDamageDetails.DamageType; c.ImagePath = objVehicleMarkDamageDetails.ImagePath; return true; });
                    }
                    else
                    {
                        rentRunningTrans.RentVehicleDamage.Add(objVehicleMarkDamageDetails);
                        iLastDamageNumber = objVehicleMarkDamageDetails.DamageNumber;
                    }
                }
            }
            //for deleting mark
            if (resultCode == Result.FirstUser)
            {
                if (data.GetStringExtra("MarkDamage") != null)
                {
                    objVehicleMarkDamageDetails = JsonConvert.DeserializeObject<VehicleMarkDamageDetails>(data.GetStringExtra("MarkDamage"));
                    if (rentRunningTrans.RentVehicleDamage.Remove(objVehicleMarkDamageDetails))
                    {
                        AlertDialog.Builder alert = new AlertDialog.Builder(this);
                        alert.SetMessage("damage is deleted.");
                        alert.SetNeutralButton("OK", delegate { });
                        alert.Create().Show();
                        // Toast.MakeText(this, "damage is deleted.", ToastLength.Short).Show();
                    }
                }
            }
        }

        //This function should be async and should be called asyncronously
        private void DrawDamageCircle(VehicleMarkDamageDetails objVehicleMarkDamage)
        {
            try
            {
                TextView lblMarkDamageNumber = new TextView(this);
                lblMarkDamageNumber.Id = objVehicleMarkDamage.DamageNumber;
                lblMarkDamageNumber.Text = objVehicleMarkDamage.DamageNumber.ToString();
                lblMarkDamageNumber.SetTextColor(Color.White);
                lblMarkDamageNumber.SetWidth(60);
                lblMarkDamageNumber.SetHeight(60);
                lblMarkDamageNumber.Gravity = GravityFlags.Center;
                if (rentRunningTrans.TransType.Equals("PRE"))
                {

                    lblMarkDamageNumber.SetBackgroundResource(Resource.Drawable.rounded_corner_black);
                }
                else if (rentRunningTrans.TransType.Equals("OUT") )
                {
                    lblMarkDamageNumber.SetTextColor(Color.White);
                    if (objVehicleMarkDamage.ID > 0)
                    {
                        lblMarkDamageNumber.SetBackgroundResource(Resource.Drawable.rounded_corner_black);
                    }
                    else
                    {
                        lblMarkDamageNumber.SetBackgroundResource(Resource.Drawable.rounded_corner_green);
                    }
                    
                }
                else if (rentRunningTrans.TransType.Equals("IN"))
                {
                    lblMarkDamageNumber.SetTextColor(Color.White);
                    if (objVehicleMarkDamage.ID > 0)
                    {
                        lblMarkDamageNumber.SetBackgroundResource(Resource.Drawable.rounded_corner_black);
                    }
                    else
                    {
                        lblMarkDamageNumber.SetBackgroundResource(Resource.Drawable.rounded_corner_red);
                    }                    
                }
               
                
                lblMarkDamageNumber.Click += MarkDamageNumber_Click;
                //lblMarkDamageNumber.SetOnTouchListener(this);
                lblMarkDamageNumber.SetOnLongClickListener(this);


                FrameLayout.LayoutParams layoutParams = new FrameLayout.LayoutParams(
                                                           FrameLayout.LayoutParams.WrapContent,
                                                           FrameLayout.LayoutParams.WrapContent);
                layoutParams.TopMargin = objVehicleMarkDamage.DamageLocationY - 30;// margin in pixels, not dps
                layoutParams.LeftMargin = objVehicleMarkDamage.DamageLocationX - 30; // margin in pixels, not dps                                                                                 

                // add into my parent view
                _markdamage.AddView(lblMarkDamageNumber, layoutParams);

            }
            catch (Exception ex)
            {
                //Do nothing
            }
        }

        //This function save Mark Damage Vehile screen as an image
        private void saveMarkDamageScreen(FrameLayout fmarkDamage, String path)
        {
            fmarkDamage.DrawingCacheEnabled = true;
            fmarkDamage.BuildDrawingCache();
            Bitmap cache = fmarkDamage.GetDrawingCache(true);

            try
            {
                //imgVehicle.SetImageBitmap(cache);

                //ByteArrayOutputStream bytes = new ByteArrayOutputStream();
                ////FileOutputStream fileOutputStream = new FileOutputStream(path);
                var stream = new System.IO.FileStream(path, System.IO.FileMode.Create);
                cache.Compress(Bitmap.CompressFormat.Png, 100, stream);
                stream.Flush();
                stream.Close();
            }
            catch (Exception ex)
            {
                // TODO: handle exception
            }
            finally
            {
                fmarkDamage.DestroyDrawingCache();
            }
        }

        //to be pressed damage marks and drag it
        public bool OnLongClick(View v)
        {
            if (v.GetType() == typeof(TextView))
            {
                var data = ClipData.NewPlainText("damagenumber", ((TextView)v).Text);
                //v.StartDrag(data, new View.DragShadowBuilder(v), null, 0);
                v.StartDrag(data, new DamageMarkShadowBuilder(v), null, 0);
                v.Visibility = (ViewStates.Invisible);
                return true;
            }
            return false;
        }

        //need to make this call async to make responsive screen
        private async void BindVehicleImages()
        {
            if (CommonFunctions.IsNetworkConnected())
            {
                Java.IO.File _dir;
                //Environment.DirectoryPictures = "Images";
                _dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/RentACar" + "/" + rentRunningTrans.RegNo);

                //km12akk
                // Get the latitude and longitude entered by the user and create a query.km12akk";//
                //string url = "https://uk1.ukvehicledata.co.uk/api/datapackage/VehicleImageData?v=2&api_nullitems=1&auth_apikey=a418b0ad-33ab-4953-9a00-9d3ea8a12319&key_VRM=" + rentRunningTrans.RegNo;

                // Fetch the vehicle information asynchronously, 
                // parse the results, then update the screen:
                // objVehicleImageList = await GetVehicleImagesfromAPIAsync(url);
                // _markdamage.SetBackgroundResource(Resource.Drawable.UK1);
                // ParseAndDisplay (json);
                //this.progressLayout.Visibility = ViewStates.Visible;
                
                _layoutVehicleImages = FindViewById<LinearLayout>(Resource.Id.layoutVehicleImages);
                //if (objVehicleImageList != null)
                {                    
                    if (_dir.Exists() && _dir.ListFiles().Count()>0)
                    {
                        Java.IO.File[] _objVehicleImageList = _dir.ListFiles();
                        //Needed to uncommment
                        for (int i = 0; i < _objVehicleImageList.Count(); i++)
                        //for (int i = 0; i < 6; i++)
                        {
                            ImageView imgVehicle = new ImageView(this);
                            imgVehicle.SetMaxWidth(60);
                            imgVehicle.SetMaxHeight(60);
                            imgVehicle.LayoutParameters = new LinearLayout.LayoutParams(150, 150);
                            imgVehicle.Visibility = ViewStates.Visible;
                            imgVehicle.Id = i + 1;
                            //var imageBitmap =CommonFunctions.GetBitmapFromUrl(objVehicleImageList.Response.DataItems.VehicleImages.ImageDetailsList[i].ImageUrl);
                           // imgVehicle.SetImageBitmap(imageBitmap);
                            imgVehicle.SetImageURI(Android.Net.Uri.Parse(_objVehicleImageList[i].AbsolutePath));
                            //imgVehicle.SetBackgroundResource(Resource.Drawable.CarExterior);
                            imgVehicle.SetOnClickListener(this);
                            // add into my parent view
                            _layoutVehicleImages.AddView(imgVehicle);
                        }
                        //objVehicleImageList["DataItems"]["VehicleImages"]["ImageDetailsList"]
                    }
                    else
                    {
                        try
                        {
                            ImageView imgVehicle = new ImageView(this);
                            imgVehicle.SetMaxWidth(60);
                            imgVehicle.SetMaxHeight(60);
                            LinearLayout.LayoutParams obj = new LinearLayout.LayoutParams(150, 150);
                            obj.SetMargins(5, 5, 5, 5);
                            //imgVehicle.LayoutParameters  = new LinearLayout.LayoutParams(150, 150);
                            imgVehicle.Visibility = ViewStates.Visible;
                            // LinearLayout.LayoutParams obj = new LinearLayout.LayoutParams(100, 100);
                            //obj.SetMargins(5,5,5,5);
                            imgVehicle.LayoutParameters = obj;

                            if (rentRunningTrans.VehicleType == "Car")
                            {
                                imgVehicle.Id = 101;
                                imgVehicle.SetBackgroundResource(Resource.Drawable.CarExterior);
                               
                                imgVehicle.SetOnClickListener(this);
                                //imgVehicle.SetPadding(10, 10, 10, 10);

                                // add into my parent view
                                _layoutVehicleImages.AddView(imgVehicle);

                                ImageView imgVehicleInterior = new ImageView(this);
                                imgVehicleInterior.SetMaxWidth(60);
                                imgVehicleInterior.SetMaxHeight(60);
                                //LinearLayout.LayoutParams objj = new LinearLayout.LayoutParams(150, 150);
                                //obj.SetMargins(5, 5, 5, 5);
                                imgVehicleInterior.LayoutParameters = obj;
                                // imgVehicleInterior.LayoutParameters = new LinearLayout.LayoutParams(150, 150);
                                imgVehicleInterior.Visibility = ViewStates.Visible;
                                imgVehicleInterior.Id = 102;
                                imgVehicleInterior.SetBackgroundResource(Resource.Drawable.VehicleInteriorImg);
                                imgVehicleInterior.SetOnClickListener(this);
                                //imgVehicleInterior.SetPadding(10, 10, 10, 10);
                                _layoutVehicleImages.AddView(imgVehicleInterior);
                                _markdamage.SetBackgroundResource(Resource.Drawable.CarExterior);
                                iCurrentDamageImageId = 101;
                            }
                            else if (rentRunningTrans.VehicleType == "Luton Van")
                            {
                                imgVehicle.Id = 201;
                                imgVehicle.SetBackgroundResource(Resource.Drawable.LutonVan);
                                imgVehicle.SetOnClickListener(this);

                                // add into my parent view
                                _layoutVehicleImages.AddView(imgVehicle);
                                _markdamage.SetBackgroundResource(Resource.Drawable.LutonVan);
                                iCurrentDamageImageId = 201;
                            }
                            else if (rentRunningTrans.VehicleType == "Standard Van")
                            {
                                imgVehicle.Id = 301;
                                imgVehicle.SetBackgroundResource(Resource.Drawable.StandardVan);
                                imgVehicle.SetOnClickListener(this);
                                // add into my parent view
                                _layoutVehicleImages.AddView(imgVehicle);
                                _markdamage.SetBackgroundResource(Resource.Drawable.StandardVan);
                                iCurrentDamageImageId = 301;
                            }

                            _markdamage.RemoveAllViews();
                            _markdamage.SetOnTouchListener(this);
                            _markdamage.SetOnDragListener(this);                            

                            if (rentRunningTrans.RentVehicleDamage != null)
                            {
                                foreach (var objVehicleMarkDamage in rentRunningTrans.RentVehicleDamage.Where(x => x.DamageImageId == iCurrentDamageImageId).OrderBy(x => x.DamageNumber))
                                {
                                    this.DrawDamageCircle(objVehicleMarkDamage);
                                    iLastDamageNumber = objVehicleMarkDamage.DamageNumber;
                                }
                            }
                               
                        }
                        catch (Exception ex)
                        {
                            //Do nothing
                        }
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
        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case 101:
                    //iCurrentDamageImageId = v.Id;
                    _markdamage.SetBackgroundResource(Resource.Drawable.CarExterior);

                    break;
                case 102:
                    _markdamage.SetBackgroundResource(Resource.Drawable.VehicleInteriorImg);

                    break;

                case 201:
                    _markdamage.SetBackgroundResource(Resource.Drawable.LutonVan);

                    break;

                case 301:
                    _markdamage.SetBackgroundResource(Resource.Drawable.StandardVan);

                    break;
                //default:
                //    _markdamage.Set(Resource.Drawable.UK1);
                //    break;
                case 1:
                    _markdamage.SetBackgroundResource(Resource.Drawable.UK1);

                    break;
                case 2:
                    _markdamage.SetBackgroundResource(Resource.Drawable.UK2);

                    break;
                case 3:
                    _markdamage.SetBackgroundResource(Resource.Drawable.UK3);

                    break;
                case 4:
                    _markdamage.SetBackgroundResource(Resource.Drawable.UK4);

                    break;
                case 5:
                    _markdamage.SetBackgroundResource(Resource.Drawable.UK5);

                    break;
                case 6:
                    _markdamage.SetBackgroundResource(Resource.Drawable.UK6);

                    break;
            }
            _markdamage.RemoveAllViews();
            _markdamage.SetOnTouchListener(this);
            _markdamage.SetOnDragListener(this);
            iCurrentDamageImageId = v.Id;

            if (rentRunningTrans.RentVehicleDamage != null)
            {
                foreach (var objVehicleMarkDamage in rentRunningTrans.RentVehicleDamage.Where(x => x.DamageImageId == v.Id).OrderBy(x => x.DamageNumber))
                {
                    this.DrawDamageCircle(objVehicleMarkDamage);
                    iLastDamageNumber = objVehicleMarkDamage.DamageNumber;
                }
            }
        }
    }

    class DamageMarkShadowBuilder : View.DragShadowBuilder
    {
        const int centerOffset = 52;
        int width, height;

        public DamageMarkShadowBuilder(View baseView) : base(baseView)
        {
        }

        public override void OnProvideShadowMetrics(Point shadowSize, Point shadowTouchPoint)
        {
            width = View.Width;
            height = View.Height;

            // This is the overall dimension of your drag shadow
            shadowSize.Set(width * 2, height * 2);
            // This one tells the system how to translate your shadow on the screen so
            // that the user fingertip is situated on that point of your canvas.
            // In my case, the touch point is in the middle of the (height, width) top-right rect
            //shadowTouchPoint.Set(width + width / 2 - centerOffset, height / 2 + centerOffset);
            shadowTouchPoint.Set(width , height );
        }

        public override void OnDrawShadow(Canvas canvas)
        {
            //const float sepAngle = (float)Math.PI / 16;
            float circleRadius = 120f;

            // Draw the shadow circles in the top-right corner
            float centerX = width + width / 2;// - centerOffset;
            float centerY = height / 2;// + centerOffset;

            var baseColor = Color.LightGreen;
            var paint = new Paint()
            {
                AntiAlias = true,
                Color = baseColor
            };

            // draw a dot where the center of the touch point (i.e. your fingertip) is
            canvas.DrawCircle(centerX, centerY, circleRadius, paint);
            ////for (int radOffset = 70; centerX + radOffset < canvas.Width; radOffset += 20)
            ////{
            ////    // Vary the alpha channel based on how far the dot is
            ////    baseColor.A = (byte)(128 * (2f * (width / 2f - 1.3f * radOffset + 60) / width) + 100);
            ////    paint.Color = baseColor;
            ////    // Draw the dots along a circle of radius radOffset and centered on centerX,centerY
            ////    for (float angle = 0; angle < Math.PI * 2; angle += sepAngle)
            ////    {
            ////        var pointX = centerX + (float)Math.Cos(angle) * radOffset;
            ////        var pointY = centerY + (float)Math.Sin(angle) * radOffset;
            ////        canvas.DrawCircle(pointX, pointY, circleRadius, paint);
            ////    }
            ////}

            ////View.DrawingCacheEnabled = true;
            // Draw the dragged view in the bottom-left corner
            ////canvas.DrawBitmap(View.DrawingCache, 0, height, null);
        }
    }
}