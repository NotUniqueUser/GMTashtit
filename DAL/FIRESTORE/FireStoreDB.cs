using Android.App;
using Firebase;
using Plugin.CloudFirestore;

namespace DAL.FIRESTORE
{
    public class FireStoreDB
    {
        private static FireStoreDB instance;
        private static IFirestore connection;

        private static readonly object padlock = new object();
        public FirebaseApp app;

        private FireStoreDB()
        {
            // app = FirebaseApp.InitializeApp((Context)AppInfo.PackageName);

            var options = new FirebaseOptions.Builder()
                .SetProjectId("confident-sweep-298817")
                .SetApplicationId("confident-sweep-298817")
                .SetApiKey("AIzaSyCXm9HJJwpHxSkDAhWWC8bdi2VG8hALSt0")
                .SetStorageBucket("")
                .Build();

            app = FirebaseApp.InitializeApp(Application.Context, options);

            connection = CrossCloudFirestore.Current.Instance;
        }

        public static IFirestore Connection
        {
            get
            {
                if (connection == null)
                    lock (padlock)
                    {
                        if (connection == null) instance = new FireStoreDB();
                    }

                return connection;
            }
        }

        public static FireStoreDB Instance
        {
            get
            {
                if (instance == null)
                    lock (padlock)
                    {
                        if (instance == null) instance = new FireStoreDB();
                    }

                return instance;
            }
        }
    }
}