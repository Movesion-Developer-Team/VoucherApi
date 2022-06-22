using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class UnityOfMeasurementAndValueAreAddedToBatchModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UnityOfMeasurement",
                table: "Batches",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "Batches",
                type: "double precision",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnityOfMeasurement",
                table: "Batches");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Batches");
        }
    }
}
