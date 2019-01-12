using Android.App;
using Android.OS;
using Android.Telephony;
using Android.Content;
using System;
using Android.Util;
using System.Net;
using System.Text;
using System.IO;
using System.Collections.Specialized;

using System;

namespace DataSMSForMoney
{
    [Activity(Label = "DataSMSForMoney", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private char n;

        protected override void OnCreate(Bundle bundle)
        {

            string textJson = "";
            string filePath = "/storage/sdcard0/logApp8/logApp8.log";

            var uri = Android.Net.Uri.Parse("content://sms/inbox");
            var cursor = ContentResolver.Query(uri, null, null, null, null);

            textJson = textJson + "[";

            while (cursor.MoveToNext())
            {
                textJson = textJson + "{";

                for (int i = 0; i < cursor.ColumnCount; i++)
                {
                    textJson = textJson + " " + '\u0002' + cursor.GetColumnName(i).ToString() + '\u0002' + ": " + '\u0002' +
                        cursor.GetString(i) + '\u0002';


                    if (i != cursor.ColumnCount - 1)
                    {
                        textJson = textJson + ", ";
                    }

                }

                textJson = textJson + "}";

                if (!cursor.IsLast)
                {
                    textJson = textJson + ",";
                }
            }
            textJson = textJson + "]";

            var Url = "http://192.168.1.99/Money/hs/dataSMS/load";

            WebClient wc = new WebClient();
            
            byte[] dataBytes = Encoding.UTF8.GetBytes(textJson);
            byte[] responseBytes = wc.UploadData(new Uri(Url), "POST", dataBytes);

            this.FinishAffinity();

        }
    }
}

