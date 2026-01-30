using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddBookingTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookedAt",
                table: "Bookings",
                newName: "BookingTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookingTime",
                table: "Bookings",
                newName: "BookedAt");
        }
    }
}
