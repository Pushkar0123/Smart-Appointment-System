using System;

namespace AppointmentAPI.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public int SlotId { get; set; }
        public Slot Slot { get; set; } 
        public string PatientUsername { get; set; }

        // public DateTime BookedAt { get; set; } = DateTime.Now; 
        public DateTime BookingTime { get; set; } = DateTime.Now;
    }
}
