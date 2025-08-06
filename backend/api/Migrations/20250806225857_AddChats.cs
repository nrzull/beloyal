using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class AddChats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "chats",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chats", x => x.id);
                    table.ForeignKey(
                        name: "fk_chats_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_chats_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "chat_frames",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    type = table.Column<int>(type: "integer", nullable: false),
                    status = table.Column<int>(type: "integer", nullable: false),
                    rate = table.Column<int>(type: "integer", nullable: true),
                    rate_comment = table.Column<string>(type: "text", nullable: true),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    agent_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_frames", x => x.id);
                    table.ForeignKey(
                        name: "fk_chat_frames_chats_chat_id",
                        column: x => x.chat_id,
                        principalTable: "chats",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_chat_frames_company_accesses_agent_id",
                        column: x => x.agent_id,
                        principalTable: "company_accesses",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "chat_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    content = table.Column<string>(type: "text", nullable: true),
                    company_id = table.Column<Guid>(type: "uuid", nullable: true),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    agent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    frame_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_chat_messages_chat_frames_frame_id",
                        column: x => x.frame_id,
                        principalTable: "chat_frames",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_chat_messages_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_chat_messages_company_accesses_agent_id",
                        column: x => x.agent_id,
                        principalTable: "company_accesses",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_chat_messages_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_chat_frames_agent_id",
                table: "chat_frames",
                column: "agent_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_frames_chat_id",
                table: "chat_frames",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_agent_id",
                table: "chat_messages",
                column: "agent_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_company_id",
                table: "chat_messages",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_customer_id",
                table: "chat_messages",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_frame_id",
                table: "chat_messages",
                column: "frame_id");

            migrationBuilder.CreateIndex(
                name: "ix_chats_company_id",
                table: "chats",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_chats_customer_id",
                table: "chats",
                column: "customer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "chat_messages");

            migrationBuilder.DropTable(
                name: "chat_frames");

            migrationBuilder.DropTable(
                name: "chats");
        }
    }
}
