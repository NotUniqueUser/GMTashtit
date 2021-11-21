using System.Threading.Tasks;
using Android.Content;
using Android.Graphics;
using DAL.FIRESTORE;
using HELPER;

namespace MODEL
{
    public class Users : BaseList_FS<User>
    {
        public static async Task<Users> SelectAll()
        {
            var users = await FireStoreDbTable<User, Users>.SelectAll("Name");
            users.Sort();

            return users;
        }

        public async Task<Bitmap> LoadPictureAsync(string id)
        {
            var image = await FireStoreStorage.LoadFromStorage(id, "Images/Users");
            return BitMapHelper.ByteArrayToBitmap(image);
        }

        public async Task<bool> Save(string imageField = null)
        {
            GenereteUpdateLists();

            if (InsertList.Count > 0)
                foreach (var u in InsertList)
                {
                    await FireStoreDbTable<User, Users>.Insert(u);

                    if (imageField != null)
                    {
                        var pinfo = typeof(User).GetProperty(imageField);
                        //SavePictureAsync(u.IdFs, BitMapHelper.Base64ToBitMap((string)pinfo.GetValue(u, null)));
                        await FireStoreStorage.SaveToStorage(u.IdFs,
                            BitMapHelper.Base64ToByteArray((string) pinfo.GetValue(u, null)), "Images/Users");
                    }
                }

            if (UpdateList.Count > 0)
                foreach (var u in UpdateList)
                {
                    await FireStoreDbTable<User, Users>.Update(u);

                    if (imageField != null)
                    {
                        var pinfo = typeof(User).GetProperty(imageField);
                        //SavePictureAsync(u.IdFs, BitMapHelper.Base64ToBitMap((string)pinfo.GetValue(u, null)));
                        await FireStoreStorage.SaveToStorage(u.IdFs,
                            BitMapHelper.Base64ToByteArray((string) pinfo.GetValue(u, null)), "Images/Users");
                    }
                }

            if (DeleteList.Count > 0)
                foreach (var u in DeleteList)
                {
                    await FireStoreDbTable<User, Users>.Delete(u);

                    if (imageField != null)
                        //DeletePictureAsync(u.IdFs);
                        await FireStoreStorage.DeleteFromStorage(u.IdFs, "Images/Users");
                }

            return base.Save();
        }

        public override bool Exist(User entity, out User existEntity)
        {
            existEntity = null;
            if (entity.Phone == null || entity.Tz == null)
                return true;
            existEntity = Find(item =>
                item.Tz != null && item.Phone != null && item.Tz.Equals(entity.Tz) && item.Phone.Equals(entity.Phone));
            return existEntity != null;
        }

        public override void Sort()
        {
            base.Sort((item1, item2) =>
            {
                var compare = item1.Family.CompareTo(item2.Family);
                return compare == 0 ? item1.Name.CompareTo(item2.Name) : compare;
            });
        }

        public static async bool LoginUser(string email, string password)
        {

            return false;
        }

        public static async Task<User> GetUser(string tz)
        {
            return await FireStoreDbTable<User, Users>.Select("Tz", tz);
        }
    }
}