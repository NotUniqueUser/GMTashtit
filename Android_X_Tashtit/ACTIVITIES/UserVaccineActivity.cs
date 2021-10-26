using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.Provider;
using HELPER;
using MODEL;

namespace Android_X_Tashtit.ACTIVITIES
{
    [Activity(Label = "UserVaccineActivity")]
    public class UserVaccineActivity : BaseActivity
    {
        private EditText uvEtTz;
        private Button uvBtnLoad;
        private TextView uvTvName;
        private TextView[] uvTvDate;
        private TextView[] uvTvWeeks;
        private User user;

        protected override void InitializeViews()
        {
            uvEtTz = FindViewById<EditText>(Resource.Id.uvEtTz);
            uvBtnLoad = FindViewById<Button>(Resource.Id.uvBtnLoad);
            uvTvName = FindViewById<TextView>(Resource.Id.uvTvName);
            uvTvDate = new TextView[3];
            uvTvWeeks = new TextView[3];
            for (int i = 0; i < uvTvDate.Length; i++)
            {
                uvTvDate[i] = FindViewById<TextView>(Resources.GetIdentifier($"uvTvDate{i + 1}", "id", PACKAGE_NAME));
                uvTvWeeks[i] = FindViewById<TextView>(Resources.GetIdentifier($"uvTvWeeks{i + 1}", "id", PACKAGE_NAME));
            }
            uvBtnLoad.Click += UvBtnLoadOnClick;
        }

        private async void UvBtnLoadOnClick(object sender, EventArgs e)
        {
            ProgressDialog dialog = Global.SetProgress(this);

            user = await Users.GetUser(uvEtTz.Text);

            if (user != null)
                uvTvName.Text = user.FullName;
            
            
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.user_vaccine_layout);
            InitializeViews();
        }
    }
}