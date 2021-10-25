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
using Android.Graphics;
using HELPER;

using MODEL;
using AndroidX.RecyclerView.Widget;
using DE.Hdodenhof.Circleimageview;

namespace Android_X_Tashtit.ADAPTERS
{
    public class UsersAdapter : BaseRecyclerAdapter<User>
    {
        public UsersAdapter(RecyclerView recyclerView, List<User> items, int? layoutId = null)
            : base(recyclerView, items, layoutId)
        { }

        protected override void OnLookupViewItems(View layout, BaseViewHolder viewHolder) {
            var txtEmail = layout.FindViewById<TextView>(Resource.Id.txtEmail); 
            var txtAge = layout.FindViewById<TextView>(Resource.Id.txtAge); 
            var imgUser = layout.FindViewById<CircleImageView>(Resource.Id.imgUser);
            var fullName = layout.FindViewById<TextView>(Resource.Id.txtFullName);
            viewHolder.AddView(fullName, "FullName");
            viewHolder.AddView(txtEmail ,"Email");
            viewHolder.AddView(txtAge, "Age");
            viewHolder.AddView(imgUser, "Img");
        }

        protected override void OnUpdateView(BaseViewHolder viewHolder, User item)
        {
            viewHolder.GetView<TextView>("FullName").Text = item.FullName;
            viewHolder.GetView<TextView>("Email").Text = item.Email;
            viewHolder.GetView<TextView>("Age").Text = (DateTime.Today.Year - item.BirthDate.Year).ToString();
            var itemImage = item.Image;
            if (itemImage != null)
                viewHolder.GetView<CircleImageView>("Img").SetImageBitmap(BitMapHelper.Base64ToBitMap(itemImage));
            else
                viewHolder.GetView<CircleImageView>("Img").SetImageResource(Resource.Drawable.person);
            
        }
    }
}