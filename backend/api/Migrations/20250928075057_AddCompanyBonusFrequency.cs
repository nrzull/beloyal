using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyBonusFrequency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "bonus_frequency_count",
                table: "companies",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<TimeSpan>(
                name: "bonus_frequency_frequency",
                table: "companies",
                type: "interval",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bonus_frequency_count",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "bonus_frequency_frequency",
                table: "companies");
        }
    }
}
