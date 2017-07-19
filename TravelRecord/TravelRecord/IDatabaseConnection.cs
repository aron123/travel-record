namespace TravelRecord
{
    public interface IDatabaseConnection
    {
        /// <summary>
        /// Return the proper connection string
        /// </summary>
        /// <returns></returns>
        SQLite.SQLiteConnection DbConnection(string dbName);
    }
}
