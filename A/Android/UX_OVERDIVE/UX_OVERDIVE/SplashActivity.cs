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
	[Activity(Label = "Ech0", Theme = "@style/Theme.Splash", Icon = "@drawable/LOGO", MainLauncher = true, NoHistory = true)]
	public class SplashActivity : Activity
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);

			SetContentView(Resource.Layout.SplashLayout);
			System.Threading.ThreadPool.QueueUserWorkItem(o => LoadActivity());
		}

		private void LoadActivity()
		{
			System.Threading.Thread.Sleep(5000); // Simulate a long pause    
			RunOnUiThread(() => StartActivity(typeof(MainActivity)));
		}

	}
}