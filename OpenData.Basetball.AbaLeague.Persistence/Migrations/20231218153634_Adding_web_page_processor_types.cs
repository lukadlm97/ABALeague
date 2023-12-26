using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Adding_web_page_processor_types : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "ProcessorTypeId",
                table: "Leagues",
                type: "smallint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProcessorTypes",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProcessorTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ProcessorTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "Unknow" },
                    { (short)2, "Euro" },
                    { (short)3, "Aba" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Leagues_ProcessorTypeId",
                table: "Leagues",
                column: "ProcessorTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Leagues_ProcessorTypes_ProcessorTypeId",
                table: "Leagues",
                column: "ProcessorTypeId",
                principalTable: "ProcessorTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Leagues_ProcessorTypes_ProcessorTypeId",
                table: "Leagues");

            migrationBuilder.DropTable(
                name: "ProcessorTypes");

            migrationBuilder.DropIndex(
                name: "IX_Leagues_ProcessorTypeId",
                table: "Leagues");

            migrationBuilder.DropColumn(
                name: "ProcessorTypeId",
                table: "Leagues");
        }
    }
}
