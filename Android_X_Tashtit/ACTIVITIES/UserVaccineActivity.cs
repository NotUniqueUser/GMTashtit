using System;
using Android.App;
using Android.OS;
using Android.Widget;
using HELPER;
using MODEL;

namespace Android_X_Tashtit.ACTIVITIES
{
    [Activity(Label = "UserVaccineActivity")]
    public class UserVaccineActivity : BaseActivity
    {
        private User user;
        private Button uvBtnFinish;
        private Button uvBtnLoad;
        private Button uvBtnVaccine;
        private EditText uvEtTz;
        private TextView[] uvTvDate;
        private TextView uvTvName;
        private TextView[] uvTvWeeks;
        private Vaccines vaccines;

        protected override void InitializeViews()
        {
            uvEtTz = FindViewById<EditText>(Resource.Id.uvEtTz);
            uvBtnLoad = FindViewById<Button>(Resource.Id.uvBtnLoad);
            uvTvName = FindViewById<TextView>(Resource.Id.uvTvName);
            uvBtnVaccine = FindViewById<Button>(Resource.Id.uvBtnVaccine);
            uvBtnFinish = FindViewById<Button>(Resource.Id.uvBtnCancel);
            uvTvDate = new TextView[3];
            uvTvWeeks = new TextView[3];
            for (var i = 0; i < uvTvDate.Length; i++)
            {
                uvTvDate[i] = FindViewById<TextView>(Resources.GetIdentifier($"uvTvDate{i + 1}", "id", PACKAGE_NAME));
                uvTvWeeks[i] = FindViewById<TextView>(Resources.GetIdentifier($"uvTvWeeks{i + 1}", "id", PACKAGE_NAME));
            }

            for (var i = 0; i < uvTvDate.Length; i++)
            {
                uvTvDate[i].Text = "";
                uvTvWeeks[i].Text = "";
            }

            uvBtnLoad.Click += UvBtnLoadOnClick;
            uvBtnFinish.Click += UvBtnFinish_Click;
            uvBtnVaccine.Click += UvBtnVaccine_Click;
        }

        private async void UvBtnVaccine_Click(object sender, EventArgs e)
        {
            if (user == null)
                return;
            var vaccine = new Vaccine(user.Tz, DateTime.Today);
            vaccines.Add(vaccine);
            await vaccines.Save();
            ReloadVaccines();
        }

        private void UvBtnFinish_Click(object sender, EventArgs e)
        {
            Finish();
        }

        private async void UvBtnLoadOnClick(object sender, EventArgs e)
        {
            var dialog = Global.SetProgress(this, message: "Loading Vaccines");
            try
            {
                user = await Users.GetUser(uvEtTz.Text);

                if (user == null)
                {
                    Toast.MakeText(this, "No user with this ID found", ToastLength.Long).Show();
                    return;
                }

                uvTvName.Text = user.FullName;

                ReloadVaccines();
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, ex.Message, ToastLength.Long).Show();
            }
            finally
            {
                dialog.Dismiss();
                Keyboard.HideKeyboard(this);
            }
        }

        private async void ReloadVaccines()
        {
            vaccines = await Vaccines.GetVaccines(user);
            if (vaccines == null || vaccines.Count < 1)
                return;
            for (var i = 0; i < vaccines.Count; i++)
            {
                uvTvDate[i].Text = vaccines[i].Date.ToString("MM/dd/yyyy");
                uvTvWeeks[i].Text = ((int) (DateTime.Today - vaccines[i].Date).TotalDays / 7).ToString();
            }
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.user_vaccine_layout);
            InitializeViews();
        }
    }
}