using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBonusScheduling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "bonus_scheduling",
                table: "companies",
                type: "interval",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "scheduled_at",
                table: "bonuses",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "bonuses",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bonus_scheduling",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "scheduled_at",
                table: "bonuses");

            migrationBuilder.DropColumn(
                name: "status",
                table: "bonuses");
        }
    }
}
