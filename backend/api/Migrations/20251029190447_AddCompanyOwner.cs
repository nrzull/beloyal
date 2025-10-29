using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCompanyOwner : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "owner_id",
                table: "companies",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_companies_owner_id",
                table: "companies",
                column: "owner_id");

            migrationBuilder.AddForeignKey(
                name: "fk_companies_users_owner_id",
                table: "companies",
                column: "owner_id",
                principalTable: "users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_companies_users_owner_id",
                table: "companies");

            migrationBuilder.DropIndex(
                name: "ix_companies_owner_id",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "owner_id",
                table: "companies");
        }
    }
}
