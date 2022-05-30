using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    public partial class SecondBigMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCode_UnassignedDiscountCodeCollection_UnassignedDis~",
                table: "DiscountCode");

            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_DiscountCode_DiscountCodeId",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Players_PlayerId",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "UnassignedDiscountCodeCollection");

            migrationBuilder.DropTable(
                name: "Vouchers");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_DiscountCodeId",
                table: "Discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscountCode",
                table: "DiscountCode");

            migrationBuilder.DropIndex(
                name: "IX_DiscountCode_UnassignedDiscountCodeCollectionsId",
                table: "DiscountCode");

            migrationBuilder.DropColumn(
                name: "IsSent",
                table: "InvitationCodes");

            migrationBuilder.DropColumn(
                name: "DiscountCodeId",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "DiscountType",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "NumberOfUsagePerCompany",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "NumberOfEmployees",
                table: "Companies");

            migrationBuilder.RenameTable(
                name: "DiscountCode",
                newName: "DiscountCodes");

            migrationBuilder.RenameColumn(
                name: "NumberOfUsagePerUser",
                table: "Discounts",
                newName: "DiscountTypeId");

            migrationBuilder.RenameColumn(
                name: "UnassignedDiscountCodeCollectionsId",
                table: "DiscountCodes",
                newName: "UsageLimit");

            migrationBuilder.RenameColumn(
                name: "UnassignedCollectionId",
                table: "DiscountCodes",
                newName: "DiscountId");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Users",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "JoinRequestId",
                table: "InvitationCodes",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ValidityPeriod_StartDate",
                table: "Discounts",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<DateTimeOffset>(
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

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Discounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BatchId",
                table: "DiscountCodes",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssignedToCompany",
                table: "DiscountCodes",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssignedToUser",
                table: "DiscountCodes",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscountCodes",
                table: "DiscountCodes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Batches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UploadTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Batches", x => x.Id);
                });

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
                name: "DiscountTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscountTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Images",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Content = table.Column<byte[]>(type: "bytea", nullable: true),
                    CategoryId = table.Column<int>(type: "integer", nullable: true),
                    PlayerId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Images", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Images_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Images_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "JoinRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Message = table.Column<string>(type: "text", nullable: true),
                    InvitationCodeId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    Declined = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JoinRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JoinRequests_InvitationCodes_InvitationCodeId",
                        column: x => x.InvitationCodeId,
                        principalTable: "InvitationCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JoinRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseTime = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    DiscountCodeId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Purchases_DiscountCodes_DiscountCodeId",
                        column: x => x.DiscountCodeId,
                        principalTable: "DiscountCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Purchases_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
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
                        name: "FK_PlayerDiscountType_DiscountTypes_DiscountTypeId",
                        column: x => x.DiscountTypeId,
                        principalTable: "DiscountTypes",
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
                name: "IX_Users_IdentityUserId",
                table: "Users",
                column: "IdentityUserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_DiscountTypeId",
                table: "Discounts",
                column: "DiscountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodes_BatchId",
                table: "DiscountCodes",
                column: "BatchId");

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCodes_DiscountId",
                table: "DiscountCodes",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyDiscount_DiscountId",
                table: "CompanyDiscount",
                column: "DiscountId");

            migrationBuilder.CreateIndex(
                name: "IX_Images_CategoryId",
                table: "Images",
                column: "CategoryId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Images_PlayerId",
                table: "Images",
                column: "PlayerId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_InvitationCodeId",
                table: "JoinRequests",
                column: "InvitationCodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_JoinRequests_UserId",
                table: "JoinRequests",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Offers_CompanyId",
                table: "Offers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_Offers_DiscountCodeId",
                table: "Offers",
                column: "DiscountCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerDiscountType_DiscountTypeId",
                table: "PlayerDiscountType",
                column: "DiscountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_DiscountCodeId",
                table: "Purchases",
                column: "DiscountCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_UserId",
                table: "Purchases",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_Batches_BatchId",
                table: "DiscountCodes",
                column: "BatchId",
                principalTable: "Batches",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCodes_Discounts_DiscountId",
                table: "DiscountCodes",
                column: "DiscountId",
                principalTable: "Discounts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_DiscountTypes_DiscountTypeId",
                table: "Discounts",
                column: "DiscountTypeId",
                principalTable: "DiscountTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Players_PlayerId",
                table: "Discounts",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_Batches_BatchId",
                table: "DiscountCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_DiscountCodes_Discounts_DiscountId",
                table: "DiscountCodes");

            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_DiscountTypes_DiscountTypeId",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Discounts_Players_PlayerId",
                table: "Discounts");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Batches");

            migrationBuilder.DropTable(
                name: "CompanyDiscount");

            migrationBuilder.DropTable(
                name: "Images");

            migrationBuilder.DropTable(
                name: "JoinRequests");

            migrationBuilder.DropTable(
                name: "Offers");

            migrationBuilder.DropTable(
                name: "PlayerDiscountType");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "DiscountTypes");

            migrationBuilder.DropIndex(
                name: "IX_Users_IdentityUserId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Discounts_DiscountTypeId",
                table: "Discounts");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiscountCodes",
                table: "DiscountCodes");

            migrationBuilder.DropIndex(
                name: "IX_DiscountCodes_BatchId",
                table: "DiscountCodes");

            migrationBuilder.DropIndex(
                name: "IX_DiscountCodes_DiscountId",
                table: "DiscountCodes");

            migrationBuilder.DropColumn(
                name: "JoinRequestId",
                table: "InvitationCodes");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "BatchId",
                table: "DiscountCodes");

            migrationBuilder.DropColumn(
                name: "IsAssignedToCompany",
                table: "DiscountCodes");

            migrationBuilder.DropColumn(
                name: "IsAssignedToUser",
                table: "DiscountCodes");

            migrationBuilder.RenameTable(
                name: "DiscountCodes",
                newName: "DiscountCode");

            migrationBuilder.RenameColumn(
                name: "DiscountTypeId",
                table: "Discounts",
                newName: "NumberOfUsagePerUser");

            migrationBuilder.RenameColumn(
                name: "UsageLimit",
                table: "DiscountCode",
                newName: "UnassignedDiscountCodeCollectionsId");

            migrationBuilder.RenameColumn(
                name: "DiscountId",
                table: "DiscountCode",
                newName: "UnassignedCollectionId");

            migrationBuilder.AlterColumn<int>(
                name: "CompanyId",
                table: "Users",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSent",
                table: "InvitationCodes",
                type: "boolean",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidityPeriod_StartDate",
                table: "Discounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "ValidityPeriod_EndDate",
                table: "Discounts",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTimeOffset),
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
                name: "DiscountCodeId",
                table: "Discounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DiscountType",
                table: "Discounts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfUsagePerCompany",
                table: "Discounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfEmployees",
                table: "Companies",
                type: "integer",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiscountCode",
                table: "DiscountCode",
                column: "Id");

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

            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    DiscountId = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vouchers_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Vouchers_Discounts_DiscountId",
                        column: x => x.DiscountId,
                        principalTable: "Discounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Discounts_DiscountCodeId",
                table: "Discounts",
                column: "DiscountCodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DiscountCode_UnassignedDiscountCodeCollectionsId",
                table: "DiscountCode",
                column: "UnassignedDiscountCodeCollectionsId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_CategoryId",
                table: "Vouchers",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Vouchers_DiscountId",
                table: "Vouchers",
                column: "DiscountId");

            migrationBuilder.AddForeignKey(
                name: "FK_DiscountCode_UnassignedDiscountCodeCollection_UnassignedDis~",
                table: "DiscountCode",
                column: "UnassignedDiscountCodeCollectionsId",
                principalTable: "UnassignedDiscountCodeCollection",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_DiscountCode_DiscountCodeId",
                table: "Discounts",
                column: "DiscountCodeId",
                principalTable: "DiscountCode",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Discounts_Players_PlayerId",
                table: "Discounts",
                column: "PlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Companies_CompanyId",
                table: "Users",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
