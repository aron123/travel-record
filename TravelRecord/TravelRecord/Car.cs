using SQLite;

namespace TravelRecord
{
    class Car
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; }

        public int CompanyID { get; set; }

        public string CarModel { get; set; }

        public string LicensePlateNumber { get; set; }
    }
}
