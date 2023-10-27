using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_Missing_Part_At_RosterItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RosterItems_Teams_TeamId",
                table: "RosterItems");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "RosterItems",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RosterItems_Teams_TeamId",
                table: "RosterItems",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RosterItems_Teams_TeamId",
                table: "RosterItems");

            migrationBuilder.AlterColumn<int>(
                name: "TeamId",
                table: "RosterItems",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_RosterItems_Teams_TeamId",
                table: "RosterItems",
                column: "TeamId",
                principalTable: "Teams",
                principalColumn: "Id");
        }
    }
}
