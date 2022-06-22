using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class SOmeAdditionalFieldsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnityOfMeasurement",
                table: "Discounts");

            migrationBuilder.AddColumn<int>(
                name: "UnityOfMeasurement",
                table: "Discounts",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "InitialPrice",
                table: "Discounts",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PriceInPoints",
                table: "Discounts",
                type: "integer",
                nullable: true);

            migrationBuilder.DropColumn(
                name: "UnityOfMeasurement",
                table: "Batches");

            migrationBuilder.AddColumn<int>(
                name: "UnityOfMeasurement",
                table: "Batches",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PriceInPoints",
                table: "Discounts");
            migrationBuilder.DropColumn(
                name: "UnityOfMeasurement",
                table: "Discounts");

            migrationBuilder.AddColumn<string>(
                name: "UnityOfMeasurement",
                table: "Discounts",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "InitialPrice",
                table: "Discounts",
                type: "integer",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.DropColumn(
                name: "UnityOfMeasurement",
                table: "Batches");

            migrationBuilder.AddColumn<string>(
                name: "UnityOfMeasurement",
                table: "Batches",
                type: "text",
                nullable: true);
        }
    }
}
