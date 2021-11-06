using System;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using Android_X_Tashtit.ADAPTERS;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using HELPER;
using MODEL;

namespace Android_X_Tashtit.ACTIVITIES
{
    [Activity(Label = "UsersActivity")]
    public class UsersActivity : BaseActivity
    {
        private const int AddUserId = 1;
        private const int EditUserId = 2;
        private FloatingActionButton fabAddUser;
        private User oldUser;
        private RecyclerView rvUsers;

        private Users users;
        private UsersAdapter usersAdapter;

        protected override void InitializeViews()
        {
            users = new Users();
            fabAddUser = FindViewById<FloatingActionButton>(Resource.Id.fabAddUser);
            rvUsers = FindViewById<RecyclerView>(Resource.Id.rvUsers);
            fabAddUser.Click += FabAddUser_Click;

            RefreshUsers();
        }

        private async void RefreshUsers()
        {
            users = await Users.SelectAll();
            usersAdapter = new UsersAdapter(rvUsers, users, Resource.Layout.single_user_layout);
            usersAdapter.ItemSelected += EditUser;
            usersAdapter.LongItemSelected += DeleteUser;
            rvUsers.SetAdapter(usersAdapter);
            rvUsers.SetLayoutManager(new LinearLayoutManager(this));
            rvUsers.AddItemDecoration(new DividerItemDecoration(this, DividerItemDecoration.Vertical));
        }

        private void DeleteUser(object sender, User e)
        {
            oldUser = e;
            Global.YesNoAlertDialog(this,
                "Confirm deleting user " + e.FullName,
                "this operation cannot be undone",
                "Confirm",
                "Cancel",
                Delete);
        }

        private async void Delete(bool obj)
        {
            if (!obj) return;
            users.Delete(oldUser);
            await users.Save();
            usersAdapter.NotifyDataSetChanged();
        }

        private void EditUser(object sender, User e)
        {
            oldUser = e;
            var intent = new Intent(this, typeof(UserActivity));
            intent.PutExtra("user", Serializer.ObjectToByteArray(e));
            StartActivityForResult(intent, EditUserId);
        }

        private void FabAddUser_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(UserActivity));
            StartActivityForResult(intent, AddUserId);
        }

        protected override async void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            if (resultCode != Result.Ok)
                return;

            var user = Serializer.ByteArrayToObject(data.GetByteArrayExtra("USER")) as User;
            switch (requestCode)
            {
                case AddUserId when !users.Exist(user):
                    users.Add(user);
                    await users.Save();
                    break;
                case AddUserId:
                    Toast.MakeText(this, "user already exists with the same phone number", ToastLength.Long)?.Show();
                    break;
                case EditUserId:
                    user.IdFs = oldUser.IdFs;
                    users.Modify(oldUser, user);
                    await users.Save();
                    break;
            }

            usersAdapter.NotifyDataSetChanged();
            base.OnActivityResult(requestCode, resultCode, data);
        }

        protected override async void OnStop()
        {
            base.OnStop();
            await users.Save();
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.users_layout);
            InitializeViews();
        }
    }
}