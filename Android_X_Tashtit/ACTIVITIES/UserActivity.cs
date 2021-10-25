using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HELPER;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DE.Hdodenhof.Circleimageview;
using MODEL;
using Android.Graphics;
using Android_X_Tashtit.ADAPTERS;
using Android.Provider;

namespace Android_X_Tashtit.ACTIVITIES
{
    [Activity(Label = "UserActivity")]
    public class UserActivity : BaseActivity
    {
        private CircleImageView imgUser;
        private EditText etFamily;
        private EditText etName;
        private EditText etBirthDate;
        private ImageButton btnBirthDate;
        private EditText etEmail;
        private EditText etPhone;
        private EditText etTz;
        private EditText etPassword;
        private Button btnSave;
        private Button btnCancel;
        private Spinner spnClinic;
        private Spinner spnUserTypes;
        private ArrayAdapter<string> adapterUserType;
        private ArrayAdapter<Clinic> adapterClinic;
        private DatePickerDialog datePicker;
        private User user;
        private Clinics clinics;
        private Bitmap bitMap;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.user_layout);
            InitializeViews();
        }

        protected override void InitializeViews()
        {
            imgUser = FindViewById<CircleImageView>(Resource.Id.imgUser);
            etFamily = FindViewById<EditText>(Resource.Id.etFamily);
            etName = FindViewById<EditText>(Resource.Id.etName);
            etBirthDate = FindViewById<EditText>(Resource.Id.etBirthDate);
            etTz = FindViewById<EditText>(Resource.Id.etTz);
            etPhone = FindViewById<EditText>(Resource.Id.etPhone);
            btnSave = FindViewById<Button>(Resource.Id.btnSave);
            btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            spnClinic = FindViewById<Spinner>(Resource.Id.spnClinic);
            btnBirthDate = FindViewById<ImageButton>(Resource.Id.btnBirthDate);
            etEmail = FindViewById<EditText>(Resource.Id.etEmail);
            etPassword = FindViewById<EditText>(Resource.Id.etPassword);

            spnUserTypes = FindViewById<Spinner>(Resource.Id.spnUserType);
            imgUser.Click += ImgUser_Click;
            btnBirthDate.Click += BtnBirthDate_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            clinics = new Clinics();
            adapterUserType = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem,
                Enum.GetNames(typeof(UserType)));
            if (spnUserTypes != null) spnUserTypes.Adapter = adapterUserType;
            GetExtra();
            SelectClinics();
        }

        private void ImgUser_Click(object sender, EventArgs e)
        {
            var alertDialog = new AlertDialog.Builder(this);

            alertDialog.SetTitle("Choose picture");
            alertDialog.SetCancelable(true);
            alertDialog.SetPositiveButton("Take new picture", (senderAlert, args)
                =>
            {
                Intent intent = new Intent(MediaStore.ActionImageCapture);
                StartActivityForResult(intent, 0);
            });
            alertDialog.SetNegativeButton("Choose image from gallery", (senderAlert, args)
                =>
            {
                Intent intent = new Intent();
                intent.SetType("image/*");
                intent.SetAction(Intent.ActionGetContent);
                StartActivityForResult(intent, 1);
            });
            Dialog dialog = alertDialog.Create();
            dialog?.Show();
        }

        private async void SelectClinics()
        {
            clinics = await clinics.SelectAll();
            adapterClinic = new ArrayAdapter<Clinic>(this, Android.Resource.Layout.SimpleSpinnerDropDownItem, clinics);
            spnClinic.Adapter = adapterClinic;
        }


        private void BtnBirthDate_Click(object sender, EventArgs e)
        {
            DateTime dt;
            try
            {
                if (!string.IsNullOrEmpty(etBirthDate.Text))
                {
                    var dateParts = SeparateBirth(etBirthDate);
                    dt = new DateTime(int.Parse(dateParts[2]), int.Parse(dateParts[1]),
                        int.Parse(dateParts[0]));
                    datePicker = new DatePickerDialog(this, OnDateClick, dt.Year, dt.Month - 1, dt.Day);
                    datePicker.Show();
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                dt = DateTime.Today;
                datePicker = new DatePickerDialog(this, OnDateClick, dt.Year, dt.Month - 1, dt.Day);

            }
            finally
            {
                datePicker.Show();
            }
        }

        private void OnDateClick(object sender, DatePickerDialog.DateSetEventArgs e)
        {
            etBirthDate.Text = e.Date.Day.ToString().PadLeft(2, '0') + "-" + e.Date.Month.ToString().PadLeft(2, '0') +
                               "-" + e.Date.Year;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            SetResult(Result.Canceled);
            Finish();
        }

        private void GetExtra()
        {
            if (Intent is {Extras: { }})
                if (Intent.HasExtra("user"))
                    user = (User) Serializer.ByteArrayToObject(Intent.GetByteArrayExtra("user"));

            if (user == null) return;
            etFamily.Text = user.Family;
            etTz.Text = user.Tz;
            etName.Text = user.Name;
            etPassword.Text = user.Password;
            etPhone.Text = user.Phone;
            etBirthDate.Text = user.BirthDate.Day + "-" + user.BirthDate.Month + "-" + user.BirthDate.Year;
            etEmail.Text = user.Email;
            spnClinic.SetSelection(clinics.FindIndex(item => (item.Id).ToString() == user.ClinicNo));
            if (user.Image != null)
            {
                bitMap = BitMapHelper.Base64ToBitMap(user.Image);
                imgUser.SetImageBitmap(bitMap);
            }
            else
            {
                imgUser.SetImageResource(Resource.Drawable.person);
            }
        }

        private static string[] SeparateBirth(TextView tv)
        {
            return tv.Text?.Split(new char[] {'/', '.', '-', ' '});
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                user = new User();
                var dateParts = SeparateBirth(etBirthDate);
                user.Family = etFamily.Text;
                user.Name = etName.Text;
                user.Phone = etPhone.Text;
                user.Email = etEmail.Text;
                user.Type = Enum.TryParse(spnUserTypes.SelectedItem?.ToString(), out UserType ut) ? ut : UserType.USER;
                user.Tz = etTz.Text;
                user.Password = etPassword.Text;
                user.BirthDate = new DateTime(Convert.ToInt32((dateParts[2])), Convert.ToInt32((dateParts[1])),
                    Convert.ToInt32((dateParts[0])));
                user.ClinicNo = clinics[spnClinic.SelectedItemPosition].Id.ToString();
                user.Image = bitMap != null ? BitMapHelper.BitMapToBase64(bitMap) : null;

                if (user.Validate())
                {
                    var intent = new Intent();
                    intent.PutExtra("USER", Serializer.ObjectToByteArray(user));
                    SetResult(Result.Ok, intent);
                    Finish();
                }
                else
                {
                    Toast.MakeText(this, "could not validate user", ToastLength.Long)?.Show();
                }
            }
            catch (Exception ex)
            {
                Toast.MakeText(this, "saving error, check the input", ToastLength.Long)?.Show();
            }
        }


        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (resultCode == Result.Ok)
            {
                if (requestCode == 0)
                {
                    bitMap = (Bitmap) data.Extras?.Get("data");
                    imgUser.SetImageBitmap(bitMap);
                }
                else if (requestCode == 1)
                {

                    var uri = data.Data;
                    if (uri != null)
                    {
                        var source = ImageDecoder.CreateSource(ContentResolver, uri);
                        bitMap = ImageDecoder.DecodeBitmap(source);
                    }

                    bitMap = BitMapHelper.ReszieBitmap(bitMap, imgUser.Width);
                    imgUser.SetImageBitmap(bitMap);

                }
            }
        }
    }
}