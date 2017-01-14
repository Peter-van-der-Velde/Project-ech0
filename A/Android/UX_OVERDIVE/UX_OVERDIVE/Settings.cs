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
using Android.Preferences;

namespace UX_OVERDIVE
{
    public class SettingsActivity : PreferenceActivity
    {
        public void onCreate(Bundle savedInstanceState)
        {
            // help nothing works
            // This is hell!
            // BRACE YOURSELVES!

            onCreate(savedInstanceState);
            //AddPreferencesFromResource(Resource.Xml.preferences);
















            //FragmentTransaction fragmentTx = this.FragmentManager.BeginTransaction();
            //PrefFragment preferensesFragment = new PrefFragment();


            // DetailsFragment aDifferentDetailsFrag = new DetailsFragment();

            //getFragmentManager().beginTransaction()
            //    .replace(.id.content, new SettingsFragment())
            //    .commit();
        }
    }
}