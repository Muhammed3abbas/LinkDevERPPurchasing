using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchasing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuantityRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "PurchaseOrderItemMapping",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "PurchaseOrderItemMapping");
        }
    }
}
