using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RUYA_API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddSiteImageSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImagePublicId",
                table: "Sites",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Sites",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePublicId",
                table: "Sites");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Sites");
        }
    }
}
