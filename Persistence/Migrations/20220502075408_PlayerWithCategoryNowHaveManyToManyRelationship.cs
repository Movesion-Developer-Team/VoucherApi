using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class PlayerWithCategoryNowHaveManyToManyRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Players_Categories_CategoryId",
                table: "Players");

            migrationBuilder.DropIndex(
                name: "IX_Players_CategoryId",
                table: "Players");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "Players");

            migrationBuilder.CreateTable(
                name: "PlayerCategories",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerCategories", x => new { x.PlayerId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_PlayerCategories_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerCategories_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerCategories_CategoryId",
                table: "PlayerCategories",
                column: "CategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerCategories");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId",
                table: "Players",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Players_CategoryId",
                table: "Players",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Players_Categories_CategoryId",
                table: "Players",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id");
        }
    }
}
