using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Google.Android.Material.TextField;
using Xamarin.Essentials;

namespace Android_X_Tashtit.ACTIVITIES
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : BaseActivity
    {
        private readonly string[] permissions =
        {
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage
        };

        private Button maBtnLogin;
        private EditText maEtEmail;
        private TextInputEditText maEtPassword;
        private LinearLayout maLlLogin;
        private TextInputLayout maLyEmail;
        private TextInputLayout maLyPassword;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
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

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}