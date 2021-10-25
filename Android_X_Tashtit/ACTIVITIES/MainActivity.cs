
using Android;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Google.Android.Material.TextField;

namespace Android_X_Tashtit.ACTIVITIES
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : BaseActivity
    {
        private TextInputLayout maLyEmail;
        private LinearLayout maLlLogin;
        private EditText maEtEmail;
        private TextInputLayout maLyPassword;
        private TextInputEditText maEtPassword;
        private Button maBtnLogin;
        private readonly string[] permissions =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage
        };

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            InitializeViews();

            RequestPermissions(permissions, 0);
        }

        protected override void InitializeViews()
        {
            maLyEmail = FindViewById<TextInputLayout>(Resource.Id.maLyEmail);
            maLlLogin = FindViewById<LinearLayout>(Resource.Id.maLlLogin);
            maEtEmail = FindViewById<EditText>(Resource.Id.maEtEmail);
            maLyPassword = FindViewById<TextInputLayout>(Resource.Id.maLyPassword);
            maEtPassword = FindViewById<TextInputEditText>(Resource.Id.maEtPassword);
            maBtnLogin = FindViewById<Button>(Resource.Id.maBtnLogin);

        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}