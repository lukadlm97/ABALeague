using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedPlayerStatisticResources : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BoxScoreUrl",
                table: "Leagues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "BoxScores",
                columns: table => new
                {
                    RosterItemId = table.Column<int>(type: "int", nullable: false),
                    RoundMatchId = table.Column<int>(type: "int", nullable: false),
                    Minutes = table.Column<TimeSpan>(type: "time", nullable: true),
                    Points = table.Column<int>(type: "int", nullable: true),
                    ShotPrc = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ShotMade2Pt = table.Column<int>(type: "int", nullable: true),
                    ShotAttempted2Pt = table.Column<int>(type: "int", nullable: true),
                    ShotPrc2Pt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ShotMade3Pt = table.Column<int>(type: "int", nullable: true),
                    ShotAttempted3Pt = table.Column<int>(type: "int", nullable: true),
                    shotPrc3Pt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ShotMade1Pt = table.Column<int>(type: "int", nullable: true),
                    ShotAttempted1Pt = table.Column<int>(type: "int", nullable: true),
                    ShotPrc1Pt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DefensiveRebounds = table.Column<int>(type: "int", nullable: true),
                    OffensiveRebounds = table.Column<int>(type: "int", nullable: true),
                    TotalRebounds = table.Column<int>(type: "int", nullable: true),
                    Assists = table.Column<int>(type: "int", nullable: true),
                    Steals = table.Column<int>(type: "int", nullable: true),
                    Turnover = table.Column<int>(type: "int", nullable: true),
                    InFavoureOfBlock = table.Column<int>(type: "int", nullable: true),
                    AgainstBlock = table.Column<int>(type: "int", nullable: true),
                    CommittedFoul = table.Column<int>(type: "int", nullable: true),
                    ReceivedFoul = table.Column<int>(type: "int", nullable: true),
                    PointFromPain = table.Column<int>(type: "int", nullable: true),
                    PointFrom2ndChance = table.Column<int>(type: "int", nullable: true),
                    PointFromFastBreak = table.Column<int>(type: "int", nullable: true),
                    PlusMinus = table.Column<int>(type: "int", nullable: true),
                    RankValue = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BoxScores", x => new { x.RoundMatchId, x.RosterItemId });
                    table.ForeignKey(
                        name: "FK_BoxScores_RosterItems_RosterItemId",
                        column: x => x.RosterItemId,
                        principalTable: "RosterItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BoxScores_RoundMatches_RoundMatchId",
                        column: x => x.RoundMatchId,
                        principalTable: "RoundMatches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BoxScores_RosterItemId",
                table: "BoxScores",
                column: "RosterItemId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BoxScores");

            migrationBuilder.DropColumn(
                name: "BoxScoreUrl",
                table: "Leagues");
        }
    }
}
