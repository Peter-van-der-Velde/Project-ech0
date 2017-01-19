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
    [Activity(Label = "TimeScript", Theme = "@style/MyCustomTheme")]
    public class TimeScript : Activity
    {
        private TextView time_display;
        //private Button pick_button;

        private int hour;
        private int minute;

        const int TIME_DIALOG_ID = 0;

        private Button buttonCancel;
        private Button buttonSave;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            // Create your application here
            SetContentView(Resource.Layout.TimeScript);

            // Capture our View elements
            time_display = FindViewById<TextView>(Resource.Id.timeDisplay);
            buttonCancel = FindViewById<Button>(Resource.Id.buttonCancel2);
            buttonSave = FindViewById<Button>(Resource.Id.buttonSave2);

            // Add a click listener to the button
            time_display.Click += (o, e) => ShowDialog(TIME_DIALOG_ID);

            ISharedPreferences pref = Application.Context.GetSharedPreferences("Time", FileCreationMode.Private);
            string Hour = pref.GetString("Hour", DateTime.Now.Hour.ToString()); //lel
            string Minute = pref.GetString("Minute", DateTime.Now.Minute.ToString());


            // Get the current time
            hour = DateTime.Now.Hour;
            minute = DateTime.Now.Minute;

            buttonCancel.Click += (obj, args) =>
            {
                this.Finish();
            };

            buttonSave.Click += (obj, args) =>
            {
                ISharedPreferencesEditor edit = pref.Edit();
                edit.PutString("Hour", Hour);
                edit.PutString("Minute", Minute);
                edit.Apply();
                this.Finish();
            };

            // Display the current date
            UpdateDisplay();
        }

        // Updates the time we display in the TextView
        private void UpdateDisplay()
        {
            string time = string.Format("{0}:{1}", hour, minute.ToString().PadLeft(2, '0'));
            time_display.Text = time;
        }

        private void TimePickerCallback(object sender, TimePickerDialog.TimeSetEventArgs e)
        {
            hour = e.HourOfDay;
            minute = e.Minute;
            UpdateDisplay();
        }

        protected override Dialog OnCreateDialog(int id)
        {
            if (id == TIME_DIALOG_ID)
            {
                return new TimePickerDialog(this, TimePickerCallback, hour, minute, false);
            }

            return null;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

    }
}