using SQLite;

namespace TravelRecord
{
    class Car
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        //public int CompanyID { get; set; }

        public string CarModel { get; set; }

        public string LicensePlateNumber { get; set; }

        public override string ToString()
        {
            return string.Format("[ Car: ID={0}, CarModel={1}, LicensePlateNumber={2} ]", ID, CarModel, LicensePlateNumber);
        }
    }
}
