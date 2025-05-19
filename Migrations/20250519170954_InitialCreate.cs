using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuffBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExerciseEquipments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseEquipments", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "ExerciseTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    YoutubeUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExerciseMuscleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseEquipmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Exercises_ExerciseEquipments_ExerciseEquipmentId",
                        column: x => x.ExerciseEquipmentId,
                        principalTable: "ExerciseEquipments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exercises_ExerciseMuscles_ExerciseMuscleId",
                        column: x => x.ExerciseMuscleId,
                        principalTable: "ExerciseMuscles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Exercises_ExerciseTypes_ExerciseTypeId",
                        column: x => x.ExerciseTypeId,
                        principalTable: "ExerciseTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgramExercises",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    ProgramId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    DaysOfWeek = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProgramExercises_Programs_ProgramId",
                        column: x => x.ProgramId,
                        principalTable: "Programs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoreSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Reps = table.Column<int>(type: "int", nullable: false),
                    Weight = table.Column<int>(type: "int", nullable: false),
                    RestTime = table.Column<int>(type: "int", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsWarmup = table.Column<bool>(type: "bit", nullable: false),
                    RepsInReserve = table.Column<int>(type: "int", nullable: false),
                    ProgramExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoreSets_ProgramExercises_ProgramExerciseId",
                        column: x => x.ProgramExerciseId,
                        principalTable: "ProgramExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ActualReps = table.Column<int>(type: "int", nullable: false),
                    ActualWeight = table.Column<int>(type: "int", nullable: false),
                    ActualRestTime = table.Column<int>(type: "int", nullable: false),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    IsMuscleFailure = table.Column<bool>(type: "bit", nullable: false),
                    IsJointPain = table.Column<bool>(type: "bit", nullable: false),
                    ProgramExerciseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoreSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserSets_CoreSets_CoreSetId",
                        column: x => x.CoreSetId,
                        principalTable: "CoreSets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserSets_ProgramExercises_ProgramExerciseId",
                        column: x => x.ProgramExerciseId,
                        principalTable: "ProgramExercises",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoreSets_ProgramExerciseId",
                table: "CoreSets",
                column: "ProgramExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ExerciseEquipmentId",
                table: "Exercises",
                column: "ExerciseEquipmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ExerciseMuscleId",
                table: "Exercises",
                column: "ExerciseMuscleId");

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ExerciseTypeId",
                table: "Exercises",
                column: "ExerciseTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramExercises_ExerciseId",
                table: "ProgramExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramExercises_ProgramId",
                table: "ProgramExercises",
                column: "ProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSets_CoreSetId",
                table: "UserSets",
                column: "CoreSetId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSets_ProgramExerciseId",
                table: "UserSets",
                column: "ProgramExerciseId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSets");

            migrationBuilder.DropTable(
                name: "CoreSets");

            migrationBuilder.DropTable(
                name: "ProgramExercises");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "ExerciseEquipments");

            migrationBuilder.DropTable(
                name: "ExerciseMuscles");

            migrationBuilder.DropTable(
                name: "ExerciseTypes");
        }
    }
}
