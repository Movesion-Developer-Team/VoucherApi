using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    public partial class DiscountTypeEntityAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Players_PlayerId",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Discounts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidityPeriod_StartDate",
                table: "Discounts",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidityPeriod_EndDate",
                table: "Discounts",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Discounts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "DiscountTypeId",
                table: "Discounts",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DiscountsTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountsTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerDiscountType",
                columns: table => new
                {
                    PlayerId = table.Column<int>(type: "integer", nullable: false),
                    DiscountTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerDiscountType", x => new { x.PlayerId, x.DiscountTypeId });
                    table.ForeignKey(
                        name: "FK_PlayerDiscountType_DiscountsTypes_DiscountTypeId",
                        column: x => x.DiscountTypeId,
                        principalTable: "DiscountsTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerDiscountType_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_DiscountTypeId",
                table: "Discounts",
                column: "DiscountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDiscountType_DiscountTypeId",
                table: "PlayerDiscountType",
                column: "DiscountTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_DiscountsTypes_DiscountTypeId",
                table: "Discounts",
                column: "DiscountTypeId",
                principalTable: "DiscountsTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Players_PlayerId",
                table: "Discounts",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_DiscountsTypes_DiscountTypeId",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Players_PlayerId",
                table: "Discounts");

            migrationBuilder.DropTable(
                name: "PlayerDiscountType");

            migrationBuilder.DropTable(
                name: "DiscountsTypes");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_DiscountTypeId",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "DiscountTypeId",
                table: "Discounts");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidityPeriod_StartDate",
                table: "Discounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidityPeriod_EndDate",
                table: "Discounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "PlayerId",
                table: "Discounts",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "Discounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Players_PlayerId",
                table: "Discounts",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
