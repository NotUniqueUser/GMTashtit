using System;
using System.IO;
using SQLite;

namespace DAL.SQLITE
{
    public class DataBase
    {
        private static string dbName = "AppDB.db";
        private static SQLiteConnection connection;
        private static DataBase instance;

        // lock
        private static readonly object padlock = new object();

        private DataBase()
        {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            var path = Path.Combine(documentsPath, dbName);

            connection = new SQLiteConnection(path);
        }

        public static string DbName
        {
            get => dbName;
            set
            {
                if (value != null) dbName = value;
            }
        }

        public SQLiteConnection Connection => connection;

        public static DataBase Instance
        {
            get
            {
                if (instance == null)
                    lock (padlock)
                    {
                        if (instance == null) instance = new DataBase();
                    }

                return instance;
            }
        }
    }
}