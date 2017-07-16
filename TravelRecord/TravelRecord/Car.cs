using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
