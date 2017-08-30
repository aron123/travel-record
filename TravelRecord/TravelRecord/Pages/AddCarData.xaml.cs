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
    public partial class AddCarData : ContentPage
    {
        Car car = new Car();
        bool IsNewCar;

        /// <summary>
        /// Initializes AddCarData page without prefilled information.
        /// </summary>
        public AddCarData()
        {
            IsNewCar = true;
            InitializeComponent();
        }

        /// <summary>
        /// Initializes AddCarData page with prefilled entries. Use when information update is required.
        /// </summary>
        /// <param name="car"></param>
        public AddCarData(Car car)
        {
            InitializeComponent();
            IsNewCar = false;
            this.car.LicensePlateNumber = car.LicensePlateNumber;

            //LicensePlateNumber is unchangeable, due to it is the primary key in database
            LicensePlateNumber.IsEnabled = false; 
            LicensePlateNumber.Text = car.LicensePlateNumber;
            CarModel.Text = car.CarModel;
        }

        private void Button_SaveCarData(object sender, EventArgs e)
        {
            car.CarModel = CarModel.Text;
            car.LicensePlateNumber = LicensePlateNumber.Text;

            if (IsNewCar)
            {
                AddNewCar(this.car);
                Navigation.PopAsync();
            }
            else
            {
                UpdateCar(this.car);
                Navigation.PopAsync();
            }
        }

        void AddNewCar(Car car)
        {
            //TODO: try-catch
            SQLiteConnection database = DependencyService.Get<IDatabaseConnection>().DbConnection("AppDatabase.db3");
            database.CreateTable<Car>();
            database.Insert(car);

            MessagingCenter.Send(this, "DatabaseOperationSucceeded", car);
        }

        void UpdateCar(Car car)
        {
            //TODO: try-catch
            SQLiteConnection database = DependencyService.Get<IDatabaseConnection>().DbConnection("AppDatabase.db3");
            database.CreateTable<Car>();
            database.Update(car);

            MessagingCenter.Send(this, "DatabaseOperationSucceeded", car);
        }
    }
}