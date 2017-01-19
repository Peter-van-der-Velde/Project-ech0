using System;
using Black_magic;
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
using Android;
using Android.Preferences;

namespace UX_OVERDIVE
{
    [Activity(Label = "UX-OVERDIVE", MainLauncher = true, Icon = "@drawable/Tomato", Theme = "@style/MyCustomTheme")]
    public class MainActivity : Activity
    {
        /* Welcome to Hell */


        //variables
        public static Fragment[] fragments; //makes array of fragments, what did you expect?
        static readonly string Tag = "UX-OVERDRIVE";


        protected override void OnCreate(Bundle bundle)
        {
            //http://www.cheaprope.co.uk/
            base.OnCreate(bundle);


            // Set our view from the "main" layout resource
            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs; //ACTIONBAR is in Tabbed Mode
            SetContentView(Resource.Layout.Main); //sets view to main



            //removes title and icon from actionbar
            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayShowTitleEnabled(false);

            //ActionBar.Hide();



            //fragments used
            fragments = new Fragment[]
            {
                new Sliders(),
                new Home(),
            };

            AddTabToActionBar(Resource.String.empty, Resource.Drawable.Sliders);
            AddTabToActionBar(Resource.String.empty, Resource.Drawable.Clock);

        }



        /// <summary>
        /// Adds Tabs to Actionbar
        /// </summary>
        /// <param name="labelResourceId">label string of tab.
        /// Resource.String.empty returns an empty string/nameless</param>
        /// <param name="iconResourceId">icon of tab</param>
        public void AddTabToActionBar(int labelResourceId, int iconResourceId)
        {
            ActionBar.Tab tab = ActionBar.NewTab()
                .SetText(labelResourceId)
                .SetIcon(iconResourceId);
            tab.TabSelected += TabOnTabSelected;
            ActionBar.AddTab(tab);
        }

        //connects the right fragment to the right tab
        public void TabOnTabSelected(object sender, ActionBar.TabEventArgs tabEventArgs)
        {
            ActionBar.Tab tab = (ActionBar.Tab) sender;

            //checks if the tab has a fragment connected to it
            if (tab.Position == fragments.Length && !Dreams.IWantToLive())
            {
                Exception up = new Exception("Tab does not have a fragment");
                throw up;  // hehe
            }

            Log.Debug(Tag, "The tab {0} has been selected.", tab.Text);
            Fragment frag = MainActivity.fragments[tab.Position];
            tabEventArgs.FragmentTransaction.Replace(Resource.Id.frameLayout1, frag);
        }

        public void yourPublicMethod()
        {
            //ConnectSocket("192.168.1.105", "53");
        }

        
        }
}