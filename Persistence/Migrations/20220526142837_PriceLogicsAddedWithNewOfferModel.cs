using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    public partial class PriceLogicsAddedWithNewOfferModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyDiscountCode");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "DiscountCodes");

            migrationBuilder.CreateTable(
                name: "Offers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Price = table.Column<double>(type: "double precision", nullable: true),
                    Availability = table.Column<int>(type: "integer", nullable: true),
                    CompanyId = table.Column<int>(type: "integer", nullable: true),
                    DiscountCodeId = table.Column<int>(type: "integer", nullable: true)
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
                name: "IX_Offers_CompanyId",
                table: "Offers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_DiscountCodeId",
                table: "Offers",
                column: "DiscountCodeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "DiscountCodes",
                type: "double precision",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompanyDiscountCode",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    DiscountCodeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyDiscountCode", x => new { x.CompanyId, x.DiscountCodeId });
                    table.ForeignKey(
                        name: "FK_CompanyDiscountCode_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyDiscountCode_DiscountCodes_DiscountCodeId",
                        column: x => x.DiscountCodeId,
                        principalTable: "DiscountCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDiscountCode_DiscountCodeId",
                table: "CompanyDiscountCode",
                column: "DiscountCodeId");
        }
    }
}
