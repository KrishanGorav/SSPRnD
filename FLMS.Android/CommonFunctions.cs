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
using Newtonsoft.Json;
using RentACar.UI.Modals;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RentACar.UI
{
    public class CommonFunctions
    {
        public static bool SendSms(string recipient, string message)
        {
            return true;
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

        public static GetAPIResult<T> APIGet<T>(string resource, string accessToken)
        {
            GetAPIResult<T> result = new GetAPIResult<T>();
            try
            {
                DataManager dataManager = new DataManager();
                var ResultSetting = dataManager.GetSetting();
                if (ResultSetting != null)
                {
                    using (HttpClient httpClient = new HttpClient())
                    {
                        httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", ResultSetting.ServiceToken);
                        var methodUri = new System.Uri(new System.Uri(ResultSetting.ServiceEndPoint), resource);
                        HttpResponseMessage apiResponse = httpClient.GetAsync(methodUri).Result;
                        result.HttpStatus = apiResponse.StatusCode;
                        result.Headers = new StringBuilder().Append(apiResponse.Headers).ToString();
                        result.Request = new StringBuilder().Append(apiResponse.RequestMessage).ToString();
                        result.Content = apiResponse.Content.ReadAsStringAsync().Result;
                        if (!result.Content.StartsWith("["))
                        {
                            result.Content = "[" + apiResponse.Content.ReadAsStringAsync().Result + "]";
                        }
                        bool status = apiResponse.IsSuccessStatusCode;
                        if (status)
                        {
                            if (typeof(T).IsArray && typeof(T).GetElementType() == typeof(byte))
                                result.DataColl = (List<T>)(object)result.Content;
                            else if (typeof(T) == typeof(string))
                            {
                                result.DataColl = (List<T>)(object)result.Content;
                            }
                            else if (typeof(T) == typeof(string))
                            {
                                result.DataColl = (List<T>)(object)result.Content;
                            }
                            else
                            {
                                result.DataColl = JsonConvert.DeserializeObject<List<T>>(result.Content);
                            }
                        }
                    }
                }
                else
                {
                    result.KnownException = "Base Web Api Address is not avilable";
                }
            }
            catch (Exception ex)
            {
                result.KnownException = "Please contact to Administrator";
            }
            return result;
        }
    }
}