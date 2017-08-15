using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite; //DEBUG

namespace TravelRecord
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddCompanyData : ContentPage
	{
		public AddCompanyData()
		{
            Title = "Cégadatok";
			InitializeComponent();
		}

        private void Button_SaveCompanyData()
        {
            Application.Current.Properties["CompanyName"] = CompanyName.Text;
            Application.Current.Properties["CompanyAddress"] = CompanyAddress.Text;
            Application.Current.Properties["CompanyVAT"] = CompanyVAT.Text;
        }

        #region DEBUG
        private void DEBUGButton_CheckData(object sender, EventArgs e)
        {
            if (Application.Current.Properties.ContainsKey("CompanyName")
                && Application.Current.Properties.ContainsKey("CompanyAddress")
                && Application.Current.Properties.ContainsKey("CompanyVAT"))
            {
                var name = Application.Current.Properties["CompanyName"] as string;
                var address = Application.Current.Properties["CompanyAddress"] as string;
                var vat = Application.Current.Properties["CompanyVAT"] as string;

                DisplayAlert("Data is", string.Format("Name: {0}, \nAddress: {1}, \nVAT: {2}", name, address, vat), "OK");
            }
            else
                DisplayAlert("NINCS TÁROLT ADAT", "", "ok");


            /*
            //DATABASE TEST
            SQLiteConnection database;
            database = DependencyService.Get<IDatabaseConnection>().DbConnection("AppDatabase.db3");

              // test data
            Car car = new Car() { CarModel = "Autoo", LicensePlateNumber="DPF-841" };
            Travel travel = new Travel() { Date = new DateTime(2017, 01, 01), StartPoint = "Obuda", Destination = "Miskolc", Distance = 200 };
            Car car2 = new Car() { CarModel = "Suzuki", LicensePlateNumber = "ABC-123" };
            Travel travel2 = new Travel() { Date = new DateTime(1999, 01, 01), StartPoint = "Litka", Destination = "Taktaharkany", Distance = 60 };

            // create two tables if not exist
            database.CreateTable<Car>();
            database.CreateTable<Travel>();

              // insert test data
            int car_id = database.Insert(car);
            int travel_id = database.Insert(travel);
            int car2_id = database.Insert(car2);
            int travel2_id = database.Insert(travel2);

            car_id = car.ID;
            travel_id = travel.ID;
            car2_id = car2.ID;
            travel2_id = travel2.ID;

            // find the data
            Car retCar = new Car();
            Travel retTravel = new Travel();
            Car retCar2 = new Car();
            Travel retTravel2 = new Travel();

            retCar = database.FindWithQuery<Car>("SELECT * FROM Car WHERE LicensePlateNumber=\"DPF-841\"");
            retTravel = database.FindWithQuery<Travel>("SELECT * FROM Travel WHERE StartPoint=\"Obuda\"");
            retCar2 = database.FindWithQuery<Car>("SELECT * FROM Car WHERE LicensePlateNumber=\"ABC-123\"");
            retTravel2 = database.FindWithQuery<Travel>("SELECT * FROM Travel WHERE StartPoint=\"Litka\"");

            // output the data
            DisplayAlert("DATA IS", retCar.ToString() + "\n\n" + retTravel.ToString() + "\n\n" + retCar2.ToString() + "\n\n" + retTravel2.ToString(), "OK");
            
            var label = new Label() { Text = "asd" };
            var layout = AddCompanyDataBody;
            layout.Children.Add(label);

    */
            
            
        }
        #endregion
    }
}