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

            var view = inflater.Inflate(Resource.Layout.Timed, container, false);

            buttonConnect = view.FindViewById<Button>(Resource.Id.button1);
            settingButton = view.FindViewById<ImageButton>(Resource.Id.settingbutton2);

            //sets the text of the button
            buttonConnect.Text = "You clicked the button " + 0.ToString() + " times.";


            //clicker.Click += clicker_Click;
            settingButton.Click += settingButton_Click;

            return view;
        }

        private void settingButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(Settings));
            this.StartActivity(intent);
        }
    }
}