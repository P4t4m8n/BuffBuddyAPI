using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuffBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class RenameMuscleIcons : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_ExerciseIcons_ExerciseIconId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_TargetMuscle_TargetMuscleId",
                table: "Exercises");

            migrationBuilder.DropTable(
                name: "ExerciseIcons");

            migrationBuilder.DropTable(
                name: "TargetMuscle");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_TargetMuscleId",
                table: "Exercises");

            migrationBuilder.RenameColumn(
                name: "TargetMuscleId",
                table: "Exercises",
                newName: "ExerciseMuscleId");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateAt",
                table: "ExerciseType",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "ExerciseType",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExerciseIconId",
                table: "Exercises",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateTable(
                name: "ExerciseMuscles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseMuscles", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_ExerciseMuscles_ExerciseIconId",
                table: "Exercises",
                column: "ExerciseIconId",
                principalTable: "ExerciseMuscles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_ExerciseMuscles_ExerciseIconId",
                table: "Exercises");

            migrationBuilder.DropTable(
                name: "ExerciseMuscles");

            migrationBuilder.DropColumn(
                name: "CreateAt",
                table: "ExerciseType");

            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "ExerciseType");

            migrationBuilder.RenameColumn(
                name: "ExerciseMuscleId",
                table: "Exercises",
                newName: "TargetMuscleId");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExerciseIconId",
                table: "Exercises",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "ExerciseIcons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseIcons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TargetMuscle",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TargetMuscle", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_TargetMuscleId",
                table: "Exercises",
                column: "TargetMuscleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_ExerciseIcons_ExerciseIconId",
                table: "Exercises",
                column: "ExerciseIconId",
                principalTable: "ExerciseIcons",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_TargetMuscle_TargetMuscleId",
                table: "Exercises",
                column: "TargetMuscleId",
                principalTable: "TargetMuscle",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
