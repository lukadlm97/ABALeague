using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ImprovedModelForCalendarHandling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoundMatch_Teams_AwayTeamId",
                table: "RoundMatch");

            migrationBuilder.DropForeignKey(
                name: "FK_RoundMatch_Teams_HomeTeamId",
                table: "RoundMatch");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoundMatch",
                table: "RoundMatch");

            migrationBuilder.RenameTable(
                name: "RoundMatch",
                newName: "RoundMatches");

            migrationBuilder.RenameIndex(
                name: "IX_RoundMatch_HomeTeamId",
                table: "RoundMatches",
                newName: "IX_RoundMatches_HomeTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_RoundMatch_AwayTeamId",
                table: "RoundMatches",
                newName: "IX_RoundMatches_AwayTeamId");

            migrationBuilder.AddColumn<int>(
                name: "LeagueId",
                table: "RoundMatches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "OffSeason",
                table: "RoundMatches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoundMatches",
                table: "RoundMatches",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_RoundMatches_LeagueId",
                table: "RoundMatches",
                column: "LeagueId");

            migrationBuilder.AddForeignKey(
                name: "FK_RoundMatches_Leagues_LeagueId",
                table: "RoundMatches",
                column: "LeagueId",
                principalTable: "Leagues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoundMatches_Teams_AwayTeamId",
                table: "RoundMatches",
                column: "AwayTeamId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoundMatches_Teams_HomeTeamId",
                table: "RoundMatches",
                column: "HomeTeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RoundMatches_Leagues_LeagueId",
                table: "RoundMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_RoundMatches_Teams_AwayTeamId",
                table: "RoundMatches");

            migrationBuilder.DropForeignKey(
                name: "FK_RoundMatches_Teams_HomeTeamId",
                table: "RoundMatches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RoundMatches",
                table: "RoundMatches");

            migrationBuilder.DropIndex(
                name: "IX_RoundMatches_LeagueId",
                table: "RoundMatches");

            migrationBuilder.DropColumn(
                name: "LeagueId",
                table: "RoundMatches");

            migrationBuilder.DropColumn(
                name: "OffSeason",
                table: "RoundMatches");

            migrationBuilder.RenameTable(
                name: "RoundMatches",
                newName: "RoundMatch");

            migrationBuilder.RenameIndex(
                name: "IX_RoundMatches_HomeTeamId",
                table: "RoundMatch",
                newName: "IX_RoundMatch_HomeTeamId");

            migrationBuilder.RenameIndex(
                name: "IX_RoundMatches_AwayTeamId",
                table: "RoundMatch",
                newName: "IX_RoundMatch_AwayTeamId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RoundMatch",
                table: "RoundMatch",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoundMatch_Teams_AwayTeamId",
                table: "RoundMatch",
                column: "AwayTeamId",
                principalTable: "Teams",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_RoundMatch_Teams_HomeTeamId",
                table: "RoundMatch",
                column: "HomeTeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
