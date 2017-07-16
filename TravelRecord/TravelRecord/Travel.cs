using System;
using SQLite;

namespace TravelRecord
{
    class Travel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; }

        public DateTime Date { get; set; }

        public string StartPoint { get; set; }

        public string Destination { get; set; }

        public int Distance { get; set; } //in kilometers
    }
}
