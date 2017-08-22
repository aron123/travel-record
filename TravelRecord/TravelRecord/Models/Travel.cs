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
    }
}
