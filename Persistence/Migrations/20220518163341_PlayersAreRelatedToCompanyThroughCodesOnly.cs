using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    public partial class PlayersAreRelatedToCompanyThroughCodesOnly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyPlayer");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.AddColumn<int>(
                name: "Limit",
                table: "DiscountCode",
                type: "integer",
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
                        name: "FK_CompanyDiscountCode_DiscountCode_DiscountCodeId",
                        column: x => x.DiscountCodeId,
                        principalTable: "DiscountCode",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DiscountCodeId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_DiscountCode_DiscountCodeId",
                        column: x => x.DiscountCodeId,
                        principalTable: "DiscountCode",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Purchases_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDiscountCode_DiscountCodeId",
                table: "CompanyDiscountCode",
                column: "DiscountCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_DiscountCodeId",
                table: "Purchases",
                column: "DiscountCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_UserId",
                table: "Purchases",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyDiscountCode");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropColumn(
                name: "Limit",
                table: "DiscountCode");

            migrationBuilder.CreateTable(
                name: "CompanyPlayer",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "integer", nullable: false),
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyPlayer", x => new { x.CompanyId, x.PlayerId });
                    table.ForeignKey(
                        name: "FK_CompanyPlayer_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyPlayer_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DiscountCodeId = table.Column<int>(type: "integer", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vouchers_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vouchers_DiscountCode_DiscountCodeId",
                        column: x => x.DiscountCodeId,
                        principalTable: "DiscountCode",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyPlayer_PlayerId",
                table: "CompanyPlayer",
                column: "PlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_CategoryId",
                table: "Vouchers",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_DiscountCodeId",
                table: "Vouchers",
                column: "DiscountCodeId",
                unique: true);
        }
    }
}
