using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C969
{
    public class Appointments
    {
        public int AppointmentID { get; set; }
        public int CustomerID { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }

        public Appointments(int appointmentID, int customerID, DateTime start, DateTime end)
        {
            AppointmentID = appointmentID;
            CustomerID = customerID;
            Start = start;
            End = end;
        }
    }
}
