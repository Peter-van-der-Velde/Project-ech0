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
        //variables
        public static Fragment[] fragments; //makes array of fragments
        
        protected override void OnCreate(Bundle bundle)
        {
            

            base.OnCreate(bundle);


            // Set our view from the "main" layout resource
            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;//ACTIONBAR is in Tabbed Mode
            SetContentView(Resource.Layout.Main);//sets view to main



            //removes title and icon from actionbar
            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayShowTitleEnabled(false);

            //fragments used
            fragments = new Fragment[]
                         {
                             new Sliders(), 
                             new Clock() 
                         };

            AddTabToActionBar(Resource.String.empty, Resource.Drawable.Sliders);
            AddTabToActionBar(Resource.String.empty, Resource.Drawable.Clock);
            //AddTabToActionBar(Resource.String.speakers_tab_label, Resource.Drawable.ic_action_speakers);
            //AddTabToActionBar(Resource.String.sessions_tab_label, Resource.Drawable.ic_action_sessions);
        }



        //variable
        static readonly string Tag = "UX-OVERDRIVE";


        /// <summary>
        /// Adds Tabs to Actionbar
        /// </summary>
        /// <param name="labelResourceId">label string of tab.
        /// Resource.String.empty returns an empty string/nameless</param>
        /// <param name="iconResourceId">icon of tab</param>
        public void AddTabToActionBar(int labelResourceId, int iconResourceId)
        {
            ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;
            ActionBar.Tab tab = ActionBar.NewTab()
                .SetText(labelResourceId)
                .SetIcon(iconResourceId);
            tab.TabSelected += TabOnTabSelected;
            ActionBar.AddTab(tab);
        }

        //connects the right fragment to the right tab
        public void TabOnTabSelected(object sender, ActionBar.TabEventArgs tabEventArgs)
        {
            ActionBar.Tab tab = (ActionBar.Tab)sender;

            Log.Debug(Tag, "The tab {0} has been selected.", tab.Text);
            Fragment frag = MainActivity.fragments[tab.Position];
            tabEventArgs.FragmentTransaction.Replace(Resource.Id.frameLayout1, frag);
        }
    }
}