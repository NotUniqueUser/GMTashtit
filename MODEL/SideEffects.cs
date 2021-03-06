using System.Threading.Tasks;
using DAL.FIRESTORE;

namespace MODEL
{
    public class SideEffects : BaseList<SideEffect>
    {
        public override bool Exist(SideEffect entity, out SideEffect existEntity)
        {
            existEntity = Find(item => item.Name.Equals(entity.Name));
            return existEntity != null;
        }

        protected override void Sort()
        {
            base.Sort((item1, item2) => item1.Name.CompareTo(item2.Name));
        }

        public static async Task<SideEffects> SelectAll()
        {
            var sideEffects = await FireStoreDbTable<SideEffect, SideEffects>.SelectAll("Name");
            return sideEffects;
        }

        public static async Task<SideEffects> SelectById(string idFs)
        {
            var sideEffects = await FireStoreDbTable<SideEffect, SideEffects>.QueryById(idFs);
            return sideEffects ?? new SideEffects();
        }

        public async Task<bool> Save()
        {
            GenereteUpdateLists();

            if (InsertList.Count > 0)
                foreach (var s in InsertList)
                    await FireStoreDbTable<SideEffect, SideEffects>.Insert(s);

            if (UpdateList.Count > 0)
                foreach (var s in UpdateList)
                    await FireStoreDbTable<SideEffect, SideEffects>.Update(s);

            if (DeleteList.Count > 0)
                foreach (var s in DeleteList)
                    await FireStoreDbTable<SideEffect, SideEffects>.Delete(s);

            return base.Save();
        }

        // public override string ToString()
        // {
        //     string effects = "";
        //     this.ForEach(item => effects += item.Name + ";");
        //     return effects;
        // }
    }
}