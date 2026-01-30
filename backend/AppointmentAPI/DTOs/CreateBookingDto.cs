using System.ComponentModel.DataAnnotations;

namespace AppointmentAPI.DTOs
{
    public class CreateBookingDto
    {
        [Required]
        public int SlotId { get; set; }

        [Required]
        public string PatientName { get; set; } = string.Empty;
    }
}
