using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RUYA_API.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddConversationChatEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_TourStops_StopId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_StopId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "AgentType",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Sender",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Timestamp",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "StopId",
                table: "Conversations");

            migrationBuilder.RenameColumn(
                name: "StartedAt",
                table: "Conversations",
                newName: "LastMessageAt");

            migrationBuilder.AddColumn<int>(
                name: "InputTokens",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Metadata",
                table: "Messages",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModelName",
                table: "Messages",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OutputTokens",
                table: "Messages",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "Messages",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CurrentArtifactId",
                table: "Conversations",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentLanguage",
                table: "Conversations",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CurrentMode",
                table: "Conversations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ModelName",
                table: "Conversations",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Conversations",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "Conversations",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Conversations",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Conversations",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConversationAttachments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ConversationId = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<int>(type: "int", nullable: true),
                    FileUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    PublicId = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    FileType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MimeType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsPrimaryFrame = table.Column<bool>(type: "bit", nullable: false),
                    VisionResultJson = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationAttachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConversationAttachments_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversationAttachments_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_CurrentArtifactId",
                table: "Conversations",
                column: "CurrentArtifactId");

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_UserId",
                table: "Conversations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationAttachments_ConversationId",
                table: "ConversationAttachments",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_ConversationAttachments_MessageId",
                table: "ConversationAttachments",
                column: "MessageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Artifacts_CurrentArtifactId",
                table: "Conversations",
                column: "CurrentArtifactId",
                principalTable: "Artifacts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Users_UserId",
                table: "Conversations",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Artifacts_CurrentArtifactId",
                table: "Conversations");

            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Users_UserId",
                table: "Conversations");

            migrationBuilder.DropTable(
                name: "ConversationAttachments");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_CurrentArtifactId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_UserId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "InputTokens",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Metadata",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ModelName",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "OutputTokens",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "CurrentArtifactId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "CurrentLanguage",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "CurrentMode",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "ModelName",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "Summary",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "Title",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Conversations");

            migrationBuilder.RenameColumn(
                name: "LastMessageAt",
                table: "Conversations",
                newName: "StartedAt");

            migrationBuilder.AddColumn<string>(
                name: "AgentType",
                table: "Messages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sender",
                table: "Messages",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Timestamp",
                table: "Messages",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "StopId",
                table: "Conversations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_StopId",
                table: "Conversations",
                column: "StopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_TourStops_StopId",
                table: "Conversations",
                column: "StopId",
                principalTable: "TourStops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
