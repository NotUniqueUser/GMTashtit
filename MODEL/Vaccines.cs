using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.FIRESTORE;

namespace MODEL
{
    public class Vaccines : BaseList_FS<Vaccine>
    {
        public static async Task<Vaccines> SelectAll()
        {
            return await FireStoreDbTable<Vaccine, Vaccines>.SelectAll();
        }

        public override bool Exist(Vaccine entity)
        {
             return FindAll(item => item.UserNo.Equals(entity.UserNo) && item.UserNo.Equals(entity.Date))[0] != null;
        }

        public override void Sort()
        {
            base.Sort((Vaccine v1, Vaccine v2) => v1.Date.Ticks.CompareTo(v2.Date.Ticks));
        }

        public static async Task<Vaccines> GetVaccines(User user)
        {
            var vaccines =  await FireStoreDbTable<Vaccine, Vaccines>.Query("UserNo", user.Tz);
            if (vaccines == null)
                return new Vaccines();
            vaccines.Sort();
            return vaccines;
        }

        public new async Task<bool> Save()
        {
            GenereteUpdateLists();

            if (InsertList.Count > 0)
                foreach (Vaccine v in InsertList)
                    await FireStoreDbTable<Vaccine, Vaccines>.Insert(v);

            if (UpdateList.Count > 0)
                foreach (Vaccine v in UpdateList)
                    await FireStoreDbTable<Vaccine, Vaccines>.Update(v);

            if (DeleteList.Count > 0)
                foreach (Vaccine v in DeleteList)
                    await FireStoreDbTable<Vaccine, Vaccines>.Delete(v);

            return base.Save();
        }
    }
}