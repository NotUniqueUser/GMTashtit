using System;
using System.Threading.Tasks;
using Plugin.FirebaseStorage;

namespace DAL.FIRESTORE
{
    public abstract class FireStoreStorage
    {
        public static async Task SaveToStorage(string id, byte[] bytes, string path = null)
        {
            try
            {
                var x = FireStoreDB.Instance;
                var reference = CrossFirebaseStorage.Current.Instance.RootReference;

                reference = MakePath(reference, path);
                reference = reference.Child(id);

                var y = /*await*/ reference.PutBytesAsync(bytes);
            }
            catch (Exception ex)
            {
            }
        }

        public static async Task<byte[]> LoadFromStorage(string id, string path = null)
        {
            try
            {
                var x = FireStoreDB.Instance;
                var reference = CrossFirebaseStorage.Current.Instance.RootReference;

                reference = MakePath(reference, path);
                reference = reference.Child(id);

                var maxDownloadSizeBytes = 1024 * 1024;

                var bytes = await reference.GetBytesAsync(maxDownloadSizeBytes);

                return bytes;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task DeleteFromStorage(string id, string path = null)
        {
            try
            {
                var x = FireStoreDB.Instance;
                var reference = CrossFirebaseStorage.Current.Instance.RootReference;

                reference = MakePath(reference, path);
                reference = reference.Child(id);

                await reference.DeleteAsync();
            }
            catch (Exception e)
            {
            }
        }

        private static IStorageReference MakePath(IStorageReference reference, string path)
        {
            if (path != null)
            {
                var vs = path.Split('/', '\\');

                if (vs.Length > 0)
                    foreach (var v in vs)
                        reference = reference.Child(v);
            }

            return reference;
        }
    }
}