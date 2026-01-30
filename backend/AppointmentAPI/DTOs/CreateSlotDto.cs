using System.ComponentModel.DataAnnotations;

namespace AppointmentAPI.DTOs
{
    public class CreateSlotDto
    {
        [Required]
        public DateTime StartTime { get; set; }

        [Required]
        public DateTime EndTime { get; set; }
    }
}
