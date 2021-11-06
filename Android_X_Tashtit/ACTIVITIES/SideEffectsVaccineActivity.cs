using System;
using System.Linq;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
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
        private SideEffectsVaccineAdapter[] adapter;
        private AppCompatImageButton[] btnAdd;
        private Button btnLoad;
        private EditText etId;
        private LinearLayout[] llAdd;
        private RecyclerView[] rvVaccine;
        private VaccineSideEffects[] sideEffects;
        private TextView[] tvAddDate;
        private TextView tvName;
        private User user;
        private Vaccines vaccines;

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
            for (var i = 0; i < 3; i++)
            {
                llAdd[i] = FindViewById<LinearLayout>(Resources.GetIdentifier($"llAdd{i + 1}", "id", PACKAGE_NAME));
                tvAddDate[i] = FindViewById<TextView>(Resources.GetIdentifier($"tvAddDate{i + 1}", "id", PACKAGE_NAME));
                btnAdd[i] = FindViewById<AppCompatImageButton>(Resources.GetIdentifier($"btnAdd{i + 1}", "id",
                    PACKAGE_NAME));
                rvVaccine[i] =
                    FindViewById<RecyclerView>(Resources.GetIdentifier($"rvVaccine{i + 1}", "id", PACKAGE_NAME));
                btnAdd[i].Tag = i;
                btnAdd[i].Click += BtnAddOnClick;
            }

            btnLoad.Click += BtnLoadOnClick;
        }

        private async void BtnAddOnClick(object sender, EventArgs e)
        {
            var effects = await SideEffects.SelectAll();
            var items = new string[effects.Count];
            for (var i = 0; i < items.Length; i++) items[i] = effects[i].Name;

            var pos = int.Parse(((AppCompatImageButton) sender).Tag.ToString());

            var b = new AlertDialog.Builder(this);
            b.SetTitle("Select side effect");

            async void AddSideEffectHandler(object o, DialogClickEventArgs args)
            {
                var se = new VaccineSideEffect(effects[args.Which].IdFs, vaccines[pos].IdFs, effects[args.Which].Name);
                sideEffects[pos].Add(se);
                await sideEffects[pos].Save();
                Load();
            }

            b.SetItems(
                items,
                AddSideEffectHandler);
            b.SetNegativeButton("Cancel", (o, args) => { b.Dispose(); });
            b.SetCancelable(false);
            b.Show();
        }

        private async void BtnLoadOnClick(object sender, EventArgs e)
        {
            var progress = Global.SetProgress(this, message: "Loading user");
            if (string.IsNullOrEmpty(etId.Text))
                return;
            user = await Users.GetUser(etId.Text);
            if (user == null)
            {
                Toast.MakeText(this, "User not found", ToastLength.Long).Show();
                return;
            }

            tvName.Text = user.FullName;
            vaccines = await Vaccines.GetVaccines(user);
            if (vaccines.Count <= 0)
            {
                Toast.MakeText(this, "user has no vaccines", ToastLength.Long).Show();
                return;
            }
            sideEffects = new VaccineSideEffects[vaccines.Count];
            for (var i = 0; i < vaccines.Count; i++)
                sideEffects[i] = await VaccineSideEffects.GetVaccineSideEffects(vaccines[i].IdFs);
            progress.Dismiss();
            Keyboard.HideKeyboard(this);
            Load();
        }

        private void Load()
        {
            for (var i = 0; i < 3; i++)
            {
                llAdd[i].Visibility = ViewStates.Gone;
                rvVaccine[i].Visibility = ViewStates.Gone;
            }

            for (var i = 0; i < (vaccines.Count > 3 ? 3 : vaccines.Count); i++)
            {
                sideEffects[i].Sort();
                llAdd[i].Visibility = ViewStates.Visible;
                rvVaccine[i].Visibility = ViewStates.Visible;
                tvAddDate[i].Text = vaccines[i].Date.ToString("d/M");
                adapter[i] = new SideEffectsVaccineAdapter(rvVaccine[i], sideEffects[i],
                    Resource.Layout.SingleSideEffects);
                rvVaccine[i].SetLayoutManager(new LinearLayoutManager(this));
                adapter[i].LongItemSelected += DeleteSideEffect;
                adapter[i].NotifyDataSetChanged();
            }
        }

        private void DeleteSideEffect(object sender, VaccineSideEffect e)
        {
            async void Delete(bool del)
            {
                if (!del)
                    return;
                var pos = vaccines.FindIndex(i => i.IdFs.Equals(e.VaccineNo));
                sideEffects[pos].Delete(e);
                await sideEffects[pos].Save();
                Load();
            }

            Global.YesNoAlertDialog(this,
                "Confirm side effect delete",
                $"{e.Remarks} will be deleted",
                "Delete",
                "Cancel", Delete);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.vaccine_side_effect);
            InitializeViews();
        }
    }
}