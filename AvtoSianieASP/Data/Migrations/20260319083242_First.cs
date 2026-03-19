using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvtoSianieASP.Data.Migrations
{
    /// <inheritdoc />
    public partial class First : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KatNum",
                table: "Serveces",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Serveces_KatNum",
                table: "Serveces",
                column: "KatNum",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Serveces_KatNum",
                table: "Serveces");

            migrationBuilder.AlterColumn<string>(
                name: "KatNum",
                table: "Serveces",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
