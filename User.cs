using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969
{
    public class User
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public int active { get; set; }
        public int CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public int LastUpdate { get; set; }
        public int LastUpdateBy { get; set; }

        public User(int userId, int username, int password, int active, int CreateDate, int CreatedBy, int LastUpdate, int LastUpdateBy)
        {
            userId = userId;
            username = username;
            password = password;
            active = active;
            CreateDate = CreateDate;
            CreatedBy = CreatedBy;
            LastUpdate = LastUpdate;
            LastUpdateBy = LastUpdateBy;

        }
    }
}
