using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RUYA_API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeTourIdNullableInMemoryAlbum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TourId",
                table: "MemoryAlbums",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "TourId",
                table: "MemoryAlbums",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
