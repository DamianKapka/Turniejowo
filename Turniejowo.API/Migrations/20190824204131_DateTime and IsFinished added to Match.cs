using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Turniejowo.API.Migrations
{
    public partial class DateTimeandIsFinishedaddedtoMatch : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "HomeTeamId",
                table: "Matches",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GuestTeamId",
                table: "Matches",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Matches",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TournamentDateTime",
                table: "Matches",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "TournamentDateTime",
                table: "Matches");

            migrationBuilder.AlterColumn<int>(
                name: "HomeTeamId",
                table: "Matches",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "GuestTeamId",
                table: "Matches",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}
