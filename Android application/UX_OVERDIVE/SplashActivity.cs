using Android.App;
using Android.OS;

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
			System.Threading.Thread.Sleep(3000); // Simulate a long pause    
			RunOnUiThread(() => StartActivity(typeof(MainActivity)));
		}

	}
}