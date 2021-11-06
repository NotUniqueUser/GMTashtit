using System.Collections.Generic;
using Android.Views;
using Android.Widget;
using AndroidX.RecyclerView.Widget;
using MODEL;

namespace Android_X_Tashtit.ADAPTERS
{
    public class CitiesAdapter : BaseRecyclerAdapter<City>
    {
        public CitiesAdapter(RecyclerView recyclerView, List<City> items, int? layoutId = null)
            : base(recyclerView, items, layoutId)
        {
        }

        protected override void OnLookupViewItems(View layout, BaseViewHolder viewHolder)
        {
            var name = layout.FindViewById<TextView>(Resource.Id.txtName);

            viewHolder.AddView(name, "name");
        }

        protected override void OnUpdateView(BaseViewHolder viewHolder, City item)
        {
            viewHolder.GetView<TextView>("name").Text = item.Name;
        }
    }
}