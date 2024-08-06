using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969
{
        public class Address
        {

            public int AddressID { get; set; }
            public string Address1 { get; set; }
            public string Address2 { get; set; }

            public int CityId { get; set; }
            public string PostalCode { get; set; }
            public int Phone { get; set; }
            public DateTime CreateDate { get; set; }
            public string CreatedBy { get; set; } 
            public DateTime LastUpdate { get; set; }
            public string LastUpdatedBy { get; set; }

            public Address(int cityId, int addressID, string address, string address2, string postalCode, int phone, DateTime createDate, string createdBy, DateTime lastUpdate, string LastUpdateBy)
            {
                CityId = cityId;
                AddressID = addressID;
                Address1 = address;
                Address2 = address2;
                PostalCode = postalCode;
                Phone = phone;
                CreateDate = createDate;
                CreatedBy = createdBy;
                LastUpdate = lastUpdate;
                LastUpdatedBy = LastUpdateBy;
            }
        }
    }
