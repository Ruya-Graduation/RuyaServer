using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RUYA_API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddHoursTicketCrowdsDescriptionToSite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Crowds",
                table: "Sites",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Sites",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Hours",
                table: "Sites",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ticket",
                table: "Sites",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Crowds",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Hours",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "Ticket",
                table: "Sites");
        }
    }
}
