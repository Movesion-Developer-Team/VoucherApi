using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Persistence.Migrations
{
    public partial class InfoFieldsAddedToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ActiveReservations",
                table: "SystemUpdates",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true)
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

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

            migrationBuilder.AddColumn<string>(
                name: "InfoTermini",
                table: "Discounts",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InfoCondizioni",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "InfoOttieni",
                table: "Discounts");

            migrationBuilder.DropColumn(
                name: "InfoTermini",
                table: "Discounts");

            migrationBuilder.AlterColumn<int>(
                name: "ActiveReservations",
                table: "SystemUpdates",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);
        }
    }
}
