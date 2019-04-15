using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Turniejowo.API.Migrations
{
    public partial class TournamentEntityExtended : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Tournaments");

            migrationBuilder.AddColumn<int>(
                name: "AmountOfTeams",
                table: "Tournaments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Tournaments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "EntryFee",
                table: "Tournaments",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Localization",
                table: "Tournaments",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOfTeams",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "EntryFee",
                table: "Tournaments");

            migrationBuilder.DropColumn(
                name: "Localization",
                table: "Tournaments");

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Tournaments",
                nullable: false,
                defaultValue: false);
        }
    }
}
