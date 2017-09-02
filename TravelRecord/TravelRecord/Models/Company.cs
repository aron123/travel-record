using SQLite;

namespace TravelRecord
{
    public class Company
    {
        // In current version of this program, only 1 company is enabled.
        [PrimaryKey]
        public int ID { get; } = 0; // equal with Car.CompanyID

        public string CompanyName { get; set; }

        public string Address { get; set; }

        public string VATNumber { get; set; }

        public override string ToString()
        {
            return string.Format("[ Person: ID={0}, CompanyName={1}, Address={2}, VATNumber={3} ]", ID, CompanyName, Address, VATNumber);
        }

    }
}
