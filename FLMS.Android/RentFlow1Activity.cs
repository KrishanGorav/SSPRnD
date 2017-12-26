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
using Xamarin.Controls;
using Android.Graphics;

namespace RentACar.UI
{
    [Activity(Label = "RentFlowActivity")]
    public class RentFlowActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.RentFlow1);
            var signature = FindViewById<SignaturePadView>(Resource.Id.signatureView);
            signature.Caption.Text = "Authorization Signature";
            signature.Caption.SetTypeface(Typeface.Serif, TypefaceStyle.BoldItalic);
            signature.Caption.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 16f);
            signature.SignaturePrompt.Text = ">>";
            signature.SignaturePrompt.SetTypeface(Typeface.SansSerif, TypefaceStyle.Normal);
            signature.SignaturePrompt.SetTextSize(global::Android.Util.ComplexUnitType.Sp, 32f);
            signature.BackgroundColor = Color.Rgb(0, 0, 0); // a light yellow.
            signature.StrokeColor = Color.White;

            signature.BackgroundImageView.SetImageResource(Resource.Drawable.CarExterior);
            //signature.BackgroundImageView.SetAlpha(16);
            signature.BackgroundImageView.SetAdjustViewBounds(true);

            var layout = new RelativeLayout.LayoutParams(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent);
            layout.AddRule(LayoutRules.CenterInParent);
            layout.SetMargins(20, 20, 20, 20);
            signature.BackgroundImageView.LayoutParameters = layout;
        }
    }
}