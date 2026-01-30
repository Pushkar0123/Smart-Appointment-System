using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AppointmentAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBookingAndSlotSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_SlotId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "PatientName",
                table: "Bookings",
                newName: "PatientUsername");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookedAt",
                table: "Bookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SlotId",
                table: "Bookings",
                column: "SlotId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_SlotId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BookedAt",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "PatientUsername",
                table: "Bookings",
                newName: "PatientName");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_SlotId",
                table: "Bookings",
                column: "SlotId");
        }
    }
}
