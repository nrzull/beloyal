using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class addBonusUsedToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "confirmation_code",
                table: "bonuses");

            migrationBuilder.DropColumn(
                name: "confirmation_code_expired_at",
                table: "bonuses");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "bonuses",
                newName: "used_token");

            migrationBuilder.CreateIndex(
                name: "ix_bonuses_used_token",
                table: "bonuses",
                column: "used_token",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_bonuses_used_token",
                table: "bonuses");

            migrationBuilder.RenameColumn(
                name: "used_token",
                table: "bonuses",
                newName: "status");

            migrationBuilder.AddColumn<string>(
                name: "confirmation_code",
                table: "bonuses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "confirmation_code_expired_at",
                table: "bonuses",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}
