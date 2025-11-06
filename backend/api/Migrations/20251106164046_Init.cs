using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Beloyal.API.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "admin_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    permissions = table.Column<List<string>>(type: "text[]", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_admin_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "auth_otps",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    phone = table.Column<string>(type: "text", nullable: false),
                    code = table.Column<string>(type: "text", nullable: false),
                    expired_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_auth_otps", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "persisted_tokens",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    hash = table.Column<string>(type: "text", nullable: false),
                    issued_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_persisted_tokens", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    verified_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    country_lang = table.Column<string>(type: "text", nullable: false),
                    phone = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: true),
                    last_name = table.Column<string>(type: "text", nullable: true),
                    middle_name = table.Column<string>(type: "text", nullable: true),
                    max_user_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    tg_user_id = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "admin_accesses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_admin_accesses", x => x.id);
                    table.ForeignKey(
                        name: "fk_admin_accesses_admin_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "admin_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_admin_accesses_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    tin = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    time_zone = table.Column<string>(type: "text", nullable: false),
                    country_code = table.Column<string>(type: "text", nullable: false),
                    cashback_percent = table.Column<decimal>(type: "numeric", nullable: false),
                    cashback_threshold = table.Column<decimal>(type: "numeric", nullable: false),
                    bonus_multiplier = table.Column<decimal>(type: "numeric", nullable: false),
                    bonus_lifetime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    bonus_frequency_count = table.Column<int>(type: "integer", nullable: true),
                    bonus_frequency_frequency = table.Column<TimeSpan>(type: "interval", nullable: true),
                    bonus_scheduling = table.Column<TimeSpan>(type: "interval", nullable: true),
                    welcome_bonus = table.Column<decimal>(type: "numeric", nullable: true),
                    level_lifetime = table.Column<TimeSpan>(type: "interval", nullable: false),
                    owner_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_companies", x => x.id);
                    table.ForeignKey(
                        name: "fk_companies_users_owner_id",
                        column: x => x.owner_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "company_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    permissions = table.Column<List<string>>(type: "text[]", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_roles", x => x.id);
                    table.ForeignKey(
                        name: "fk_company_roles_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "levels",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    cashback_percent = table.Column<decimal>(type: "numeric", nullable: false),
                    points_capacity = table.Column<decimal>(type: "numeric", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_levels", x => x.id);
                    table.ForeignKey(
                        name: "fk_levels_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "mailings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    content = table.Column<string>(type: "text", nullable: false),
                    attachment_keys = table.Column<List<string>>(type: "text[]", nullable: true),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mailings", x => x.id);
                    table.ForeignKey(
                        name: "fk_mailings_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "max_bots",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_max_bots", x => x.id);
                    table.ForeignKey(
                        name: "fk_max_bots_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "shops",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    time_zone = table.Column<string>(type: "text", nullable: true),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shops", x => x.id);
                    table.ForeignKey(
                        name: "fk_shops_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tg_bots",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    token = table.Column<string>(type: "text", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_tg_bots", x => x.id);
                    table.ForeignKey(
                        name: "fk_tg_bots_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "white_companies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    tin = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    expired_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_white_companies", x => x.id);
                    table.ForeignKey(
                        name: "fk_white_companies_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "company_accesses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    blocked_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_company_accesses", x => x.id);
                    table.ForeignKey(
                        name: "fk_company_accesses_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_company_accesses_company_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "company_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_company_accesses_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
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

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    bonus_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    level_points_amount = table.Column<decimal>(type: "numeric", nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_id = table.Column<Guid>(type: "uuid", nullable: false),
                    max_bot_id = table.Column<Guid>(type: "uuid", nullable: true),
                    tg_bot_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.id);
                    table.ForeignKey(
                        name: "fk_customers_companies_company_id",
                        column: x => x.company_id,
                        principalTable: "companies",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_customers_max_bots_max_bot_id",
                        column: x => x.max_bot_id,
                        principalTable: "max_bots",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_customers_tg_bots_tg_bot_id",
                        column: x => x.tg_bot_id,
                        principalTable: "tg_bots",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_customers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shop_accesses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    shop_id = table.Column<Guid>(type: "uuid", nullable: false),
                    company_access_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_shop_accesses", x => x.id);
                    table.ForeignKey(
                        name: "fk_shop_accesses_company_accesses_company_access_id",
                        column: x => x.company_access_id,
                        principalTable: "company_accesses",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_shop_accesses_shops_shop_id",
                        column: x => x.shop_id,
                        principalTable: "shops",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "mailing_units",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    mailing_id = table.Column<Guid>(type: "uuid", nullable: false),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_mailing_units", x => x.id);
                    table.ForeignKey(
                        name: "fk_mailing_units_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_mailing_units_mailings_mailing_id",
                        column: x => x.mailing_id,
                        principalTable: "mailings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "bonuses",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    scheduled_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    type = table.Column<string>(type: "text", nullable: false),
                    status = table.Column<string>(type: "text", nullable: false),
                    amount = table.Column<decimal>(type: "numeric", nullable: false),
                    raw_cash_income = table.Column<decimal>(type: "numeric", nullable: false),
                    cash_income = table.Column<decimal>(type: "numeric", nullable: false),
                    cashback_percent = table.Column<decimal>(type: "numeric", nullable: false),
                    cashback_threshold = table.Column<decimal>(type: "numeric", nullable: false),
                    promotion_title = table.Column<string>(type: "text", nullable: true),
                    promotion_variant = table.Column<string>(type: "text", nullable: true),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    agent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    shop_id = table.Column<Guid>(type: "uuid", nullable: true),
                    booster_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_bonuses", x => x.id);
                    table.ForeignKey(
                        name: "fk_bonuses_boosters_booster_id",
                        column: x => x.booster_id,
                        principalTable: "boosters",
                        principalColumn: "id");
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

            migrationBuilder.CreateTable(
                name: "chat_messages",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    read_at = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    attachment_keys = table.Column<List<string>>(type: "text[]", nullable: true),
                    company_id = table.Column<Guid>(type: "uuid", nullable: true),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    agent_id = table.Column<Guid>(type: "uuid", nullable: true),
                    chat_id = table.Column<Guid>(type: "uuid", nullable: false),
                    mailing_unit_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_chat_messages", x => x.id);
                    table.ForeignKey(
                        name: "fk_chat_messages_chats_chat_id",
                        column: x => x.chat_id,
                        principalTable: "chats",
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
                    table.ForeignKey(
                        name: "fk_chat_messages_mailing_units_mailing_unit_id",
                        column: x => x.mailing_unit_id,
                        principalTable: "mailing_units",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_admin_accesses_role_id",
                table: "admin_accesses",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_admin_accesses_user_id",
                table: "admin_accesses",
                column: "user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_bonuses_agent_id",
                table: "bonuses",
                column: "agent_id");

            migrationBuilder.CreateIndex(
                name: "ix_bonuses_booster_id",
                table: "bonuses",
                column: "booster_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_bonuses_customer_id",
                table: "bonuses",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_bonuses_shop_id",
                table: "bonuses",
                column: "shop_id");

            migrationBuilder.CreateIndex(
                name: "ix_boosters_customer_id",
                table: "boosters",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_agent_id",
                table: "chat_messages",
                column: "agent_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_chat_id",
                table: "chat_messages",
                column: "chat_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_company_id",
                table: "chat_messages",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_customer_id",
                table: "chat_messages",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_chat_messages_mailing_unit_id",
                table: "chat_messages",
                column: "mailing_unit_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_chats_company_id",
                table: "chats",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_chats_customer_id",
                table: "chats",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_companies_owner_id",
                table: "companies",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "ix_companies_tin",
                table: "companies",
                column: "tin",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_company_accesses_company_id",
                table: "company_accesses",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_accesses_role_id",
                table: "company_accesses",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_company_accesses_user_id_company_id",
                table: "company_accesses",
                columns: new[] { "user_id", "company_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_company_roles_company_id",
                table: "company_roles",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_customers_company_id",
                table: "customers",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_customers_max_bot_id",
                table: "customers",
                column: "max_bot_id");

            migrationBuilder.CreateIndex(
                name: "ix_customers_tg_bot_id",
                table: "customers",
                column: "tg_bot_id");

            migrationBuilder.CreateIndex(
                name: "ix_customers_user_id_company_id",
                table: "customers",
                columns: new[] { "user_id", "company_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_levels_company_id",
                table: "levels",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_mailing_units_customer_id",
                table: "mailing_units",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_mailing_units_mailing_id",
                table: "mailing_units",
                column: "mailing_id");

            migrationBuilder.CreateIndex(
                name: "ix_mailings_company_id",
                table: "mailings",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_max_bots_company_id",
                table: "max_bots",
                column: "company_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_promotions_company_id",
                table: "promotions",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_shop_accesses_company_access_id_shop_id",
                table: "shop_accesses",
                columns: new[] { "company_access_id", "shop_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_shop_accesses_shop_id",
                table: "shop_accesses",
                column: "shop_id");

            migrationBuilder.CreateIndex(
                name: "ix_shop_promotions_promotion_id",
                table: "shop_promotions",
                column: "promotion_id");

            migrationBuilder.CreateIndex(
                name: "ix_shop_promotions_shop_id",
                table: "shop_promotions",
                column: "shop_id");

            migrationBuilder.CreateIndex(
                name: "ix_shops_company_id",
                table: "shops",
                column: "company_id");

            migrationBuilder.CreateIndex(
                name: "ix_tg_bots_company_id",
                table: "tg_bots",
                column: "company_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_max_user_id",
                table: "users",
                column: "max_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_phone",
                table: "users",
                column: "phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_tg_user_id",
                table: "users",
                column: "tg_user_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_white_companies_company_id",
                table: "white_companies",
                column: "company_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_white_companies_tin",
                table: "white_companies",
                column: "tin",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "admin_accesses");

            migrationBuilder.DropTable(
                name: "auth_otps");

            migrationBuilder.DropTable(
                name: "bonuses");

            migrationBuilder.DropTable(
                name: "chat_messages");

            migrationBuilder.DropTable(
                name: "levels");

            migrationBuilder.DropTable(
                name: "persisted_tokens");

            migrationBuilder.DropTable(
                name: "shop_accesses");

            migrationBuilder.DropTable(
                name: "shop_promotions");

            migrationBuilder.DropTable(
                name: "white_companies");

            migrationBuilder.DropTable(
                name: "admin_roles");

            migrationBuilder.DropTable(
                name: "boosters");

            migrationBuilder.DropTable(
                name: "chats");

            migrationBuilder.DropTable(
                name: "mailing_units");

            migrationBuilder.DropTable(
                name: "company_accesses");

            migrationBuilder.DropTable(
                name: "promotions");

            migrationBuilder.DropTable(
                name: "shops");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "mailings");

            migrationBuilder.DropTable(
                name: "company_roles");

            migrationBuilder.DropTable(
                name: "max_bots");

            migrationBuilder.DropTable(
                name: "tg_bots");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
