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
    class Home : Fragment
    {
        private Button buttonConnect;
        private ImageButton settingButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Temprature, container, false);

            buttonConnect = view.FindViewById<Button>(Resource.Id.button1);
            settingButton = view.FindViewById<ImageButton>(Resource.Id.settingbutton2);


            return view;
        }


    }
}