using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
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
        private string speechCompleteString = null;

        private Button settinButton;
        private ImageButton rektButton;
        private ImageButton settingButton;

        public SpeechFragment(MainActivity activity)
        {
            mainActivity = activity;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Speech, container, false);

            // get the resources from the layout
            rektButton = view.FindViewById<ImageButton>(Resource.Id.btnRecord);
            settingButton = view.FindViewById<ImageButton>(Resource.Id.set_Bspeech);
            
            // check to see if we can actually record - if we can, assign the event to the button
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                // no microphone, no recording. Disable the button and output an alert
                noMicError();
            }
            else
            {
                rektButton.Click += xxXClicKXxx;
                
            }

            settingButton.Click += settingButton_Click;

            return view;
        }

        private void xxXClicKXxx(object sender, EventArgs e)
        {
            mainActivity.xxXTouwSlayerXxx();
            //OVERDRIVE();
        }

        private void settingButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(Settings));
            this.StartActivity(intent);
        }

        private void noMicError()
        {
            var alert = new AlertDialog.Builder(rektButton.Context);
            alert.SetTitle("You don't seem to have a microphone to record with");
            alert.SetPositiveButton("OK", (sender, e) =>
            {
                rektButton.Enabled = false;
                return;
            });

            alert.Show();
        }

        
    }
}