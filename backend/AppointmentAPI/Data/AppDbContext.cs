using Microsoft.EntityFrameworkCore;
using AppointmentAPI.Models;

namespace AppointmentAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Slot> Slots { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public DbSet<User> Users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Slot>()
            .HasOne(s => s.Booking)
            .WithOne(b => b.Slot)
            .HasForeignKey<Booking>(b => b.SlotId);
    }
    }

}
