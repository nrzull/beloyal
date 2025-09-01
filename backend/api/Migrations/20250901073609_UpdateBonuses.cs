using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBonuses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_mailings_shops_shop_id",
                table: "mailings");

            migrationBuilder.DropTable(
                name: "bonus_records");

            migrationBuilder.DropIndex(
                name: "ix_mailings_shop_id",
                table: "mailings");

            migrationBuilder.DropColumn(
                name: "shop_id",
                table: "mailings");

            migrationBuilder.RenameColumn(
                name: "bonus",
                table: "customers",
                newName: "bonus_amount");

            migrationBuilder.CreateTable(
                name: "bonuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    cash_income = table.Column<decimal>(type: "numeric", nullable: false),
                    cashback_percent = table.Column<decimal>(type: "numeric", nullable: false),
                    cashback_threshold = table.Column<decimal>(type: "numeric", nullable: false),
                    confirmation_code = table.Column<string>(type: "text", nullable: true),
                    confirmation_code_expired_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    agent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    shop_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bonuses", x => x.id);
                    table.ForeignKey(
                        name: "fk_bonuses_company_accesses_agent_id",
                        column: x => x.agent_id,
                        principalTable: "company_accesses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_bonuses_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_bonuses_shops_shop_id",
                        column: x => x.shop_id,
                        principalTable: "shops",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_bonuses_agent_id",
                table: "bonuses",
                column: "agent_id");

            migrationBuilder.CreateIndex(
                name: "ix_bonuses_customer_id",
                table: "bonuses",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_bonuses_shop_id",
                table: "bonuses",
                column: "shop_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bonuses");

            migrationBuilder.RenameColumn(
                name: "bonus_amount",
                table: "customers",
                newName: "bonus");

            migrationBuilder.AddColumn<Guid>(
                name: "shop_id",
                table: "mailings",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "bonus_records",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    agent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    shop_id = table.Column<Guid>(type: "uuid", nullable: true),
                    bonus = table.Column<decimal>(type: "numeric", nullable: false),
                    cashback_percent = table.Column<decimal>(type: "numeric", nullable: false),
                    cashback_threshold = table.Column<decimal>(type: "numeric", nullable: false),
                    confirmation_code = table.Column<string>(type: "text", nullable: true),
                    confirmation_code_expired_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    income = table.Column<decimal>(type: "numeric", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bonus_records", x => x.id);
                    table.ForeignKey(
                        name: "fk_bonus_records_company_accesses_agent_id",
                        column: x => x.agent_id,
                        principalTable: "company_accesses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_bonus_records_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_bonus_records_shops_shop_id",
                        column: x => x.shop_id,
                        principalTable: "shops",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_mailings_shop_id",
                table: "mailings",
                column: "shop_id");

            migrationBuilder.CreateIndex(
                name: "ix_bonus_records_agent_id",
                table: "bonus_records",
                column: "agent_id");

            migrationBuilder.CreateIndex(
                name: "ix_bonus_records_customer_id",
                table: "bonus_records",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_bonus_records_shop_id",
                table: "bonus_records",
                column: "shop_id");

            migrationBuilder.AddForeignKey(
                name: "fk_mailings_shops_shop_id",
                table: "mailings",
                column: "shop_id",
                principalTable: "shops",
                principalColumn: "id");
        }
    }
}
