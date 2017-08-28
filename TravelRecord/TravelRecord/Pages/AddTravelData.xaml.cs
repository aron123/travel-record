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
        /// <summary>
        /// The travel which will be added to the database.
        /// </summary>
        Travel travel = new Travel();

        bool IsNewTravel;

        /// <summary>
        /// This constructor is used when the goal is to add new travel to database.
        /// </summary>
        /// <param name="LicensePlateNumber">License plate number of the car, which has a new travel.</param>
        /// <param name="IsNewTravel">Boolean that represent that it is a new travel or an existing one.</param>
        public AddTravelData(string LicensePlateNumber)
        {
            IsNewTravel = true;
            this.travel.CarLicensePlate = LicensePlateNumber;

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

        /// <summary>
        /// This constructor is used when the goal is to edit an existing travel.
        /// </summary>
        /// <param name="travel">The travel wanted to be edited.</param>
        public AddTravelData(Travel travel)
        {
            InitializeComponent();

            this.IsNewTravel = false;
            this.travel.ID = travel.ID;
            this.travel.CarLicensePlate = travel.CarLicensePlate;

            TravelDate.Date = new DateTime(travel.TravelDate.Year, travel.TravelDate.Month, travel.TravelDate.Day);
            StartPoint.Text = travel.StartPoint;
            Destination.Text = travel.Destination;
            Distance.Text = travel.Distance.ToString();

            Title = "Utazás szerkesztése: " + travel.CarLicensePlate;
        }

        private void DatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            this.travel.TravelDate = TravelDate.Date;
        }

        private void Button_SaveTravelData(object sender, EventArgs e)
        {
            this.travel.TravelDate = TravelDate.Date;
            this.travel.StartPoint = StartPoint.Text;
            this.travel.Destination = Destination.Text;

            try
            {
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

            if (IsNewTravel)
            {
                AddNewTravel(this.travel);
                Navigation.PopAsync();
            }
            else
            {
                UpdateTravel(this.travel);
                Navigation.PopAsync();
            }
        }

        /// <summary>
        /// Add new travel to the database.
        /// </summary>
        /// <param name="travel">Travel to be added to database.</param>
        private void AddNewTravel(Travel travel)
        {
            SQLiteConnection database;
            database = DependencyService.Get<IDatabaseConnection>().DbConnection("AppDatabase.db3");
            database.CreateTable<Travel>();

            database.Insert(travel);

            MessagingCenter.Send(this, "DatabaseOperationSucceeded", travel);
        }

        /// <summary>
        /// Updates an existing travel's data in database based on its ID property.
        /// </summary>
        /// <param name="travel">Travel to be changed in database.</param>
        private void UpdateTravel(Travel travel)
        {
            //TODO: try-catch
            SQLiteConnection database;
            database = DependencyService.Get<IDatabaseConnection>().DbConnection("AppDatabase.db3");
            database.CreateTable<Travel>();

            database.Update(travel);

            MessagingCenter.Send(this, "DatabaseOperationSucceeded", travel);
        }
    }
}