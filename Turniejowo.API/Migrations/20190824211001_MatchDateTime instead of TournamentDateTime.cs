using Microsoft.EntityFrameworkCore.Migrations;

namespace Turniejowo.API.Migrations
{
    public partial class MatchDateTimeinsteadofTournamentDateTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TournamentDateTime",
                table: "Matches",
                newName: "MatchDateTime");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MatchDateTime",
                table: "Matches",
                newName: "TournamentDateTime");
        }
    }
}
