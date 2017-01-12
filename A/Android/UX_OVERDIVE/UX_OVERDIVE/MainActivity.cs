using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace UX_OVERDIVE
{
    [Activity(Label = "UX_OVERDIVE", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {

            base.OnCreate(bundle);


            // Set our view from the "main" layout resource
            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;//ACTIONBAR is in Tabbed Mode
            SetContentView(Resource.Layout.Main);


            

            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayShowTitleEnabled(false);


            //Tabs
            var _Sliders = this.ActionBar.NewTab();
            _Sliders.SetIcon(Resource.Drawable.Sliders);
            _Sliders.TabSelected += (sender, args) =>
            {
                // Do something when tab is selected
            };
             ActionBar.AddTab(_Sliders);


            var _Time_Script = this.ActionBar.NewTab();
            _Time_Script.SetIcon(Resource.Drawable.Clock);
            _Time_Script.TabSelected += (sender, args) =>
            {
                // Do something when tab is selected
            };
            this.ActionBar.AddTab(_Time_Script);
        }
    }
}

