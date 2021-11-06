using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Object = Java.Lang.Object;

namespace Android_X_Tashtit.ADAPTERS
{
    public abstract class BaseRecyclerAdapter<T> : RecyclerView.Adapter
    {
        private readonly List<T> items;
        private readonly int? layoutId;
        private readonly RecyclerView recyclerView;

        protected BaseRecyclerAdapter(RecyclerView recyclerView, List<T> items, int? layoutId = null)
        {
            this.items = items;
            //items.CollectionChanged += this.OnCollectionChanged;
            this.layoutId = layoutId;
            this.recyclerView = recyclerView;
            if (recyclerView != null)
            {
                this.recyclerView.SetAdapter(this);
                this.recyclerView.AddOnChildAttachStateChangeListener(new AttachStateChangeListener(this));
            }
        }

        public override int ItemCount => items != null ? items.Count : 0;

        public event EventHandler<T> ItemSelected;

        protected virtual void OnItemSelected(T e)
        {
            ItemSelected?.Invoke(this, e);
        }

        public event EventHandler<T> LongItemSelected;

        protected virtual void OnLongItemSelected(T e)
        {
            LongItemSelected?.Invoke(this, e);
        }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var baseViewHolder = (BaseViewHolder) holder;
            OnUpdateView(baseViewHolder, items[position]);
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            var viewId = OnGetViewId(viewType);
            var layout = LayoutInflater.From(parent.Context).Inflate(viewId, parent, false);


            var genericViewHolder = new BaseViewHolder(layout);

            OnLookupViewItems(layout, genericViewHolder);
            return genericViewHolder;
        }

        public override int GetItemViewType(int position)
        {
            return GetViewIdForType(items[position]);
        }

        protected virtual int OnGetViewId(int viewType)
        {
            if (layoutId == null)
                throw new InvalidOperationException(
                    "No layoutId provided on adapter constructor, you need to override OnGetViewId and provide a valid resource is for this viewType");

            return layoutId.Value;
        }

        protected abstract void OnLookupViewItems(View layout, BaseViewHolder viewHolder);

        protected abstract void OnUpdateView(BaseViewHolder viewHolder, T item);

        protected virtual int GetViewIdForType(T item)
        {
            return 0;
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    NotifyItemInserted(e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    NotifyItemRemoved(e.OldStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Replace:
                    NotifyItemChanged(e.OldStartingIndex);
                    NotifyItemChanged(e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Move:
                    NotifyItemRemoved(e.OldStartingIndex);
                    NotifyItemRemoved(e.NewStartingIndex);
                    break;
                case NotifyCollectionChangedAction.Reset:
                    NotifyDataSetChanged();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void RemoveItem(int position)
        {
            items.RemoveAt(position);
            // ?? NotifyDataSetChanged();
            // ?? NotifyItemChanged(position);
        }

        /// <summary>
        ///     Subscribes to view click so that we can have ItemSelected w/o any custom code
        /// </summary>
        /// <seealso cref="Android.Support.V7.Widget.RecyclerView.Adapter" />
        internal class AttachStateChangeListener : Object, RecyclerView.IOnChildAttachStateChangeListener
        {
            private readonly BaseRecyclerAdapter<T> parentAdapter;

            public AttachStateChangeListener(BaseRecyclerAdapter<T> parentAdapter)
            {
                this.parentAdapter = parentAdapter;
            }

            public void OnChildViewAttachedToWindow(View view)
            {
                view.Click += View_Click;
                view.LongClick += View_LongClick;
            }

            public void OnChildViewDetachedFromWindow(View view)
            {
                view.Click -= View_Click;
                view.LongClick -= View_LongClick;
            }

            private void View_Click(object sender, EventArgs e)
            {
                var holder = (BaseViewHolder) parentAdapter.recyclerView.GetChildViewHolder((View) sender);
                var clickedPosition = holder.AdapterPosition;
                parentAdapter.OnItemSelected(parentAdapter.items[clickedPosition]);
            }

            private void View_LongClick(object sender, EventArgs e)
            {
                var holder = (BaseViewHolder) parentAdapter.recyclerView.GetChildViewHolder((View) sender);
                var clickedPosition = holder.AdapterPosition;
                parentAdapter.OnLongItemSelected(parentAdapter.items[clickedPosition]);
            }
        }
    }
}