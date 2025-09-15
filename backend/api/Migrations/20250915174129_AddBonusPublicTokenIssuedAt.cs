using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBonusPublicTokenIssuedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_bonuses_used_token",
                table: "bonuses");

            migrationBuilder.DropColumn(
                name: "used_token",
                table: "bonuses");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "public_token_issued_at",
                table: "bonuses",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "public_token_issued_at",
                table: "bonuses");

            migrationBuilder.AddColumn<string>(
                name: "used_token",
                table: "bonuses",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "ix_bonuses_used_token",
                table: "bonuses",
                column: "used_token",
                unique: true);
        }
    }
}
