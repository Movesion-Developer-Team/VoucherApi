using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    public partial class BatchLogicsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BatchId",
                table: "DiscountCodes",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UploadTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodes_BatchId",
                table: "DiscountCodes",
                column: "BatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_Batches_BatchId",
                table: "DiscountCodes",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_Batches_BatchId",
                table: "DiscountCodes");

            migrationBuilder.DropTable(
                name: "Batches");

            migrationBuilder.DropIndex(
                name: "IX_DiscountCodes_BatchId",
                table: "DiscountCodes");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "DiscountCodes");
        }
    }
}
