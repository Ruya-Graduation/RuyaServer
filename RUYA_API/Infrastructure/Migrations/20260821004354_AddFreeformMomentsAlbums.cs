using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RUYA_API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddFreeformMomentsAlbums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumItems_Artifacts_ArtifactId",
                table: "AlbumItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MemoryAlbums_Tours_TourId",
                table: "MemoryAlbums");

            migrationBuilder.DropColumn(
                name: "GeneratedAt",
                table: "MemoryAlbums");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "MemoryAlbums");

            migrationBuilder.DropColumn(
                name: "Label",
                table: "AlbumItems");

            migrationBuilder.AlterColumn<string>(
                name: "SummaryText",
                table: "MemoryAlbums",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "StartDate",
                table: "MemoryAlbums",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<int>(
                name: "ArtifactId",
                table: "AlbumItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "AiSummary",
                table: "AlbumItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "Caption",
                table: "AlbumItems",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DayLabel",
                table: "AlbumItems",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicId",
                table: "AlbumItems",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumItems_Artifacts_ArtifactId",
                table: "AlbumItems",
                column: "ArtifactId",
                principalTable: "Artifacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MemoryAlbums_Tours_TourId",
                table: "MemoryAlbums",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AlbumItems_Artifacts_ArtifactId",
                table: "AlbumItems");

            migrationBuilder.DropForeignKey(
                name: "FK_MemoryAlbums_Tours_TourId",
                table: "MemoryAlbums");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "MemoryAlbums");

            migrationBuilder.DropColumn(
                name: "Caption",
                table: "AlbumItems");

            migrationBuilder.DropColumn(
                name: "DayLabel",
                table: "AlbumItems");

            migrationBuilder.DropColumn(
                name: "PublicId",
                table: "AlbumItems");

            migrationBuilder.AlterColumn<string>(
                name: "SummaryText",
                table: "MemoryAlbums",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "GeneratedAt",
                table: "MemoryAlbums",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "MemoryAlbums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "ArtifactId",
                table: "AlbumItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AiSummary",
                table: "AlbumItems",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Label",
                table: "AlbumItems",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_AlbumItems_Artifacts_ArtifactId",
                table: "AlbumItems",
                column: "ArtifactId",
                principalTable: "Artifacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MemoryAlbums_Tours_TourId",
                table: "MemoryAlbums",
                column: "TourId",
                principalTable: "Tours",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
