using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedLeagueOrganizationSchemeAndWayOfHandlingForKnockoutAndGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BracketPosition",
                table: "SeasonResources",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "SeasonResources",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "CompetitionOrganizationId",
                table: "Leagues",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CompetitionOrganizations",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompetitionOrganizations", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CompetitionOrganizations",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "League" },
                    { (short)2, "Groups" },
                    { (short)3, "Knockout" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_CompetitionOrganizationId",
                table: "Leagues",
                column: "CompetitionOrganizationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leagues_CompetitionOrganizations_CompetitionOrganizationId",
                table: "Leagues",
                column: "CompetitionOrganizationId",
                principalTable: "CompetitionOrganizations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leagues_CompetitionOrganizations_CompetitionOrganizationId",
                table: "Leagues");

            migrationBuilder.DropTable(
                name: "CompetitionOrganizations");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_CompetitionOrganizationId",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "BracketPosition",
                table: "SeasonResources");

            migrationBuilder.DropColumn(
                name: "Group",
                table: "SeasonResources");

            migrationBuilder.DropColumn(
                name: "CompetitionOrganizationId",
                table: "Leagues");
        }
    }
}
