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
        private MainActivity mainActivity;

        private Button buttonConnect;
        private ImageButton settingButton;
        public TextView textViewTempValue, textViewHumiValue;
        Timer timerTemp;
        public string temp;
        public string humi;

        public Home(MainActivity activity)
        {
            mainActivity = activity;
            mainActivity.temperature = this;
            mainActivity.humidity = this;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Temprature, container, false);

            buttonConnect = view.FindViewById<Button>(Resource.Id.button1);
            settingButton = view.FindViewById<ImageButton>(Resource.Id.settingbutton2);

            textViewTempValue = view.FindViewById<TextView>(Resource.Id.textViewTempValue);
            textViewHumiValue = view.FindViewById<TextView>(Resource.Id.textViewHumiValue);

            //sets the text of the button
            //.Text = "You clicked the button " + 0.ToString() + " times.";


            //clicker.Click += clicker_Click;
            //settingButton.Click += settingButton_Click;

            timerTemp = new System.Timers.Timer() { Interval = 2000, Enabled = true };
            timerTemp.Elapsed += (obj, args) =>
            {
            //mainActivity.connector.SendMessage("a");
            //mainActivity.connector.SendMessage("b");
                textViewTempValue.Text = temp;
                textViewHumiValue.Text = humi;
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