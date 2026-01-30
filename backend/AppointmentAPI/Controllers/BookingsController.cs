using AppointmentAPI.Data;
using AppointmentAPI.Models;
using AppointmentAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace AppointmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
     // All endpoints require authentication
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/bookings
        // Doctor + Patient
        [Authorize] 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
        {
            return await _context.Bookings
                .Include(b => b.Slot)
                .ToListAsync();
        }

        // GET: api/bookings/5
        // Doctor + Patient
        [HttpGet("{id}")]
        public async Task<ActionResult<Booking>> GetBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Slot)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return booking;
        }

        // POST: api/bookings
        // ONLY Patient
        [Authorize(Roles = "Patient")]
        [HttpPost("{SlotId}")] 
        public async Task<IActionResult> CreateBookingDto(int SlotId)
        {
            var slot = await _context.Slots
                .Include(s => s.Booking)
                .FirstOrDefaultAsync(s => s.Id == SlotId);

            if (slot == null)
                return NotFound("Slot does not exist");

            if (slot.EndTime < DateTime.Now)
                return BadRequest("Cannot book an expired slot");

            if (slot.Booking != null)
                return BadRequest("Slot already booked");

            if (slot.EndTime < DateTime.Now)
                return BadRequest("Slot expired");

            var patientUsername = User.FindFirstValue(ClaimTypes.Name);

            if (string.IsNullOrEmpty(patientUsername))
                return Unauthorized("Invalid user");

            var booking = new Booking
            {
                SlotId = SlotId,
                PatientUsername = patientUsername,
                BookingTime = DateTime.Now
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Booking confirmed",
                bookingId = booking.Id,
                slotId = booking.SlotId
            });
        }

        // DELETE: api/bookings/5
        // ONLY Doctor
        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}