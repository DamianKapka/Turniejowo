using Microsoft.EntityFrameworkCore.Migrations;

namespace Turniejowo.API.Migrations
{
    public partial class PointsQtyaddedtoPointsentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PointsQty",
                table: "Points",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PointsQty",
                table: "Points");
        }
    }
}
