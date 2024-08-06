using System;

namespace C969
{
    public class Appointment
    {
        public int AppointmentId { get; set; }
        public int CustomerId { get; set; }
        public int userId { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string location { get; set; }
        public string contact { get; set; }
        public string type { get; set; }
        public string url { get; set; }
        public DateTime start { get; set; }
        public DateTime end { get; set; }
        public DateTime CreateDate { get; set; }
        public int CreatedBy { get; set; }
        public int LastUpdate { get; set; }
        public int LastUpdateBy { get; set; }

        public Appointment(int AppointmentId, int CustomerId, int userId, string title, string description, string location, string contact, string type, string url, DateTime start, DateTime end, DateTime CreateDate, int LastUpdate, int LastUpdateBy)
        {
            AppointmentId = AppointmentId;
            CustomerId = CustomerId;
            userId = userId;
            title = title;
            description = description;
            location = location;
            contact = contact;
            type = type;
            url = url;
            start = start;
            end = end;
            CreateDate = CreateDate;
            CreatedBy = CreatedBy;
            LastUpdate = LastUpdate;
            LastUpdateBy = LastUpdateBy;

        }
    }
}
