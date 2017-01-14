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

        //here goes the code for each "Tab"
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            //Select the View you want to use for this tab
            var view = inflater.Inflate(Resource.Layout.Login, container, false);

            //var sampleTextView = view.FindViewById<TextView>(Resource.Id.sampleTextView);
            //sampleTextView.Text = "sample fragment text 2";

            return view;
        }

    }
    class Clock : Fragment
    {
        private int clickCount = 0;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Timed, container, false);
            var clicker = view.FindViewById<TextView>(Resource.Id.button1);
            var settingButton = view.FindViewById<Button>(Resource.Id.settingbutton2);

            //sets the text of the button
            clicker.Text = "You clicked the button " + clickCount++ + " times.";

            //simple button clicker
            view.FindViewById<Button>(Resource.Id.button1).Click += delegate
            {
                clicker.Text = "You clicked the button " + clickCount++ + " times.";
                    
            };

            settingButton.Click += delegate {
                //Intent intent = new Intent(SettingsActivity.);
                //intent.AddFlags(Intent.ExtraShortcutIcon)
                //Context.StartActivity(intent);
            };

            return view;
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
