using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMessageMailingUnit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "mailing_unit_id",
                table: "chat_messages",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_mailing_unit_id",
                table: "chat_messages",
                column: "mailing_unit_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_chat_messages_mailing_units_mailing_unit_id",
                table: "chat_messages",
                column: "mailing_unit_id",
                principalTable: "mailing_units",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_chat_messages_mailing_units_mailing_unit_id",
                table: "chat_messages");

            migrationBuilder.DropIndex(
                name: "ix_chat_messages_mailing_unit_id",
                table: "chat_messages");

            migrationBuilder.DropColumn(
                name: "mailing_unit_id",
                table: "chat_messages");
        }
    }
}
