using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddResourceSelectorsForHtmlContent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ResourceSelector",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LeagueId = table.Column<int>(type: "int", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HtmlQuerySelectorId = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResourceSelector", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResourceSelector_Leagues_LeagueId",
                        column: x => x.LeagueId,
                        principalTable: "Leagues",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ResourceSelector_LeagueId",
                table: "ResourceSelector",
                column: "LeagueId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ResourceSelector");
        }
    }
}
