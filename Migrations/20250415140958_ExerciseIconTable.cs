﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BuffBuddyAPI.Migrations
{
    /// <inheritdoc />
    public partial class ExerciseIconTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImgUrl",
                table: "Exercises");

            migrationBuilder.AddColumn<Guid>(
                name: "ExerciseIconId",
                table: "Exercises",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ExerciseIcon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ImgUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseIcon", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_ExerciseIconId",
                table: "Exercises",
                column: "ExerciseIconId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_ExerciseIcon_ExerciseIconId",
                table: "Exercises",
                column: "ExerciseIconId",
                principalTable: "ExerciseIcon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_ExerciseIcon_ExerciseIconId",
                table: "Exercises");

            migrationBuilder.DropTable(
                name: "ExerciseIcon");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_ExerciseIconId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "ExerciseIconId",
                table: "Exercises");

            migrationBuilder.AddColumn<string>(
                name: "ImgUrl",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
