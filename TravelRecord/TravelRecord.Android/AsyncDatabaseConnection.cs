using SQLite;
using TravelRecord.Droid;
using System.IO;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(AsyncDatabaseConnection))]

namespace TravelRecord.Droid
{
    public class AsyncDatabaseConnection : IAsyncDatabaseConnection
    {
        /// <summary>
        /// Create the Android-specific path for the database file
        /// </summary>
        /// <param name="dbName">Name of the SQLite3 database file (e.g. Users.db3)</param>
        /// <returns>SQLiteConnection to database</returns>
        public SQLiteAsyncConnection DbConnection(string dbName)
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), dbName);

            return new SQLiteAsyncConnection(path);
        }
    }
}