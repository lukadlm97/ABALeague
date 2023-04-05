using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Added_data_seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "CodeIso", "CodeIso2", "Name" },
                values: new object[,]
                {
                    { 2, "BIH", "BH", "Bosnia and Herzegovina" },
                    { 3, "CRO", "HR", "Croatia" }
                });

            migrationBuilder.InsertData(
                table: "Leagues",
                columns: new[] { "Id", "CreatedBy", "CreatedDate", "OfficalName", "Season", "ShortName", "StandingUrl", "TeamParticipansUrl", "UpdateBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, "Sys", new DateTime(2023, 4, 3, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(4481), "NLB ABA League", "2022/23", "ABA", "ur1", "ur2", "Sys", new DateTime(2023, 4, 5, 18, 52, 34, 672, DateTimeKind.Utc).AddTicks(4496) },
                    { 2, "Sys", new DateTime(2023, 4, 3, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(4511), "NLB ABA League 2", "2022/23", "ABA2", "ur1", "ur2", "Sys", new DateTime(2023, 4, 5, 18, 52, 34, 672, DateTimeKind.Utc).AddTicks(4512) }
                });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "CountryId", "CreatedBy", "CreatedDate", "DateOfBirth", "Height", "Name", "Nationality", "PositionId", "UpdateBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 3, 1, "Sys", new DateTime(2023, 4, 3, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(6660), new DateTime(1991, 4, 5, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(6652), 193, "Dragan Milosavljevic", "Serbian", (short)2, "Sys", new DateTime(2023, 4, 5, 18, 52, 34, 672, DateTimeKind.Utc).AddTicks(6662) },
                    { 4, 3, "Sys", new DateTime(2023, 4, 3, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(6675), new DateTime(1987, 4, 5, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(6673), 210, "Miro Bilan", "Croatian", (short)5, "Sys", new DateTime(2023, 4, 5, 18, 52, 34, 672, DateTimeKind.Utc).AddTicks(6676) },
                    { 5, 1, "Sys", new DateTime(2023, 4, 3, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(6680), new DateTime(2002, 4, 5, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(6678), 201, "Uros Trifunovic", "Serbian", (short)3, "Sys", new DateTime(2023, 4, 5, 18, 52, 34, 672, DateTimeKind.Utc).AddTicks(6680) }
                });

            migrationBuilder.InsertData(
                table: "Teams",
                columns: new[] { "Id", "Capacity", "City", "CountryId", "CreatedBy", "CreatedDate", "Name", "ShortCode", "UpdateBy", "UpdatedDate", "Venue" },
                values: new object[,]
                {
                    { 1, 20000, "Belgrade", 1, "Sys", new DateTime(2023, 4, 3, 20, 52, 34, 673, DateTimeKind.Utc).AddTicks(4059), "Partizan", "PAR", "Sys", new DateTime(2023, 4, 5, 18, 52, 34, 673, DateTimeKind.Utc).AddTicks(4061), "Arena" },
                    { 2, 5000, "Aleksandrovac", 2, "Sys", new DateTime(2023, 4, 3, 20, 52, 34, 673, DateTimeKind.Utc).AddTicks(4073), "Igokea", "IGO", "Sys", new DateTime(2023, 4, 5, 18, 52, 34, 673, DateTimeKind.Utc).AddTicks(4074), "Dom sportova" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Leagues",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Leagues",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
