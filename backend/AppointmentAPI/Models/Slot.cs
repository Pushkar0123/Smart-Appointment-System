using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace AppointmentAPI.Models
{
    public class Slot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        public int Id { get; set; }
        public DateTimeOffset StartTime { get; set; }
        public DateTimeOffset EndTime { get; set; }

        // Navigation property
        public Booking? Booking { get; set; }

        //Status of the slot
        // public SlotStatus Status { get; set; }  // This property is now directly set    
        // We won’t store Status in DB
        // It will be calculated dynamically
    }

    public enum SlotStatus
    {
        Available,
        Booked,
        Expired
    }    
}

