using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateWhiteCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "company_id",
                table: "white_companies",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_white_companies_company_id",
                table: "white_companies",
                column: "company_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_white_companies_companies_company_id",
                table: "white_companies",
                column: "company_id",
                principalTable: "companies",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_white_companies_companies_company_id",
                table: "white_companies");

            migrationBuilder.DropIndex(
                name: "ix_white_companies_company_id",
                table: "white_companies");

            migrationBuilder.DropColumn(
                name: "company_id",
                table: "white_companies");
        }
    }
}
