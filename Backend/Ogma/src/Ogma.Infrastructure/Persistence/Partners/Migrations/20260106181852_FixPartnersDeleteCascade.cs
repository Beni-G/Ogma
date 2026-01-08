using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ogma.Infrastructure.Persistence.Partners.Migrations
{
    /// <inheritdoc />
    public partial class FixPartnersDeleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_partner_roles_partner_role_types_roles_id",
                table: "partner_roles");

            migrationBuilder.AddForeignKey(
                name: "fk_partner_roles_partner_role_types_roles_id",
                table: "partner_roles",
                column: "roles_id",
                principalTable: "partner_role_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_partner_roles_partner_role_types_roles_id",
                table: "partner_roles");

            migrationBuilder.AddForeignKey(
                name: "fk_partner_roles_partner_role_types_roles_id",
                table: "partner_roles",
                column: "roles_id",
                principalTable: "partner_role_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
