using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBoosters : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "booster_id",
                table: "bonuses",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "boosters",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    expired_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    cashback_percent = table.Column<decimal>(type: "numeric", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_boosters", x => x.id);
                    table.ForeignKey(
                        name: "fk_boosters_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_bonuses_booster_id",
                table: "bonuses",
                column: "booster_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_boosters_customer_id",
                table: "boosters",
                column: "customer_id");

            migrationBuilder.AddForeignKey(
                name: "fk_bonuses_boosters_booster_id",
                table: "bonuses",
                column: "booster_id",
                principalTable: "boosters",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_bonuses_boosters_booster_id",
                table: "bonuses");

            migrationBuilder.DropTable(
                name: "boosters");

            migrationBuilder.DropIndex(
                name: "ix_bonuses_booster_id",
                table: "bonuses");

            migrationBuilder.DropColumn(
                name: "booster_id",
                table: "bonuses");
        }
    }
}
