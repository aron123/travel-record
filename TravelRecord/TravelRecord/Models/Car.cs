using SQLite;

namespace TravelRecord
{
    class Car
    {
        [PrimaryKey]
        public string LicensePlateNumber { get; set; }

        //public int CompanyID { get; set; }

        public string CarModel { get; set; }

        public override string ToString()
        {
            return string.Format("[ Car: LicensePlateNumber={0}, CarModel={1}  ]", LicensePlateNumber, CarModel );
        }
    }
}
