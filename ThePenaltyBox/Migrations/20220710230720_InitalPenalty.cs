using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThePenaltyBox.Migrations
{
    public partial class InitalPenalty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Penalties",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GameDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Player = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Team = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PenaltyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Opponent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Referees = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Home = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalties", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Penalties");
        }
    }
}
