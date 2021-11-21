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
        private CheckBox maCbRememberMe;
        private EditText maEtEmail;
        private Users users;
        private TextInputEditText maEtPassword;
        private LinearLayout maLlLogin;
        private TextInputLayout maLyEmail;
        private TextInputLayout maLyPassword;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            InitializeViews();
            users = await Users.SelectAll();
            RequestPermissions(permissions, 0);
            var userEmail = GetSharedPreferences("LOGIN", FileCreationMode.Private).GetString("EMAIL", null);
            if (userEmail != null)
            {
                CurrentUser = users.Find("Email", userEmail);
            }
        }

        protected override void InitializeViews()
        {
            maLyEmail = FindViewById<TextInputLayout>(Resource.Id.maLyEmail);
            maLlLogin = FindViewById<LinearLayout>(Resource.Id.maLlLogin);
            maEtEmail = FindViewById<EditText>(Resource.Id.maEtEmail);
            maCbRememberMe = FindViewById<CheckBox>(Resource.Id.maCbRemeberMe);
            maLyPassword = FindViewById<TextInputLayout>(Resource.Id.maLyPassword);
            maEtPassword = FindViewById<TextInputEditText>(Resource.Id.maEtPassword);
            maBtnLogin = FindViewById<Button>(Resource.Id.maBtnLogin);
            maTvRegister = FindViewById<TextView>(Resource.Id.maTvRegister);

            maTvRegister.Click += MaTvRegister_Click;
            maBtnLogin.Click += MaBtnLogin_Click;
        }

        private void MaBtnLogin_Click(object sender, System.EventArgs e)
        {
            var user = users.Find("Email", maEtEmail.Text);
            if(user == null)
            {
                Toast.MakeText(this, "User not found", ToastLength.Long).Show();
            }
            if(user.Password != maEtPassword.Text)
            {
                Toast.MakeText(this, "Incorrect password!", ToastLength.Long).Show();
            }
            var preferences = GetSharedPreferences("LOGIN", FileCreationMode.Private);
            if (maCbRememberMe.Checked)
            {
                var editor = preferences.Edit();
                editor.PutString("EMAIL", user.Email);
                editor.PutString("PASSWORD", user.Password);
                editor.Apply();
            }
            CurrentUser = user;
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