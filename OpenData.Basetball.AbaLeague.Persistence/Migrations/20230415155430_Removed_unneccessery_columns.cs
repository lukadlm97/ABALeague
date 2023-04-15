using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OpenData.Basetball.AbaLeague.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Removed_unneccessery_columns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "City",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "Venue",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "TeamParticipansUrl",
                table: "Leagues");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Venue",
                table: "Teams",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TeamParticipansUrl",
                table: "Leagues",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Leagues",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "TeamParticipansUrl", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 4, 3, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(4481), "ur2", new DateTime(2023, 4, 5, 18, 52, 34, 672, DateTimeKind.Utc).AddTicks(4496) });

            migrationBuilder.UpdateData(
                table: "Leagues",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "TeamParticipansUrl", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 4, 3, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(4511), "ur2", new DateTime(2023, 4, 5, 18, 52, 34, 672, DateTimeKind.Utc).AddTicks(4512) });

            migrationBuilder.UpdateData(
                table: "Players",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "DateOfBirth", "Height", "Name", "PositionId", "UpdatedDate" },
                values: new object[] { new DateTime(2023, 4, 3, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(6680), new DateTime(2002, 4, 5, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(6678), 201, "Uros Trifunovic", (short)3, new DateTime(2023, 4, 5, 18, 52, 34, 672, DateTimeKind.Utc).AddTicks(6680) });

            migrationBuilder.InsertData(
                table: "Players",
                columns: new[] { "Id", "CountryId", "CreatedBy", "CreatedDate", "DateOfBirth", "Height", "Name", "Nationality", "PositionId", "UpdateBy", "UpdatedDate" },
                values: new object[,]
                {
                    { 1, 1, "Sys", new DateTime(2023, 4, 3, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(6660), new DateTime(1991, 4, 5, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(6652), 193, "Dragan Milosavljevic", "Serbian", (short)2, "Sys", new DateTime(2023, 4, 5, 18, 52, 34, 672, DateTimeKind.Utc).AddTicks(6662) },
                    { 2, 3, "Sys", new DateTime(2023, 4, 3, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(6675), new DateTime(1987, 4, 5, 20, 52, 34, 672, DateTimeKind.Utc).AddTicks(6673), 210, "Miro Bilan", "Croatian", (short)5, "Sys", new DateTime(2023, 4, 5, 18, 52, 34, 672, DateTimeKind.Utc).AddTicks(6676) }
                });

            migrationBuilder.UpdateData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Capacity", "City", "CreatedDate", "UpdatedDate", "Venue" },
                values: new object[] { 20000, "Belgrade", new DateTime(2023, 4, 3, 20, 52, 34, 673, DateTimeKind.Utc).AddTicks(4059), new DateTime(2023, 4, 5, 18, 52, 34, 673, DateTimeKind.Utc).AddTicks(4061), "Arena" });

            migrationBuilder.UpdateData(
                table: "Teams",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Capacity", "City", "CreatedDate", "UpdatedDate", "Venue" },
                values: new object[] { 5000, "Aleksandrovac", new DateTime(2023, 4, 3, 20, 52, 34, 673, DateTimeKind.Utc).AddTicks(4073), new DateTime(2023, 4, 5, 18, 52, 34, 673, DateTimeKind.Utc).AddTicks(4074), "Dom sportova" });
        }
    }
}
