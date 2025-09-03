using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class RemoveChatFrame : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chat_messages_chat_frames_frame_id",
                table: "chat_messages");

            migrationBuilder.DropTable(
                name: "chat_frames");

            migrationBuilder.DropIndex(
                name: "ix_chat_messages_frame_id",
                table: "chat_messages");

            migrationBuilder.DropColumn(
                name: "frame_id",
                table: "chat_messages");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "frame_id",
                table: "chat_messages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "chat_frames",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    agent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    rate = table.Column<int>(type: "integer", nullable: true),
                    rate_comment = table.Column<string>(type: "text", nullable: true),
                    status = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_frames", x => x.id);
                    table.ForeignKey(
                        name: "fk_chat_frames_chats_chat_id",
                        column: x => x.chat_id,
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_chat_frames_company_accesses_agent_id",
                        column: x => x.agent_id,
                        principalTable: "company_accesses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_frame_id",
                table: "chat_messages",
                column: "frame_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_frames_agent_id",
                table: "chat_frames",
                column: "agent_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_frames_chat_id",
                table: "chat_frames",
                column: "chat_id");

            migrationBuilder.AddForeignKey(
                name: "fk_chat_messages_chat_frames_frame_id",
                table: "chat_messages",
                column: "frame_id",
                principalTable: "chat_frames",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
