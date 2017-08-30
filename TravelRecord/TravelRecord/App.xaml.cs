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
            int IsEmpty = database.ExecuteScalar<int>("SELECT COUNT(*) FROM Car LIMIT 1;");
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
    }
}
