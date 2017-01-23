using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

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

        TimeScript timescript;

        private Clock clock;

        TextView timeDisplay;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            clock = this;
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.timedItems, container, false);



            settingButton = view.FindViewById<ImageButton>(Resource.Id.settingbutton3); //Leads to timedItems.axml
            //settingButton = view.FindViewById<ImageButton>(Resource.Id.settingbutton2);
            set_addButton = view.FindViewById<ImageButton>(Resource.Id.set_Add); // those who keep commenting this line, pls kys.
            timeDisplay = view.FindViewById<TextView>(Resource.Id.txtTime);

            ISharedPreferences pref = Application.Context.GetSharedPreferences("Time", FileCreationMode.Private);
            if(Convert.ToInt16(pref.GetString("Minute", DateTime.Now.Hour.ToString())) > 10)
                timeDisplay.Text = pref.GetString("Hour", DateTime.Now.Hour.ToString()) + ":" + pref.GetString("Minute", DateTime.Now.Hour.ToString());
            else
                timeDisplay.Text = pref.GetString("Hour", DateTime.Now.Hour.ToString()) + ":0" + pref.GetString("Minute", DateTime.Now.Hour.ToString());

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
