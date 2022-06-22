using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class CompanyPortfolioModelAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyPortfolio_Companies_CompanyId",
                table: "CompanyPortfolio");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyPortfolio_Discounts_DiscountId",
                table: "CompanyPortfolio");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_CompanyPortfolio_CompanyPortfolioId",
                table: "DiscountCodes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CompanyPortfolio_CompanyId_DiscountId",
                table: "CompanyPortfolio");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyPortfolio",
                table: "CompanyPortfolio");

            migrationBuilder.RenameTable(
                name: "CompanyPortfolio",
                newName: "CompanyPortfolios");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyPortfolio_DiscountId",
                table: "CompanyPortfolios",
                newName: "IX_CompanyPortfolios_DiscountId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CompanyPortfolios_CompanyId_DiscountId",
                table: "CompanyPortfolios",
                columns: new[] { "CompanyId", "DiscountId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyPortfolios",
                table: "CompanyPortfolios",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyPortfolios_Companies_CompanyId",
                table: "CompanyPortfolios",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyPortfolios_Discounts_DiscountId",
                table: "CompanyPortfolios",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_CompanyPortfolios_CompanyPortfolioId",
                table: "DiscountCodes",
                column: "CompanyPortfolioId",
                principalTable: "CompanyPortfolios",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyPortfolios_Companies_CompanyId",
                table: "CompanyPortfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyPortfolios_Discounts_DiscountId",
                table: "CompanyPortfolios");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_CompanyPortfolios_CompanyPortfolioId",
                table: "DiscountCodes");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_CompanyPortfolios_CompanyId_DiscountId",
                table: "CompanyPortfolios");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyPortfolios",
                table: "CompanyPortfolios");

            migrationBuilder.RenameTable(
                name: "CompanyPortfolios",
                newName: "CompanyPortfolio");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyPortfolios_DiscountId",
                table: "CompanyPortfolio",
                newName: "IX_CompanyPortfolio_DiscountId");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_CompanyPortfolio_CompanyId_DiscountId",
                table: "CompanyPortfolio",
                columns: new[] { "CompanyId", "DiscountId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyPortfolio",
                table: "CompanyPortfolio",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyPortfolio_Companies_CompanyId",
                table: "CompanyPortfolio",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyPortfolio_Discounts_DiscountId",
                table: "CompanyPortfolio",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_CompanyPortfolio_CompanyPortfolioId",
                table: "DiscountCodes",
                column: "CompanyPortfolioId",
                principalTable: "CompanyPortfolio",
                principalColumn: "Id");
        }
    }
}
