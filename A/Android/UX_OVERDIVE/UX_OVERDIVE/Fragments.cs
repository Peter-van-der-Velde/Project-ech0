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


namespace UX_OVERDIVE
{


    /// <summary>
    /// These classes are used for the various views between the tabs.
    /// </summary>
    class Sliders : Fragment
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Tab, container, false);
            var sampleTextView = view.FindViewById<TextView>(Resource.Id.sampleTextView);
            sampleTextView.Text = "sample fragment text 2";

            return view;
        }

    }
    class Clock : Fragment
    {
        private int clickCount = 0;

        //here goes the code for each "Tab"
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
                base.OnCreateView(inflater, container, savedInstanceState);

                //Select the View you want to use for this tab
                var view = inflater.Inflate(Resource.Layout.Timed, container, false);
                var button = view.FindViewById<TextView>(Resource.Id.button1);

                //sets the text of the button
                button.Text = "You clicked the button " + clickCount++ + " times.";

                //simple button clicker
                view.FindViewById<Button>(Resource.Id.button1).Click += delegate
                {
                    button.Text = "You clicked the button " + clickCount++ + " times.";
                };

                return view;
        }

    }
}
