using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mihaylov.Api.Other.Database.Migrations
{
    /// <inheritdoc />
    public partial class addcountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "show",
                table: "TicketProviders",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<byte>(
                name: "ConcertTypeId",
                schema: "show",
                table: "Concerts",
                type: "tinyint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CountryId",
                schema: "show",
                table: "Bands",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConcertTypes",
                schema: "show",
                columns: table => new
                {
                    ConcertTypeId = table.Column<byte>(type: "tinyint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ConcertTypeId", x => x.ConcertTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Countries",
                schema: "show",
                columns: table => new
                {
                    CountryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("CountryId", x => x.CountryId);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Concerts_ConcertTypeId",
                schema: "show",
                table: "Concerts",
                column: "ConcertTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Bands_CountryId",
                schema: "show",
                table: "Bands",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_ConcertTypes_Name",
                schema: "show",
                table: "ConcertTypes",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Name",
                schema: "show",
                table: "Countries",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bands_Countries_CountryId",
                schema: "show",
                table: "Bands",
                column: "CountryId",
                principalSchema: "show",
                principalTable: "Countries",
                principalColumn: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Concerts_ConcertTypes_ConcertTypeId",
                schema: "show",
                table: "Concerts",
                column: "ConcertTypeId",
                principalSchema: "show",
                principalTable: "ConcertTypes",
                principalColumn: "ConcertTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bands_Countries_CountryId",
                schema: "show",
                table: "Bands");

            migrationBuilder.DropForeignKey(
                name: "FK_Concerts_ConcertTypes_ConcertTypeId",
                schema: "show",
                table: "Concerts");

            migrationBuilder.DropTable(
                name: "ConcertTypes",
                schema: "show");

            migrationBuilder.DropTable(
                name: "Countries",
                schema: "show");

            migrationBuilder.DropIndex(
                name: "IX_Concerts_ConcertTypeId",
                schema: "show",
                table: "Concerts");

            migrationBuilder.DropIndex(
                name: "IX_Bands_CountryId",
                schema: "show",
                table: "Bands");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "show",
                table: "TicketProviders");

            migrationBuilder.DropColumn(
                name: "ConcertTypeId",
                schema: "show",
                table: "Concerts");

            migrationBuilder.DropColumn(
                name: "CountryId",
                schema: "show",
                table: "Bands");
        }
    }
}
