using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuffBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameExerciseCloumnEquipment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_ExerciseEquipments_EquipmentId",
                table: "Exercises");

            migrationBuilder.RenameColumn(
                name: "EquipmentId",
                table: "Exercises",
                newName: "ExerciseEquipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_EquipmentId",
                table: "Exercises",
                newName: "IX_Exercises_ExerciseEquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_ExerciseEquipments_ExerciseEquipmentId",
                table: "Exercises",
                column: "ExerciseEquipmentId",
                principalTable: "ExerciseEquipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_ExerciseEquipments_ExerciseEquipmentId",
                table: "Exercises");

            migrationBuilder.RenameColumn(
                name: "ExerciseEquipmentId",
                table: "Exercises",
                newName: "EquipmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Exercises_ExerciseEquipmentId",
                table: "Exercises",
                newName: "IX_Exercises_EquipmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_ExerciseEquipments_EquipmentId",
                table: "Exercises",
                column: "EquipmentId",
                principalTable: "ExerciseEquipments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
