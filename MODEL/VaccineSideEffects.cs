using System.Threading.Tasks;
using DAL.FIRESTORE;

namespace MODEL
{
    public class VaccineSideEffects : BaseList_FS<VaccineSideEffect>
    {
        public static async Task<VaccineSideEffects> SelectAll()
        {
            var vaccineSideEffects =
                await FireStoreDbTable<VaccineSideEffect, VaccineSideEffects>.SelectAll("vaccineNo");
            return vaccineSideEffects;
        }

        public static async Task<VaccineSideEffects> GetVaccineSideEffects(string id)
        {
            var vaccineSideEffects =
                await FireStoreDbTable<VaccineSideEffect, VaccineSideEffects>.Query("VaccineNo", id);
            return vaccineSideEffects ?? new VaccineSideEffects();
        }

        public override bool Exist(VaccineSideEffect entity, out VaccineSideEffect existEntity)
        {
            existEntity = Find(item => item.SideEfectNo.Equals(entity.SideEfectNo));
            return existEntity != null;
        }

        public override void Sort()
        {
            base.Sort((item1, item2) => item1.Remarks.CompareTo(item2.Remarks));
        }

        public new async Task<bool> Save()
        {
            GenereteUpdateLists();

            if (InsertList.Count > 0)
                foreach (var v in InsertList)
                    await FireStoreDbTable<VaccineSideEffect, VaccineSideEffects>.Insert(v);

            if (UpdateList.Count > 0)
                foreach (var v in UpdateList)
                    await FireStoreDbTable<VaccineSideEffect, VaccineSideEffects>.Update(v);

            if (DeleteList.Count > 0)
                foreach (var v in DeleteList)
                    await FireStoreDbTable<VaccineSideEffect, VaccineSideEffects>.Delete(v);

            return base.Save();
        }
    }
}