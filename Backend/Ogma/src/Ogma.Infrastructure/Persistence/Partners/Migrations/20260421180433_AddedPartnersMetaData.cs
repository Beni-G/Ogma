using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ogma.Infrastructure.Persistence.Partners.Migrations
{
    /// <inheritdoc />
    public partial class AddedPartnersMetaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "partners",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "partners",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "partners",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "partner_role_types",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "partner_role_types",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "partner_role_types",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "partner_identifier",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "partner_identifier",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "partner_identifier",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "partner_contact",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "partner_contact",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "partner_contact",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "partner_bank_accounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "partner_bank_accounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "partner_bank_accounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "partners");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "partners");

            migrationBuilder.DropColumn(
                name: "version",
                table: "partners");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "partner_role_types");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "partner_role_types");

            migrationBuilder.DropColumn(
                name: "version",
                table: "partner_role_types");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "partner_identifier");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "partner_identifier");

            migrationBuilder.DropColumn(
                name: "version",
                table: "partner_identifier");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "partner_contact");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "partner_contact");

            migrationBuilder.DropColumn(
                name: "version",
                table: "partner_contact");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "partner_bank_accounts");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "partner_bank_accounts");

            migrationBuilder.DropColumn(
                name: "version",
                table: "partner_bank_accounts");
        }
    }
}
