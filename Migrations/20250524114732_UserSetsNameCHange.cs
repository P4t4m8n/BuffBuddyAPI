using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuffBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserSetsNameCHange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActualWeight",
                table: "UserSets",
                newName: "Weight");

            migrationBuilder.RenameColumn(
                name: "ActualRestTime",
                table: "UserSets",
                newName: "RestTime");

            migrationBuilder.RenameColumn(
                name: "ActualReps",
                table: "UserSets",
                newName: "Reps");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "UserSets",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "UserSets");

            migrationBuilder.RenameColumn(
                name: "Weight",
                table: "UserSets",
                newName: "ActualWeight");

            migrationBuilder.RenameColumn(
                name: "RestTime",
                table: "UserSets",
                newName: "ActualRestTime");

            migrationBuilder.RenameColumn(
                name: "Reps",
                table: "UserSets",
                newName: "ActualReps");
        }
    }
}
