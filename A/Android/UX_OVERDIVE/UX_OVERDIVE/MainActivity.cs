using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Timers;
using Android.Content.PM;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Android.Graphics;
using Android.Util;
using System.Threading.Tasks;

namespace UX_OVERDIVE
{
    [Activity(Label = "UX-OVERDIVE", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        static readonly string Tag = "UX-OVERDRIVE";

        Fragment[] _fragments;

        protected override void OnCreate(Bundle bundle)
        {
            

            base.OnCreate(bundle);


            // Set our view from the "main" layout resource
            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;//ACTIONBAR is in Tabbed Mode
            SetContentView(Resource.Layout.Main);

            //removes title and icon
            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayShowTitleEnabled(false);

            _fragments = new Fragment[]
                         {
                             new SampleTabFragment2(),
                             new SampleTabFragment1()
                         };

            AddTabToActionBar(Resource.String.Hello, Resource.Drawable.Clock);
            AddTabToActionBar(Resource.String.Hello, Resource.Drawable.Home);
            //AddTabToActionBar(Resource.String.speakers_tab_label, Resource.Drawable.ic_action_speakers);
            //AddTabToActionBar(Resource.String.sessions_tab_label, Resource.Drawable.ic_action_sessions);

            //int count = 1;
            //PRESS THINGIES
            //Button _settingButton = FindViewById<Button>(Resource.Id.button_settings);
            

            //IF settings is clicked
            //_settingButton.Click += (sender, args) => { _settingButton.Text = string.Format("lel {0}", aaa++); };

        }
        void AddTabToActionBar(int labelResourceId, int iconResourceId)
        {
            ActionBar.Tab tab = ActionBar.NewTab()
                                         .SetText(labelResourceId)
                                         .SetIcon(iconResourceId);
            tab.TabSelected += TabOnTabSelected;
            ActionBar.AddTab(tab);
        }

        void TabOnTabSelected(object sender, ActionBar.TabEventArgs tabEventArgs)
        {
            ActionBar.Tab tab = (ActionBar.Tab)sender;

            Log.Debug(Tag, "The tab {0} has been selected.", tab.Text);
            Fragment frag = _fragments[tab.Position];
            tabEventArgs.FragmentTransaction.Replace(Resource.Id.frameLayout1, frag);
        }
    }
}

