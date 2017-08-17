using System;
using SQLite;

namespace TravelRecord
{
    class Travel
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
            return string.Format("[ Travel: CarLicensePlate={0}, TravelDate={1}, StartPoint={2}, Destination={3}, Distance={4} ]", CarLicensePlate, TravelDate.ToString(), StartPoint, Destination, Distance);
        }
    }
}
