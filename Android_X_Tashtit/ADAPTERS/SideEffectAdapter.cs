using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MODEL;

namespace Android_X_Tashtit.ADAPTERS
{
    public class SideEffectsAdapter : BaseRecyclerAdapter<SideEffect>
    {
        public SideEffectsAdapter(RecyclerView recyclerView, List<SideEffect> items, int? layoutId = null)
            : base(recyclerView, items, layoutId)
        {
        }

        protected override void OnLookupViewItems(View layout, BaseViewHolder viewHolder)
        {
            var name = layout.FindViewById<TextView>(Resource.Id.txtSideEffectName);

            viewHolder.AddView(name, "name");
        }

        protected override void OnUpdateView(BaseViewHolder viewHolder, SideEffect item)
        {
            viewHolder.GetView<TextView>("name").Text = item.Name;
        }
    }
}