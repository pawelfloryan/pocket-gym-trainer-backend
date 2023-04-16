using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocketGymTrainer.Migrations
{
    /// <inheritdoc />
    public partial class fourthmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Section",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<string>>(
                name: "Description",
                table: "Exercise",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Exercise",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Exercise",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SectionId",
                table: "Exercise",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Exercise");

            migrationBuilder.DropColumn(
                name: "SectionId",
                table: "Exercise");
        }
    }
}
