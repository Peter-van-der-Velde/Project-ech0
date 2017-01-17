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
    //next fragment
    class Clock : Fragment
    {
        //variables
        private int clickCount = 0;
        private Button clicker;
        private ImageButton settingButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Timed, container, false);

            clicker = view.FindViewById<Button>(Resource.Id.button1);
            settingButton = view.FindViewById<ImageButton>(Resource.Id.settingbutton2);

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
            Intent intent = new Intent(Activity, typeof(Settings));
            this.StartActivity(intent);
        }
    }
}
