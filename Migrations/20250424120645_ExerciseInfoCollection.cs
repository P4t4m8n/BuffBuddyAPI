using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuffBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class ExerciseInfoCollection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_ExerciseMuscles_ExerciseIconId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_ExerciseIconId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ExerciseIconId",
                table: "Exercises");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ExerciseMuscleId",
                table: "Exercises",
                column: "ExerciseMuscleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_ExerciseMuscles_ExerciseMuscleId",
                table: "Exercises",
                column: "ExerciseMuscleId",
                principalTable: "ExerciseMuscles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_ExerciseMuscles_ExerciseMuscleId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_ExerciseMuscleId",
                table: "Exercises");

            migrationBuilder.AddColumn<Guid>(
                name: "ExerciseIconId",
                table: "Exercises",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ExerciseIconId",
                table: "Exercises",
                column: "ExerciseIconId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_ExerciseMuscles_ExerciseIconId",
                table: "Exercises",
                column: "ExerciseIconId",
                principalTable: "ExerciseMuscles",
                principalColumn: "Id");
        }
    }
}
