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
            //DEBUG_InitializeTables();
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
            if (IsCompanyDataSet())
            {
                if (IsCarDataSet())
                {
                    MainPage = new NavigationPage(new ListTravels());
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
            Car car2 = new Car() { LicensePlateNumber = "XYZ987", CarModel = "Trabant 601" };
            Travel travel0 = new Travel()
            {
                CarLicensePlate = "ABC123",
                TravelDate = new DateTime(2017, 02, 15),
                StartPoint = "Encs",
                Destination = "Miskolc",
                Distance = 60
            };

            Travel travel1 = new Travel()
            {
                CarLicensePlate = "ABC123",
                TravelDate = new DateTime(2015, 05, 15),
                StartPoint = "Miskolc",
                Destination = "Encs",
                Distance = 60
            };

            Travel travel2 = new Travel()
            {
                CarLicensePlate = "ABC123",
                TravelDate = new DateTime(2020, 12, 31),
                StartPoint = "Budapest",
                Destination = "Hatvan",
                Distance = 59
            };

            Travel travel3 = new Travel()
            {
                CarLicensePlate = "ABC123",
                TravelDate = new DateTime(2007, 05, 06),
                StartPoint = "Győr",
                Destination = "Szeged",
                Distance = 280
            };

            Travel[] travels = { travel0, travel1, travel2, travel3 };

            SQLiteConnection database;
            database = DependencyService.Get<IDatabaseConnection>().DbConnection("AppDatabase.db3");

            database.DropTable<Car>();
            database.DropTable<Travel>();

            database.CreateTable<Car>();
            database.CreateTable<Travel>();

            database.Insert(car);
            database.Insert(car2);

            for(int i = 0; i < travels.Length; i++)
            {
                database.Insert(travels[i]);
            }
        }
    }
}
