using System.ComponentModel.DataAnnotations;

namespace AppointmentAPI.DTOs
{
    public class CreateSlotDto
    {
        [Required]
        public DateTimeOffset StartTime { get; set; }

        [Required]
        public DateTimeOffset EndTime { get; set; }
    }
}
