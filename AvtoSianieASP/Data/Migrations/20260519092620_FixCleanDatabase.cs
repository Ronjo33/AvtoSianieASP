using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvtoSianieASP.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixCleanDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ReservationDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ReservationTime",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReservationDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ReservationTime",
                table: "Orders");
        }
    }
}
