using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class DiscountCodeByMistakeDidnotExistInConfigurationOfDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDiscountCode_DiscountCode_DiscountCodeId",
                table: "CompanyDiscountCode");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCode_Discounts_DiscountId",
                table: "DiscountCode");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_DiscountCode_DiscountCodeId",
                table: "Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscountCode",
                table: "DiscountCode");

            migrationBuilder.RenameTable(
                name: "DiscountCode",
                newName: "DiscountCodes");

            migrationBuilder.RenameIndex(
                name: "IX_DiscountCode_DiscountId",
                table: "DiscountCodes",
                newName: "IX_DiscountCodes_DiscountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscountCodes",
                table: "DiscountCodes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDiscountCode_DiscountCodes_DiscountCodeId",
                table: "CompanyDiscountCode",
                column: "DiscountCodeId",
                principalTable: "DiscountCodes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_Discounts_DiscountId",
                table: "DiscountCodes",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_DiscountCodes_DiscountCodeId",
                table: "Purchases",
                column: "DiscountCodeId",
                principalTable: "DiscountCodes",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyDiscountCode_DiscountCodes_DiscountCodeId",
                table: "CompanyDiscountCode");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_Discounts_DiscountId",
                table: "DiscountCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Purchases_DiscountCodes_DiscountCodeId",
                table: "Purchases");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscountCodes",
                table: "DiscountCodes");

            migrationBuilder.RenameTable(
                name: "DiscountCodes",
                newName: "DiscountCode");

            migrationBuilder.RenameIndex(
                name: "IX_DiscountCodes_DiscountId",
                table: "DiscountCode",
                newName: "IX_DiscountCode_DiscountId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscountCode",
                table: "DiscountCode",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyDiscountCode_DiscountCode_DiscountCodeId",
                table: "CompanyDiscountCode",
                column: "DiscountCodeId",
                principalTable: "DiscountCode",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCode_Discounts_DiscountId",
                table: "DiscountCode",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Purchases_DiscountCode_DiscountCodeId",
                table: "Purchases",
                column: "DiscountCodeId",
                principalTable: "DiscountCode",
                principalColumn: "Id");
        }
    }
}
