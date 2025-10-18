using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePromotions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "promotion_title",
                table: "bonuses",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "promotion_variant",
                table: "bonuses",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "promotion_title",
                table: "bonuses");

            migrationBuilder.DropColumn(
                name: "promotion_variant",
                table: "bonuses");
        }
    }
}
