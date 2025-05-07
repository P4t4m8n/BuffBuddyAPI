using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuffBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class DaysOfWeekEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DaysOfWeek",
                table: "ProgramExercises",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DaysOfWeek",
                table: "ProgramExercises");
        }
    }
}
