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
        private Button recButton;
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
            recButton = view.FindViewById<Button>(Resource.Id.btnRecord);
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
                recButton.Click += xxXClicKXxx;

                
            }

            settingButton.Click += settingButton_Click;

            return view;
        }

        private void xxXClicKXxx(object sender, EventArgs e)
        {
            mainActivity.xxXTouwSlayerXxx();
        }

        private void settingButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(Settings));
            this.StartActivity(intent);
        }

        private void noMicError()
        {
            var alert = new AlertDialog.Builder(recButton.Context);
            alert.SetTitle("You don't seem to have a microphone to record with");
            alert.SetPositiveButton("OK", (sender, e) =>
            {
                recButton.Enabled = false;
                return;
            });

            alert.Show();
        }

        private void OVERDRIVE()
        {
            bool boolOne = false;
            bool boolTwo = false;
            bool boolThree = false;
            bool boolAll = false;
            bool boolOn = false;
            bool boolOff = false;
            bool boolConnect = false;

            speechCompleteString = mainActivity.textSpeechInput;
            string[] words = speechCompleteString.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = words[i].Trim().ToLower();
                switch (words[i])
                {
                    case "one":
                        boolOne = true;
                        break;
                    case "two":
                        boolTwo = true;
                        break;
                    case "three":
                        boolThree = true;
                        break;
                    case "all":
                        boolAll = true;
                        break;
                    case "on":
                        boolOn = true;
                        break;
                    case "off":
                        boolOff = true;
                        break;
                    case "connect":
                        boolConnect = true;
                        break;
                }
            }

            if (boolOn)
            {
                Console.WriteLine("BoolON is true");
                if (boolConnect)
                {
                    ISharedPreferences pref = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
                    string IPADDRESS = pref.GetString("IP", "192.168.1.102");
                    string PORT = pref.GetString("PORT", "53");

                    mainActivity.SwitchConnect(IPADDRESS, PORT);
                }
                if (boolOne) mainActivity.SwitchDevice(1);
                if (boolTwo) mainActivity.SwitchDevice(2);
                if (boolThree) mainActivity.SwitchDevice(3);
                if (boolAll) mainActivity.SwitchDevice(4);


            }
            else if (boolOff)
            {
                Console.WriteLine("BoolOff is true");
                if (boolOne) mainActivity.SwitchDevice(1);
                if (boolTwo) mainActivity.SwitchDevice(2);
                if (boolThree) mainActivity.SwitchDevice(3);
                if (boolAll) mainActivity.SwitchDevice(4);
            }

        }
    }
}