using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;

namespace TravelRecord
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddTravelData : ContentPage
    {
        Travel travel = new Travel();

        /// <summary>
        /// Add new travel to Travel table
        /// </summary>
        /// <param name="LicensePlateNumber">License plate number of the car, which has a new travel.</param>
        public AddTravelData(string LicensePlateNumber)
        {
            SQLiteConnection database;
            database = DependencyService.Get<IDatabaseConnection>().DbConnection("AppDatabase.db3");

            Car test = new Car();
            test = database.FindWithQuery<Car>("SELECT * FROM Car WHERE LicensePlateNumber=?", LicensePlateNumber);
            if (test == null)
                throw new NotImplementedException(); //TODO: Implement this.
            database.Close();

            Title = "Új utazás: " + LicensePlateNumber;

            InitializeComponent();
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            this.travel.TravelDate = TravelDate.Date;
        }

        private void Button_SaveTravelData(object sender, EventArgs e)
        {
            try
            {
                this.travel.TravelDate = TravelDate.Date;
                this.travel.StartPoint = StartPoint.Text;
                this.travel.Destination = Destination.Text;
                this.travel.Distance = Convert.ToInt32(Distance.Text);
            }
            catch (FormatException ex)
            {
                DisplayAlert("Helytelen távolság adat", "A távolság formátuma nem helyesen van megadva, csak számokat írj be!", "OK");
                Distance.Text = "";
                Distance.Focus();
                return;
            }
            catch (OverflowException ex)
            {
                DisplayAlert("Helytelen távolság adat", "A megadott távolság túl magas vagy túl alacsony.", "OK");
                Distance.Text = "";
                Distance.Focus();
                return;
            }

            SQLiteConnection database;
            database = DependencyService.Get<IDatabaseConnection>().DbConnection("AppDatabase.db3");
            database.CreateTable<Travel>();

            database.Insert(travel);
        }

        
    }
}