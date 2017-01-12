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
            SetContentView(Resource.Layout.Main);

            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            var _Sliders = this.ActionBar.NewTab();
            //_Sliders.SetText();
            _Sliders.SetIcon(Resource.Drawable.Sliders);

            var _Time_Script = this.ActionBar.NewTab();
            //_Time.SetText("LEL");
            _Time_Script.SetIcon(Resource.Drawable.Clock);


            _Sliders.TabSelected += delegate (object sender, ActionBar.TabEventArgs e) {
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer,
                    new SampleTabFragment());
            };

            _Time_Script.TabSelected += delegate (object sender, ActionBar.TabEventArgs e) {
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer,
                    new SampleTabFragment());
            };

            this.ActionBar.AddTab(_Sliders);
            this.ActionBar.AddTab(_Time_Script);
        }
    }
}

