using System;
using System.Threading.Tasks;
using DAL.FIRESTORE;

namespace MODEL
{
    public class VaccineSideEffects : BaseList<VaccineSideEffect>
    {
        public VaccineSideEffects()
        {
        }

        public async Task<VaccineSideEffects> SelectAll()
        {
            VaccineSideEffects vaccineSideEffects = await FireStoreDbTable<VaccineSideEffect, VaccineSideEffects>.SelectAll("vaccineNo", Order_By_Direction.ACSCENDING);
            return vaccineSideEffects;
        }
        public override bool Exist(VaccineSideEffect entity, out VaccineSideEffect existEntity)
        {
            existEntity = Find(item => item.SideEfectNo.Equals(entity.SideEfectNo));
            return existEntity != null;
        }

        protected override void Sort()
        {
            base.Sort((item1, item2) => item1.VaccineNo.CompareTo(item2.VaccineNo));
        }
        public async Task<bool> Save()
        {
            GenereteUpdateLists();

            if (InsertList.Count > 0)
                foreach (VaccineSideEffect v in InsertList)
                    await FireStoreDbTable<VaccineSideEffect, VaccineSideEffects>.Insert(v);

            if (UpdateList.Count > 0)
                foreach (VaccineSideEffect v in UpdateList)
                    await FireStoreDbTable<VaccineSideEffect, VaccineSideEffects>.Update(v);

            if (DeleteList.Count > 0)
                foreach (VaccineSideEffect v in DeleteList)
                    await FireStoreDbTable<VaccineSideEffect, VaccineSideEffects>.Delete(v);

            return base.Save();
        }

    }
}

