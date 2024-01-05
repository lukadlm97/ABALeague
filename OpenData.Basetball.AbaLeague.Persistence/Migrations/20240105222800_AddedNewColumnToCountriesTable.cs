using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedNewColumnToCountriesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AbaLeagueCode",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AbaLeagueCode",
                table: "Countries");
        }
    }
}
