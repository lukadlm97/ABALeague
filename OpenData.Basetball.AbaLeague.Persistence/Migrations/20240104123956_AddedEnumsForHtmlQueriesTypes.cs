using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddedEnumsForHtmlQueriesTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HtmlQuerySelector",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HtmlQuerySelector", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "HtmlQuerySelector",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "Unknow" },
                    { (short)2, "StandingsTable" },
                    { (short)3, "StandingsRow" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HtmlQuerySelector");
        }
    }
}
