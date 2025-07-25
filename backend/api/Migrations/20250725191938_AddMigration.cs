using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_company_accesses_user_id",
                table: "company_accesses");

            migrationBuilder.CreateIndex(
                name: "ix_company_accesses_user_id_company_id",
                table: "company_accesses",
                columns: new[] { "user_id", "company_id" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_company_accesses_user_id_company_id",
                table: "company_accesses");

            migrationBuilder.CreateIndex(
                name: "ix_company_accesses_user_id",
                table: "company_accesses",
                column: "user_id");
        }
    }
}
