using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Google.Android.Material.TextField;
using HELPER;
using MODEL;
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

        private TextView maTvRegister;
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
            maTvRegister = FindViewById<TextView>(Resource.Id.maTvRegister);

            maTvRegister.Click += MaTvRegister_Click;
        }

        private void MaTvRegister_Click(object sender, System.EventArgs e)
        {
            StartActivityForResult(new Intent(this, typeof(ACTIVITIES.UserActivity)), 1);
        }

        protected override async void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (resultCode != Result.Ok)
                return;


            var user = Serializer.ByteArrayToObject(data.GetByteArrayExtra("USER")) as User;
            Users users = await Users.SelectAll();
            if (!users.Exist(user))
            {
                users.Add(user);
                await users.Save();
            }
            else
            {
                Toast.MakeText(this, "User already exists", ToastLength.Long).Show();
            }
            base.OnActivityResult(requestCode, resultCode, data);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}