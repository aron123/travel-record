namespace TravelRecord
{
    public interface IAsyncDatabaseConnection
    {
        /// <summary>
        /// Create the proper platform-specific path for database and return an SQLiteAsyncConnection object
        /// </summary>
        /// <returns></returns>
        SQLite.SQLiteAsyncConnection DbConnection(string dbName);
    }
}
