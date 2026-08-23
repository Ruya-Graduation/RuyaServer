using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RUYA_API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMemoryAlbumMomentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "MemoryAlbums",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "MemoryAlbums",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "MemoryAlbums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "AlbumItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "MemoryAlbums");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "MemoryAlbums");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "MemoryAlbums");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "AlbumItems");
        }
    }
}
