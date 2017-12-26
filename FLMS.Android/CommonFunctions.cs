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
using Android.Provider;
using Java.IO;
using System.Net;
using System.Xml.Linq;
using Android.Net;


namespace RentACar.UI
{
    public class CommonFunctions
    {
        public static bool SendSms(string recipient, string message)
        {
            string url = "https://sgw01.cm.nl/gateway.ashx";
            Guid productToken = new Guid("888b3ea9-ae67-41fb-a4d7-f74c58272970");
            String sSender = "988377322";
            string sSmsXml = CreateSMSXml(productToken, sSender, recipient, message);
            string sResponse = doHttpPost(url, sSmsXml);
            if (sResponse.StartsWith(""))
                return true;
            else
                return false;
        }

        public static void CreateDirectoryForApp()
        {
            try
            {
                Java.IO.File _dir;
                //Environment.DirectoryPictures = "Images";
                _dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/RentACar");
                if (!_dir.Exists())
                {
                    _dir.Mkdirs();
                }
            }
            catch (Exception ex)
            { }
        }

        public static bool IsNetworkConnected()
        {
            var connectivityManager = (ConnectivityManager)Application.Context.GetSystemService(Context.ConnectivityService);
            var activeNetworkInfo = connectivityManager.ActiveNetworkInfo;
            if (activeNetworkInfo != null && activeNetworkInfo.IsConnectedOrConnecting)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static void CreateDirectoryForRegNo(string strRegNo)
        {
            try
            {
                Java.IO.File _dir;
                //Environment.DirectoryPictures = "Images";
                _dir = new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/RentACar" + "/" + strRegNo);
                if (!_dir.Exists())
                {
                    _dir.Mkdirs();
                }
            }
            catch (Exception ex)
            { }
        }

        public static Android.Graphics.Bitmap GetBitmapFromUrl(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] bytes = webClient.DownloadData(url);
                //Drawable d = Drawable.CreateFromStream(bytes, "123");
                if (bytes != null && bytes.Length > 0)
                {
                    return Android.Graphics.BitmapFactory.DecodeByteArray(bytes, 0, bytes.Length);
                }
            }
            return null;
        }

        private static string CreateSMSXml(Guid productToken, string sender, string recipient, string message)
        {
            return
                    new XElement("MESSAGES",
                        new XElement("AUTHENTICATION",
                            new XElement("PRODUCTTOKEN", productToken)
                    ),
                    new XElement("MSG",
                        new XElement("FROM", sender),
                        new XElement("TO", recipient),
                        new XElement("BODY", message)
                    )
                ).ToString();
        }

        private static string doHttpPost(string apiUrl, string requestString)
        {
            try
            {
                var webClient = new WebClient();
                webClient.Encoding = Encoding.UTF8;
                return webClient.UploadString(apiUrl, requestString);
            }
            catch (WebException wex)
            {
                return string.Format("{0} - {1}", wex.Status, wex.Message);
            }
        }
     
    }
}