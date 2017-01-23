using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Speech;
using Android.Util;
using Android.Views;
using Android.Widget;


namespace UX_OVERDIVE
{
    public class SpeechFragment : Fragment
    {
        public MainActivity mainActivity;

        private bool isRecording;
        private readonly int VOICE = 10;
        private TextView textBox;
        private Button recButton;
        private Button updateButton;

        public SpeechFragment(MainActivity activity)
        {
            mainActivity = activity;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Speech, container, false);

            // get the resources from the layout
            recButton = view.FindViewById<Button>(Resource.Id.btnRecord);
            textBox = view.FindViewById<TextView>(Resource.Id.textYourText);
            updateButton = view.FindViewById<Button>(Resource.Id.buttonUpdate);

            // check to see if we can actually record - if we can, assign the event to the button
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                // no microphone, no recording. Disable the button and output an alert
                var alert = new AlertDialog.Builder(recButton.Context);
                alert.SetTitle("You don't seem to have a microphone to record with");
                alert.SetPositiveButton("OK", (sender, e) =>
                {
                    textBox.Text = "No microphone present";
                    recButton.Enabled = false;
                    return;
                });

                alert.Show();
            }
            else
                recButton.Click += xxXClicKXxx;
                    
                
            updateButton.Click += (obj, args) => { textBox.Text = mainActivity.textSpeechInput; }; 

            return view;
        }

        private void xxXClicKXxx(object sender, EventArgs e)
        {
            mainActivity.xxXTouwSlayerXxx();
        }
    }
}