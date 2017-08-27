using System;
using SQLite;

namespace TravelRecord
{
    public class Travel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string CarLicensePlate { get; set; }

        public DateTime TravelDate { get; set; }

        public string StartPoint { get; set; }

        public string Destination { get; set; }

        public int Distance { get; set; } //in kilometers

        public override string ToString()
        {
            return string.Format("[ Travel: ID={0}, CarLicensePlate={1}, TravelDate={2}, StartPoint={3}, Destination={4}, Distance={5} ]", ID, CarLicensePlate, TravelDate.ToString(), StartPoint, Destination, Distance);
        }

        [Ignore]
        public string Route { get { return StartPoint + " - " + Destination; } }
        [Ignore]
        public string DistanceWithUnit { get { return Distance.ToString() + " km"; } }

        // === These properties are used for data binding in ListTravels page ===
        [Ignore]
        public string Text { get { return Route; } }
        [Ignore]
        public string Detail { get { return TravelDate.ToString("yyyy. MM. dd.") + ", " + DistanceWithUnit; } }
        // === ===
    }
}
