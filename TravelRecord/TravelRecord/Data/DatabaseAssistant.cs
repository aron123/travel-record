using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Forms;

namespace TravelRecord
{
    public static class DatabaseAssistant
    {
        static string dbName = "AppDatabase.db3";

        public static SQLiteConnection Connect()
        {
            try
            {
                return DependencyService.Get<IDatabaseConnection>().DbConnection(dbName);
            }
            catch (SQLiteException e)
            {
                throw e;
            }
        }
    }
}
