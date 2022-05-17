using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class DiscountTypesChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_DiscountsTypes_DiscountTypeId",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDiscountType_DiscountsTypes_DiscountTypeId",
                table: "PlayerDiscountType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscountsTypes",
                table: "DiscountsTypes");

            migrationBuilder.RenameTable(
                name: "DiscountsTypes",
                newName: "DiscountTypes");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DiscountTypes",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscountTypes",
                table: "DiscountTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_DiscountTypes_DiscountTypeId",
                table: "Discounts",
                column: "DiscountTypeId",
                principalTable: "DiscountTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDiscountType_DiscountTypes_DiscountTypeId",
                table: "PlayerDiscountType",
                column: "DiscountTypeId",
                principalTable: "DiscountTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_DiscountTypes_DiscountTypeId",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerDiscountType_DiscountTypes_DiscountTypeId",
                table: "PlayerDiscountType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscountTypes",
                table: "DiscountTypes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "DiscountTypes");

            migrationBuilder.RenameTable(
                name: "DiscountTypes",
                newName: "DiscountsTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscountsTypes",
                table: "DiscountsTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_DiscountsTypes_DiscountTypeId",
                table: "Discounts",
                column: "DiscountTypeId",
                principalTable: "DiscountsTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerDiscountType_DiscountsTypes_DiscountTypeId",
                table: "PlayerDiscountType",
                column: "DiscountTypeId",
                principalTable: "DiscountsTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
