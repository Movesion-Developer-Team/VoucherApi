using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class DiscountInfoFieldsUpdatedToOneFieldOnly : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InfoAPaginaAcquisizione",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "InfoCondizioni",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "InfoOttieni",
                table: "Discounts");

            migrationBuilder.RenameColumn(
                name: "InfoTermini",
                table: "Discounts",
                newName: "Info");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Info",
                table: "Discounts",
                newName: "InfoTermini");

            migrationBuilder.AddColumn<string>(
                name: "InfoAPaginaAcquisizione",
                table: "Discounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InfoCondizioni",
                table: "Discounts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InfoOttieni",
                table: "Discounts",
                type: "text",
                nullable: true);
        }
    }
}
