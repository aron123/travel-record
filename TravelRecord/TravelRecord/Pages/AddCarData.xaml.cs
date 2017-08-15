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
        public AddCarData()
        {
            InitializeComponent();
        }

        private void Button_SaveCarData(object sender, EventArgs e)
        {
            Car car = new Car() { CarModel = CarModel.Text, LicensePlateNumber = LicensePlateNumber.Text };

            SQLiteConnection database;
            database = DependencyService.Get<IDatabaseConnection>().DbConnection("AppDatabase.db3");

            database.CreateTable<Car>();
            database.Insert(car);
        }
    }
}