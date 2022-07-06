using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Alluring.Migrations
{
    public partial class updateTableOrderDetail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_ProductVariant",
                table: "OrderDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_ProductVariant_ProductVariantId",
                table: "OrderDetail",
                column: "ProductVariantId",
                principalTable: "ProductVariant",
                principalColumn: "ProductVariantId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_ProductVariant_ProductVariantId",
                table: "OrderDetail");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_ProductVariant",
                table: "OrderDetail",
                column: "ProductVariantId",
                principalTable: "ProductVariant",
                principalColumn: "ProductVariantId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
