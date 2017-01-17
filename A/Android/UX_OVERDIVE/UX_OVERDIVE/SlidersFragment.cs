using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Timers;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Graphics;

namespace UX_OVERDIVE
{

    /// <summary>
    /// These classes are used for the various views between the tabs.
    /// </summary>
    class Sliders : Fragment
    {

        //here go possible variables
        private Switch switchArduinoConnect;
        private Switch switchDevice1;
        private Switch switchDevice2;
        private Switch switchDevice3;
        private Switch switchAllDevices;

        private TextView textViewConnectArduino;
        private TextView textViewDevice1;
        private TextView textViewDevice2;
        private TextView textViewDevice3;
        private TextView textViewAllDevices;

        private ImageButton settingButton;

        private string IPADDRESS;
        private string PORT;

        //here goes the code for each "Tab"
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            //Select the View you want to use for this tab
            var view = inflater.Inflate(Resource.Layout.Sliders, container, false);

            switchDevice1 = view.FindViewById<Switch>(Resource.Id.switch_dv1);
            switchArduinoConnect = view.FindViewById<Switch>(Resource.Id.switch_Connect);

            textViewDevice1 = view.FindViewById<TextView>(Resource.Id.text_dv1);
            textViewConnectArduino = view.FindViewById<TextView>(Resource.Id.textConnectToArduino);

            settingButton = view.FindViewById<ImageButton>(Resource.Id.set_B);


            //loading settings/PREFERENCES!!!
            ISharedPreferences pref = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
            IPADDRESS = pref.GetString("IP", "192.168.100");
            PORT = pref.GetString("PORT", "53");


            settingButton.Click += settingButton_Click;
            //code
            switchDevice1.CheckedChange += (obj, args) =>
            {
                //DOES NOTHING

                //if (switchDevice1.Checked)
                //    textViewDevice1.Text = IPADDRESS;
                //if (!switchDevice1.Checked)
                //    textViewDevice1.Text = PORT;
            };

            return view;
        }


        private void settingButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(Settings));
            this.StartActivity(intent);
        }
    }
}