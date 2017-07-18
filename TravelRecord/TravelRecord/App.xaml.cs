using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace TravelRecord
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
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
        /// <summary>
        /// Set the main page of the application depending on company's data is set or unset.
        /// </summary>
        public void SetMainPage()
        {
            //TODO: Change MainPage to travel records' page
            MainPage = IsCompanyDataSet() ? new NavigationPage(new TravelRecord.MainPage()) : new NavigationPage(new TravelRecord.AddCompanyData());
        }
    }
}
