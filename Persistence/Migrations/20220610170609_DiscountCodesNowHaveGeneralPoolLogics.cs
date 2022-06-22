using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class DiscountCodesNowHaveGeneralPoolLogics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_CompanyPortfolios_CompanyPortfolioId",
                table: "DiscountCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_Players_PlayerId",
                table: "DiscountCodes");

            migrationBuilder.DropIndex(
                name: "IX_DiscountCodes_CompanyPortfolioId",
                table: "DiscountCodes");

            migrationBuilder.DropColumn(
                name: "CompanyPortfolioId",
                table: "DiscountCodes");

            migrationBuilder.DropColumn(
                name: "IsAssignedToCompany",
                table: "DiscountCodes");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "DiscountCodes",
                newName: "DiscountId");

            migrationBuilder.RenameIndex(
                name: "IX_DiscountCodes_PlayerId",
                table: "DiscountCodes",
                newName: "IX_DiscountCodes_DiscountId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAssignedToUser",
                table: "DiscountCodes",
                type: "boolean",
                nullable: true,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "TemporaryReserved",
                table: "DiscountCodes",
                type: "boolean",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "PlayerId",
                table: "Batches",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Batches_PlayerId",
                table: "Batches",
                column: "PlayerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_Players_PlayerId",
                table: "Batches",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_Discounts_DiscountId",
                table: "DiscountCodes",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_Players_PlayerId",
                table: "Batches");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_Discounts_DiscountId",
                table: "DiscountCodes");

            migrationBuilder.DropIndex(
                name: "IX_Batches_PlayerId",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "TemporaryReserved",
                table: "DiscountCodes");

            migrationBuilder.DropColumn(
                name: "PlayerId",
                table: "Batches");

            migrationBuilder.RenameColumn(
                name: "DiscountId",
                table: "DiscountCodes",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_DiscountCodes_DiscountId",
                table: "DiscountCodes",
                newName: "IX_DiscountCodes_PlayerId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAssignedToUser",
                table: "DiscountCodes",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true,
                oldDefaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CompanyPortfolioId",
                table: "DiscountCodes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssignedToCompany",
                table: "DiscountCodes",
                type: "boolean",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodes_CompanyPortfolioId",
                table: "DiscountCodes",
                column: "CompanyPortfolioId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_CompanyPortfolios_CompanyPortfolioId",
                table: "DiscountCodes",
                column: "CompanyPortfolioId",
                principalTable: "CompanyPortfolios",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_Players_PlayerId",
                table: "DiscountCodes",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }
    }
}
