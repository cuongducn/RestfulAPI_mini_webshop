using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_Alluring.Migrations
{
    public partial class updateSomeTB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DiscountId",
                table: "Product",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "Discount",
                columns: table => new
                {
                    DiscountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DiscountName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DiscountDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PercentDiscount = table.Column<int>(type: "int", nullable: true),
                    isActive = table.Column<bool>(type: "bit", nullable: true),
                    DateStart = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateStop = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Discount", x => x.DiscountId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Product_DiscountId",
                table: "Product",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Product_Discount",
                table: "Product",
                column: "DiscountId",
                principalTable: "Discount",
                principalColumn: "DiscountId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Product_Discount",
                table: "Product");

            migrationBuilder.DropTable(
                name: "Discount");

            migrationBuilder.DropIndex(
                name: "IX_Product_DiscountId",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Product");
        }
    }
}
