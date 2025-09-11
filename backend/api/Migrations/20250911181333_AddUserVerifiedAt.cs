using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddUserVerifiedAt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "address",
                table: "users");

            migrationBuilder.RenameColumn(
                name: "verified_at",
                table: "company_accesses",
                newName: "blocked_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "blocked_at",
                table: "company_accesses",
                newName: "verified_at");

            migrationBuilder.AddColumn<string>(
                name: "address",
                table: "users",
                type: "text",
                nullable: true);
        }
    }
}
