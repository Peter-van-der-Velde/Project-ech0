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
using System.Timers;

namespace UX_OVERDIVE
{
    public class Home : Fragment
    {
        //Home is were Joey works
        private ImageButton settingButton;
        public TextView textViewTempValue, textViewHumiValue, textViewDev1Off;
        Timer  timerSockets;
        private MainActivity mainActivity;

        public Home(MainActivity activity)
        {
           mainActivity = activity;
           mainActivity.temperature = this;
           mainActivity.humidity = this;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.Temperature, container, false);
            settingButton = view.FindViewById<ImageButton>(Resource.Id.set_B2Temp);

            //sets the text of the button
            settingButton.Click += settingButton_Click;


            //Temperature and Humidity id
            textViewTempValue = view.FindViewById<TextView>(Resource.Id.textViewTempValue);
            textViewHumiValue = view.FindViewById<TextView>(Resource.Id.textViewHumiValue);
            textViewDev1Off = view.FindViewById<TextView>(Resource.Id.textViewDev1Off);
            int Dev1Temp = Convert.ToInt32(textViewDev1Off);
            int Temperature1 = Convert.ToInt32(textViewTempValue);

            //Temp & Humi case "a" and "b" from arduino
            timerSockets = new System.Timers.Timer() { Interval = 2000, Enabled = true }; // Interval >= 750
            timerSockets.Elapsed += (obj, args) =>
            {
                mainActivity.connector.SendMessage("a");
                mainActivity.connector.SendMessage("b");
            };

            if (Temperature1 > Dev1Temp)
            {
                mainActivity.connector.SendMessage("c");
            }

            return view;
        }



        private void settingButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(Settings));
            this.StartActivity(intent);
        }
    }
}