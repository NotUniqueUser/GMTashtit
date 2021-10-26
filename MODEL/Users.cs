using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Android.Graphics;
using Android.OS;
using DAL.FIRESTORE;
using HELPER;

namespace MODEL
{
    public class Users : BaseList_FS<User>
    {
        public static async Task<Users> SelectAll()
        {
            Users users = await FireStoreDbTable<User, Users>.SelectAll("Name", Order_By_Direction.ACSCENDING);
            users.Sort();

            return users;
        }

        public async Task<Bitmap> LoadPictureAsync(string id)
        {
            byte[] image = await FireStoreStorage.LoadFromStorage(id, "Images/Users");
            return BitMapHelper.ByteArrayToBitmap(image);
        }

        public new async Task<bool> Save(string imageField = null)
        {
            GenereteUpdateLists();

            if (InsertList.Count > 0)
                foreach (User u in InsertList)
                {
                    await FireStoreDbTable<User, Users>.Insert(u);

                    if (imageField != null)
                    {
                        PropertyInfo pinfo = typeof(User).GetProperty(imageField);
                        //SavePictureAsync(u.IdFs, BitMapHelper.Base64ToBitMap((string)pinfo.GetValue(u, null)));
                        await FireStoreStorage.SaveToStorage(u.IdFs, BitMapHelper.Base64ToByteArray((string)pinfo.GetValue(u, null)), "Images/Users");
                    }
                }

            if (UpdateList.Count > 0)
                foreach (User u in UpdateList)
                {
                    await FireStoreDbTable<User, Users>.Update(u);

                    if (imageField != null)
                    {
                        PropertyInfo pinfo = typeof(User).GetProperty(imageField);
                        //SavePictureAsync(u.IdFs, BitMapHelper.Base64ToBitMap((string)pinfo.GetValue(u, null)));
                        await FireStoreStorage.SaveToStorage(u.IdFs, BitMapHelper.Base64ToByteArray((string)pinfo.GetValue(u, null)), "Images/Users");
                    }
                }

            if (DeleteList.Count > 0)
                foreach (User u in DeleteList)
                {
                    await FireStoreDbTable<User, Users>.Delete(u);

                    if (imageField != null)
                    {
                        //DeletePictureAsync(u.IdFs);
                        await FireStoreStorage.DeleteFromStorage(u.IdFs, "Images/Users");
                    }
                }

            return base.Save();
        }

        public override bool Exist(User entity, out User existEntity)
        {
            existEntity = Find(item => item.Tz.Equals(entity.Tz) && item.Phone.Equals(entity.Phone));
            return existEntity != null;
        }

        public override void Sort()
        {
            base.Sort((item1, item2) =>
            {
                int compare = item1.Family.CompareTo(item2.Family);
                return (compare == 0) ? item1.Name.CompareTo(item2.Name) : compare;
            });
        }

        public static async Task<User> GetUser(string tz)
        {
            return await FireStoreDbTable<User, Users>.Select("Tz", tz);
        }
    }
}
