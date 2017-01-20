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
        private ImageButton settingButton;
        private ImageButton set_addButton;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.timedItems, container, false);


            settingButton = view.FindViewById<ImageButton>(Resource.Id.settingbutton3); //Leads to timedItems.axml
            //settingButton = view.FindViewById<ImageButton>(Resource.Id.settingbutton2);
            set_addButton = view.FindViewById<ImageButton>(Resource.Id.set_Add);


            settingButton.Click += settingButton_Click;
            set_addButton.Click += openTimeScript;

            return view;
        }

        private void settingButton_Click(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(Settings));
            this.StartActivity(intent);
        }

        private void openTimeScript(object sender, EventArgs e)
        {
            Intent intent = new Intent(Activity, typeof(TimeScript));
            this.StartActivity(intent);
        }
    }
}
