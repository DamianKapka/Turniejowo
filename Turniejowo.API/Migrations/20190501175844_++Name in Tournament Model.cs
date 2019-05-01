using Microsoft.EntityFrameworkCore.Migrations;

namespace Turniejowo.API.Migrations
{
    public partial class NameinTournamentModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Localization",
                table: "Tournaments",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Tournaments",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "Tournaments");

            migrationBuilder.AlterColumn<string>(
                name: "Localization",
                table: "Tournaments",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
