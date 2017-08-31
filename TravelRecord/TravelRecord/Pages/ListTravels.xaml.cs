using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;

namespace TravelRecord
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListTravels : ContentPage
    {
        ObservableCollection<Travel> TravelList { get; set; }
        ObservableCollection<Car> CarList { get; set; }
        SQLiteConnection database = App.Database;

        /// <summary>
        /// Actually selected car's license plate number
        /// </summary>
        string LicensePlateNumber
        {
            get
            {
                try { return Cars.Items[Cars.SelectedIndex]; }
                catch (ArgumentOutOfRangeException e) { return null; }
            }
        }

        public ListTravels()
        {
            InitializeComponent();
            BindingContext = this;

            CarList = LoadCars();
            Cars.ItemsSource = CarList;
            Cars.SelectedIndex = 0;
            // Picker_CarSelected is called, TravelList loaded

            //Update CarList when Cars table changed
            MessagingCenter.Subscribe<ManageCars>(this, "CarListChanged", (sender) =>
            {
                CarList = LoadCars();
                Cars.ItemsSource = CarList;
                Cars.SelectedIndex = 0;
            });

            MessagingCenter.Subscribe<App>(this, "GenerateInstallNavigationStack", (sender) =>
            {
                Navigation.PushAsync(new AddCarData());
                Navigation.PushAsync(new AddCompanyData());
                
                MessagingCenter.Subscribe<AddCarData, Car>(this, "DatabaseOperationSucceeded", (_sender, car) =>
                {
                    CarList = LoadCars();
                    Cars.ItemsSource = CarList;
                    Cars.SelectedIndex = 0;

                    Application.Current.Properties["Installed"] = true;

                    MessagingCenter.Unsubscribe<AddCarData, Car>(this, "DatabaseOperationSucceeded");
                });

                MessagingCenter.Unsubscribe<App>(this, "GenerateInstallNavigationStack");
                
            });
        }

        async void Button_AddNewTravel(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddTravelData(LicensePlateNumber));
            MessagingCenter.Subscribe<AddTravelData, Travel>(this, "DatabaseOperationSucceeded", (_sender, travel) =>
            {
                TravelList.Add(travel);
                TravelList = SortListByTravelDateDesc(TravelList);
                Travels.ItemsSource = TravelList;
                MessagingCenter.Unsubscribe<AddTravelData, Travel>(this, "DatabaseOperationSucceeded");
            });
        }

        async void Item_HandleTapped(object sender, ItemTappedEventArgs e)
        {
            if (((ListView)sender).SelectedItem == null)
                return;

            var action = await DisplayActionSheet("Művelet:", "Mégse", null, "Szerkesztés", "Törlés");
            Travel item;

            switch (action)
            {
                case "Mégse":
                    ((ListView)sender).SelectedItem = null;
                    return;
                case "Szerkesztés":
                    item = e.Item as Travel;

                    // update travel in database
                    await Navigation.PushAsync(new AddTravelData(item));

                    // update travel in UI list
                    MessagingCenter.Subscribe<AddTravelData, Travel>(this, "DatabaseOperationSucceeded", (_sender, travel) => {
                        UpdateItemInList(TravelList, travel);
                        TravelList = SortListByTravelDateDesc(TravelList);
                        Travels.ItemsSource = TravelList;
                        MessagingCenter.Unsubscribe<AddTravelData, Travel>(this, "DatabaseOperationSucceeded");
                    });
                    break;
                case "Törlés":
                    item = e.Item as Travel;
                    try
                    {
                        RemoveFromDatabaseAndList(TravelList, item);
                    }
                    catch (SQLiteException ex)
                    {
                        await DisplayAlert("Hiba", "Nem sikerült törölni az utazást.", "OK");
                        ((ListView)sender).SelectedItem = null;
                    }
                    break;
                default:
                    await DisplayAlert("Hiba", "Ismeretlen hiba történt.", "OK");
                    break;
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        void Picker_CarSelected(object sender, EventArgs e)
        {
            SetTitle("Utazások: " + LicensePlateNumber);
            TravelList = LoadTravels(LicensePlateNumber);
            Travels.ItemsSource = TravelList;
        }

        /// <summary>
        /// Load cars from database
        /// </summary>
        /// <returns>Collection with cars</returns>
        ObservableCollection<Car> LoadCars()
        {
            List<Car> items = database.Query<Car>("SELECT * FROM Car ORDER BY LicensePlateNumber ASC");
            return new ObservableCollection<Car>(items);
        }

        /// <summary>
        /// Load travels from database
        /// </summary>
        /// <returns>Collection with travels</returns>
        ObservableCollection<Travel> LoadTravels(string LicensePlateNumber)
        {
            List<Travel> items = database.Query<Travel>("SELECT * FROM Travel WHERE CarLicensePlate=? ORDER BY TravelDate DESC", LicensePlateNumber);
            return new ObservableCollection<Travel>(items);
        }

        /* This method fails, but dunno why.
        // https://forums.xamarin.com/discussion/101914/generic-method-to-load-all-rows-from-a-table-fails
        ObservableCollection<T> Load<T>() where T : class, new()
        {
            List<T> items = database.Query<T>("SELECT * FROM ?", typeof(T).Name);
            return new ObservableCollection<T>(items);
        }
        */

        /// <summary>
        /// Update a travel in a collection based on its ID property.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item">Item with new information and original ID.</param>
        void UpdateItemInList(ObservableCollection<Travel> list, Travel item)
        {
            var found = list.FirstOrDefault(i => i.ID == item.ID);
            if (found != null)
            {
                int index = list.IndexOf(found);
                list.RemoveAt(index);
                list.Insert(index, item);
            }
        }
        
        /// <summary>
        /// Removes the given item from database and if it succeeded, removes it from the given collection
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item"></param>
        void RemoveFromDatabaseAndList(ObservableCollection<Travel> list, Travel item)
        {
            bool success = false;

            try
            {
                database.Delete<Travel>(item.ID);
                success = true;
            }
            catch (SQLiteException ex)
            {
                throw ex;
            }
            finally
            {
                if (success)
                    list.Remove(item);
            }
        }

        /// <summary>
        /// Sort the given list by items' TravelDate property in descending order.
        /// </summary>
        /// <param name="list">The sortable list</param>
        /// <returns>The sorted list</returns>
        ObservableCollection<Travel> SortListByTravelDateDesc(ObservableCollection<Travel> list)
        {
            return new ObservableCollection<Travel>(list.OrderByDescending(x => x.TravelDate).ToList());
        }

        void SetTitle(string title)
        {
            Title = title;
        }

        async void ToolbarItem_EditCompanyData(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCompanyData(
                Application.Current.Properties["CompanyName"].ToString(),
                Application.Current.Properties["CompanyAddress"].ToString(),
                Application.Current.Properties["CompanyVAT"].ToString()
                )
            );
        }

        void ToolbarItem_ManageCars(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ManageCars());
        }

        async void ToolbarItem_About(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new About());
        }
    }
}