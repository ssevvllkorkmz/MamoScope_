using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MamoScope_API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMotorDriversModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.RenameTable(
                name: "MotorDrivers",
                newName: "MotorDrivers",
                newSchema: "public");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "MotorDrivers",
                schema: "public",
                newName: "MotorDrivers");
        }
    }
}
