using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

namespace UX_OVERDIVE
{
    [Activity(Label = "Settings", Theme = "@style/MyCustomTheme")]
    public class Settings : Activity
    {
        private string IPADDRESS;
        private string PORT;

        private EditText editTextIP;
        private EditText editTextPORT;

        private Button buttonCancel;
        private Button buttonSave;

        private FrameLayout FL;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ActionBar.SetHomeButtonEnabled(true);
            ActionBar.SetDisplayHomeAsUpEnabled(true);

            // Create your application here
            SetContentView(Resource.Layout.Settings);

            editTextIP = FindViewById<EditText>(Resource.Id.editTextIPADDRESS);
            editTextPORT = FindViewById<EditText>(Resource.Id.editTextPORT);
            buttonCancel = FindViewById<Button>(Resource.Id.buttonCancel);
            buttonSave = FindViewById<Button>(Resource.Id.buttonSave);
            FL = FindViewById<FrameLayout>(Resource.Id.FL_lel);

            ISharedPreferences pref = Application.Context.GetSharedPreferences("Settings", FileCreationMode.Private);
            string wrtIP = pref.GetString("IP", "192.168.1.100");
            string wrtPORT = pref.GetString("PORT", "53");

            editTextIP.Text = wrtIP;
            editTextPORT.Text = wrtPORT;

            buttonSave.Click += (obj, args) =>
            {
                ISharedPreferencesEditor edit = pref.Edit();
                edit.PutString("IP", editTextIP.Text.Trim());
                edit.PutString("PORT", editTextPORT.Text.Trim());
                edit.Apply();
                this.Finish();
            };

            buttonCancel.Click += (obj, args) =>
            {
                this.Finish();
            };

            FL.Click += (obj, args) =>
            {
                InputMethodManager inputManager =
                    (InputMethodManager) this.GetSystemService(Activity.InputMethodService);
                inputManager.HideSoftInputFromWindow(this.CurrentFocus.WindowToken, HideSoftInputFlags.None);
            };
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                default:
                    return base.OnOptionsItemSelected(item);
            }
        }

    }
}
