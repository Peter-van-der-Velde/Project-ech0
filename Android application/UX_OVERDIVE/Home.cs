using System;
using Android.App;
using Android.Content;
using Android.OS;
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

            timerTemp = new System.Timers.Timer() { Interval = 2000, Enabled = true };
            timerTemp.Elapsed += (obj, args) =>
            {
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