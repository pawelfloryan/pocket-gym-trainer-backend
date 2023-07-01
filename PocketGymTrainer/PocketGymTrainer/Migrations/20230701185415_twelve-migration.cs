using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocketGymTrainer.Migrations
{
    /// <inheritdoc />
    public partial class twelvemigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WeekDay",
                table: "Workout");

            migrationBuilder.AddColumn<DateTime>(
                name: "WorkoutDate",
                table: "Workout",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkoutDate",
                table: "Workout");

            migrationBuilder.AddColumn<string>(
                name: "WeekDay",
                table: "Workout",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
