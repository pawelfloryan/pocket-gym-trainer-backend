using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PocketGymTrainer.Migrations
{
    /// <inheritdoc />
    public partial class seventeenthmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Completed",
                table: "Exercise",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Completed",
                table: "Exercise");
        }
    }
}
