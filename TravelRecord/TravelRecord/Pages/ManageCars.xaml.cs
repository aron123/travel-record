﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;

namespace TravelRecord
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ManageCars : ContentPage
    {
        SQLiteConnection database = App.Database;
        private ObservableCollection<Car> CarList { get; set; }

        public ManageCars()
        {
            InitializeComponent();

            CarList = LoadCars();
            Cars.ItemsSource = CarList;
            BindingContext = this;
        }

        async void Item_HandleTapped(object sender, ItemTappedEventArgs e)
        {
            if (((ListView)sender).SelectedItem == null)
                return;

            var action = await DisplayActionSheet("Művelet:", "Mégse", null, "Szerkesztés", "Törlés");
            Car item;

            switch (action)
            {
                case "Mégse":
                    ((ListView)sender).SelectedItem = null;
                    return;
                case "Szerkesztés":
                    item = e.Item as Car;

                    // update car in database
                    await Navigation.PushAsync(new AddCarData(item));

                    // update car in UI list
                    MessagingCenter.Subscribe<AddCarData, Car>(this, "DatabaseOperationSucceeded", (_sender, car) => {
                        UpdateItemInList(CarList, car);
                        CarList = SortListByLicensePlateAsc(CarList);
                        Cars.ItemsSource = CarList;

                        MessagingCenter.Send<ManageCars>(this, "CarListChanged");
                        MessagingCenter.Unsubscribe<AddCarData, Car>(this, "DatabaseOperationSucceeded");
                    });
                    break;
                case "Törlés":
                    item = e.Item as Car;
                    bool success = false;

                    try
                    {
                        RemoveFromDatabaseAndList(CarList, item);
                        success = true;
                        
                    }
                    catch (SQLiteException ex)
                    {
                        await DisplayAlert("Hiba", "Nem sikerült törölni az autót.", "OK");
                        ((ListView)sender).SelectedItem = null;
                    }
                    finally
                    {
                        if (success) MessagingCenter.Send<ManageCars>(this, "CarListChanged");
                    }
                    
                    break;
                default:
                    await DisplayAlert("Hiba", "Valami nem jó.", "OK");
                    break;
            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }

        ObservableCollection<Car> LoadCars()
        {
            List<Car> items = database.Query<Car>("SELECT * FROM Car ORDER BY LicensePlateNumber ASC");
            return new ObservableCollection<Car>(items);
        }

        async void ToolbarItem_AddNewCar(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddCarData());
            MessagingCenter.Subscribe<AddCarData, Car>(this, "DatabaseOperationSucceeded", (_sender, car) =>
            {
                CarList.Add(car);
                CarList = SortListByLicensePlateAsc(CarList);
                Cars.ItemsSource = CarList;

                MessagingCenter.Send<ManageCars>(this, "CarListChanged");
                MessagingCenter.Unsubscribe<AddCarData, Car>(this, "DatabaseOperationSucceeded");
            });
        }

        /// <summary>
        /// Sort the given list's items to ascending order, based on their LicensePlateNumber property. 
        /// </summary>
        /// <param name="list">Sortable list</param>
        /// <returns>Sorted list</returns>
        ObservableCollection<Car> SortListByLicensePlateAsc(ObservableCollection<Car> list)
        {
            return new ObservableCollection<Car>(list.OrderBy(x => x.LicensePlateNumber).ToList());
        }

        /// <summary>
        /// Update a car in a collection based on its LicensePlateNumber property.
        /// </summary>
        /// <param name="list"></param>
        /// <param name="item">Item with new information and original ID.</param>
        void UpdateItemInList(ObservableCollection<Car> list, Car item)
        {
            var found = list.FirstOrDefault(i => i.LicensePlateNumber == item.LicensePlateNumber);
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
        void RemoveFromDatabaseAndList(ObservableCollection<Car> list, Car item)
        {
            bool success = false;
            try
            {
                database.Query<Travel>("DELETE FROM Travel WHERE CarLicensePlate=?", item.LicensePlateNumber);
                database.Delete<Car>(item.LicensePlateNumber);
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
    }
}