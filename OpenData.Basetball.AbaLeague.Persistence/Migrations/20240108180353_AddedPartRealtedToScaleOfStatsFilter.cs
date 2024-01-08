using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedPartRealtedToScaleOfStatsFilter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameLengths",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameLengths", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LevelsOfScale",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LevelsOfScale", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StatsProperties",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StatsProperties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeagueGameLengths",
                columns: table => new
                {
                    LeagueId = table.Column<int>(type: "int", nullable: false),
                    GameLengthId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueGameLengths", x => new { x.GameLengthId, x.LeagueId });
                    table.ForeignKey(
                        name: "FK_LeagueGameLengths_GameLengths_GameLengthId",
                        column: x => x.GameLengthId,
                        principalTable: "GameLengths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_LeagueGameLengths_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "RangeScales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MinValue = table.Column<int>(type: "int", nullable: true),
                    MaxValue = table.Column<int>(type: "int", nullable: true),
                    StatsPropertyId = table.Column<short>(type: "smallint", nullable: false),
                    LevelOfScaleId = table.Column<short>(type: "smallint", nullable: false),
                    GameLengthId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RangeScales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RangeScales_GameLengths_GameLengthId",
                        column: x => x.GameLengthId,
                        principalTable: "GameLengths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RangeScales_LevelsOfScale_LevelOfScaleId",
                        column: x => x.LevelOfScaleId,
                        principalTable: "LevelsOfScale",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_RangeScales_StatsProperties_StatsPropertyId",
                        column: x => x.StatsPropertyId,
                        principalTable: "StatsProperties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.InsertData(
                table: "GameLengths",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "FourtyMinutes" },
                    { (short)2, "FourtyEightMinutes" }
                });

            migrationBuilder.InsertData(
                table: "LevelsOfScale",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "Team" },
                    { (short)2, "League" },
                    { (short)3, "Player" }
                });

            migrationBuilder.InsertData(
                table: "StatsProperties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "Points" },
                    { (short)2, "Rebounds" },
                    { (short)3, "Assists" },
                    { (short)4, "Turnover" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueGameLengths_LeagueId",
                table: "LeagueGameLengths",
                column: "LeagueId");

            migrationBuilder.CreateIndex(
                name: "IX_RangeScales_GameLengthId",
                table: "RangeScales",
                column: "GameLengthId");

            migrationBuilder.CreateIndex(
                name: "IX_RangeScales_LevelOfScaleId",
                table: "RangeScales",
                column: "LevelOfScaleId");

            migrationBuilder.CreateIndex(
                name: "IX_RangeScales_StatsPropertyId",
                table: "RangeScales",
                column: "StatsPropertyId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueGameLengths");

            migrationBuilder.DropTable(
                name: "RangeScales");

            migrationBuilder.DropTable(
                name: "GameLengths");

            migrationBuilder.DropTable(
                name: "LevelsOfScale");

            migrationBuilder.DropTable(
                name: "StatsProperties");
        }
    }
}
