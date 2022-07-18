using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PenaltyBox.API.Migrations
{
    public partial class AddingSeasonType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeasonType",
                table: "Penalties",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeasonType",
                table: "Penalties");
        }
    }
}
