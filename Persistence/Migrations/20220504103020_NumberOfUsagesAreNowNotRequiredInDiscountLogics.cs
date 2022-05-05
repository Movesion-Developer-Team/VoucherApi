using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    public partial class NumberOfUsagesAreNowNotRequiredInDiscountLogics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCode_UnassignedDiscountCodeCollections_UnassignedDi~",
                table: "DiscountCode");

            migrationBuilder.DropTable(
                name: "UnassignedDiscountCodeCollections");

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfUsagePerUser",
                table: "Discounts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfUsagePerCompany",
                table: "Discounts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

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

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCode_UnassignedDiscountCodeCollection_UnassignedDis~",
                table: "DiscountCode",
                column: "UnassignedDiscountCodeCollectionsId",
                principalTable: "UnassignedDiscountCodeCollection",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCode_UnassignedDiscountCodeCollection_UnassignedDis~",
                table: "DiscountCode");

            migrationBuilder.DropTable(
                name: "UnassignedDiscountCodeCollection");

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfUsagePerUser",
                table: "Discounts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NumberOfUsagePerCompany",
                table: "Discounts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "UnassignedDiscountCodeCollections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnassignedDiscountCodeCollections", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCode_UnassignedDiscountCodeCollections_UnassignedDi~",
                table: "DiscountCode",
                column: "UnassignedDiscountCodeCollectionsId",
                principalTable: "UnassignedDiscountCodeCollections",
                principalColumn: "Id");
        }
    }
}
