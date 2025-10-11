using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPromotions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "promotions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    variant = table.Column<string>(type: "text", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_promotions", x => x.id);
                    table.ForeignKey(
                        name: "fk_promotions_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shop_promotions",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    shop_id = table.Column<Guid>(type: "uuid", nullable: false),
                    promotion_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shop_promotions", x => x.id);
                    table.ForeignKey(
                        name: "fk_shop_promotions_promotions_promotion_id",
                        column: x => x.promotion_id,
                        principalTable: "promotions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_shop_promotions_shops_shop_id",
                        column: x => x.shop_id,
                        principalTable: "shops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_promotions_company_id",
                table: "promotions",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_shop_promotions_promotion_id",
                table: "shop_promotions",
                column: "promotion_id");

            migrationBuilder.CreateIndex(
                name: "ix_shop_promotions_shop_id",
                table: "shop_promotions",
                column: "shop_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "shop_promotions");

            migrationBuilder.DropTable(
                name: "promotions");
        }
    }
}
