using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddChatToChatMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "chat_id",
                table: "chat_messages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_chat_id",
                table: "chat_messages",
                column: "chat_id");

            migrationBuilder.AddForeignKey(
                name: "fk_chat_messages_chats_chat_id",
                table: "chat_messages",
                column: "chat_id",
                principalTable: "chats",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chat_messages_chats_chat_id",
                table: "chat_messages");

            migrationBuilder.DropIndex(
                name: "ix_chat_messages_chat_id",
                table: "chat_messages");

            migrationBuilder.DropColumn(
                name: "chat_id",
                table: "chat_messages");
        }
    }
}
