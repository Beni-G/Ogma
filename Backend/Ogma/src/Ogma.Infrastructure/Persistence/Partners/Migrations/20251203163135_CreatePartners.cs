using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Ogma.Infrastructure.Persistence.Partners.Migrations
{
    /// <inheritdoc />
    public partial class CreatePartners : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "partner_role_types",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    code = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    color = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_role_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "partners",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    individual_first_name = table.Column<string>(type: "text", nullable: true),
                    individual_last_name = table.Column<string>(type: "text", nullable: true),
                    company_name = table.Column<string>(type: "text", nullable: true),
                    is_natural_person = table.Column<bool>(type: "boolean", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    display_name = table.Column<string>(type: "text", nullable: true),
                    hq_address = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partners", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "partner_bank_accounts",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    partner_id = table.Column<long>(type: "bigint", nullable: false),
                    bank = table.Column<string>(type: "text", nullable: false),
                    iban = table.Column<string>(type: "text", nullable: false),
                    currency = table.Column<string>(type: "text", nullable: false),
                    bic = table.Column<string>(type: "text", nullable: true),
                    is_default = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_bank_accounts", x => new { x.partner_id, x.id });
                    table.ForeignKey(
                        name: "fk_partner_bank_accounts_partners_partner_id",
                        column: x => x.partner_id,
                        principalTable: "partners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "partner_contact",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    partner_id = table.Column<long>(type: "bigint", nullable: false),
                    first_name = table.Column<string>(type: "text", nullable: false),
                    last_name = table.Column<string>(type: "text", nullable: false),
                    email = table.Column<string>(type: "text", nullable: true),
                    phone = table.Column<string>(type: "text", nullable: true),
                    mobile = table.Column<string>(type: "text", nullable: true),
                    title = table.Column<string>(type: "text", nullable: true),
                    job_title = table.Column<string>(type: "text", nullable: true),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_contact", x => x.id);
                    table.ForeignKey(
                        name: "fk_partner_contact_partners_partner_id",
                        column: x => x.partner_id,
                        principalTable: "partners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "partner_identifier",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    partner_id = table.Column<long>(type: "bigint", nullable: false),
                    type = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: false),
                    validity_start = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    validity_end = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    is_primary = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_identifier", x => x.id);
                    table.ForeignKey(
                        name: "fk_partner_identifier_partners_partner_id",
                        column: x => x.partner_id,
                        principalTable: "partners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "partner_roles",
                columns: table => new
                {
                    partner_id = table.Column<long>(type: "bigint", nullable: false),
                    roles_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_partner_roles", x => new { x.partner_id, x.roles_id });
                    table.ForeignKey(
                        name: "fk_partner_roles_partner_role_types_roles_id",
                        column: x => x.roles_id,
                        principalTable: "partner_role_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_partner_roles_partners_partner_id",
                        column: x => x.partner_id,
                        principalTable: "partners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_partner_contact_partner_id",
                table: "partner_contact",
                column: "partner_id");

            migrationBuilder.CreateIndex(
                name: "ix_partner_identifier_partner_id",
                table: "partner_identifier",
                column: "partner_id");

            migrationBuilder.CreateIndex(
                name: "ix_partner_roles_roles_id",
                table: "partner_roles",
                column: "roles_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "partner_bank_accounts");

            migrationBuilder.DropTable(
                name: "partner_contact");

            migrationBuilder.DropTable(
                name: "partner_identifier");

            migrationBuilder.DropTable(
                name: "partner_roles");

            migrationBuilder.DropTable(
                name: "partner_role_types");

            migrationBuilder.DropTable(
                name: "partners");
        }
    }
}
