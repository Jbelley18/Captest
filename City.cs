using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969
{
    public class City
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public int CountryID { get; set; }
        public int CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public int LastUpdate { get; set; }
        public int LastUpdateBy { get; set; }

        public City(int cityId, string cityName, int countryID, int createDate, int createdBy, int lastUpdate, int lastUpdateBy)
        {
            CityId = cityId;
            CityName = cityName;
            CountryID =  countryID;
            CreateDate = createDate;
            CreatedBy = createdBy;
            LastUpdate = lastUpdate;
            LastUpdateBy = lastUpdateBy;
        }
    }
}
