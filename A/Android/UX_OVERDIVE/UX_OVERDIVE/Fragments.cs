using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace UX_OVERDIVE
{

    /// <summary>
    /// These classes are used for the various views between the tabs.
    /// </summary>
    class Sliders : Fragment
    {
        //here go possible variables
        private Switch switchDevice1;
        private Switch switchDevice2;
        private Switch switchDevice3;
        private Switch switchAllDevices;

        private TextView textViewDevice1;
        private TextView textViewDevice2;
        private TextView textViewDevice3;
        private TextView textViewAllDevices;

        //here goes the code for each "Tab"
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            //Select the View you want to use for this tab
            var view = inflater.Inflate(Resource.Layout.Sliders, container, false);

            switchDevice1 = view.FindViewById<Switch>(Resource.Id.switch_dv1);
            textViewDevice1 = view.FindViewById<TextView>(Resource.Id.text_dv1);

            
            //code
            switchDevice1.CheckedChange += (obj, args) =>
            {
                if (switchDevice1.Checked)
                    textViewDevice1.Text = "ON";
                if (!switchDevice1.Checked)
                    textViewDevice1.Text = "OFF";
            };

            return view;
        }



    }








    //next fragment
    class Clock : Fragment
    {
        //variables
        private int clickCount = 0;
        private Button clicker;
        private Button settingButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Timed, container, false);
            clicker = view.FindViewById<Button>(Resource.Id.button1);
            settingButton = view.FindViewById<Button>(Resource.Id.settingbutton2);

            //sets the text of the button
            clicker.Text = "You clicked the button " + clickCount++ + " times.";

            
            clicker.Click += clicker_Click;
            settingButton.Click += settingButton_Click;

            return view;
        }

        private void clicker_Click(object sender, EventArgs e)
        {
            clicker.Text = "You clicked the button " + clickCount++ + " times.";
        }

        private void settingButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(Activity2));
            this.StartActivity(intent);
        }
    }


    public class PrefFragment : PreferenceFragment
    {
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //AddPreferencesFromResource(Resource.Xml.preferences);
        }
    }
}
