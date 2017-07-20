using System;
using SQLite;

namespace TravelRecord
{
    class Travel
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public DateTime Date { get; set; }

        public string StartPoint { get; set; }

        public string Destination { get; set; }

        public int Distance { get; set; } //in kilometers

        public override string ToString()
        {
            return string.Format("[ Travel: ID={0}, Date={1}, StartPoint={2}, Destination={3}, Distance={4} ]", ID, Date.ToString(), StartPoint, Destination, Distance);
        }
    }
}
