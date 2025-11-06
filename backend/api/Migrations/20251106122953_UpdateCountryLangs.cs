using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCountryLangs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "country_langs",
                table: "companies");

            migrationBuilder.AddColumn<int>(
                name: "country_langs_capacity",
                table: "companies",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "country_langs_capacity",
                table: "companies");

            migrationBuilder.AddColumn<string>(
                name: "country_langs",
                table: "companies",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
