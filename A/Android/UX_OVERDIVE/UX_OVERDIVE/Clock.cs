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

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = inflater.Inflate(Resource.Layout.Timed, container, false);

            settingButton = view.FindViewById<ImageButton>(Resource.Id.settingbutton2);

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
