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

            //removes title and icon
            ActionBar.SetDisplayShowHomeEnabled(false);
            ActionBar.SetDisplayShowTitleEnabled(false);

            

            //PRESS THINGIES
            Button _settingButton = FindViewById<Button>(Resource.Id.button_settings);



            //Creates tabs
            var _Sliders = this.ActionBar.NewTab();
            var _Time_Script = this.ActionBar.NewTab();

            //sets Icons of Tabs
            _Sliders.SetIcon(Resource.Drawable.Sliders);
            _Time_Script.SetIcon(Resource.Drawable.Clock);

            _Sliders.TabSelected += (sender, args) =>
            {
                //changes view
                SetContentView(Resource.Layout.Main);

                //changes the icon to the appropiote icons
                _Time_Script.SetIcon(Resource.Drawable.Clock);
                _Sliders.SetIcon(Resource.Drawable.Home);
            };
             ActionBar.AddTab(_Sliders);

           
            
            //if _Time tab is selected the following will be execute
            _Time_Script.TabSelected += (sender, args) =>
            {
                SetContentView(Resource.Layout.Timed);

                //read above
                _Time_Script.SetIcon(Resource.Drawable.Home);
                _Sliders.SetIcon(Resource.Drawable.Sliders);
            };
            this.ActionBar.AddTab(_Time_Script);

            //IF settings is clicked
            _settingButton.Click += (sender,  e) =>
            {
                _settingButton.Text = "lel";
            };

        }
    }
}

