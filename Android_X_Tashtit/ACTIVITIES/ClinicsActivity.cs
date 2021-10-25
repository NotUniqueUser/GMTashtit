using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android_X_Tashtit.ADAPTERS;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using HELPER;
using MODEL;

namespace Android_X_Tashtit.ACTIVITIES
{
    [Activity(Label = "ClinicsActivity")]
    public class ClinicsActivity : BaseActivity
    {
        private RecyclerView lvClinics;
        private EditText etClinic;
        private ImageButton btnOk;
        private ImageButton btnCancel;
        private TextView txtHeader;
        private LinearLayout llData;
        private FloatingActionButton fabAdd;

        private Clinics clinics;
        private Clinic oldClinic;
        private ClinicsAdapter adapter;

        int position = -1;

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your application here
            SetContentView(Resource.Layout.Clinics);

            InitializeViews();

            txtHeader.Text = "Clinics list";
            etClinic.Hint = "New clinic";

            clinics = new Clinics();
            ProgressDialog progress = Global.SetProgress(this);
            clinics = await clinics.SelectAll();
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
            lvClinics = FindViewById<RecyclerView>(Resource.Id.lvClinics);
            etClinic = FindViewById<EditText>(Resource.Id.etClinics);
            btnOk = FindViewById<ImageButton>(Resource.Id.btnOk);
            btnCancel = FindViewById<ImageButton>(Resource.Id.btnCancel);
            txtHeader = FindViewById<TextView>(Resource.Id.txtHeaderClinics);
            fabAdd = FindViewById<FloatingActionButton>(Resource.Id.fabAddClinics);
            llData = FindViewById<LinearLayout>(Resource.Id.llData);

            btnOk.Click += BtnOk_Click;
            btnCancel.Click += BtnCancel_Click;
            fabAdd.Click += delegate { llData.Visibility = ViewStates.Visible; };
        }

        private void SetupRecyclerView()
        {
            adapter = new ClinicsAdapter(lvClinics, clinics, Resource.Layout.SingleClinic);
            lvClinics.SetAdapter(adapter);
            lvClinics.SetLayoutManager(new LinearLayoutManager(this));
            lvClinics.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Vertical));

            adapter.ItemSelected += Adapter_ItemSelected;
            adapter.LongItemSelected += Adapter_LongItemSelected; ;
        }

        private void Adapter_LongItemSelected(object sender, Clinic e)
        {
            position = clinics.FindIndex(item => item.Name == e.Name);
            Global.YesNoAlertDialog(this,
                                    "Confim Delete",
                                    "Once '" + clinics[position].Name + "' deleted the move cannot be undone",
                                    "Yes",
                                    "No",
                                    Delete);
        }

        private async void Delete(bool obj)
        {
            if (obj)
            {
                Clinic clinic = clinics[position];
                clinics.Delete(clinic);
                await clinics.Save();
                adapter.NotifyDataSetChanged();
            }
        }

        private void Adapter_ItemSelected(object sender, Clinic e)
        {
            position = clinics.FindIndex(item => item.Name == e.Name);
            oldClinic = e;
            etClinic.Text = e.Name;
            llData.Visibility = ViewStates.Visible;

            Keyboard.HideKeyboard(this, false);
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            etClinic.Text = "";
            position = -1;
            oldClinic = null;

            llData.Visibility = ViewStates.Gone;
            Keyboard.HideKeyboard(this, true);
        }

        protected override async void OnStop()
        {
            base.OnStop();
            await clinics.Save();
        }


        private void BtnOk_Click(object sender, EventArgs e)
        {
            bool isNew = position == -1;
            bool dataSetChanged = false;

            llData.Visibility = ViewStates.Visible;
            Keyboard.HideKeyboard(this);

            if (etClinic.Text != "")
            {
                Clinic clinic = new Clinic(etClinic.Text);

                if (clinic.Validate())
                {
                    if (isNew)
                    {
                        if (clinics.Add(clinic))
                            dataSetChanged = true;
                    }
                    else
                    {
                        if (clinics.Modify(oldClinic, clinic))
                            dataSetChanged = true;
                    }

                    if (dataSetChanged)
                    {
                        etClinic.Text = "";
                        position = -1;

                        adapter.NotifyDataSetChanged();

                        llData.Visibility = ViewStates.Gone;
                        Keyboard.HideKeyboard(this, true);
                    }
                    else
                    {
                        AlertDialog.Builder alertDiag = new AlertDialog.Builder(this);

                        alertDiag.SetTitle("Exists");
                        alertDiag.SetMessage(clinic.Name + " already exists");

                        alertDiag.SetCancelable(true);

                        alertDiag.SetPositiveButton("OK", (senderAlert, args)
                        =>
                        {
                            alertDiag.Dispose();
                        });

                        Dialog diag = alertDiag.Create();
                        diag.Show();
                    }
                }
                else
                {
                    Toast.MakeText(this, "City not validates", ToastLength.Short).Show();
                }
            }
        
        }
    }
}