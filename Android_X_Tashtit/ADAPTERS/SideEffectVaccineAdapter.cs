using System;

using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MODEL;
using System.Collections.Generic;

namespace Android_X_Tashtit.ADAPTERS
{
    public class SideEffectsVaccineAdapter : BaseRecyclerAdapter<VaccineSideEffect>
    {
        public SideEffectsVaccineAdapter(RecyclerView recyclerView, VaccineSideEffects items, int? layoutId = null)
            : base(recyclerView, items, layoutId)
        { }

        protected override void OnLookupViewItems(View layout, BaseViewHolder viewHolder)
        {
            TextView name = layout.FindViewById<TextView>(Resource.Id.txtSideEffectName);

            viewHolder.AddView(name, "name");
        }

        protected override void OnUpdateView(BaseViewHolder viewHolder, VaccineSideEffect item)
        {
            viewHolder.GetView<TextView>("name").Text = item.Remarks;
        }
    }
}