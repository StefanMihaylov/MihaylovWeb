using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mihaylov.Api.Other.Database.Migrations
{
    /// <inheritdoc />
    public partial class removeconcerttypeenum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "show",
                table: "ConcertTypes",
                keyColumn: "ConcertTypeId",
                keyValue: (byte)1);

            migrationBuilder.DeleteData(
                schema: "show",
                table: "ConcertTypes",
                keyColumn: "ConcertTypeId",
                keyValue: (byte)2);

            migrationBuilder.DeleteData(
                schema: "show",
                table: "ConcertTypes",
                keyColumn: "ConcertTypeId",
                keyValue: (byte)3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "show",
                table: "ConcertTypes",
                columns: new[] { "ConcertTypeId", "Name" },
                values: new object[,]
                {
                    { (byte)1, "Metal" },
                    { (byte)2, "Folklore" },
                    { (byte)3, "Theatre" }
                });
        }
    }
}
