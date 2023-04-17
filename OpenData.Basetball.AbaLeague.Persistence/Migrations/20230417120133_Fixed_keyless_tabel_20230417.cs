using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Fixed_keyless_tabel_20230417 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SeasonResources_TeamId",
                table: "SeasonResources");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SeasonResources",
                table: "SeasonResources",
                columns: new[] { "TeamId", "LeagueId" });

          }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_SeasonResources",
                table: "SeasonResources");
            
            migrationBuilder.CreateIndex(
                name: "IX_SeasonResources_TeamId",
                table: "SeasonResources",
                column: "TeamId");
        }
    }
}
