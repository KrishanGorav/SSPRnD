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
using Xamarin.Controls;

namespace RentACar.UI
{
    [Activity(Label = "Customer Signature")]
    public class RentFlowCustomerSignatureActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            ActionBar actionBar = ((Activity)this).ActionBar;
            actionBar.SetDisplayShowHomeEnabled(true);
            actionBar.SetLogo(Resource.Drawable.ic_launcher);
            actionBar.SetDisplayUseLogoEnabled(true);
            // Create your application here
            SetContentView(Resource.Layout.RentFlowCustomerSignature);
            var signatureCustomer = FindViewById<SignaturePadView>(Resource.Id.signatureCustomer);
            signatureCustomer.Caption.Text = "Sign in";
            signatureCustomer.Caption.SetTextColor(Color.Rgb(0, 0, 0));
            signatureCustomer.Caption.SetTypeface(Typeface.Serif, TypefaceStyle.BoldItalic);
            signatureCustomer.Caption.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 16f);
            signatureCustomer.SignaturePrompt.Text = "";
            signatureCustomer.BackgroundColor = Color.Rgb(255, 255, 255); // a light yellow.
            signatureCustomer.StrokeColor = Color.Black;
            signatureCustomer.StrokeWidth = 4;
            signatureCustomer.ClearLabel.Text = "Refresh";
            signatureCustomer.ClearLabel.SetTextColor(Color.Rgb(0, 0, 0));
        }
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.dialog_complete_menu, menu);
            return base.OnCreateOptionsMenu(menu);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Resource.Id.menu_done:
                    var intent_sendsms = new Intent(this, typeof(RentFlowSignatureActivity));
                    StartActivity(intent_sendsms);
                    break;

            }
            //Toast.MakeText(this, "Top ActionBar pressed: " + item.TitleFormatted, ToastLength.Short).Show();
            return base.OnOptionsItemSelected(item);
        }
    }
}