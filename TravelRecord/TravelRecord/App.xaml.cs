using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using SQLite;

namespace TravelRecord
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            DEBUG_InitializeTables();
            SetMainPage();
        }

        protected override void OnStart()
        {
            // Handle when app starts
            SetMainPage();
        }

        protected override void OnSleep()
        {
            // Handle when app sleeps
        }

        protected override void OnResume()
        {
            // Handle when app resumes
            SetMainPage();
        }

        public bool IsCompanyDataSet()
        {
            if (Application.Current.Properties.ContainsKey("CompanyName")
                && Application.Current.Properties.ContainsKey("CompanyAddress")
                && Application.Current.Properties.ContainsKey("CompanyVAT"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsCarDataSet()
        {
            SQLiteConnection database;
            database = DependencyService.Get<IDatabaseConnection>().DbConnection("AppDatabase.db3");

            // Check if Car table is exist
            string IsExist = database.ExecuteScalar<string>("SELECT name FROM sqlite_master WHERE type='table' AND name='Car';"); 
            if (IsExist == null) return false;

            //Check if existing Car table is empty
            int IsEmpty = database.Query<Car>("SELECT 1 FROM Car LIMIT 1;").Count;
            if (IsEmpty == 0) return false;

            return true;
        }

        /// <summary>
        /// Set the main page of the application depending on company's and cars' data is set or unset.
        /// </summary>
        public void SetMainPage()
        {
            //TODO: * Change MainPage to travel records' page
            //      * Create algorithm to find initial car's license plate
            SQLiteConnection database;
            database = DependencyService.Get<IDatabaseConnection>().DbConnection("AppDatabase.db3");
            var initial = database.FindWithQuery<Car>("SELECT LicensePlateNumber FROM Car ORDER BY LicensePlateNumber ASC LIMIT 1");

            MainPage = new NavigationPage(new AddTravelData(initial.LicensePlateNumber)); return;

            if (IsCompanyDataSet())
            {
                if (IsCarDataSet())
                {
                    MainPage = new NavigationPage(new MainPage());
                }
                else
                {
                    MainPage = new NavigationPage(new AddCarData());
                }
            }
            else
            {
                MainPage = new NavigationPage(new AddCompanyData());
            }

        }

        private void DEBUG_InitializeTables()
        {
            Car car = new Car() { LicensePlateNumber = "ABC123", CarModel = "Sárga Ferrari" };
            Travel travel = new Travel() { CarLicensePlate = "ABC123", TravelDate = new DateTime(2017, 02, 15),
                                           StartPoint = "Litka", Destination = "Miskolc", Distance = 60 };
            SQLiteConnection database;
            database = DependencyService.Get<IDatabaseConnection>().DbConnection("AppDatabase.db3");

            database.DropTable<Car>();
            database.DropTable<Travel>();

            database.CreateTable<Car>();
            database.CreateTable<Travel>();

            database.Insert(car);
            database.Insert(travel);

            database.Close();
        }
    }
}
