namespace TravelRecord
{
    public interface IDatabaseConnection
    {
        /// <summary>
        /// Create the proper platform-specific path for database and return an SQLiteConnection object
        /// </summary>
        /// <returns></returns>
        SQLite.SQLiteConnection DbConnection(string dbName);
    }
}
