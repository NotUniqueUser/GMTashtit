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
using Android_X_Tashtit.ADAPTERS;
using AndroidX.AppCompat.Widget;
using AndroidX.RecyclerView.Widget;
using HELPER;
using MODEL;

namespace Android_X_Tashtit.ACTIVITIES
{
    [Activity(Label = "SideEffectsVaccineActivity")]
    public class SideEffectsVaccineActivity : BaseActivity
    {
        private EditText etId;
        private Button btnLoad;
        private TextView tvName;
        private LinearLayout[] llAdd;
        private TextView[] tvAddDate;
        private AppCompatImageButton[] btnAdd;
        private RecyclerView[] rvVaccine;
        private SideEffectsVaccineAdapter[] adapter;
        private User user;
        private Vaccines vaccines;
        private VaccineSideEffects[] sideEffects;
        protected override void InitializeViews()
        {
            etId = FindViewById<EditText>(Resource.Id.etId);
            btnLoad = FindViewById<Button>(Resource.Id.btnLoad);
            tvName = FindViewById<TextView>(Resource.Id.tvName);
            llAdd = new LinearLayout[3];
            tvAddDate = new TextView[3];
            adapter = new SideEffectsVaccineAdapter[3];
            btnAdd = new AppCompatImageButton[3];
            rvVaccine = new RecyclerView[3];
            for (int i = 0; i < 3; i++)
            {
                llAdd[i] = FindViewById<LinearLayout>(Resources.GetIdentifier($"llAdd{i + 1}", "id", PACKAGE_NAME));
                tvAddDate[i] = FindViewById<TextView>(Resources.GetIdentifier($"tvAddDate{i + 1}", "id", PACKAGE_NAME));
                btnAdd[i] = FindViewById<AppCompatImageButton>(Resources.GetIdentifier($"btnAdd{i + 1}", "id", PACKAGE_NAME));
                rvVaccine[i] = FindViewById<RecyclerView>(Resources.GetIdentifier($"rvVaccine{i + 1}", "id", PACKAGE_NAME));
                btnAdd[i].Tag = i;
                btnAdd[i].Click += BtnAddOnClick;
            }
            
            btnLoad.Click += BtnLoadOnClick;
        }

        private async void BtnAddOnClick(object sender, EventArgs e)
        {
            var effects = await SideEffects.SelectAll();
            string[] items = new string[effects.Count];
            for (int i = 0; i < items.Length; i++)
            {
                items[i] = effects[i].Name;
            }
            int pos = int.Parse(((AppCompatImageButton) sender).Tag.ToString());

            AlertDialog.Builder b = new AlertDialog.Builder(this);
            b.SetTitle("Select side effect");
            b.SetItems(
                items,
                async(o, args) =>
                {
                    VaccineSideEffect se = new VaccineSideEffect(effects[args.Which].IdFs, vaccines[pos].IdFs, effects[args.Which].Name);
                    sideEffects[pos].Add(se);
                    await sideEffects[pos].Save();
                    Load();
                });
            b.SetNegativeButton("Cancel", (o, args) =>
            {
                b.Dispose();
            });
            b.SetCancelable(false);
            b.Show();
        }

        private async void BtnLoadOnClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(etId.Text))
                return;
            user = await Users.GetUser(etId.Text);
            if (user == null)
            {
                // TODO
                return;
            }

            tvName.Text = user.FullName;
            vaccines = await Vaccines.GetVaccines(user);
            if (vaccines.Count <= 0)
            {
                // TODO
                return;
            }
            sideEffects = new VaccineSideEffects[vaccines.Count];
            for (int i = 0; i < vaccines.Count; i++)
            {
                sideEffects[i] = await VaccineSideEffects.GetVaccineSideEffects(vaccines[i].IdFs);
            }
            Load();
        }

        private void Load()
        {
            for (int i = 0; i < 3; i++)
            {
                llAdd[i].Visibility = ViewStates.Gone;
                rvVaccine[i].Visibility = ViewStates.Gone;
            }
            for (int i = 0; i < (vaccines.Count > 3 ? 3 : vaccines.Count); i++)
            {
                llAdd[i].Visibility = ViewStates.Visible;
                rvVaccine[i].Visibility = ViewStates.Visible;
                tvAddDate[i].Text = vaccines[i].Date.ToString("d/M");
                adapter[i] = new SideEffectsVaccineAdapter(rvVaccine[i], sideEffects[i],
                    Resource.Layout.SingleSideEffects);
                rvVaccine[i].SetAdapter(adapter[i]);
            }
        }
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.vaccine_side_effect);
            InitializeViews();
            // Create your application here
        }
    }
}