using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class MultipleRelationsChangedBetweenDiscountCodeVoucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_DiscountCode_DiscountCodeId",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Categories_CategoryId",
                table: "Vouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Discounts_DiscountId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_DiscountId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_DiscountCodeId",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "DiscountCodeId",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "UnassignedDiscountCodeCollectionsId",
                table: "DiscountCode",
                newName: "UnassignedDiscountCodesCollectionId");

            migrationBuilder.RenameIndex(
                name: "IX_DiscountCode_UnassignedDiscountCodeCollectionsId",
                table: "DiscountCode",
                newName: "IX_DiscountCode_UnassignedDiscountCodesCollectionId");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Vouchers",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "DiscountCodeId",
                table: "Vouchers",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Discounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "DiscountCode",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_DiscountCodeId",
                table: "Vouchers",
                column: "DiscountCodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCode_DiscountId",
                table: "DiscountCode",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCode_Discounts_DiscountId",
                table: "DiscountCode",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Categories_CategoryId",
                table: "Vouchers",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_DiscountCode_DiscountCodeId",
                table: "Vouchers",
                column: "DiscountCodeId",
                principalTable: "DiscountCode",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCode_Discounts_DiscountId",
                table: "DiscountCode");

            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_Categories_CategoryId",
                table: "Vouchers");

            migrationBuilder.DropForeignKey(
                name: "FK_Vouchers_DiscountCode_DiscountCodeId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Vouchers_DiscountCodeId",
                table: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_DiscountCode_DiscountId",
                table: "DiscountCode");

            migrationBuilder.DropColumn(
                name: "DiscountCodeId",
                table: "Vouchers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "DiscountId",
                table: "DiscountCode");

            migrationBuilder.RenameColumn(
                name: "UnassignedDiscountCodesCollectionId",
                table: "DiscountCode",
                newName: "UnassignedDiscountCodeCollectionsId");

            migrationBuilder.RenameIndex(
                name: "IX_DiscountCode_UnassignedDiscountCodesCollectionId",
                table: "DiscountCode",
                newName: "IX_DiscountCode_UnassignedDiscountCodeCollectionsId");

            migrationBuilder.AlterColumn<int>(
                name: "CategoryId",
                table: "Vouchers",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountId",
                table: "Vouchers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DiscountCodeId",
                table: "Discounts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_DiscountId",
                table: "Vouchers",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_DiscountCodeId",
                table: "Discounts",
                column: "DiscountCodeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_DiscountCode_DiscountCodeId",
                table: "Discounts",
                column: "DiscountCodeId",
                principalTable: "DiscountCode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Categories_CategoryId",
                table: "Vouchers",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vouchers_Discounts_DiscountId",
                table: "Vouchers",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
