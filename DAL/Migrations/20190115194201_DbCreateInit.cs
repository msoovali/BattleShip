using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class DbCreateInit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(nullable: false)
                        .Annotation("MySQL:AutoIncrement", true),
                    PlayerOne = table.Column<string>(maxLength: 64, nullable: false),
                    PlayerTwo = table.Column<string>(maxLength: 64, nullable: true),
                    PlayerOneBoard = table.Column<string>(nullable: true),
                    PlayerTwoBoard = table.Column<string>(nullable: true),
                    GameShips = table.Column<string>(nullable: true),
                    Rows = table.Column<int>(nullable: false),
                    Cols = table.Column<int>(nullable: false),
                    PlayerOneTurn = table.Column<int>(nullable: false),
                    GameOver = table.Column<int>(nullable: false),
                    NumberOfShips = table.Column<int>(nullable: false),
                    Ai = table.Column<int>(nullable: false),
                    ShipsCanTouch = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
