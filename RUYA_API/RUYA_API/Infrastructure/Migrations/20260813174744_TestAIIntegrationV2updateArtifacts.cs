using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RUYA_API.RUYA_API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class TestAIIntegrationV2updateArtifacts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Material",
                table: "Artifacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PlaceOfDiscovery",
                table: "Artifacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Material",
                table: "Artifacts");

            migrationBuilder.DropColumn(
                name: "PlaceOfDiscovery",
                table: "Artifacts");
        }
    }
}
