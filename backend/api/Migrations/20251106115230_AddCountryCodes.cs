using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCountryCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_companies_users_owner_id",
                table: "companies");

            migrationBuilder.AddColumn<string>(
                name: "country_lang",
                table: "users",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "owner_id",
                table: "companies",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "country_code",
                table: "companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "country_langs",
                table: "companies",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "fk_companies_users_owner_id",
                table: "companies",
                column: "owner_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_companies_users_owner_id",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "country_lang",
                table: "users");

            migrationBuilder.DropColumn(
                name: "country_code",
                table: "companies");

            migrationBuilder.DropColumn(
                name: "country_langs",
                table: "companies");

            migrationBuilder.AlterColumn<Guid>(
                name: "owner_id",
                table: "companies",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "fk_companies_users_owner_id",
                table: "companies",
                column: "owner_id",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}
