using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace UX_OVERDIVE
{

    /// <summary>
    /// These classes are used for the various views between the tabs.
    /// </summary>
    public class Sliders : Fragment
    {
        public MainActivity mainActivity;

        //here go possible variables
        public Switch switchArduinoConnect;
        public Switch switchDevice1;
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

        public Sliders(MainActivity activity)
        {
            mainActivity = activity;
        }

        //here goes the code for each "Tab"
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            //Select the View you want to use for this tab
            var view = inflater.Inflate(Resource.Layout.Sliders, container, false);

            switchDevice1 = view.FindViewById<Switch>(Resource.Id.switch_dv1);
            switchDevice2 = view.FindViewById<Switch>(Resource.Id.switch_dv2);
            switchDevice3 = view.FindViewById<Switch>(Resource.Id.switch_dv3);
            switchAllDevices = view.FindViewById<Switch>(Resource.Id.switch_alldv);
            switchArduinoConnect = view.FindViewById<Switch>(Resource.Id.switch_Connect);

            textViewDevice1 = view.FindViewById<TextView>(Resource.Id.text_dv1);
            textViewConnectArduino = view.FindViewById<TextView>(Resource.Id.textConnectToArduino);

            settingButton = view.FindViewById<ImageButton>(Resource.Id.set_B);

            settingButton.Click += settingButton_Click;
            //code

            switchArduinoConnect.CheckedChange += (obj, args) =>
            {
                ISharedPreferences pref = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
                IPADDRESS = pref.GetString("IP", "192.168.1.102");
                PORT = pref.GetString("PORT", "53");

                if (switchArduinoConnect != null)
                    mainActivity.SwitchConnect(IPADDRESS, PORT);
            };

            switchDevice1.CheckedChange += (obj, args) =>
            {
                mainActivity.SwitchDevice(1);
            };

            switchDevice2.CheckedChange += (obj, args) =>
            {
                mainActivity.SwitchDevice(2);
            };
            switchDevice3.CheckedChange += (obj, args) =>
            {
                mainActivity.SwitchDevice(3);
            };
            switchAllDevices.CheckedChange += (obj, args) =>
            {
                mainActivity.SwitchDevice(4);
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