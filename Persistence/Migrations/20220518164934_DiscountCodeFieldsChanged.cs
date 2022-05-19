using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class DiscountCodeFieldsChanged : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfUsagePerCompany",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "NumberOfUsagePerUser",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "Limit",
                table: "DiscountCode",
                newName: "UsageLimit");

            migrationBuilder.AddColumn<bool>(
                name: "IsAssignedToCompany",
                table: "DiscountCode",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAssignedToUser",
                table: "DiscountCode",
                type: "boolean",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAssignedToCompany",
                table: "DiscountCode");

            migrationBuilder.DropColumn(
                name: "IsAssignedToUser",
                table: "DiscountCode");

            migrationBuilder.RenameColumn(
                name: "UsageLimit",
                table: "DiscountCode",
                newName: "Limit");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfUsagePerCompany",
                table: "Discounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfUsagePerUser",
                table: "Discounts",
                type: "integer",
                nullable: true);
        }
    }
}
