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
        static SQLiteConnection database;

        public static SQLiteConnection Database
        {
            get
            {
                if (database == null)
                {
                    try { database = DatabaseAssistant.Connect(); }
                    catch (SQLiteException e) { Current.MainPage = new Error("Hiba", "Az adatbázishoz nem lehet csatlakozni."); }
                }
                return database;
            }

        }

        public App()
        {
            InitializeComponent();
        }

        protected override void OnStart()
        {
            // Handle when app starts
            InitializeTables();
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

        void InitializeTables()
        {
            Database.CreateTable<Car>();
            Database.CreateTable<Travel>();
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
            //Check if Car table is empty
            int IsEmpty = Database.ExecuteScalar<int>("SELECT COUNT(*) FROM Car LIMIT 1;");
            if (IsEmpty == 0) return false;

            return true;
        }

        /// <summary>
        /// Set the main page of the application depending on company's and cars' data is set or unset.
        /// </summary>
        public void SetMainPage()
        {
            if (Current.Properties.ContainsKey("Installed") == false || (bool)Current.Properties["Installed"] == false)
            {
                ConfigureInstallation();
                return;
            }

            MainPage = new NavigationPage(new ListTravels());
        }

        void ConfigureInstallation()
        {
            Application.Current.Properties["Installed"] = false;
            Application.Current.SavePropertiesAsync();
            MainPage = new NavigationPage(new ListTravels());
            MessagingCenter.Send<App>(this, "GenerateInstallNavigationStack");
        }
    }
}
