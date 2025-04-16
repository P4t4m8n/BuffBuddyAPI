using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuffBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class addMisc : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Equipment",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "TargetMuscle",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "Exercises");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Exercises",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ImgUrl",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EquipmentId",
                table: "Exercises",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ExerciseTypeId",
                table: "Exercises",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TargetMuscleId",
                table: "Exercises",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExerciseType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseType", x => x.Id);
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
                name: "IX_Exercises_EquipmentId",
                table: "Exercises",
                column: "EquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ExerciseTypeId",
                table: "Exercises",
                column: "ExerciseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_TargetMuscleId",
                table: "Exercises",
                column: "TargetMuscleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Equipment_EquipmentId",
                table: "Exercises",
                column: "EquipmentId",
                principalTable: "Equipment",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_ExerciseType_ExerciseTypeId",
                table: "Exercises",
                column: "ExerciseTypeId",
                principalTable: "ExerciseType",
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Equipment_EquipmentId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_ExerciseType_ExerciseTypeId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_TargetMuscle_TargetMuscleId",
                table: "Exercises");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "ExerciseType");

            migrationBuilder.DropTable(
                name: "TargetMuscle");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_EquipmentId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_ExerciseTypeId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_TargetMuscleId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "EquipmentId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ExerciseTypeId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "TargetMuscleId",
                table: "Exercises");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Exercises",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "ImgUrl",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Equipment",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TargetMuscle",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
