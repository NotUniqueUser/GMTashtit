using System;
using System.Threading.Tasks;
using DAL.FIRESTORE;

namespace MODEL
{
    public class SideEffects : BaseList<SideEfect>
    {
        public SideEffects()
        {
        }

        public override bool Exist(SideEfect entity, out SideEfect existEntity)
        {
            existEntity = Find(item => item.Name.Equals(entity.Name));
            return existEntity != null;
        }

        protected override void Sort()
        {
            base.Sort((item1, item2) => item1.Name.CompareTo(item2.Name));
        }
        public async Task<SideEffects> SelectAll()
        {
            SideEffects sideEffects = await FireStoreDbTable<SideEfect, SideEffects>.SelectAll("Name", Order_By_Direction.ACSCENDING);
            return sideEffects;
        }

        public async Task<bool> Save()
        {
            GenereteUpdateLists();

            if (InsertList.Count > 0)
                foreach (SideEfect s in InsertList)
                    await FireStoreDbTable<SideEfect, SideEffects>.Insert(s);

            if (UpdateList.Count > 0)
                foreach (SideEfect s in UpdateList)
                    await FireStoreDbTable<SideEfect, SideEffects>.Update(s);

            if (DeleteList.Count > 0)
                foreach (SideEfect s in DeleteList)
                    await FireStoreDbTable<SideEfect, SideEffects>.Delete(s);

            return base.Save();
        }
    }
}
