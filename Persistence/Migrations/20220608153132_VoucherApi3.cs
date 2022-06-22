using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    public partial class VoucherApi3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_Discounts_DiscountId",
                table: "DiscountCodes");

            migrationBuilder.DropTable(
                name: "CompanyDiscount");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.RenameColumn(
                name: "DiscountId",
                table: "DiscountCodes",
                newName: "PlayerId");

            migrationBuilder.RenameIndex(
                name: "IX_DiscountCodes_DiscountId",
                table: "DiscountCodes",
                newName: "IX_DiscountCodes_PlayerId");

            migrationBuilder.AddColumn<int>(
                name: "CompanyPortfolioId",
                table: "DiscountCodes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountTypeId",
                table: "Batches",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "PurchasePrice",
                table: "Batches",
                type: "double precision",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompanyPortfolio",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    DiscountId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPortfolio", x => x.Id);
                    table.UniqueConstraint("AK_CompanyPortfolio_CompanyId_DiscountId", x => new { x.CompanyId, x.DiscountId });
                    table.ForeignKey(
                        name: "FK_CompanyPortfolio_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyPortfolio_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodes_CompanyPortfolioId",
                table: "DiscountCodes",
                column: "CompanyPortfolioId");

            migrationBuilder.CreateIndex(
                name: "IX_Batches_DiscountTypeId",
                table: "Batches",
                column: "DiscountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPortfolio_DiscountId",
                table: "CompanyPortfolio",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Batches_DiscountTypes_DiscountTypeId",
                table: "Batches",
                column: "DiscountTypeId",
                principalTable: "DiscountTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_CompanyPortfolio_CompanyPortfolioId",
                table: "DiscountCodes",
                column: "CompanyPortfolioId",
                principalTable: "CompanyPortfolio",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_Players_PlayerId",
                table: "DiscountCodes",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Batches_DiscountTypes_DiscountTypeId",
                table: "Batches");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_CompanyPortfolio_CompanyPortfolioId",
                table: "DiscountCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_Players_PlayerId",
                table: "DiscountCodes");

            migrationBuilder.DropTable(
                name: "CompanyPortfolio");

            migrationBuilder.DropIndex(
                name: "IX_DiscountCodes_CompanyPortfolioId",
                table: "DiscountCodes");

            migrationBuilder.DropIndex(
                name: "IX_Batches_DiscountTypeId",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "CompanyPortfolioId",
                table: "DiscountCodes");

            migrationBuilder.DropColumn(
                name: "DiscountTypeId",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "Batches");

            migrationBuilder.RenameColumn(
                name: "PlayerId",
                table: "DiscountCodes",
                newName: "DiscountId");

            migrationBuilder.RenameIndex(
                name: "IX_DiscountCodes_PlayerId",
                table: "DiscountCodes",
                newName: "IX_DiscountCodes_DiscountId");

            migrationBuilder.CreateTable(
                name: "CompanyDiscount",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    DiscountId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDiscount", x => new { x.CompanyId, x.DiscountId });
                    table.ForeignKey(
                        name: "FK_CompanyDiscount_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyDiscount_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CompanyId = table.Column<int>(type: "integer", nullable: true),
                    DiscountCodeId = table.Column<int>(type: "integer", nullable: true),
                    Availability = table.Column<int>(type: "integer", nullable: true),
                    Price = table.Column<double>(type: "double precision", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Offers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Offers_DiscountCodes_DiscountCodeId",
                        column: x => x.DiscountCodeId,
                        principalTable: "DiscountCodes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDiscount_DiscountId",
                table: "CompanyDiscount",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_CompanyId",
                table: "Offers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_DiscountCodeId",
                table: "Offers",
                column: "DiscountCodeId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_Discounts_DiscountId",
                table: "DiscountCodes",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id");
        }
    }
}
