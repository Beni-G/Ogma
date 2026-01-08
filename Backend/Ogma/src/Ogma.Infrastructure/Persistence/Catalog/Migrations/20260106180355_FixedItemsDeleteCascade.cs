using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ogma.Infrastructure.Persistence.Catalog.Migrations
{
    /// <inheritdoc />
    public partial class FixedItemsDeleteCascade : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_items_categories_category_id",
                table: "items");

            migrationBuilder.DropForeignKey(
                name: "fk_items_item_types_item_type_id",
                table: "items");

            migrationBuilder.AddForeignKey(
                name: "fk_items_categories_category_id",
                table: "items",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_items_item_types_item_type_id",
                table: "items",
                column: "item_type_id",
                principalTable: "item_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_items_categories_category_id",
                table: "items");

            migrationBuilder.DropForeignKey(
                name: "fk_items_item_types_item_type_id",
                table: "items");

            migrationBuilder.AddForeignKey(
                name: "fk_items_categories_category_id",
                table: "items",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_items_item_types_item_type_id",
                table: "items",
                column: "item_type_id",
                principalTable: "item_types",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
