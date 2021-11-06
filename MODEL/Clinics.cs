using System.Threading.Tasks;
using DAL.FIRESTORE;

namespace MODEL
{
    public class Clinics : BaseList_FS<Clinic>
    {
        public override bool Exist(Clinic entity, out Clinic existEntity)
        {
            existEntity = Find(item => item.Name.Equals(entity.Name));
            return existEntity != null;
        }

        public override void Sort()
        {
            base.Sort((item1, item2) => item1.Name.CompareTo(item2.Name));
        }

        public async Task<Clinics> SelectAll()
        {
            var sideEffects = await FireStoreDbTable<Clinic, Clinics>.SelectAll("Name");
            return sideEffects;
        }

        public async Task<bool> Save()
        {
            GenereteUpdateLists();

            if (InsertList.Count > 0)
                foreach (var c in InsertList)
                    await FireStoreDbTable<Clinic, Clinics>.Insert(c);

            if (UpdateList.Count > 0)
                foreach (var c in UpdateList)
                    await FireStoreDbTable<Clinic, Clinics>.Update(c);

            if (DeleteList.Count > 0)
                foreach (var c in DeleteList)
                    await FireStoreDbTable<Clinic, Clinics>.Delete(c);

            return base.Save();
        }
    }
}