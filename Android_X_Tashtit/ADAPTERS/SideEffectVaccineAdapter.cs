using System;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MODEL;
using System.Collections.Generic;

namespace Android_X_Tashtit.ADAPTERS
{
    public class SideEffectsVaccineAdapter : BaseRecyclerAdapter<string>
    {
        public SideEffectsVaccineAdapter(RecyclerView recyclerView, List<string> items, int? layoutId = null)
            : base(recyclerView, items, layoutId)
        { }

        protected override void OnLookupViewItems(View layout, BaseViewHolder viewHolder)
        {
            TextView name = layout.FindViewById<TextView>(Resource.Id.txtSideEffectName);

            viewHolder.AddView(name, "name");
        }

        protected override void OnUpdateView(BaseViewHolder viewHolder, string item)
        {
            viewHolder.GetView<TextView>("name").Text = item;
        }
    }
}