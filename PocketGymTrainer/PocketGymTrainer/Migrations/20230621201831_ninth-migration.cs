using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocketGymTrainer.Migrations
{
    /// <inheritdoc />
    public partial class ninthmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Section",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Exercise",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Exercise");
        }
    }
}
