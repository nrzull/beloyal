using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBonusRecords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "bonus",
                table: "customers",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "bonus_records",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    income = table.Column<decimal>(type: "numeric", nullable: false),
                    bonus = table.Column<decimal>(type: "numeric", nullable: false),
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "bonus_records");

            migrationBuilder.DropColumn(
                name: "bonus",
                table: "customers");
        }
    }
}
