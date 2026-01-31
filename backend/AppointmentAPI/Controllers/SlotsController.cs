using AppointmentAPI.Data;
using AppointmentAPI.Models;
using AppointmentAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AppointmentAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SlotsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SlotsController(AppDbContext context)
        {
            _context = context;
        }

        // Public - Get All Slots (for testing / demo)
        // GET: api/slots
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetAllSlotsPublic()
        {
            var slots = await _context.Slots
                .Include(s => s.Booking)
                .OrderBy(s => s.StartTime)
                .Select(s => new
                {
                    s.Id,
                    s.StartTime,
                    s.EndTime,
                    status =
                        s.EndTime < DateTime.Now
                            ? "Expired"
                            : s.Booking != null
                                ? "Booked"
                                : "Available"
                })
                .ToListAsync();

            return Ok(slots);
        }

        // ===================== DOCTOR =====================

        // Doctor - Get All Slots
        // GET: api/slots/doctor
        [Authorize(Roles = "Doctor")]
        [HttpGet("doctor")]
        public async Task<IActionResult> GetDoctorSlots()
        {
            var slots = await _context.Slots
                .Include(s => s.Booking)
                .OrderByDescending(s => s.StartTime)
                .Select(s => new
                {
                    s.Id,
                    s.StartTime,
                    s.EndTime,
                    status =
                        s.EndTime < DateTime.Now
                            ? "Expired"
                            : s.Booking != null
                                ? "Booked"
                                : "Available"
                })
                .ToListAsync();

            return Ok(slots);
        }

        // Create Slot
        // POST: api/slots
        [Authorize(Roles = "Doctor")]
        [HttpPost]
        public async Task<ActionResult<Slot>> CreateSlot(CreateSlotDto dto)
        {
            if (dto.EndTime <= dto.StartTime)
                return BadRequest("EndTime must be after StartTime");

            var isOverlapping = await _context.Slots.AnyAsync(s =>
                dto.StartTime < s.EndTime &&
                dto.EndTime > s.StartTime
            );

            if (isOverlapping)
                return BadRequest("Time slot overlaps with an existing slot");

            if (!ModelState.IsValid)
            return BadRequest(ModelState);    

            var slot = new Slot
            {
                StartTime = dto.StartTime,
                EndTime = dto.EndTime
            };

            _context.Slots.Add(slot);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetSlot), new { id = slot.Id }, slot);
        }

        // Update Slot
        // PUT: api/slots/5
        [Authorize(Roles = "Doctor")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSlot(int id, Slot slot)
        {
            if (id != slot.Id)
                return BadRequest();

            _context.Entry(slot).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Delete Slot
        // DELETE: api/slots/5
        [Authorize(Roles = "Doctor")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlot(int id)
        {
            var slot = await _context.Slots.FindAsync(id);

            if (slot == null)
                return NotFound();

            _context.Slots.Remove(slot);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // ===================== PATIENT =====================

        // Patient - Get Available Slots (with pagination)
        // GET: api/slots/available
        [Authorize(Roles = "Patient")]
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableSlots(int page = 1, int pageSize = 5)
        {
            var query = _context.Slots
                .Include(s => s.Booking)
                .Where(s => s.EndTime > DateTime.Now && s.Booking == null);

            var totalRecords = await query.CountAsync();

            var slots = await query
                .OrderByDescending(s => s.StartTime)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(s => new
                {
                    s.Id,
                    s.StartTime,
                    s.EndTime,
                    status = "Available"
                })
                .ToListAsync();

            return Ok(new
            {
                page,
                pageSize,
                totalRecords,
                totalPages = (int)Math.Ceiling(totalRecords / (double)pageSize),
                data = slots
            });
        }

        // Get Slot by ID
        // GET: api/slots/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Slot>> GetSlot(int id)
        {
            var slot = await _context.Slots.FindAsync(id);

            if (slot == null)
                return NotFound();

            return slot;
        }


       // Delete Expired Slot
        // DELETE: api/slots/expired/5
        [Authorize(Roles = "Doctor")]
        [HttpDelete("expired/{id}")]
        public async Task<IActionResult> DeleteExpiredSlot(int id)
        {
            var slot = await _context.Slots
                .Include(s => s.Booking)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (slot == null)
                return NotFound("Slot not found");

            // Only allow delete if expired
            if (slot.EndTime >= DateTime.Now)
                return BadRequest("Slot is not expired");

            // // Safety: do not delete booked slots
            // if (slot.Booking != null)
            //     return BadRequest("Booked slot cannot be deleted");

            if (slot.Booking != null)
            {
                _context.Bookings.Remove(slot.Booking);
            }

            _context.Slots.Remove(slot);
            await _context.SaveChangesAsync();

            return Ok("Expired slot deleted");
        }

        // [HttpGet("expired")]
        // public IActionResult GetExpiredSlots()
        // {
        //     var expiredSlots = _context.Slots
        //         .Include(s => s.Booking)
        //         .Where(s => s.EndTime < DateTime.Now && s.Booking == null)
        //         .ToList();

        //     return Ok(expiredSlots);
        // }

    }
}