using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchasing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class QuantityRemovedFromitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerialNumber",
                table: "PurchaseOrderItems");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SerialNumber",
                table: "PurchaseOrderItems",
                type: "int",
                nullable: true);
        }
    }
}
