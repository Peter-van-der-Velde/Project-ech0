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

namespace UX_OVERDIVE
{
    [Activity(Label = "Settings")]
    public class Settings : Activity
    {
        private string IPADDRESS;
        private string PORT;

        private EditText editTextIP;
        private EditText editTextPORT;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Settings);

            editTextIP   = FindViewById<EditText>(Resource.Id.editTextIPADDRESS);
            editTextPORT = FindViewById<EditText>(Resource.Id.editTextPORT);

            //
            ISharedPreferences pref = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
            string wrtIP = pref.GetString("IP", "192.168.100");
            string wrtPORT = pref.GetString("PORT", "53");

            editTextIP.Text = wrtIP;
            editTextPORT.Text = wrtPORT;



        }
    }
}