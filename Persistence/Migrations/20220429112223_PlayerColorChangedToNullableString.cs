using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    public partial class PlayerColorChangedToNullableString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Color",
                table: "Players",
                type: "text",
                nullable: true,
                defaultValue: "Yellow",
                oldClrType: typeof(int),
                oldType: "integer",
                oldDefaultValue: 166);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Color",
                table: "Players",
                type: "integer",
                nullable: false,
                defaultValue: 166,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true,
                oldDefaultValue: "Yellow");
        }
    }
}
