using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewColumnToLeagueWithNumberOfRoundsThatShouldBePlayed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoundsToPlay",
                table: "Leagues",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoundsToPlay",
                table: "Leagues");
        }
    }
}
