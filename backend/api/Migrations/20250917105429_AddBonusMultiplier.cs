using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBonusMultiplier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "bonus_multiplier",
                table: "companies",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "bonus_multiplier",
                table: "companies");
        }
    }
}
