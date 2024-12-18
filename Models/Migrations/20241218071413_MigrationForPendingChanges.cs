using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BagAPI.Models.Migrations
{
    /// <inheritdoc />
    public partial class MigrationForPendingChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "phone",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "phone",
                table: "Users",
                type: "text",
                nullable: true);
        }
    }
}
