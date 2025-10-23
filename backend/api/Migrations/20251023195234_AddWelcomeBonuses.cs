using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddWelcomeBonuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "welcome_bonus",
                table: "companies",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "welcome_bonus",
                table: "companies");
        }
    }
}
