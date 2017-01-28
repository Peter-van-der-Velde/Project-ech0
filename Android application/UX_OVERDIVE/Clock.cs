using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace UX_OVERDIVE
{
    public class TimerObject
    {
        public string hour = "";
        public string minute = "";
        public bool switch1 = false;
        public bool switch2 = false;
        public bool switch3 = false;
    }

    //next fragment
    class Clock : Fragment
    {
        //variables
        private ImageButton settingButton;
        private ImageButton set_addButton;

        public static Dictionary<int, TimerObject> timers = new Dictionary<int, TimerObject>();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.timedItems, container, false);

            settingButton = view.FindViewById<ImageButton>(Resource.Id.settingbutton3);
            set_addButton = view.FindViewById<ImageButton>(Resource.Id.set_Add);

            ISharedPreferences pref = Application.Context.GetSharedPreferences("Time", FileCreationMode.Private);
            int timerCounter = 0;

            for (int i = 0; i <= 5; i++)
            {
                SetUIActive(i, false, view);
            }

            if (timers.Count == 0)
            {
                while (true)
                {
                    TimerObject timerObject = new TimerObject();

                    string returnedHour = pref.GetString("Hour" + timerCounter, "");
                    if (!string.IsNullOrWhiteSpace(returnedHour))
                    {
                        timerObject.hour = returnedHour;
                    }
                    else
                        break;

                    string returnedMinute = pref.GetString("Minute" + timerCounter, "");
                    if (!string.IsNullOrWhiteSpace(returnedMinute))
                    {
                        timerObject.minute = returnedMinute;
                    }
                    else
                        break;

                    timers.Add(timerCounter, timerObject);
                    timerCounter++;
                }
            }

            foreach(int i in timers.Keys)
            {
                if(Convert.ToInt32(timers[i].minute) > 9)
                    SetUIActive(i, true, view, timers[i].hour + ":" + timers[i].minute);
                else
                    SetUIActive(i, true, view, timers[i].hour + ":0" + timers[i].minute);
            }

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
            if (timers.Keys.Count > 6)
                return;

            if (timers.Keys.Count != 0)
                timers.Add(timers.Keys.Last() + 1, new TimerObject());
            else
                timers.Add(0, new TimerObject());
            int key = timers.Keys.Last();

            Intent intent = new Intent(Activity, typeof(TimeScript));
            intent.PutExtra("TimerToEdit", key);
            this.StartActivity(intent);
        }

        public void SetUIActive(int key, bool state, View view)
        {
            switch(key)
            {
                case 0:
                    if(state == false)
                        view.FindViewById<TableRow>(Resource.Id.tableRow0).Visibility = ViewStates.Gone;
                    else
                        view.FindViewById<TableRow>(Resource.Id.tableRow0).Visibility = ViewStates.Visible;
                    break;
                case 1:
                    if (state == false)
                        view.FindViewById<TableRow>(Resource.Id.tableRow1).Visibility = ViewStates.Gone;
                    else
                        view.FindViewById<TableRow>(Resource.Id.tableRow1).Visibility = ViewStates.Visible;
                    break;
                case 2:
                    if (state == false)
                        view.FindViewById<TableRow>(Resource.Id.tableRow2).Visibility = ViewStates.Gone;
                    else
                        view.FindViewById<TableRow>(Resource.Id.tableRow2).Visibility = ViewStates.Visible;
                    break;
                case 3:
                    if (state == false)
                        view.FindViewById<TableRow>(Resource.Id.tableRow3).Visibility = ViewStates.Gone;
                    else
                        view.FindViewById<TableRow>(Resource.Id.tableRow3).Visibility = ViewStates.Visible;
                    break;
                case 4:
                    if (state == false)
                        view.FindViewById<TableRow>(Resource.Id.tableRow4).Visibility = ViewStates.Gone;
                    else
                        view.FindViewById<TableRow>(Resource.Id.tableRow4).Visibility = ViewStates.Visible;
                    break;
                case 5:
                    if (state == false)
                        view.FindViewById<TableRow>(Resource.Id.tableRow5).Visibility = ViewStates.Gone;
                    else
                        view.FindViewById<TableRow>(Resource.Id.tableRow5).Visibility = ViewStates.Visible;
                    break;
            }
        }

        public void SetUIActive(int key, bool state, View view, string message)
        {
            switch (key)
            {
                case 0:
                    if (state == false)
                    {
                        view.FindViewById<TableRow>(Resource.Id.tableRow0).Visibility = ViewStates.Gone;
                        view.FindViewById<TextView>(Resource.Id.txtTime0).Text = message;
                    }
                    else
                    {
                        view.FindViewById<TableRow>(Resource.Id.tableRow0).Visibility = ViewStates.Visible;
                        view.FindViewById<TextView>(Resource.Id.txtTime0).Text = message;
                    }
                    break;
                case 1:
                    if (state == false)
                    {
                        view.FindViewById<TableRow>(Resource.Id.tableRow1).Visibility = ViewStates.Gone;
                        view.FindViewById<TextView>(Resource.Id.txtTime1).Text = message;
                    }
                    else
                    {
                        view.FindViewById<TableRow>(Resource.Id.tableRow1).Visibility = ViewStates.Visible;
                        view.FindViewById<TextView>(Resource.Id.txtTime1).Text = message;
                    }
                    break;
                case 2:
                    if (state == false)
                    {
                        view.FindViewById<TableRow>(Resource.Id.tableRow2).Visibility = ViewStates.Gone;
                        view.FindViewById<TextView>(Resource.Id.txtTime2).Text = message;
                    }
                    else
                    {
                        view.FindViewById<TableRow>(Resource.Id.tableRow2).Visibility = ViewStates.Visible;
                        view.FindViewById<TextView>(Resource.Id.txtTime2).Text = message;
                    }
                    break;
                case 3:
                    if (state == false)
                    {
                        view.FindViewById<TableRow>(Resource.Id.tableRow3).Visibility = ViewStates.Gone;
                        view.FindViewById<TextView>(Resource.Id.txtTime3).Text = message;
                    }
                    else
                    {
                        view.FindViewById<TableRow>(Resource.Id.tableRow3).Visibility = ViewStates.Visible;
                        view.FindViewById<TextView>(Resource.Id.txtTime3).Text = message;
                    }
                    break;
                case 4:
                    if (state == false)
                    {
                        view.FindViewById<TableRow>(Resource.Id.tableRow4).Visibility = ViewStates.Gone;
                        view.FindViewById<TextView>(Resource.Id.txtTime4).Text = message;
                    }
                    else
                    {
                        view.FindViewById<TableRow>(Resource.Id.tableRow4).Visibility = ViewStates.Visible;
                        view.FindViewById<TextView>(Resource.Id.txtTime4).Text = message;
                    }
                    break;
                case 5:
                    if (state == false)
                    {
                        view.FindViewById<TableRow>(Resource.Id.tableRow5).Visibility = ViewStates.Gone;
                        view.FindViewById<TextView>(Resource.Id.txtTime5).Text = message;
                    }
                    else
                    {
                        view.FindViewById<TableRow>(Resource.Id.tableRow5).Visibility = ViewStates.Visible;
                        view.FindViewById<TextView>(Resource.Id.txtTime5).Text = message;
                    }
                    break;
            }
        }
    }
}
