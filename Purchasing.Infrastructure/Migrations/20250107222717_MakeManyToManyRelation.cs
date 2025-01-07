using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Purchasing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MakeManyToManyRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseOrderItemMapping",
                columns: table => new
                {
                    SerialNumber = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PurchaseOrderPOnumber = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    PurchaseOrderItemCode = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseOrderItemMapping", x => x.SerialNumber);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItemMapping_PurchaseOrderItems_PurchaseOrderItemCode",
                        column: x => x.PurchaseOrderItemCode,
                        principalTable: "PurchaseOrderItems",
                        principalColumn: "Code",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseOrderItemMapping_PurchaseOrders_PurchaseOrderPOnumber",
                        column: x => x.PurchaseOrderPOnumber,
                        principalTable: "PurchaseOrders",
                        principalColumn: "POnumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItemMapping_PurchaseOrderItemCode",
                table: "PurchaseOrderItemMapping",
                column: "PurchaseOrderItemCode");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseOrderItemMapping_PurchaseOrderPOnumber",
                table: "PurchaseOrderItemMapping",
                column: "PurchaseOrderPOnumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseOrderItemMapping");
        }
    }
}
