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
    class SampleTabFragment1 : Fragment
        {
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                //int count = 1;
                var view = inflater.Inflate(Resource.Layout.Timed, container, false);
                //var sampleTextView = view.FindViewById<TextView>(Resource.Id.sampleTextView);
                //sampleTextView.Text = "sample fragment text 2";
                
                   

                return view;
            }

      }
    class SampleTabFragment2 : Fragment
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
}
