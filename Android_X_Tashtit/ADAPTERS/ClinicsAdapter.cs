using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MODEL;
using System;
using System.Collections.Generic;

namespace Android_X_Tashtit.ADAPTERS
{
    public class ClinicsAdapter : BaseRecyclerAdapter<Clinic>
    {
        public ClinicsAdapter(RecyclerView recyclerView, List<Clinic> items, int? layoutId = null)
            : base(recyclerView, items, layoutId)
        { }
        protected override void OnLookupViewItems(View layout, BaseViewHolder viewHolder)
        {
            TextView name = layout.FindViewById<TextView>(Resource.Id.txtClinicsName);

            viewHolder.AddView(name, "name");
        }

        protected override void OnUpdateView(BaseViewHolder viewHolder, Clinic item)
        {
            viewHolder.GetView<TextView>("name").Text = item.Name;
        }
    }
}