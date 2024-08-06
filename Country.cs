using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969
{
    public class Country
    {
        public int CountryId { get; set; }
        public string CountryName { get; set; }
        public int CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public int LastUpdate { get; set; }
        public int LastUpdateBy { get; set; }

        public Country(int countryId, string countryName, int createDate, int createdBy, int lastUpdate, int lastUpdateBy)
        {
            CountryId = countryId;
            CountryName = countryName;
            CreateDate = createDate;
            CreatedBy = createdBy;
            LastUpdate = lastUpdate;
            LastUpdateBy = lastUpdateBy;
        }
    }
}
