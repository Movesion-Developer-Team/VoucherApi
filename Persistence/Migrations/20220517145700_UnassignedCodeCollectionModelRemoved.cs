using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    public partial class UnassignedCodeCollectionModelRemoved : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCode_UnassignedDiscountCodeCollection_UnassignedDis~",
                table: "DiscountCode");

            migrationBuilder.DropTable(
                name: "UnassignedDiscountCodeCollection");

            migrationBuilder.DropIndex(
                name: "IX_DiscountCode_UnassignedDiscountCodesCollectionId",
                table: "DiscountCode");

            migrationBuilder.DropColumn(
                name: "UnassignedCollectionId",
                table: "DiscountCode");

            migrationBuilder.DropColumn(
                name: "UnassignedDiscountCodesCollectionId",
                table: "DiscountCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UnassignedCollectionId",
                table: "DiscountCode",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnassignedDiscountCodesCollectionId",
                table: "DiscountCode",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "UnassignedDiscountCodeCollection",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnassignedDiscountCodeCollection", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCode_UnassignedDiscountCodesCollectionId",
                table: "DiscountCode",
                column: "UnassignedDiscountCodesCollectionId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCode_UnassignedDiscountCodeCollection_UnassignedDis~",
                table: "DiscountCode",
                column: "UnassignedDiscountCodesCollectionId",
                principalTable: "UnassignedDiscountCodeCollection",
                principalColumn: "Id");
        }
    }
}
