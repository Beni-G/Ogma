using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ogma.Infrastructure.Persistence.Orders.Migrations
{
    /// <inheritdoc />
    public partial class FixOrderLinesConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_line_orders_order_id",
                table: "order_line");

            migrationBuilder.DropPrimaryKey(
                name: "pk_order_line",
                table: "order_line");

            migrationBuilder.RenameTable(
                name: "order_line",
                newName: "order_lines");

            migrationBuilder.RenameIndex(
                name: "ix_order_line_order_id",
                table: "order_lines",
                newName: "ix_order_lines_order_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_order_lines",
                table: "order_lines",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_order_lines_orders_order_id",
                table: "order_lines",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_order_lines_orders_order_id",
                table: "order_lines");

            migrationBuilder.DropPrimaryKey(
                name: "pk_order_lines",
                table: "order_lines");

            migrationBuilder.RenameTable(
                name: "order_lines",
                newName: "order_line");

            migrationBuilder.RenameIndex(
                name: "ix_order_lines_order_id",
                table: "order_line",
                newName: "ix_order_line_order_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_order_line",
                table: "order_line",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_order_line_orders_order_id",
                table: "order_line",
                column: "order_id",
                principalTable: "orders",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
