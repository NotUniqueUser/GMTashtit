using System;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android_X_Tashtit.ADAPTERS;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using HELPER;
using MODEL;

namespace Android_X_Tashtit.ACTIVITIES
{
    [Activity(Label = "SideEffectsActivity")]
    public class SideEffectsActivity : BaseActivity
    {
        private SideEffectsAdapter adapter;
        private ImageButton btnCancel;
        private ImageButton btnOk;
        private EditText etSideEffect;
        private FloatingActionButton fabAdd;
        private LinearLayout llData;
        private SideEffect oldSideEffect;

        private int position = -1;
        private RecyclerView rvSideEffects;

        private SideEffects sideEffects;
        private TextView txtHeader;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.SideEffects);

            InitializeViews();

            txtHeader.Text = "Side Effects list";
            etSideEffect.Hint = "New side effect";

            var progress = Global.SetProgress(this);
            sideEffects = await SideEffects.SelectAll();
            SetupRecyclerView();
            progress.Dismiss();

            Keyboard.HideKeyboard(this, true);

            /*
             cities.FetchAndListen();
            adapter.NotifyDataSetChanged();
            */
        }

        protected override void InitializeViews()
        {
            rvSideEffects = FindViewById<RecyclerView>(Resource.Id.lvSideEffects);
            etSideEffect = FindViewById<EditText>(Resource.Id.etSideEffect);
            btnOk = FindViewById<ImageButton>(Resource.Id.btnOk);
            btnCancel = FindViewById<ImageButton>(Resource.Id.btnCancel);
            txtHeader = FindViewById<TextView>(Resource.Id.txtSideEffects);
            fabAdd = FindViewById<FloatingActionButton>(Resource.Id.fabAddSE);
            llData = FindViewById<LinearLayout>(Resource.Id.llData);

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += BtnCancel_Click;
            fabAdd.Click += delegate { llData.Visibility = ViewStates.Visible; };
        }

        private void SetupRecyclerView()
        {
            adapter = new SideEffectsAdapter(rvSideEffects, sideEffects, Resource.Layout.SingleSideEffects);
            rvSideEffects.SetAdapter(adapter);
            rvSideEffects.SetLayoutManager(new LinearLayoutManager(this));
            rvSideEffects.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Vertical));

            adapter.ItemSelected += Adapter_ItemSelected;
            adapter.LongItemSelected += Adapter_LongItemSelected;
        }

        private void Adapter_LongItemSelected(object sender, SideEffect e)
        {
            position = sideEffects.FindIndex(item => item.Name == e.Name);
            Global.YesNoAlertDialog(this,
                "Confirm Delete",
                "Once '" + sideEffects[position].Name + "' deleted the move cannot be undone",
                "Yes",
                "No",
                Delete);
        }

        private async void Delete(bool obj)
        {
            if (obj)
            {
                var sideEffect = sideEffects[position];
                sideEffects.Delete(sideEffect);
                await sideEffects.Save();

                adapter.NotifyDataSetChanged();
            }
        }

        private void Adapter_ItemSelected(object sender, SideEffect e)
        {
            position = sideEffects.FindIndex(item => item.Name == e.Name);
            oldSideEffect = e;
            etSideEffect.Text = e.Name;
            llData.Visibility = ViewStates.Visible;

            Keyboard.HideKeyboard(this);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            etSideEffect.Text = "";
            position = -1;
            oldSideEffect = null;

            llData.Visibility = ViewStates.Gone;
            Keyboard.HideKeyboard(this, true);
        }

        protected override async void OnStop()
        {
            base.OnStop();
            await sideEffects.Save();
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            var isNew = position == -1;
            var dataSetChanged = false;

            llData.Visibility = ViewStates.Visible;
            Keyboard.HideKeyboard(this);

            if (etSideEffect.Text == "") return;
            var sideEffect = new SideEffect(etSideEffect.Text);

            if (sideEffect.Validate())
            {
                if (isNew)
                {
                    if (sideEffects.Add(sideEffect))
                        dataSetChanged = true;
                }
                else
                {
                    if (sideEffects.Modify(oldSideEffect, sideEffect))
                        dataSetChanged = true;
                }

                if (dataSetChanged)
                {
                    etSideEffect.Text = "";
                    position = -1;

                    adapter.NotifyDataSetChanged();

                    llData.Visibility = ViewStates.Gone;
                    Keyboard.HideKeyboard(this, true);
                }
                else
                {
                    var alertDialog = new AlertDialog.Builder(this);

                    alertDialog.SetTitle("Exists");
                    alertDialog.SetMessage(sideEffect.Name + " already exists");

                    alertDialog.SetCancelable(true);

                    alertDialog.SetPositiveButton("OK", (senderAlert, args)
                        =>
                    {
                        alertDialog.Dispose();
                    });

                    Dialog dialog = alertDialog.Create();
                    dialog?.Show();
                }
            }
            else
            {
                Toast.MakeText(this, "side effect not validates", ToastLength.Short)?.Show();
            }
        }
    }
}