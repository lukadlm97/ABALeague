using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMoreOptionsForHtmlQuerySelectors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "HtmlQuerySelector",
                keyColumn: "Id",
                keyValue: (short)3,
                column: "Name",
                value: "StandingsRowName");

            migrationBuilder.InsertData(
                table: "HtmlQuerySelector",
                columns: new[] { "Id", "Name" },
                values: new object[] { (short)4, "StandingsRowUrl" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "HtmlQuerySelector",
                keyColumn: "Id",
                keyValue: (short)4);

            migrationBuilder.UpdateData(
                table: "HtmlQuerySelector",
                keyColumn: "Id",
                keyValue: (short)3,
                column: "Name",
                value: "StandingsRow");
        }
    }
}
